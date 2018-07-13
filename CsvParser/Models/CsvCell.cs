using System;
using System.Collections.Generic;
using System.Text;

namespace CsvParser.Models
{
	public class CsvCell 
    {
		public string Value { get; set; }
		public int MaxWidth { get; set; } = 0;
		  
	}
}
