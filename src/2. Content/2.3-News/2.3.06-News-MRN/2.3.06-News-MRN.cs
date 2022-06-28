using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using System;
using Configuration;
using Refinitiv.Data;

namespace _2._3._06_News_MRN
{
    // **********************************************************************************************************************
    // 2.3.06-News-MRN
    // The following example demonstrates how to subscribe to the Machine Readable News (MRN) service to retrieve any of the
    // available datafeeds (Headlines/Stories, Analytics Assets or Analytics events) within the MRN service.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session into the platform...
                using ISession session = Sessions.GetSession();

                // Open the session
                session.Open();

                // Create an MRN definition
                var mrn = MachineReadableNews.Definition();

                // Choose the type of data feed from the MRN service
                string input;
                do
                {
                    Console.Write("\nChoose an MRN Data Feed (0 - Story, 1 - Analytics Assets, 2 - Analytics Events) [Enter to Exit]: ");
                    input = Console.ReadLine();

                    if (input.Length > 0)
                    {
                        try
                        {
                            // Validate the selection
                            var feed = (MachineReadableNews.Datafeed)Enum.Parse(typeof(MachineReadableNews.Datafeed), input);

                            if (Enum.IsDefined(typeof(MachineReadableNews.Datafeed), feed))
                            {
                                // Set the datafeed then retrieve a streaming object.
                                using var stream = mrn.NewsDatafeed(feed).GetStream();

                                // Define our real-time processing then open the stream...
                                stream.OnError((err, s) => Console.WriteLine($"{DateTime.Now}:{err}"))
                                      .OnStatus((status, s) => Console.WriteLine($"Status for feed: {s.Definition.DataFeed} => {status}"))
                                      .OnNewsStory((newsItem, s) => OnNewsStory(newsItem))
                                      .OnNewsAnalyticsAssets((newsItem, s) => OnNewsAnalyticsAssets(newsItem))
                                      .OnNewsAnalyticsEvents((newsItem, s) => OnNewsAnalyticsEvents(newsItem))
                                      .Open();

                                Console.ReadLine();
                            }
                        }
                        catch (ArgumentException) { }
                    }
                } while (input.Length > 0);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void OnNewsStory(IMRNStoryData news)
        {
            Console.WriteLine($"{ DateTime.Now:HH:mm:ss}. Story Body {news.NewsStory.Length} bytes ({news.HeadlineTitle})");
        }

        private static void OnNewsAnalyticsAssets(IMRNAnalyticsData news)
        {
            Console.WriteLine($"{ DateTime.Now:HH:mm:ss}. Scores for {news.Scores.Count} asset(s). ({news.HeadlineTitle})");
            foreach (var score in news.Scores)
            {
                Console.WriteLine($"\t{score.AssetName} ({score.AssetClass}");
                Console.WriteLine($"\t\tRelevance: {score.Relevance}\tSentiment (-:0:+) {score.SentimentNegative}:{score.SentimentNeutral}:{score.SentimentPositive}");
            }
        }

        private static void OnNewsAnalyticsEvents(IMRNAnalyticsData news)
        {
            Console.WriteLine($"{ DateTime.Now:HH:mm:ss}. Scores for {news.Scores.Count} asset(s). ({news.HeadlineTitle})");
        }
    }
}
