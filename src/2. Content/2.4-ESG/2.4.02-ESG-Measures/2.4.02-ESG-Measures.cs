using Refinitiv.Data.Content.ESG;
using Refinitiv.Data.Core;
using System;
using Configuration;
using Common_Examples;

namespace _2._4._02_ESG_Measures
{
    // **********************************************************************************************************************
    // 2.4.02-ESG-Measures
    // The following example retrieves the environment measures for the specified list of companies.
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
                using (ISession session = Sessions.GetSession())
                {
                    // Open the session
                    if (session.Open() == Session.State.Opened)
                    {
                        // Show ESG measure scores with 2-year history
                        Console.WriteLine("\nESG Measures Full based on company RICs...");
                        var response = Measures.Definition("IBM.N", "MSFT.O").Start(-1)
                                                                             .End(0)
                                                                             .GetData();
                        Common.DisplayTable("Measure Scores with 2-year history", response, 10);

                        // Show ESG measure scores with 1-year history, based on a Perm ID
                        Console.WriteLine("\nESG Measures Standard based on company Perm IDs...");
                        response = Measures.Definition("4295904307", "8589934326").Start(0)
                                                                                  .End(0)
                                                                                  .ServiceType(ServiceType.standard)
                                                                                  .GetData();
                        Common.DisplayTable("ESG Measures 1-year history", response, 10);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
