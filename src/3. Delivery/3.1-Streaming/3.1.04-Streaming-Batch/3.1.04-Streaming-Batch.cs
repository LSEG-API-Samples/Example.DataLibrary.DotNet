using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using System;

namespace _3._1._04_Streaming_Batch
{
    // **********************************************************************************************************************
    // 3.1.04-Streaming-Batch
    // The following example demonstrates the use of retrieving multiple instruments by specifying a batch.
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
                using ISession session = Configuration.Sessions.GetSession();

                // Open the session and test the state...
                if (session.Open() == Session.State.Opened)
                {
                    // Define a stream to retrieve a batch of level 1 instruments...
                    using var stream = OMMStream.Definition("EUR=", "GBP=", "CAD=").GetStream()
                                                    .OnRefresh((item, msg, s) => DumpMsg(item, msg))
                                                    .OnUpdate((item, msg, s) => DumpMsg(item, msg))
                                                    .OnError((item, err, s) => Console.WriteLine(err))
                                                    .OnStatus((item, msg, s) => Console.WriteLine(msg))
                                                    .OnComplete(s => Console.WriteLine("\nInitial response for all instruments complete.  Updates will follow based on changes in the market..."));
                    // Open the stream...
                    stream.Open();

                    // Wait for data to come in then hit any key to close the stream...
                    Console.ReadKey();
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

        // Based on market data events, reach into the message and pull out the fields of interest for our display.
        private static void DumpMsg(string item, JObject msg)
        {
            // The MarketPrice message contains a nested structure containing the fields included within the market data event.
            var fields = msg["Fields"];

            if (fields != null)
            {
                // Our quote fields
                double bid = (double)fields["BID"];
                double ask = (double)fields["ASK"];

                // Display the quote for the asset we're watching
                Console.WriteLine($"{ DateTime.Now:HH:mm:ss}: {item} ({bid,6}/{ask,6}) - {fields["DSPLY_NAME"]}");
            }
        }
    }
}
