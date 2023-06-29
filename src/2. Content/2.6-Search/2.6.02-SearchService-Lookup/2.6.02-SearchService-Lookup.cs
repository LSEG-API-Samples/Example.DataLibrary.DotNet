using Common_Examples;
using Configuration;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using System;
using System.Collections.Generic;

namespace _2._6._02_Search_Lookup
{
    // **********************************************************************************************************************
    // 2.6.02-SearchService-Lookup
    // The following example demonstrates some basic capabilities of the lookup facility in search converting symbols.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Credentials to define your login credentials for the desktop.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Note: The service is only available on the desktop.
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

                if (session.Open() == Session.State.Opened)
                {
                    // RICs - convert to all specified symbol types
                    var lookup = Lookup.Definition().View(Search.View.SearchAll)
                                                    .Terms("VOD.L,IBM,AAPL.O,MSFT.O")
                                                    .Scope("RIC")
                                                    .Select("DocumentTitle, CUSIP, SEDOL, TickerSymbol, IssueISIN, IssuerOAPermID")
                                                    .GetData();
                    Common.DisplayTable("\nRIC Lookup for 5 valid items using selected output", lookup);

                    // RICs - detect input and convert to all known symbol types using default output
                    lookup = Lookup.Definition().View(Search.View.SearchAll)
                                                .Terms("VOD.L,037833100,IBM,US5949181045,503085358")
                                                .GetData();
                    Common.DisplayTable("\nRIC Lookup for 5 valid items using default output", lookup);
                    DisplayMatches("\nRIC Lookup for 5 valid items:", lookup);
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
