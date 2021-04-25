using Common_Examples;
using Refinitiv.Data.Content.ESG;
using Refinitiv.Data.Core;
using System;

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
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    if (session.Open() == Session.State.Opened)
                    {

                        // Show ESG measure scores with 2-year history
                        Console.WriteLine("\nESG Measures Full based on company RICs...");
                        Common.DisplayTable(Measures.Definition("IBM.N", "MSFT.O").Start(-1)
                                                                                  .End(0)
                                                                                  .GetData(), "ESG Measures 2-year history");

                        // Show ESG measure scores with 1-year history, based on a Perm ID
                        Console.WriteLine("\nESG Measures Standard based on company Perm IDs...");
                        Common.DisplayTable(Measures.Definition("4295904307", "8589934326").Start(0)
                                                                                           .End(0)
                                                                                           .GetData(), "ESG Measures 1-year history");
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
