using LSEG.Data.Content.News;
using LSEG.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using BetterConsoles.Tables.Builders;
using BetterConsoles.Tables.Models;
using System.Drawing;
using BetterConsoles.Tables.Configuration;
using System.Text;

namespace _2._3._08_News_TopNews
{
    // **********************************************************************************************************************
    // 2.3.08-News-TopNews
    // The following example presents the top news categories.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: Desktop
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            // Set console encoding to UTF-8
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                // Create a session into the platform
                using ISession session = Configuration.Sessions.GetSession();

                if (session.Open() == Session.State.Opened)
                {
                    // Top News
                    var top = TopNews.Definition().GetData();

                    if (top.IsSuccess)
                        DisplayNews(top);
                    else
                        Console.WriteLine($"Failed to execute request: {top.HttpHeaders}");
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
