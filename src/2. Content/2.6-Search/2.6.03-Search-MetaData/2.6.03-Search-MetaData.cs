using Configuration;
using ConsoleTables;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace _2._6._03_Search_MetaData
{
    class Program
    {
        static void Main(string[] _)
        {
            using ISession session = Sessions.GetSession();
            if (session.Open() == Session.State.Opened)
            {
                // Request the metadata properties for the GovCorpInstruments View
                var response = MetaData.Definition(Search.View.GovCorpInstruments).GetData();

                if (response.IsSuccess)
                {
                    var table = response.Data.Table;
                    DisplayTable(table);

                    // Search for a specific property
                    var result = table.Select("Property = 'RCSAssetCategory'");
                    Console.WriteLine($"Property {result[0]["Property"]}: Navigable: {result[0]["Navigable"]}, Sortable: {result[0]["Sortable"]}");

                    result = table.Select("Property = 'RCSCountryGenealogy'");
                    Console.WriteLine($"Property {result[0]["Property"]}: Navigable: {result[0]["Navigable"]}, Sortable: {result[0]["Sortable"]}");

                    result = table.Select("Property = 'SPsRatingsScope.CurrentRatingRank'");
                    Console.WriteLine($"Property {result[0]["Property"]}: Navigable: {result[0]["Navigable"]}, Sortable: {result[0]["Sortable"]}");

                    // Search for a specific nested property and all its sub properties
                    result = table.Select("Property LIKE 'RatingsScope.*'");
                    DisplayRows(table.Columns, result);
                }
            }
        }

        private static void DisplayTable(DataTable table)
        {
            if (table != null && table.Columns.Count > 0)
            {
                DisplayRows(table.Columns, table.Rows.Cast<DataRow>());
            }
            else
            {
                Console.WriteLine($"Response contains an empty data set");
            }

            Console.WriteLine("\nHit any key...");
            Console.ReadKey();
        }

        private static void DisplayRows(DataColumnCollection columns, IEnumerable<DataRow> rows)
        {
            var console = new ConsoleTable();

            IList<string> cols = new List<string>();
            foreach (DataColumn col in columns)
                cols.Add(col.ColumnName);

            console.AddColumn(cols);

            IList<object> rowData = new List<object>();
            foreach (DataRow dataRow in rows)
            {
                foreach (object item in dataRow.ItemArray)
                    rowData.Add(item);

                console.AddRow(rowData.ToArray());
                rowData.Clear();
            }

            if (console.Columns.Count > 0)
            {
                Console.WriteLine("\n");
                console.Write(Format.MarkDown);
            }
        }
    }
}
