using Common_Examples;
using Refinitiv.Data.Content.HistoricalPricing;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._1._03_HistoricalPricing_TimeRange
{
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

                // Daily summaries for the last 30 days. Start and end dates must be UTC formatted
                // Note: The days reported only include trading days.
                var last_30_days = DateTime.UtcNow.AddDays(-30);
                Console.WriteLine($"Date: {last_30_days}");
                var response = Summaries.Definition("VOD.L").Interval(Summaries.Interval.P1D)
                                                            .Fields("TRDPRC_1", "LOW_1", "HIGH_1")
                                                            .Start(last_30_days)
                                                            .End(DateTime.UtcNow)
                                                            .GetData();
                Common.DisplayTable("Historical daily Summaries - last 30 trading days", response);

                // Monthly summaries for last calendar year.
                var lastYear = DateTime.Now.Year - 1;
                var start = new DateTime(lastYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var end = new DateTime(lastYear, 12, 31, 0, 0, 0, DateTimeKind.Utc);
                response = Summaries.Definition("VOD.L").Interval(Summaries.Interval.P1M)
                                                        .Fields("TRDPRC_1", "LOW_1", "HIGH_1")
                                                        .Start(start)
                                                        .End(end)
                                                        .GetData();
                Common.DisplayTable("Historical monthly Summaries - last calendar year", response);
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
