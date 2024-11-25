using LSEG.Data.Content.ESG;
using LSEG.Data.Core;
using System;
using Configuration;
using Common_Examples;

namespace _2._4._02_ESG_Measures
{
    // **********************************************************************************************************************
    // 2.4.02-ESG-Measures
    // The following example retrieves the environment measures for the specified list of companies.
    //
    // Note: The service is only available within RDP. Refer to the .Solutions folder Configuration.Credentials file to
    //       specify the required RDP credentials. Alternatively, users to specify credentials within their own
    //       configuration file.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session into the platform...
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.RDPv1);

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
