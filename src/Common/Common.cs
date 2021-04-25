using ConsoleTables;
using Refinitiv.Data.Content.Data;
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
    public class Common
    {
        // **************************************************************************************************************************************
        // DisplayTable
        //
        // Convenience routine to layout columns and rows of data contained within the response.  Data is echoed to the console.
        // **************************************************************************************************************************************
        public static void DisplayTable(IDataSetResponse response, string header=default)
        {
            Console.WriteLine("\n******************************************************************************************************************");
            if (response.IsSuccess)
            {
                if (header != default) Console.Write($"{Environment.NewLine}{header}");
                if (response.Data.Universe != null)
                    Console.Write($" for item(s):\n{string.Join("\n", response.Data.Universe.Select(w => $"\tItem: {w.Instrument} {w.CommonName}"))}");

                Console.WriteLine();

                if (response.Data?.Table != null && response.Data?.Table.Rows.Count > 0)
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
                else
                {
                    Console.WriteLine($"Response contains an empty data set: {response.Data?.Raw}");
                }

                Console.WriteLine($"Fields:\n{string.Join("\n", response.Data.Fields?.Select(f => $"\t{f.Name} ({f.Type})"))}");
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }
        }
    }
}
