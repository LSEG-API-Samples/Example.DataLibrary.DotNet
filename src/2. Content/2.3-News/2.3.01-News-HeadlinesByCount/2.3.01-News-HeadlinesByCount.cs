using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using System;
using System.Linq;
using Configuration;

namespace _2._3._01_News_HeadlinesByCount
{
    // **********************************************************************************************************************
    // 2.3.01-News-HeadlinesByCount
    // The following example demonstrates basic usage of the News interface to retrieve headlines.  Users can specify a
    // specific query which will filter out news headlines based on criteria.  Optionally, they can specify the total maximum
    // number of headlines to be returned.
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
                if (session.Open() == Session.State.Opened)
                {

                    // Default Count: Retrieve the most recent 100 headlines
                    DisplayHeadlines(Headlines.Definition().GetData());

                    // Default Count: Retrieve most recent 100 headlines for Apple
                    DisplayHeadlines(Headlines.Definition().Query("R:AAPL.O").GetData());

                    // Specify Count: Retrieve most recent N headlines for Apple
                    DisplayHeadlines(Headlines.Definition().Query("R:AAPL.O")
                                                           .Count(15)
                                                           .GetData());

                    // Specify Count: Retrieve large batch for the most recent N headlines for Apple
                    DisplayHeadlines(Headlines.Definition().Query("R:AAPL.O")
                                                           .Count(350)
                                                           .GetData());

                    // Same as last one except provide a callback to report each page of headlines returned
                    DisplayHeadlines(Headlines.Definition().Query("R:AAPL.O")
                                                           .Count(350)
                                                           .GetData((response, def, h) => DisplayHeadlines(response)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        static void DisplayHeadlines(IHeadlinesResponse headlines)
        {
            if (headlines.IsSuccess)
            {
                Console.WriteLine($"\nRetrieved a total of {headlines.Data.Headlines.Count} headlines.  Small sample:");
                foreach (var headline in headlines.Data.Headlines.Take(5))
                    Console.WriteLine($"\t{headline.CreationDate}\t{headline.HeadlineTitle}");
            }
            else
                Console.WriteLine($"Issue retrieving headlines: {headlines.HttpStatus}");
        }
    }
}
