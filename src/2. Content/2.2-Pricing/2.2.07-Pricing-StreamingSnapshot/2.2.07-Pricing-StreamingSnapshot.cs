using Refinitiv.Data.Content.Pricing;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._2._07_Pricing_StreamingSnapshot
{
    // **********************************************************************************************************************
    // 2.2.07-Pricing-StreamingSnapshot
    // The following example demonstrates a basic use case of the Pricing interface to retrieve a snapshot of prices from
    // the streaming services available within the platform.  The interface supports the ability to specify a list of items
    // and the fields for each to retrieve.
    //
    // Suggestion: This example uses streaming services.  For users that do not have access to streaming services, but do
    //             have access to the snapshot pricing service within RDP, you can refer to the 2.2.01-Pricing-Snapshot
    //             example.
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

                // Specify a list of items and fields to retrieve. Retrieve the stream, indicating a snapshot, i.e. streaming=false.
                using var stream = Pricing.Definition("EUR=", "CAD=", "USD=").Fields("DSPLY_NAME", "BID", "ASK")
                                                                             .GetStream()
                                                                  .Streaming(false)
                                                                  .OnStatus((item, status, s) => Console.WriteLine(status))
                                                                  .OnError((item, err, s) => Console.WriteLine(err));

                stream.Open();

                // The OpenState will be closed - requesting for a snapshot results in the items as non-streaming
                // To verify the stream has cached values, we can check the count.
                if (stream.Count() > 0)
                {
                    // Print 1 field
                    Console.WriteLine($"\nEUR=[DSPLY_NAME]: {stream["EUR="]["DSPLY_NAME"]}");

                    // Print the contents of one item
                    Console.WriteLine($"\nEUR= contents: {stream["EUR="].Fields()}");

                    // Iterate through the response and print out specific fields for each entry
                    Console.WriteLine("\nIterate through the cache and display a couple of fields");
                    foreach (var item in stream)
                        Console.WriteLine($"Quote for item: {item.Key}\n{item.Value.Fields("BID", "ASK")}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
