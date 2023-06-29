using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using BetterConsoles.Tables.Builders;
using BetterConsoles.Tables.Models;
using System.Drawing;
using BetterConsoles.Tables.Configuration;

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

                    DisplayNews(top);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute.");
                Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
                if (e.InnerException is not null) Console.WriteLine(e.InnerException);
                Console.WriteLine("***************");
            }
        }

        private static void DisplayNews(ITopNewsResponse response)
        {
            var headerFormat = new CellFormat() { ForegroundColor = Color.LightSeaGreen };
            IList<string> columns = new List<string>() { "Name", "Top News ID", "Revision ID", "Revision Date" };

            foreach (var category in response.Data.Categories)
            {
                Console.WriteLine($"Category: {category.Key}");

                var builder = new TableBuilder(headerFormat);
                foreach (var name in columns)
                    builder.AddColumn(name);

                var table = builder.Build();
                table.Config = TableConfig.Unicode();

                table.Config.wrapText = true;
                table.Config.textWrapLimit = 40;

                IList<object> rowData = new List<object>();
                foreach (var page in category.Value)
                {
                    rowData.Clear();
                    rowData.Add(page.Page);
                    rowData.Add(page.TopNewsID);
                    rowData.Add(page.RevisionID);
                    rowData.Add(page.RevisionDate);

                    table.AddRow(rowData.ToArray());
                }

                Console.Write(table);
            }
        }
    }
}
