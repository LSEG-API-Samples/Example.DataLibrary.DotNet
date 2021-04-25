using ConsoleTables;
using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2._3._09_News_TopNewsHeadlines
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
                    if (top.IsSuccess)
                    {
                        // Retrieve a headlines ID from one of the categories
                        var newsId = top.Data.Categories["Main"]?[0].TopNewsID;

                        var headlines = TopNewsHeadlines.Definition(newsId).GetData();
                        DisplayHeadlines(headlines);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void DisplayHeadlines(ITopNewsHeadlinesResponse response)
        {
            if (response.IsSuccess)
            {
                IList<string> columns = new List<string>() { "Publish Date", "Headline", "Story ID" };
                var console = new ConsoleTable(columns.ToArray());
                IList<object> rowData = new List<object>();

                foreach (var headline in response.Data.Headlines)
                {
                    rowData.Clear();
                    rowData.Add(headline.VersionCreated);
                    rowData.Add(headline.Text);
                    rowData.Add(headline.StoryId);

                    console.AddRow(rowData.ToArray());
                }
                console.Write(Format.MarkDown);
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }
        }
    }
}
