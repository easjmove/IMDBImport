using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMDBImport
{
    //around 90 seconds
    class BulkInsert : IInsert
    {
        public enum TableTypes
        {
            tableString, tableInt, tableBool
        }

        public void InsertData(SqlConnection sqlconn, List<TitleBasic> allTitles)
        {
            DataTable TitleTable = new DataTable("TitlesBasic");
            TitleTable.Columns.Add("tconst", typeof(string));
            TitleTable.Columns.Add("titleType", typeof(string));
            TitleTable.Columns.Add("primaryTitle", typeof(string));
            TitleTable.Columns.Add("originalTitle", typeof(string));
            TitleTable.Columns.Add("isAdult", typeof(bool));
            TitleTable.Columns.Add("startYear", typeof(int));
            TitleTable.Columns.Add("endYear", typeof(int));
            TitleTable.Columns.Add("runTimeMinutes", typeof(int));

            foreach (TitleBasic title in allTitles)
            {
                DataRow row = TitleTable.NewRow();
                AddValueToRow(title.tconst, row, "tconst", TableTypes.tableString);
                AddValueToRow(title.titleType, row, "titleType", TableTypes.tableString);
                AddValueToRow(title.primaryTitle, row, "primaryTitle", TableTypes.tableString);
                AddValueToRow(title.originalTitle, row, "originalTitle", TableTypes.tableString);
                AddValueToRow(title.isAdult, row, "isAdult", TableTypes.tableBool);
                AddValueToRow(title.startYear, row, "startYear", TableTypes.tableInt);
                AddValueToRow(title.endYear, row, "endYear", TableTypes.tableInt);
                AddValueToRow(title.runTimeMinutes, row, "runTimeMinutes", TableTypes.tableInt);

                TitleTable.Rows.Add(row);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlconn, SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.BulkCopyTimeout = 0;
            // set the destination table name
            bulkCopy.DestinationTableName = "TitlesBasic";

            try
            {
                // write the data in the "dataTable"
                bulkCopy.WriteToServer(TitleTable);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                {
                    string pattern = @"\d+";
                    Match match = Regex.Match(ex.Message.ToString(), pattern);
                    var index = Convert.ToInt32(match.Value) - 1;

                    FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                    var sortedColumns = fi.GetValue(bulkCopy);
                    var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                    FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                    var metadata = itemdata.GetValue(items[index]);

                    var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    throw new Exception(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
                }
                throw ex;
            }
        }

        public static void AddValueToRow(string value, DataRow row, string rowName, TableTypes type)
        {
            if (value.ToLower() == "\\n")
            {
                row[rowName] = DBNull.Value;
            }
            else
            {
                switch (type)
                {
                    case TableTypes.tableString:
                        row[rowName] = value;
                        break;
                    case TableTypes.tableInt:
                        row[rowName] = int.Parse(value);
                        break;
                    case TableTypes.tableBool:
                        row[rowName] = (value == "1"); ;
                        break;
                }
            }
        }
    }
}
