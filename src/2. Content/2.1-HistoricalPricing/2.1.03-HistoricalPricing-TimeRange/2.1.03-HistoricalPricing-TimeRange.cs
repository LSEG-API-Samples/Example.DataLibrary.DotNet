using System;
using Common_Examples;
using Configuration;
using LSEG.Data.Content.HistoricalPricing;
using LSEG.Data.Core;

namespace _2._1._03_HistoricalPricing_TimeRange
{
    // **********************************************************************************************************************
    // 2.1.03-HistoricalPricing-TimeRange
    // The HistoricalPricing TimeRange example demonstrates how to retrieve historical content based on time ranges.
    // The example uses a common method to display the table of data returned.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: Desktop.
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

                // Daily summaries ~30 days ago. Start and end dates must be UTC formatted
                // Note: The days reported only include trading days.
                var thirty_days_ago = DateTime.UtcNow.Date.AddDays(-30);

                // Ensure we fall during the week
				thirty_days_ago = thirty_days_ago.DayOfWeek switch
				{
					DayOfWeek.Saturday => thirty_days_ago.AddDays(-1),
					DayOfWeek.Sunday => thirty_days_ago.AddDays(-2),
					_ => thirty_days_ago
				};

				Console.WriteLine($"~30 days ago: {thirty_days_ago:D}");

                var response = Summaries.Definition("VOD.L").Interval(Summaries.Interval.P1D)
                                                            .Fields("TRDPRC_1", "LOW_1", "HIGH_1")
                                                            .Start(thirty_days_ago)
                                                            .End(DateTime.UtcNow)
                                                            .GetData();
                Common.DisplayTable("Daily Summaries - last ~30 trading days", response);

                // Monthly summaries for last calendar year.
                var lastYear = DateTime.Now.Year - 1;
                var start = new DateTime(lastYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var end = new DateTime(lastYear, 12, 31, 0, 0, 0, DateTimeKind.Utc);
                response = Summaries.Definition("VOD.L").Interval(Summaries.Interval.P1M)
                                                        .Fields("TRDPRC_1", "LOW_1", "HIGH_1")
                                                        .Start(start)
                                                        .End(end)
                                                        .GetData();
                Common.DisplayTable($"Monthly Summaries - last calendar year - start: {start} end: {end}", response);

                // Using a more natural way to create interday dates
                var sd = new DateTime(lastYear, 1, 1);
                var ed = new DateTime(lastYear, 12, 31);
                response = Summaries.Definition("VOD.L").Interval(Summaries.Interval.P1M)
                                                        .Fields("TRDPRC_1", "LOW_1", "HIGH_1", "OPEN_PRC")
                                                        .Start(start)
                                                        .End(end)
                                                        .GetData();
				Common.DisplayTable($"Monthly Summaries - last calendar year - start: {sd:d} end: {ed:d}", response); ;

				// Look at hourly bars for a full trading day - ~30 days ago			
				response = Summaries.Definition("AAPL.O").Interval(Summaries.Interval.PT1H)
                                                         .Sessions(HistoricalPricing.Sessions.normal)
                                                         .Fields("OPEN_PRC", "HIGH_1", "LOW_1", "TRDPRC_1", "ACVOL_UNS")
                                                         .Start(thirty_days_ago)
                                                         .End(thirty_days_ago)
                                                         .GetData();
                Common.DisplayTable("Hourly bars - ~30 days ago", response);
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
