using CsvParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvParser
{
	public class CsvTable
	{
		#region Fields
		private readonly List<CsvRow> _rows;
		private FileInfo _fileInfo;
		#endregion
		#region Constuctors
		public CsvTable(string file_path, bool isHeaderFirstFow = false)
		{
			IsHeaderFirstFow = isHeaderFirstFow;
			_rows = new List<CsvRow>();
			Parser(Loader(file_path));

			UnixTime = DateTimeOffset.Now;

		}
		#endregion
		#region Properties
		public bool IsHeaderFirstFow { get; private set; }

		public List<CsvRow> Rows { get => _rows; }

		public DateTimeOffset UnixTime { get; }
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
				throw e;
			}

			catch (Exception e)
			{
				throw new Exception("Помилка відкриття файлу:", e);
			}
		}
		private void Parser(string csv)
		{
			try
			{
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

					if ((csv[indexSatart] == '\"'))
					{
						delimiter = "\",";
						indexEnd = csv.IndexOf(delimiter, indexSatart) == -1 ? csv.Length : csv.IndexOf(delimiter, indexSatart);
						indexRowEnd = csv.IndexOf("\n", indexEnd) == -1 ? csv.Length : csv.IndexOf("\n", indexEnd);
					}
					else
					{
						delimiter = ",";
						indexRowEnd = csv.IndexOf("\n", indexSatart) == -1 ? csv.Length : csv.IndexOf("\n", indexSatart);
						indexEnd = Math.Min(csv.IndexOf(delimiter, indexSatart), indexRowEnd);
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
			}
			catch (ArgumentOutOfRangeException e)
			{
				throw new Exception("Помилковий формат таблиці:", e);
			}
			catch (Exception e)
			{
				throw e;
			}
			return;
		}
		#endregion
		#region Public Methods
		public void ConsolePrint()
		{
			CsvRow header = Rows.Where(r => r.IsHeader).FirstOrDefault();


			string topFileStr = "File: {" + $"{FileName}" + "}";

			if (header != null)
			{
				int tableWidth;
				int halthPadding = 0;
				tableWidth  = header.Cells.Sum(c => c.MaxWidth) + 5;
				halthPadding = tableWidth > topFileStr.Length ? (tableWidth / 2) - (topFileStr.Length / 2) : 0;
				Console.WriteLine(new string(' ', halthPadding) + topFileStr);

				for (int i = 0; i < tableWidth + (header.Cells.Count) * 2  +5; i++ ) { Console.Write("─"); }
				int intchar = 0;
				Console.Write($"\n│   │");
				foreach (CsvCell cell in header.Cells)
				{
					Console.Write(" " + new string(' ', ((cell.MaxWidth - 1) / 2))
						+ (char)(intchar + 65)
						+ new string(' ', cell.MaxWidth - ((cell.MaxWidth - 1) / 2)) + "│");
					intchar++;
				}
				Console.WriteLine();
				for (int i = 0; i < tableWidth + (header.Cells.Count) * 2 + 5; i++)
				{
					Console.Write("─");
				}
				Console.Write($"\n│   │");

				foreach (CsvCell cell in header.Cells)
				{
					int tmp = cell.MaxWidth - cell.Value.Length;
					Console.Write( " " + new string(' ', ((cell.MaxWidth - cell.Value.Length) / 2))
						+ cell.Value
						+ new string(' ', 1 + (cell.MaxWidth - ((cell.MaxWidth - cell.Value.Length) / 2 + cell.Value.Length))) + "│");
				}
				Console.WriteLine();
				for (int i = 0; i < tableWidth + (header.Cells.Count) * 2 + 5; i++)
				{
					Console.Write("─");
				}
				Console.WriteLine();

				IEnumerable<CsvRow> body = Rows.Where(r => r.IsHeader == false);
				int rowNum = 1;
				int coll = 0;
				foreach (CsvRow row in body)
				{
					Console.Write ($"│{rowNum,-3}│");
					
					foreach (CsvCell cell in row.Cells)
					{
						Console.Write($" {cell.Value}{new string(' ', header.Cells[coll].MaxWidth - cell.Value.Length)} │");
						coll++;
					}
					Console.WriteLine();
					rowNum++;
					coll = 0;
				}
				for (int i = 0; i < tableWidth + (header.Cells.Count) * 2 + 5; i++) { Console.Write("─"); }

				string ts = "Timestamp: {" + $"{UnixTime.ToUnixTimeSeconds() }" + "}";
				Console.Write (Environment.NewLine);
				Console.WriteLine( new string (' ', tableWidth + header.Cells.Count*3 - ts.Length)  + ts);
				 
			}

			else
			{
				foreach (CsvRow row in _rows)
				{
					foreach (CsvCell cell in row.Cells)
					{
						Console.Write($"{cell.Value, -30} │");
					}
					Console.WriteLine();
				}
			}
		}
		#endregion

	}
}
//
