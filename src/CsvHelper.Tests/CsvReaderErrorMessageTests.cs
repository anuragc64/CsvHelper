using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;
using Moq;
using Xunit;

namespace CsvHelper.Tests
{
	public class CsvReaderErrorMessageTests
	{
		[Fact]
		public void Test()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			using( var reader = new StreamReader( stream ) )
			using( var csvReader = new CsvReader( reader ) )
			{
				csvReader.Configuration.AllowComments = true;
				writer.WriteLine( "IntColumn,StringColumn" );
				writer.WriteLine( "# comment" );
				writer.WriteLine();
				writer.WriteLine( ",one" );
				writer.WriteLine( "2,two" );
				writer.Flush();
				stream.Position = 0;

				try
				{
					var records = csvReader.GetRecords<TestClass>().ToList();
					throw new Exception();
				}
				catch( CsvReaderException ex )
				{
					Assert.True( ex.ToString().Contains( "Row: 4" ) );
				}
			}
		}

		private class TestClass
		{
			public int IntColumn { get; set; }

			public string StringColumn { get; set; }
		}
	}
}
