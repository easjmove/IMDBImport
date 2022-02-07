using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBImport
{
    //Estimated 2,5 hours
    class NormalInsert : IInsert
    {
        public void InsertData(SqlConnection sqlconn, List<TitleBasic> allTitles)
        {
            foreach (TitleBasic title in allTitles)
            {
                SqlCommand sqlComm = new SqlCommand("INSERT INTO [dbo].[TitlesBasic] " +
                                                   "([Tconst],[TitleType],[PrimaryTitle],[OriginalTitle],[IsAdult],[StartYear],[EndYear],[RuntimeMinutes]) " +
                                                   "VALUES " +
                                                   "('" + title.tconst + "'," + CheckForNull(title.titleType, true) + "," + CheckForNull(title.primaryTitle, true) +
                                                   "," + CheckForNull(title.originalTitle, true) + "," + CheckForNull(title.isAdult,false) +
                                                   "," + CheckForNull(title.startYear, false) + "," + CheckForNull(title.endYear, false) +
                                                   "," + CheckForNull(title.runTimeMinutes, false) + ")", sqlconn);
                try
                {
                    sqlComm.ExecuteNonQuery();
                    //Console.WriteLine("INSERTED: " + tconst);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(sqlComm.CommandText);
                    throw ex;
                }
            }
        }

        public static string CheckForNull(string input, bool includeQuotes)
        {
            if (input.ToLower() == "\\n")
            {
                return "NULL";
            }
            if (includeQuotes)
            {
                return "'" + input.Replace("'", "''") + "'";
            }
            return input;
        }
    }
}
