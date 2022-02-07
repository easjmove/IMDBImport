using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBImport
{
    //Estimated 2 hours
    class PreparedInsert : IInsert
    {
        public void InsertData(SqlConnection sqlconn, List<TitleBasic> allTitles)
        {
            SqlCommand sqlComm = new SqlCommand("INSERT INTO [dbo].[TitlesBasic] " +
                                                "([Tconst],[TitleType],[PrimaryTitle],[OriginalTitle],[IsAdult],[StartYear],[EndYear],[RuntimeMinutes]) " +
                                                "VALUES " +
                                                "(@tconst,@titleType,@primaryTitle,@originalTitle,@isAdult,@startYear,@endYear,@runTimeMinutes)", sqlconn);
            SqlParameter tconstPar = new SqlParameter("@tconst", SqlDbType.VarChar, 50);
            SqlParameter titleTypePar = new SqlParameter("@titleType", SqlDbType.VarChar, 50);
            SqlParameter primaryTitlePar = new SqlParameter("@primaryTitle", SqlDbType.VarChar, 255);
            SqlParameter originalTitlePar = new SqlParameter("@originalTitle", SqlDbType.VarChar, 255);
            SqlParameter isAdultPar = new SqlParameter("@isAdult", SqlDbType.Bit);
            SqlParameter startYearPar = new SqlParameter("@startYear", SqlDbType.Int);
            SqlParameter endYearPar = new SqlParameter("@endYear", SqlDbType.Int);
            SqlParameter runTimeMinutesPar = new SqlParameter("@runTimeMinutes", SqlDbType.Int);

            sqlComm.Parameters.Add(tconstPar);
            sqlComm.Parameters.Add(titleTypePar);
            sqlComm.Parameters.Add(primaryTitlePar);
            sqlComm.Parameters.Add(originalTitlePar);
            sqlComm.Parameters.Add(isAdultPar);
            sqlComm.Parameters.Add(startYearPar);
            sqlComm.Parameters.Add(endYearPar);
            sqlComm.Parameters.Add(runTimeMinutesPar);

            sqlComm.Prepare();

            foreach (TitleBasic title in allTitles)
            {
                SetParameterValue(title.tconst, tconstPar);
                SetParameterValue(title.titleType, titleTypePar);
                SetParameterValue(title.primaryTitle, primaryTitlePar);
                SetParameterValue(title.originalTitle, originalTitlePar);
                SetParameterValue(title.isAdult, isAdultPar);
                SetParameterValue(title.startYear, startYearPar);
                SetParameterValue(title.endYear, endYearPar);
                SetParameterValue(title.runTimeMinutes, runTimeMinutesPar);

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

        public static void SetParameterValue(string input, SqlParameter parameter)
        {
            if (input.ToLower() == "\\n")
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                switch (parameter.DbType)
                {
                    case DbType.Boolean:
                        parameter.Value = (input == "1");
                        break;
                    case DbType.Int32:
                        parameter.Value = int.Parse(input);
                        break;
                    default:
                        parameter.Value = input;
                        break;
                }
            }
        }
    }
}
