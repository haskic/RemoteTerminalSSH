using System.Data.OleDb;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace SSHZhakar.Tests
{
    class ExcelReader
    {
        static void Main(string[] args)
        {
            string FILENAME = "zhakardata.xlsx";
            List<TestCaseData> list =  ExcelReader.ReadFromExcel(FILENAME, "Sh2").ToList();

            //Console.WriteLine(list[0].Arguments[0]);
            //Console.WriteLine(list[0].Arguments[1]);
            //List<TestCaseData> list2 = ExcelReader.ReadFromExcel(FILENAME, "Sh1").ToList();
            //Console.WriteLine(list2[0].Arguments[0]);
            //Console.WriteLine(list2[0].Arguments[1]);


            Console.WriteLine("CUrrent path = " + Assembly.GetExecutingAssembly().Location);

            Console.Read();
        }
        public static IEnumerable<TestCaseData> ReadFromExcel(string excelFileName, string excelsheetTabName)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xslLocation = Path.Combine(executableLocation, "data/" + excelFileName);

            string cmdText = "SELECT * FROM [" + excelsheetTabName + "$]";

            if (!File.Exists(xslLocation))
                throw new Exception(string.Format("File name: {0}", xslLocation), new FileNotFoundException());

            string connectionStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\";", xslLocation);

            var testCases = new List<TestCaseData>();
            using (var connection = new OleDbConnection(connectionStr))
            {
                connection.Open();
                var command = new OleDbCommand(cmdText, connection);
                var reader = command.ExecuteReader();

                if (reader == null)
                    throw new Exception(string.Format("No data return from file, file name:{0}", xslLocation));
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString());
                    var row = new List<string>();
                    var feildCnt = reader.FieldCount;
                    Console.WriteLine("COUNT = " + feildCnt);
                    for (var i = 0; i < feildCnt; i++)
                    {
                        row.Add(reader.GetValue(i).ToString());
                        //Console.WriteLine(reader.GetValue(i).ToString());
                    }

                    testCases.Add(new TestCaseData(row.ToArray()));
                }
            }
            Console.WriteLine("CASES length = " + testCases.Count);
            if (testCases != null)
                foreach (TestCaseData testCaseData in testCases)
                    if (testCaseData.Arguments[0] != "") { 
                    yield return testCaseData;

                    }
        }

    }
}
