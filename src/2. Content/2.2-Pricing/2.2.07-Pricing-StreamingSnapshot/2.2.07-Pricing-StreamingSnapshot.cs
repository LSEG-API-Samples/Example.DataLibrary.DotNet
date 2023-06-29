using Refinitiv.Data.Content.Pricing;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._2._07_Pricing_StreamingSnapshot
{
    // **********************************************************************************************************************
    // 2.2.07-Pricing-StreamingSnapshot
    // The following example demonstrates how to retrieve snapshot/refresh data from the streaming services. The following
    // examples are demonstrated:
    //
    //  o Pricing snapshot
    //    Retrieve a snapshot of the current pricing information for a list of instruments
    //
    //  o Chain snapshot
    //    Retrieve a snapshot of the constituents that make up a specific chain (Eg: Nasdaq - Top 25 by Volume)
    //
    // Suggestion: This example uses streaming services.  For users that do not have access to streaming services, but do
    //             have access to the snapshot pricing service within RDP, you can refer to the 2.2.01-Pricing-Snapshot
    //             or 2.2.01-Pricing-Chains examples.
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

                // Request a price snapshot from the streaming service
                SnapshotPrices();

                Console.Write("\n<Enter> to request a chain snapshot..."); Console.ReadLine();

                // Request a chain snapshot from the streaming service
                SnapshotChains();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute.");
                Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
                if (e.InnerException is not null) Console.WriteLine(e.InnerException);
                Console.WriteLine("***************");
            }
        }

        private static void SnapshotPrices()
        {
            // Specify a list of items and fields to retrieve. Retrieve the stream, indicating a snapshot, i.e. streaming=false.
            using var prices = Pricing.Definition("EUR=", "CAD=", "USD=").Fields("DSPLY_NAME", "BID", "ASK")
                                                                         .GetStream()
                                                              // This flag turns the request into a snapshot/refresh only
                                                              .Streaming(false)
                                                              .OnStatus((item, status, s) => Console.WriteLine(status))
                                                              .OnError((item, err, s) => Console.WriteLine(err));

            prices.Open();

            // The OpenState will be closed - requesting for a snapshot results in the items as non-streaming
            // To verify the stream has cached values, we can check the count.
            if (prices.Count() > 0)
            {
                var item = "EUR=";

                // Print 1 field
                Console.WriteLine($"\nDisplay some details for the cached item: {item}:");
                Console.WriteLine($"Pulling out the field 'DSPLY_NAME': {prices[item]["DSPLY_NAME"]}");

                // Print the contents of one item
                Console.WriteLine($"\n{item} contents: {prices[item].Fields()}");

                // Iterate through the response and print out specific fields for each entry
                Console.WriteLine("\nIterate through the cache and display a couple of fields");
                foreach (var ric in prices)
                    Console.WriteLine($"Quote for item: {ric.Key}\n{ric.Value.Fields("BID", "ASK")}");
            }
        }

        private static void SnapshotChains()
        {
            // Specify a list of items and fields to retrieve. Retrieve the stream, indicating a snapshot, i.e. streaming=false.
            using var chain = Chain.Definition(".AV.O").GetStream().Streaming(false)
                                                                   .OnStatus((item, status, s) => Console.WriteLine(status))
                                                                   .OnError((item, err, s) => Console.WriteLine(err));

            chain.Open();

            // The OpenState will be closed - requesting for a snapshot results in the chain as non-streaming
            // To verify the chain has contituents, we can check the count.
            if (chain.Constituents.Count > 0)
            {
                Console.WriteLine($"\nRetrieved Chain RIC: {chain.DisplayName}");

                // Display the 30 first elements of the chain
                int idx = 0;
                foreach (var constituent in chain.Constituents)
                    Console.WriteLine($"\t{++idx,2}. {constituent}");
            }
        }
    }
}
