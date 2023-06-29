using Common_Examples;
using Refinitiv.Data.Content.HistoricalPricing;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._1._01_HistoricalPricing_Summaries
{
    // **********************************************************************************************************************
    // 2.1.01-HistoricalPricing-Summaries
    // The HistoricalPricing Summaries example demonstrates both intraday and interday data retrievals from the platform.
    // The example uses a common method to display the table of data returned.
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
                // Create the platform session.
                using ISession session = Sessions.GetSession();

                // Open the session
                session.Open();

                // Retrieve Intraday Summaries with PT1M (5-minute interval).  Specify to capture only 2 rows.
                var response = Summaries.Definition("VOD.L").Interval(Summaries.Interval.PT5M)
                                                            .Fields("DATE_TIME", "OPEN_BID", "OPEN_ASK", "BID", "ASK",
                                                                    "BID_LOW_1", "ASK_LOW_1", "BID_HIGH_1", "ASK_HIGH_1")
                                                            .Count(2)
                                                            .GetData();
                Common.DisplayTable("Historical Intraday Summaries", response);

                // Retrieve Interday Summaries with P1D (1-day interval).  
                response = Summaries.Definition("VOD.L").Interval(Summaries.Interval.P1D)
                                                        .Fields("DATE", "TRDPRC_1", "MKT_OPEN", "VWAP", "LOW_1", "HIGH_1")
                                                        .GetData();
                Common.DisplayTable("Historical Interday Summaries", response);

                // Retrieve interday summaries for a list of instruments
                response = Summaries.Definition().Universe("IBM.N", "AAPL.O", "TSLA.O")
                                                 .GetData();
                Common.DisplayTable("Historical Interday Summaries for a list of instruments", response);
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
