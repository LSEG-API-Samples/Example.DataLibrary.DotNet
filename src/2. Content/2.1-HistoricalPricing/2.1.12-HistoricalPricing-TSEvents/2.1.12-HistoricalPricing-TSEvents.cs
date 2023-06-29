using Common_Examples;
using Configuration;
using Refinitiv.Data.Content.HistoricalPricing;
using Refinitiv.Data.Core;

// **********************************************************************************************************************
// 2.1.12-HistoricalPricing-TSEvents
// The HistoricalPricing TimeSeries (TS) Events example demonstrates daily bars and supports the ability to capture
// realtime INSERTs for each realtime event.
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

    // Create the platform session.
    using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

    // Open the session
    session.Open();

    // Create a Historical Pricing stream retrieve pricing events
    var stream = Events.Definition("VOD.L").Fields("RTL", "EVENT_TYPE", "TRDPRC_1", "TRDVOL_1", "QUALIFIERS")
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