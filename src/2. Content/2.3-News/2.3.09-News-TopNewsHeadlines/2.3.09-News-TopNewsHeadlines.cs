using LSEG.Data.Content.News;
using LSEG.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using BetterConsoles.Tables.Models;
using System.Drawing;
using BetterConsoles.Tables.Builders;
using BetterConsoles.Tables.Configuration;
using System.Text;

namespace _2._3._09_News_TopNewsHeadlines
{
    // **********************************************************************************************************************
    // 2.3.09-News-TopNewsHeadlines
    // The following example presents the top news headlines.
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
                using ISession session = Sessions.GetSession();

                if (session.Open() == Session.State.Opened)
                {
                    // Top News
                    var top = TopNews.Definition().GetData();
                    if (top.IsSuccess)
                    {
                        if (top.Data.Categories.Count > 0)
                        {
                            // Retrieve a headlines ID from one of the categories
                            var newsId = top.Data.Categories["Main"]?[0].TopNewsID;

                            Console.WriteLine($"Retrieving top news headlines based on the news ID: {newsId}");
                            var headlines = TopNewsHeadlines.Definition(newsId).GetData();

                            DisplayNews(headlines);
                        }
                        else
                            Console.WriteLine("No headlines found");
                    }
                    else
                        Console.WriteLine($"Unable to retrieve Top news: {top.HttpStatus}");
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

        private static void DisplayNews(ITopNewsHeadlinesResponse response)
        {
            var headerFormat = new CellFormat() { ForegroundColor = Color.LightSeaGreen };

            var builder = new TableBuilder(headerFormat);
            foreach (var name in new List<string>() { "Publish Date", "Headline", "Story ID" })
                builder.AddColumn(name);

            var table = builder.Build();
            table.Config = TableConfig.Unicode();

            table.Config.wrapText = true;
            table.Config.textWrapLimit = 40;

            IList<object> rowData = new List<object>();
            foreach (var headline in response.Data.Headlines)
            {
                rowData.Clear();
                rowData.Add(headline.VersionCreated);
                rowData.Add(headline.Text);
                rowData.Add(headline.StoryId);

                table.AddRow(rowData.ToArray());
            }

            Console.Write(table);
        }
    }
}
