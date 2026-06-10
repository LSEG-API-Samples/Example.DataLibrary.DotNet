using Common_Examples;
using Configuration;
using LSEG.Data.Content.HistoricalPricing;
using LSEG.Data.Core;

namespace _2._1._04_HistoricalPricing_Chunking
{
	// **********************************************************************************************************************
	// 2.1.04-HistoricalPricing-Chunking
	// The HistoricalPricing Chunking example demonstrates the chunking feature within the Historical Pricing service allowing
	// large responses to be managed as chunks of data returned in the backend.
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
				// Create the session where data is to be retrieved...
				using ISession session = Sessions.GetSession();

				// Open the session
				session.Open();

				// Setup date range
				var oneYearAgo = DateTime.UtcNow.AddYears(-1);
				var yesterday = DateTime.UtcNow.AddDays(-1);

				// Create a definition to retrieve hourly bars going back 1 year - normal trading hours
				var definition = Summaries.Definition("AMZN.O").Interval(Summaries.Interval.PT1H)
															   .Fields("TRDPRC_1", "LOW_1", "HIGH_1")
															   .Sessions(HistoricalPricing.Sessions.normal)
															   .Start(oneYearAgo)
															   .End(yesterday)
															   .UseChunking(true);	// Back-end will deliver data in chunks

				// Test 1: Chunks will be accumulated and entire result returned
				var response = definition.GetData();
				Common.DisplayTable("Hourly bars going back 1 year", response);

				// Test 2: Each chunk from backend will be delivered within our lambda expression
				definition.GetData((response, chunk, p, s) =>
				{
					Common.DisplayTable(
						$"Chunk index: {chunk.ChunkIndex}. " +
						$"Record count: {chunk.RecordsPerChunk}. " +
						$"Last chunk: {chunk.IsLastChunk}",
						response
					);
				});
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
