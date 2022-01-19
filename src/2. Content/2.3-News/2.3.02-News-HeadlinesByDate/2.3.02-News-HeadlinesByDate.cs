using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using System;
using System.Linq;

namespace _2._3._02_News_HeadlinesByDate
{
    // **********************************************************************************************************************
    // 2.3.02-News-HeadlinesByDate
    // The News interfaces provide options to query for headlines within a specified time period.  The following example 
    // demonstrates the behavior and how to retrieve headlines based on a time period.
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
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    if (session.Open() == Session.State.Opened)
                    {
                        // ***************************************************************************************************************
                        // Note: Each request below specifies a count of zero (0) which implies all available headlines within the query.
                        // ***************************************************************************************************************

                        // Use date specified within query 1 year ago
                        var last_year = DateTime.UtcNow.AddYears(-1);
                        var dateRange = $"{last_year:yyyy-MM-dd},{last_year.AddDays(6):yyyy-MM-dd}";
                        Console.WriteLine($"\nRetrieve all headlines for query: '{dateRange}'...");
                        DisplayHeadlines(Headlines.Definition().Query($"Apple daterange:{dateRange}")
                                                               .Count(0)
                                                               .Sort(Headlines.SortOrder.oldToNew)
                                                               .GetData());

                        // Use date specifier within query - last 5 days
                        Console.WriteLine("Retrieve all headlines for query: 'Apple last 5 days'...");
                        DisplayHeadlines(Headlines.Definition().Query("Apple last 5 days")
                                                               .Count(0)
                                                               .Sort(Headlines.SortOrder.oldToNew)
                                                               .GetData());

                        // Same as previous except show each page response from the platform
                        Console.WriteLine("Same as previous except show each page response...");
                        DisplayHeadlines(Headlines.Definition().Query("Apple last 5 days")
                                                               .Count(0)
                                                               .Sort(Headlines.SortOrder.oldToNew)
                                                               .GetData((response, def, s) => Console.Write($"{response.Data.Headlines.Count}, ")));
                    }
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
                Console.WriteLine($"Retrieved a total of {headlines.Data.Headlines.Count} headlines.  Small sample:");
                foreach (var headline in headlines.Data.Headlines.Take(5))
                    Console.WriteLine($"\t{headline.CreationDate}\t{headline.HeadlineTitle}");
            }
            else
                Console.WriteLine($"Issue retrieving headlines: {headlines.HttpStatus}");

            Console.WriteLine("\n************************************************************************\n");
        }
    }
}
