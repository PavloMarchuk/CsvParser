using CsvParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace test
{
	class Program
	{
		static void Main(string[] args)
		{
			//string file_path = @"c:\temp\file1.csv";
			//string s =@"C:\Users\Admin\Desktop\Parser\CsvParser\test\bin\Debug\netcoreapp2.0\win10-x64\test.exe --file=[c:\temp\file1.csv]  ";
			//CommandLineApplication commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);
			//CommandArgument names = null;


			Dictionary<string, string> properties = new Dictionary<string, string>();
			foreach (string s in args)
			{
				properties.Add(s.Substring(s.IndexOf("--") + 2, s.IndexOf("=[") - 2), s.Substring(s.IndexOf("=[") + 2, s.IndexOf("]") - (s.IndexOf("=[") + 2)));
			}

			foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
				Console.WriteLine("  {0} = {1}", de.Key, de.Value);


			bool haveHeader = true;
			if (properties.ContainsKey("file"))
			{
				if (properties.ContainsKey("header-first-row"))
				{
					haveHeader = Convert.ToBoolean(properties["header-first-row"]);
				}
				CsvTable ct = new CsvTable(properties["file"], haveHeader);
				ct.ConsolePrint();
			}
			else
			{
				Console.WriteLine("Please enter a Path");
			}

			Console.ReadLine();
		}

		
	}
}
