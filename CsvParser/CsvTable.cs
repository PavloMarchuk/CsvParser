using CsvParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvParser
{
	public class CsvTable
	{
		#region Fields
		private readonly List<CsvRow> _rows;
		private   FileInfo _fileInfo;
		#endregion
		#region Constuctors
		public CsvTable(string file_path, bool isHeaderFirstFow = false)
		{
			_rows = new List<CsvRow>();
			Parser(Loader(file_path));

			UnixTime = DateTimeOffset.Now;
			IsHeaderFirstFow = isHeaderFirstFow;
		}
		#endregion
		#region Properties
		public bool IsHeaderFirstFow { get; private set; }

		public List<CsvRow> Rows { get => _rows; }

		public DateTimeOffset UnixTime { get;   }
		public string FileName { get => _fileInfo.Name; }
		#endregion
		#region Private Methods
		private string Loader(string file_path)
		{
			try
			{
				_fileInfo = new FileInfo(file_path);
				return File.ReadAllText(_fileInfo.FullName);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Помилковий формат таблиці:");
				throw e;
			} 
			
			catch (Exception e)
			{
				Console.WriteLine("Помилка відкриття файлу:");
				throw e;
			}
		}
		private void Parser(string csv)
		{
			//CsvRow row = new CsvRow(IsHeaderFirstFow);
			CsvRow headerRow = new CsvRow(IsHeaderFirstFow);
			CsvRow row = headerRow;

			int coll = 0;
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
				row.Cells.Add(new CsvCell
				{
					Value = csv
					.Substring(indexSatart + delimiter.Length - 1, indexEnd - (indexSatart + delimiter.Length - 1) - endRow)
					.Replace("\"\"", "\"")
				});

				if (headerRow.Cells[coll].MaxWidth < row.Cells[coll].Value.Length)
				{
					headerRow.Cells[coll].MaxWidth = row.Cells[coll].Value.Length;
				}
				coll++;
				indexSatart = indexEnd + delimiter.Length;

				if (lastColl)
				{
					_rows.Add(row);
					row = new CsvRow();
					coll = 0;
				}
			}
			return;
		}
		#endregion
		#region Public Methods
		public void ConsolePrint()
		{
			

			
			foreach (CsvRow row in _rows)
			{

			}
		}
		#endregion
	}
}
