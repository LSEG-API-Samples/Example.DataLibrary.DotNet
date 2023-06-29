using Common_Examples;
using Configuration;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using System;
using System.Collections.Generic;

namespace _2._6._05_SearchLight_Lookup
{
    // **********************************************************************************************************************
    // 2.6.05-SearchService-Lookup
    // The following example demonstrates some basic capabilities of the lookup facility in search converting symbols.
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
                // Note: The Search Light service is available to Wealth clients.
                using ISession session = Sessions.GetSession();

                if (session.Open() == Session.State.Opened)
                {
                    // RICs - convert RICs with default output
                    var lookup = Lookup.Definition().Terms("VOD.L,AAPL.O,IBM,MSFT.O")
                                                    .Scope("RIC")
                                                    .Select("DocumentTitle, IssueISIN")
                                                    .GetData();
                    Common.DisplayTable("Lookup for 4 valid RICs using default output", lookup);

                    // RICs - convert to all known symbol types using targeted output
                    lookup = Lookup.Definition().Terms("VOD.L,AAPL.O,IBM,MSFT.O")
                                                .Scope("RIC")
                                                .Select("DocumentTitle,CUSIP,SEDOL,RIC,TickerSymbol,IssueISIN,IssuerOAPermID,FundClassLipperID")
                                                .GetData();
                    Common.DisplayTable("Lookup for 4 valid RICs using targeted output", lookup);

                    // CUSIPs - convert to all known symbol types using targeted output
                    lookup = Lookup.Definition().Terms("037833100,459200101,594918104")
                                                .Scope("CUSIP")
                                                .Select("DocumentTitle,SEDOL,RIC,TickerSymbol,IssueISIN,IssuerOAPermID,FundClassLipperID")
                                                .GetData();
                    Common.DisplayTable("Lookup for 3 valid CUSIPs using targeted output", lookup);

                    DisplayMatches("\nAlternate output...", lookup);

                    // Invalid request - Scope must be defined
                    lookup = Lookup.Definition().Terms("037833100,459200101,594918104")
                                                .Select("DocumentTitle,SEDOL,RIC,TickerSymbol,IssueISIN,IssuerOAPermID,FundClassLipperID")
                                                .GetData();
                    Common.DisplayTable("\n*****Invlid request - scope must be defined", lookup);
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

        private static void DisplayMatches(string label, ILookupResponse response)
        {
            Console.WriteLine($"\n{label}\n");
            if (response.IsSuccess)
            {
                // Matches
                foreach (KeyValuePair<string, JObject> match in response.Data.Matches)
                {
                    Console.WriteLine($"{match.Key} - {match.Value}");
                }

                // Warnings
                foreach (string warning in response.Data.Warnings)
                {
                    Console.WriteLine($"WARNING: {warning}");
                }
            }
            else
            {
                Console.WriteLine(response.HttpStatus);
            }

            Console.Write("\nHit any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
