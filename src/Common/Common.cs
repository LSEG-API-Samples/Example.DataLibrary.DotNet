using BetterConsoles.Tables.Builders;
using BetterConsoles.Tables.Configuration;
using BetterConsoles.Tables.Models;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.Data;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Content.Symbology;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

// **********************************************************************************************************************
// Common
// Convenient methods used across the projects defined within the Content layer.
// **********************************************************************************************************************
namespace Common_Examples
{
    public static class Common
    {
        public static bool Prompt { get; set; } = true;
        public static bool ShowUniverse { get; set; } = false;
        const int maxLength = 30;

        public static void DisplayTable(string label, IDataSetResponse response)
        {
            DisplayTable(label, response, 0);
        }

        public static void DisplayTable(string label, IDataSetResponse response, int maxCols, int maxRows = 0)
        {
            Console.WriteLine($"\n{label}");
            if (response.IsSuccess)
            {
                if (FormatTable(response.Data.Table, maxCols, maxRows))
                {
                    if (ShowUniverse)
                        Console.WriteLine($"\nUniverse:\n{string.Join("\n", response.Data.Records.Select(w => DumpUniverse(w)))}");
                }
                else
                    Console.WriteLine($"Response contains an empty data set.");

                if (response.Data.Errors is not null)
                {
                    Console.Write("\nEncounterd errors within the response. Hit <enter> to view...");
                    Console.ReadLine();
                    Console.WriteLine($"\n***Error Details:\n{response.Data.Errors}");
                }
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }

            if (Prompt)
            {
                Console.Write("\nHit <enter> to continue...");
                Console.ReadLine();
            }
        }

        private static string DumpUniverse(IDataSetRecord record)
        {
            return $"Instrument: [{record.Universe.Instrument}] => {record.Data.Count} rows, {record.Fields.Count} fields\n" +
                   $"\tCommon Name: {record.Universe.CommonName}\n" +
                   $"\tPerm ID: {record.Universe.PermID}\n" +
                   $"\tCurrency: {record.Universe.Currency}\n";
        }

        public static void DisplayTable(string label, ISearchResponse response)
        {
            DisplayTable(label, response, 0, 0);
        }

        public static void DisplayTable(string label, ISearchResponse response, int maxCols, int maxRows = 0)
        {
            Console.WriteLine($"\n{label}");

            if (response.IsSuccess)
            {
                Console.WriteLine($"Hits: {response.Data.Total}");

                // Warnings
                if (response.Data.Warnings.Count > 0)
                    Console.WriteLine($"\n{string.Join(",", response.Data.Warnings.Select(warning => $"{warning}"))}");

                if (FormatTable(response.Data.Table, maxCols, maxRows))
                {
                    // Navigators
                    if (response.Data?.Navigators != null)
                        Console.WriteLine($"Navigators:\n{response.Data.Navigators}");
                }
                else
                    Console.WriteLine($"Response contains an empty data set: {response.Data?.Raw}");
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }

            if (Prompt)
            {
                Console.Write("\nHit <enter> to continue...");
                Console.ReadLine();
            }
        }

        public static void DisplayTable(string label, ILookupResponse response)
        {
            DisplayTable(label, response, 0);
        }

        public static void DisplayTable(string label, ILookupResponse response, int maxCols, int maxRows = 0)
        {
            Console.WriteLine($"\n{label}");

            if (response.IsSuccess)
            {
                if (FormatTable(response.Data.Table, maxCols, maxRows))
                {
                    // Warnings
                    if (response.Data.Warnings.Count > 0)
                        Console.WriteLine($"\n{string.Join(",", response.Data.Warnings.Select(warning => $"{warning}"))}");
                }
                else
                    Console.WriteLine($"Response contains an empty data set: {response.Data?.Raw}");
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }

            if (Prompt)
            {
                Console.Write("\nHit <enter> to continue...");
                Console.ReadLine();
            }
        }

        public static void DisplayTable(string label, ISymbolConversionResponse response)
        {
            DisplayTable(label, response, 0);
        }

        public static void DisplayTable(string label, ISymbolConversionResponse response, int maxCols, int maxRows = 0)
        {
            Console.WriteLine($"\n{label}");

            if (response.IsSuccess)
            {
                if (FormatTable(response.Data.Table, maxCols, maxRows))
                {
                    // Warnings
                    if (response.Data.Warnings.Count > 0)
                        Console.WriteLine($"\n{string.Join(",", response.Data.Warnings.Select(warning => $"{warning}"))}");
                }
                else
                    Console.WriteLine($"Response contains an empty data set: {response.Data?.Raw}");
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }

            if (Prompt)
            {
                Console.Write("\nHit <enter> to continue...");
                Console.ReadLine();
            }
        }

        public static void DisplayTable(IMetaDataResponse response)
        {
            DisplayTable(null, response, 0);
        }

        public static void DisplayTable(string label, IMetaDataResponse response)
        {
            DisplayTable(label, response, 0);
        }

        public static void DisplayTable(string label, IMetaDataResponse response, int maxCols, int maxRows = 0)
        {
            if (label != null) Console.WriteLine($"\n{label}");

            if (response.IsSuccess)
            {
                if (!FormatTable(response.Data.Table, maxCols, maxRows))
                    Console.WriteLine($"Response contains an empty data set: {response.Data?.Raw}");
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }

            if (Prompt)
            {
                Console.Write("\nHit <enter> to continue...");
                Console.ReadLine();
            }
        }

        public static void DisplayTable(string label, DataTable table)
        {
            if (label != null) Console.WriteLine($"\n{label}");

            if (!FormatTable(table, 0, 0))
                Console.WriteLine("Response contains an empty data set");

            if (Prompt)
            {
                Console.Write("\nHit <enter> to continue...");
                Console.ReadLine();
            }
        }

        public static void DisplayTable(string label, DataTable table, int maxCols, int maxRows = 0)
        {
            if (label != null) Console.WriteLine($"\n{label}");

            if (!FormatTable(table, maxCols, maxRows))
                Console.WriteLine("Response contains an empty data set");

            if (Prompt)
            {
                Console.Write("\nHit <enter> to continue...");
                Console.ReadLine();
            }
        }

        // FormatTable
        // Generic routine to display dataTable
        private static bool FormatTable(DataTable dataTable, int maxCols, int maxRows)
        {
            if (dataTable is not null && dataTable.Columns.Count > 0)
            {
                var rows = dataTable.Select();
                DisplayRows(dataTable.Columns, rows, maxCols, maxRows);
                if (Prompt)
                    Console.WriteLine($"Total rows in table: {(maxRows > 0 ? maxRows : rows.Length)}.  Total hit count: {rows.Length}");
            }

            return dataTable is not null;
        }

        // DisplayRows
        // Generic routine to display the rows of a table
        public static void DisplayRows(DataColumnCollection columns, IEnumerable<DataRow> rows, int maxCols=0, int maxRows=0)
        {
            CellFormat headerFormat = new() { ForegroundColor = Color.LightSeaGreen };
            var builder = new TableBuilder(headerFormat);

            var displayCols = maxCols <= 0 ? columns.Count : maxCols;
            foreach (DataColumn col in columns)
            {
                builder.AddColumn(col.ColumnName);
                if (--displayCols == 0) break;
            }

            if (maxCols > 0 && maxCols < columns.Count)
                builder.AddColumn($"{columns.Count - maxCols} more");

            var table = builder.Build();
            table.Config = TableConfig.Unicode();

            table.Config.wrapText = true;
            table.Config.textWrapLimit = 40;

            var rowCount = rows.ToArray().Length;
            var displayRows = maxRows <= 0 ? rowCount : maxRows;

            displayCols = maxCols <= 0 ? columns.Count : maxCols;
            IList<object> rowData = new List<object>();
            foreach (DataRow dataRow in rows)
            {
                foreach (object item in dataRow.ItemArray)
                {
                    if (rowData.Count == displayCols)
                    {
                        rowData.Add("...");
                        break;
                    }

                    switch (item)
                    {
                        case null:
                            break;
                        case JValue val:
                            var str = val.ToString();
                            rowData.Add(str);
                            break;
                        case IEnumerable<JToken> list:
                            var listStr = string.Join(", ", list.Select(node => $"{node}"));
                            rowData.Add((listStr?.Length > maxLength) ? $"{listStr[..maxLength]}..." : listStr);
                            break;
                        case double doubleValue:
                        case decimal decimalValue:
                        case float floatValue:
                            rowData.Add($"{item:#0.#####;(#0.#####);0}");
                            break;
                        case DBNull dbNull:
                            rowData.Add("<NA>");
                            break;
                        default:
                            rowData.Add(item);
                            break;
                    }
                }

                table.AddRow(rowData.ToArray());
                rowData.Clear();

                if (--displayRows == 0) break;
            }

            if (maxRows > 0 && maxRows < rowCount)
                table.AddRow($"<{rowCount - maxRows} more>...");

            Console.Write(table);
        }
    }
}
