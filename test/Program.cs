using CsvParser;
using System;
using System.Collections.Generic;
using System.IO;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
			//
			//tmp file_path
			// tmp --header-first-row=true
			string file_path = @"c:\temp\file1.csv";
			CsvTable ct = new CsvTable(file_path, true);
			//Timestamp: { unix_timestamp}
			Console.WriteLine("File: {" + $"{ct.FileName}" + "}");
			Console.WriteLine("Timestamp: {" + $"{ct.UnixTime.ToUnixTimeSeconds() }" + "}");
			Console.WriteLine();
			ct.ConsolePrint();
		}

		static void Main2(string[] args)
		{
			//string s =@"C:\b2b_Widjets\CSV_Parser\CVS_Parser\ConsoleCvsParser\bin\Debug\ConsoleCvsParser.exe --file=[file1.csv]";

			string path = @"c:\temp\file1.csv";

			string input = Environment.CommandLine;
			string pattern = "--file=[";

			int indexFileParam = input.IndexOf(pattern);
			if (indexFileParam > 1)
			{
				int indexEndParam = input.IndexOf("]", indexFileParam);
				path = Directory.GetCurrentDirectory()
					+ @"\"
					+ input.Substring(indexFileParam + pattern.Length, indexEndParam - (indexFileParam + pattern.Length));
			}


			if (!File.Exists(path))
			{
				string[] createText =
				{
					"Year, Make, Brand, Description, Price",
					"1997,Ford,E350,\"ac, abs, moon\",3000.00",
					"1999,Chevy,\"Venture \"\"Extended Edition\"\"\",,4900.00",
					"1996,Jeep,Grand Cherokee,\"MUST SELL!",
					"air, moon roof, loaded\",4799.00"
				};
				File.WriteAllLines(path, createText);
			}
			//else { Console.WriteLine("file not found 404"); }


			string readText = File.ReadAllText(path);
			Print(Parser(readText), Path.GetFileName(path));
		}

		static List<List<string>> Parser(string csv)
		{
			List<List<string>> res = new List<List<string>>();
			List<string> row = new List<string>();

			int indexSatart = 0;
			int indexEnd = 0;
			int indexRowEnd = 0;

			while (indexSatart < csv.Length)
			{
				bool lastColl = false;
				string delimiter = ",";

				if (!(csv[indexSatart] == '\"'))
				{
					delimiter = ",";
					indexRowEnd = csv.IndexOf("\n", indexSatart) == -1 ? csv.Length : csv.IndexOf("\n", indexSatart);
					indexEnd = Math.Min(csv.IndexOf(delimiter, indexSatart), indexRowEnd);
				}
				else
				{
					delimiter = "\",";
					indexEnd = csv.IndexOf(delimiter, indexSatart) == -1 ? csv.Length : csv.IndexOf(delimiter, indexSatart);
					indexRowEnd = csv.IndexOf("\n", indexEnd) == -1 ? csv.Length : csv.IndexOf("\n", indexEnd);
				}

				if (indexEnd == -1 | indexEnd == indexRowEnd)
				{
					indexEnd = indexRowEnd;
					lastColl = true;
				}
				int endRow = indexEnd == indexRowEnd ? 1 : 0;

				row.Add(csv.
					Substring(indexSatart + delimiter.Length - 1, indexEnd - (indexSatart + delimiter.Length - 1) - endRow)
					.Replace("\"\"", "\""));
				indexSatart = indexEnd + delimiter.Length;

				if (lastColl)
				{
					res.Add(row);
					row = new List<string>();
				}
			}
			return res;
		}

		static void Print(List<List<string>> strL, string fName)
		{
			//long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
			//DateTimeOffset(now).ToUnixTimeSeconds()
			//long unixTime2 = ; 
			Console.WriteLine("File: {" + $"{fName}" + "}");
			//Console.WriteLine("Timestamp: {" + $"{unixTime}" + "}");
			Console.WriteLine("Timestamp: {" + $"{DateTimeOffset.Now.ToUnixTimeSeconds()}" + "}");

			Console.WriteLine("------------------------------------------------------------");
			foreach (List<string> row in strL)
			{
				foreach (string str in row)
				{
					Console.Write($"{$"[{str}]",-30} ");
				}
				Console.WriteLine();
			}
			Console.WriteLine("\n\n\n@Pavlo Marchuk +38(050)5519211");
			Console.ReadLine();
		}
	}
}
