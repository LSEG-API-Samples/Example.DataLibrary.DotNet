using LSEG.Data.Content.News;
using LSEG.Data.Core;
using System;
using Configuration;
using System.Text;

namespace _2._3._04_News_OnlineReports
{
    // **********************************************************************************************************************
    // 2.3.04-News-OnlineReports
    // The following example presents the hierarchy of online reports.
    //
    // Note: The service is only available within RDP. Refer to the .Solutions folder Configuration.Credentials file to
    //       specify the required RDP credentials. Alternatively, users to specify credentials within their own
    //       configuration file.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            // Set console encoding to UTF-8
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                // Create a session into the platform...
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.RDPv1);

                // Open the session
                session.Open();

                Console.WriteLine("Online Report\t\tReport IDs");
                Console.WriteLine("=============\t\t================================================================");

                // Online Reports Listing
                var listing = OnlineReportsListing.Definition().GetData();

                if (listing.IsSuccess)
                {
                    foreach (IOnlineReport report in listing.Data.OnlineReports)
                    {
                        Console.Write($"{report.Name}\t");
                        foreach (var regionReport in report.Reports)
                        {
                            Console.Write($"{regionReport["reportId"]} ");
                        }

                        Console.WriteLine();
                    }

                    // Get Online Report details
                    IOnlineReportsResponse reportStory = OnlineReports.Definition("OLUSSCIENCE").GetData();    // US Science reports
                    if (reportStory.IsSuccess)
                    {
                        foreach (IOnlineReportStory story in reportStory.Data.OnlineReportStories)
                        {
                            Console.WriteLine("----------------------------------------------------------------------------------");
                            Console.WriteLine($"{story.CreationDate}: {story.HeadlineTitle}");
                            Console.WriteLine($"content Type: {story.ContentType}\n{story.NewsStory}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine(listing.HttpStatus);
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
    }
}
