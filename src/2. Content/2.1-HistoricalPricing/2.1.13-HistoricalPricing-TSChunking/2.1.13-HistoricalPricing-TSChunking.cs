using Common_Examples;
using Configuration;
using LSEG.Data.Content.HistoricalPricing;
using LSEG.Data.Core;

namespace _2._1._13_HistoricalPricing_TSChunking
{
	// **********************************************************************************************************************
	// 2.1.13-HistoricalPricing-TSChunking
	// The HistoricalPricing TimeSeries(TS) Intraday Chunking example demonstrates hourly bars going back about 1 year,
	// presenting realtime INSERTs and UPDATEs.
	//
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
				Common.Prompt = false;

				// Create a desktop session.
				using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

				// Open the session
				session.Open();

				// Create a definition to retrieve hourly bars going back ~1 year - normal trading hours
				// Note: BlendRealtimeInSnapshot ensures the latest reported INSERT bar has up-to-date values
				var stream = Summaries.Definition("AMZN.O").Interval(Summaries.Interval.PT1H)
														   .Fields("TRDPRC_1", "LOW_1", "HIGH_1")
														   .Sessions(HistoricalPricing.Sessions.normal)
														   .Count(2000)		   // This goes back about 1-year
														   .UseChunking(true)  // Back-end will deliver data in chunks
														   .GetStream().BlendRealtimeInSnapshot(true);

				// Specify the TSI lambda expressions to capture 'Insert' and 'Update' events
				// Note: The initial INSERT containg historical data will combine all chunks from the backend
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
		}
	}
}