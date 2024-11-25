using Common_Examples;
using Configuration;
using LSEG.Data.Content.HistoricalPricing;
using LSEG.Data.Core;

// **********************************************************************************************************************
// 2.1.11-HistoricalPricing-TSIntraday
// The HistoricalPricing TimeSeries (TS) Intraday example demonstrates minute bars and supports the ability to capture
// realtime INSERTs (new minute bars) and UPDATEs to the latest bar in realtime.
//
// The example uses a common method to display the table of updates and inserts.
//
// Note: Streaming timeseries bars are presently supported for Desktop sessions only.
// **********************************************************************************************************************
try
{
    Common.Prompt = false;

    // Create the platform session.
    using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

    // Open the session
    session.Open();

    // Create a Historical Pricing stream - specifying the desired 'intraday' interval
    var stream = Summaries.Definition("VOD.L").Fields("BID", "ASK", "HIGH_1", "LOW_1", "TRDPRC_1", "ACVOL_UNS", "NUM_MOVES")
                                              .Interval(Summaries.Interval.PT1M)
                                              .Sessions(HistoricalPricing.Sessions.normal)
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

