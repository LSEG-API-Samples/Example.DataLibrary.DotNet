using ConsoleTables;
using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2._3._08_News_TopNews
{
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session into the platform
                using ISession session = Configuration.Sessions.GetSession();

                if (session.Open() == Session.State.Opened)
                {
                    // Top News
                    var top = TopNews.Definition().GetData();

                    DisplayCategories(top);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void DisplayCategories(ITopNewsResponse response)
        {
            if (response.IsSuccess)
            {
                IList<string> columns = new List<string>() { "Name", "Top News ID", "Revision ID", "Revision Date" };

                foreach (var category in response.Data.Categories)
                {
                    Console.WriteLine($"Category: {category.Key}");

                    var console = new ConsoleTable(columns.ToArray());

                    IList<object> rowData = new List<object>();
                    foreach (var page in category.Value)
                    {
                        rowData.Clear();
                        rowData.Add(page.Page);
                        rowData.Add(page.TopNewsID);
                        rowData.Add(page.RevisionID);
                        rowData.Add(page.RevisionDate);

                        console.AddRow(rowData.ToArray());
                    }

                    console.Write(Format.MarkDown);
                }
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }
        }
    }
}
