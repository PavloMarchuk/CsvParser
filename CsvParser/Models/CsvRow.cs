using System;
using System.Collections.Generic;
using System.Text;

namespace CsvParser.Models
{
	public class CsvRow
	{
		private List<CsvCell> _cells;

		public CsvRow(bool isHeader = false)
		{
			IsHeader = isHeader;
			_cells = new List<CsvCell>();
		}

		public List<CsvCell> Cells { get => _cells;   }
		public bool IsHeader { get; private set; }
	}
}
