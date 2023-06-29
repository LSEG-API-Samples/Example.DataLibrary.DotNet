using Refinitiv.Data.Content.Pricing;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using System;
using Configuration;

namespace _2._2._04_Pricing_StreamingAddRemove
{
    // **********************************************************************************************************************
    // 2.2.04-Pricing-StreamingAddRemove
    // The following example demonstrates how to add to or remove items from your streaming cache.  Items added will be
    // automatically opened if the stream is already opened.
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
                // Create a session into the platform...
                using ISession session = Sessions.GetSession();

                // Open the session
                session.Open();

                // Create a streaming price interface for a list of instruments
                using var stream = Pricing.Definition("EUR=", "CAD=", "GBP=").Fields("DSPLY_NAME", "BID", "ASK")
                                                                             .GetStream()
                                                              .OnStatus((item, status, s) => Console.WriteLine(status))
                                                              .OnError((item, err, s) => Console.WriteLine(err));
                if (stream.Open() == Stream.State.Opened)
                {
                    // Dump the cache to show the current items we're watching
                    DumpCache("Stream defined with 3 items", stream);

                    // Add 2 new currencies...
                    stream.AddItems("JPY=", "MXN=");

                    // Dump cache again...
                    DumpCache("Stream after adding 2 items", stream);

                    // Remove 2 different currencies...
                    stream.RemoveItems("CAD=", "GBP=");

                    // Final dump
                    DumpCache("Stream after removing 2 items", stream);

                    // Close streams
                    Console.WriteLine("\nClosing opened streams...");
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

        private static void DumpCache(string details, IPricingStream stream)
        {
            Console.WriteLine("\n*************************************Current cached items**********************************");
            Console.WriteLine($"{details}:\n");

            foreach (var entry in stream)
                Console.WriteLine($"{entry.Key}: {entry.Value["DSPLY_NAME"]}");

            Console.Write("<Enter> to continue..."); Console.ReadLine();
        }
    }
}
