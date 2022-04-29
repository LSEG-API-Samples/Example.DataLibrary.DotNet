using Refinitiv.Data.Content.Pricing;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._2._01_Pricing_Snapshot
{
    // **********************************************************************************************************************
    // 2.2.01-Pricing-Snapshot
    // The following example demonstrates a basic use case of the Pricing interface to retrieve a snapshot of prices from
    // the platform.  The interface supports the ability to specify a list of items and the fields for each to retrieve.
    //
    // Suggestion: This example uses the pricing snapshot endpoint service within RDP. For users that do not have access
    //             to this endpoint, but do have access to the streaming services, you can instead retrieve snapshot prices
    //             through the streaming interfaces.  Refer to example 2.2.07-Pricing-StreamingSnapshot.
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
                using (ISession session = Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Specify a list of items and fields to retrieve a snapshot of prices from the platform
                    var response = Pricing.Definition("EUR=", "CAD=", "USD=").Fields("DSPLY_NAME", "BID", "ASK")
                                                                             .GetData();

                    if (response.IsSuccess)
                    {
                        // Print 1 field
                        Console.WriteLine($"\nEUR=[DSPLY_NAME]: {response.Data.Prices["EUR="]["DSPLY_NAME"]}");

                        // Print the contents of one item
                        Console.WriteLine($"\nEUR= contents: {response.Data.Prices["EUR="].Fields()}");

                        // Iterate through the response and print out specific fields for each entry
                        Console.WriteLine("\nIterate through the cache and display a couple of fields");
                        foreach (var item in response.Data.Prices)
                            Console.WriteLine($"Quote for item: {item.Key}\n{item.Value.Fields("BID", "ASK")}");
                    }
                    else
                        Console.WriteLine($"Request failed: {response.HttpStatus}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
