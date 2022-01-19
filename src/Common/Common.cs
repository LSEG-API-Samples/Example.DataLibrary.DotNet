using ConsoleTables;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.Data;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Content.Symbology;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

// **********************************************************************************************************************
// Common
// Convenient methods used across the projects defined within the Content layer.
// **********************************************************************************************************************
namespace Common_Examples
{
    public static class Common
    {
        // **************************************************************************************************************************************
        // DisplayDataSet
        //
        // Convenience routine to layout columns and rows of data contained within the response.  Data is echoed to the console.
        // **************************************************************************************************************************************
        public static void DisplayDataSet(IDataSetResponse response, string header = default)
        {
            Console.WriteLine("\n******************************************************************************************************************");
            if (response.IsSuccess)
            {
                if (header != default) Console.Write($"{Environment.NewLine}{header}");
                if (response.Data?.Records != null)
                    Console.Write($"\nUniverse:{string.Join(",", response.Data.Records.Select(w => DisplayUniverse(w)))}");

                Console.WriteLine();

                // The Table propery below represents the data grid returned from the platform.
                // The Table is a .Net DataTable containing our rows and columns of data representing our time series grid. 
                // Process the rows and columns and utilize the ConsoleTable class to display to the console.
                if (response.Data?.Table != null && response.Data?.Table.Rows.Count > 0)
                    DisplayTable(response.Data.Table);
                else
                    Console.WriteLine($"Response contains an empty data set: {response.Data?.Raw}");
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }
            Console.Write("\n<Enter> to continue..."); Console.ReadLine();
        }

        static string DisplayUniverse(IDataSetRecord record)
        {
            return $"\nItem: {record.Universe.Instrument} => {record.Data.Count} rows, {record.Fields.Count} fields";
        }

        public static void DisplaySymbology(ISymbolConversionResponse response, string header = default)
        {
            Console.WriteLine("\n******************************************************************************************************************");
            if (response.IsSuccess)
            {
                if (header != default) Console.Write($"{Environment.NewLine}{header}");

                // Warnings
                foreach (string warning in response.Data.Warnings)
                {
                    Console.WriteLine($"**** {warning}");
                }

                Console.WriteLine();

                // The Table propery below represents the data grid returned from the platform.
                // The Table is a .Net DataTable containing our rows and columns of data representing our conversions. 
                // Process the rows and columns and utilize the ConsoleTable class to display to the console.
                if (response.Data?.Table != null && response.Data?.Table.Rows.Count > 0)
                    DisplayTable(response.Data.Table);
                else
                    Console.WriteLine($"Response contains an empty data set: {response.Data?.Raw}");
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }
            Console.Write("\n<Enter> to continue..."); Console.ReadLine();
        }

        // **************************************************************************************************************************************
        // DisplayTable
        //
        // Convenience routine to layout columns and rows of data contained within the response.  Data is echoed to the console.
        // **************************************************************************************************************************************
        public static void DisplaySearch(ISearchResponse response, string header=default)
        {
            Console.WriteLine("\n******************************************************************************************************************");
            if (header != default) Console.Write($"{Environment.NewLine}{header}");
            if (response.IsSuccess)
            {
                // Warnings
                foreach (string warning in response.Data.Warnings)
                {
                    Console.WriteLine($"**** {warning}");
                }

                // Total docs in matching set
                Console.WriteLine($"\nTotal document hits: {response.Data.Total}.");
                Console.WriteLine($"Received: {response.Data.Hits.Count}\n");

                // The Table propery below represents the data grid returned from the platform.
                // The Table is a .Net DataTable containing our rows and columns of data representing our document hits. 
                // Process the rows and columns and utilize the ConsoleTable class to display to the console.
                if (response.Data?.Table != null && response.Data?.Table.Rows.Count > 0)
                    DisplayTable(response.Data.Table);
                else
                    Console.WriteLine($"Response contains an empty data set: {response.Data?.Raw}");
            }
            else
            {
                Console.WriteLine($"\nIsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }
            Console.Write("\n<Enter> to continue..."); Console.ReadLine();
        }

        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        private static void DisplayTable(DataTable table)
        {
            // The Table propery below represents the data grid returned from the platform.
            // The Table is a .Net DataTable containing our rows and columns of data representing our time series grid. 
            // Process the rows and columns and utilize the ConsoleTable class to display to the console.
            var console = new ConsoleTable();

            IList<string> columns = new List<string>();
            foreach (DataColumn col in table.Columns)
                columns.Add(col.ColumnName);

            console.AddColumn(columns);
            
            IList<object> rowData = new List<object>();
            foreach (DataRow dataRow in table.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    switch (item)
                    {
                        case null:
                            break;
                        case JValue val:
                            rowData.Add(val.ToString());
                            break;
                        case IEnumerable<JToken> list:
                            rowData.Add($"[{string.Join(", ", list.Select(node => $"{node}"))}]");
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
                Console.WriteLine();
                console.Write(Format.MarkDown);
            }
        }
    }
}
