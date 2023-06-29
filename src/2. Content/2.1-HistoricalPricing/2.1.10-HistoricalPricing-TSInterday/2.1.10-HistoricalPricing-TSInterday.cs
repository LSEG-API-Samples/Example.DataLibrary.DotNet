using Common_Examples;
using Configuration;
using Refinitiv.Data.Content.HistoricalPricing;
using Refinitiv.Data.Core;

// **********************************************************************************************************************
// 2.1.10-HistoricalPricing-TSInterday
// The HistoricalPricing TimeSeries (TS) Interday example demonstrates daily bars and supports the ability to capture
// realtime INSERTs (new interday bars) and UPDATEs to the latest bar in realtime.
//
// The example uses a common method to display the table of updates and inserts.
//
// Note: To configure settings for your environment, visit the following files within the .Solutions folder.
//       The TS capabilities are presently supported for Desktop sessions only.
//      1. Ensure the Configuration.Session defines the 'Desktop' access channel.
//      2. Configuration.Credentials to define your login credentials for the specified access channel.
// **********************************************************************************************************************
try
{
    Common.Prompt = false;

    // Create the desktop session.
    using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

    // Open the session
    session.Open();

    // Create a Historical Pricing stream - specifying the desired 'interday' interval
    var stream = Summaries.Definition("VOD.L").Fields("BID", "ASK", "HIGH_1", "LOW_1", "OPEN_PRC", "TRDPRC_1", "ACVOL_UNS")
                                              .Interval(Summaries.Interval.P1D)
                                              .Count(5)
                                              .GetStream();

    // Specify the TSI lambda expressions to capture 'Insert' and 'Update' events
    stream.OnInsert((data, stream) => Common.DisplayTable($"INSERT: {DateTime.Now}", data.Table))
          .OnUpdate((data, stream) => Common.DisplayTable($"UPDATE: {DateTime.Now}", data.Table))
          .OnStatus((status, stream) => Console.WriteLine($"Status: {status}"))
          .OnError((error, stream) => Console.WriteLine($"Error: {error}"))
          .Open();

    Console.ReadLine();
}
catch (Exception e)
{
    Console.WriteLine($"\n**************\nFailed to execute.");
    Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
    if (e.InnerException is not null) Console.WriteLine(e.InnerException);
    Console.WriteLine("***************");
}
