using Configuration;
using ConsoleTables;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace _2._6._02_Search_Lookup
{
    class Program
    {
        static void Main(string[] _)
        {
            using ISession session = Sessions.GetSession();
            if (session.Open() == Session.State.Opened)
            {
                // RICs - convert to all specified symbol types
                var lookup = Lookup.Definition().Terms("VOD.L,IBM,AAPL.O,MSFT.O")
                                                .Scope("RIC")
                                                .Select("DocumentTitle, CUSIP, SEDOL, TickerSymbol, IssueISIN, IssuerOAPermID")
                                                .GetData();
                DisplayTable("\nRIC Lookup for 5 valid items using selected output", lookup);

                // RICs - detect input and convert to all known symbol types using default output
                lookup = Lookup.Definition().View(Search.View.SearchAll)
                                            .Terms("VOD.L,037833100,IBM,US5949181045,503085358")
                                            .GetData();
                DisplayTable("\nRIC Lookup for 5 valid items using default output", lookup);
                DisplayMatches("\nRIC Lookup for 5 valid items:", lookup);
            }
        }

        private static void DisplayTable(string label, ILookupResponse response, int truncate = 40)
        {
            Console.WriteLine($"\n{label}");
            if (response.IsSuccess)
            {
                if (response.Data?.Table != null && response.Data.Table.Columns.Count > 0)
                {
                    var console = new ConsoleTable();

                    IList<string> columns = new List<string>();
                    foreach (DataColumn col in response.Data.Table.Columns)
                        columns.Add(col.ColumnName);

                    console.AddColumn(columns);

                    IList<object> rowData = new List<object>();
                    foreach (DataRow dataRow in response.Data.Table.Rows)
                    {
                        foreach (object item in dataRow.ItemArray)
                        {
                            var type = item.GetType();

                            switch (item)
                            {
                                case string str:
                                    //var str = val.ToString();
                                    rowData.Add(truncate > 0 && str.Length > truncate ? $"{str.Substring(0, truncate)}..." : str);
                                    break;
                                default:
                                    rowData.Add(item);
                                    break;
                            }
                        }

                        console.AddRow(rowData.ToArray());
                        rowData.Clear();
                    }

                    if (console.Columns.Count > 0)
                    {
                        Console.WriteLine("\n");
                        console.Write(Format.MarkDown);
                    }
                }

                // Warnings
                if (response.Data.Warnings.Count > 0)
                    Console.WriteLine($"\n{string.Join(',', response.Data.Warnings.Select(warning => $"{warning}"))}");
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }

            Console.Write("\nHit any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }

        private static void DisplayMatches(string label, ILookupResponse response)
        {
            Console.WriteLine($"\n{label}\n");
            if (response.IsSuccess)
            {
                // Matches
                foreach (KeyValuePair<string, JObject> match in response.Data.Matches)
                {
                    Console.WriteLine($"{match.Key} - {match.Value}");
                }

                // Warnings
                foreach (string warning in response.Data.Warnings)
                {
                    Console.WriteLine($"WARNING: {warning}");
                }
            }
            else
            {
                Console.WriteLine(response.HttpStatus);
            }

            Console.Write("\nHit any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
