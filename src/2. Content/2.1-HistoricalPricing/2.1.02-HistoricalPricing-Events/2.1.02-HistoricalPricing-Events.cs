using Common_Examples;
using Refinitiv.Data.Content.HistoricalPricing;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._1._02_HistoricalPricing_Events
{
    // **********************************************************************************************************************
    // 2.1.02-HistoricalPricing-Events
    // The HistoricalPricing Events example demonstrates how to capture trade tick-based data.
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

                // Retrieve tick pricing events.  Default: 20 rows of data.  Specified trades only and specific columns of data.
                var response = Events.Definition("VOD.L").EventTypes(Events.EventType.trade)
                                                         .Fields("DATE_TIME", "EVENT_TYPE", "TRDPRC_1", "TRDVOL_1")
                                                         .GetData();
                Common.DisplayTable("Historical Trade events", response);

                // Retrieve tick events for a group of instruments..
                response = Events.Definition().Universe("VOD.L", "MSFT.O", "EUR=")
                                              .Fields("DATE_TIME", "TRDPRC_1", "MID_PRICE", "CTBTR_1", "BID", "ASK")
                                              .GetData();
                Common.DisplayTable("Historical events for multiple instruments", response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
