using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace IMDBImport
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConn = new SqlConnection("Server=localhost;Database=IMDB;User Id=imdb_user;Password=ImDbWuUu;");
            sqlConn.Open();

            List<TitleBasic> allTitles = ReadAllTitles(@"C:\Users\zealand\Downloads\DB Elective\title.basics.tsv\data.tsv", 50000);
            Console.WriteLine("Read " + allTitles.Count + " titles from file");


            bool readInput = true;
            while (readInput)
            {
                IInsert inserter = null;
                Console.WriteLine("Write 1 for Normal insert, 2 for Prepared, 3 for Entity Framework, 4 for Bulk");
                Console.WriteLine("5 for clearing DB, 6 for end");
                string input = Console.ReadLine();
                int inputNumber = int.Parse(input);
                switch (inputNumber)
                {
                    case 1:
                        inserter = new NormalInsert();
                        break;
                    case 2:
                        inserter = new PreparedInsert();
                        break;
                    case 3:
                        inserter = new EFInsert();
                        break;
                    case 4:
                        inserter = new BulkInsert();
                        break;
                    case 5:
                        Console.WriteLine("Deleting all rows");
                        DeleteAllRows(sqlConn);
                        Console.WriteLine("All rows deleted");
                        break;
                    case 6:
                        readInput = false;
                        break;
                }

                if (inserter != null)
                {
                    DateTime startTime = DateTime.Now;
                    Console.WriteLine("Inserts begin at:" + startTime.ToString());
                    inserter.InsertData(sqlConn, allTitles);
                    DateTime endTime = DateTime.Now;
                    Console.WriteLine("Insert time: " + endTime.Subtract(startTime).TotalSeconds);
                    Console.WriteLine("Inserts end at:" + endTime.ToString());
                }
            }
            sqlConn.Close();
        }

        public static void DeleteAllRows(SqlConnection sqlConn)
        {
            SqlCommand sqlComm = new SqlCommand("DELETE FROM TitlesBasic", sqlConn);
            sqlComm.CommandTimeout = 0;
            sqlComm.ExecuteNonQuery();
        }

        public static List<TitleBasic> ReadAllTitles(string filePath, int maxRows)
        {
            List<TitleBasic> allTitles = new List<TitleBasic>();
            int counter = 0;

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\zealand\Downloads\DB Elective\title.basics.tsv\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 9)
                    {
                        allTitles.Add(new TitleBasic(splitLine));
                    }
                }

                if (maxRows != 0 && counter++ >= maxRows)
                {
                    break;
                }
            }
            return allTitles;
        }
    }
}

