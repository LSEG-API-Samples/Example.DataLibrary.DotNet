using Refinitiv.Data.Content.Pricing;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2._2._06_Pricing_StreamingChain
{
    // **********************************************************************************************************************
    // 2.2.06-Pricing-StreamingChain
    // The following example demonstrates how to request and process chains that are active, such as the Nasdaq Top 25. 
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
                // Create a session into the platform
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    session.Open();

                    // Creating a streaming chain and manage updates
                    ProcessChain(Chain.Definition(".AV.O").GetStream().Streaming(true)
                                                                             .OnAdd((index, newv, c) => 
                                                                                Console.WriteLine($"\tNew constituent {newv} added at index: {index}"))
                                                                             .OnRemove((index, oldv, c) => 
                                                                                Console.WriteLine($"\tRemoved constituent {oldv} added at index: {index}"))
                                                                             .OnUpdate((index, oldv, newv, c) => 
                                                                                Console.WriteLine($"Index {index} within our Chain changed from {oldv} => {newv}"))
                                                                             .OnStatus((item, status, c) => 
                                                                                Console.WriteLine($"Status for item: {item} {status}"))
                                                                             .OnError((item, error, c) => 
                                                                                Console.WriteLine($"Error for item: {item} {error}")));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        // ProcessChain
        // Based on the chain request parameters, 
        private static void ProcessChain(IChainStream chain, bool changesEchoed = true)
        {
            if (chain.Open() == Stream.State.Opened)
            {
                // Get the current snapshot of the chain after our request
                DisplayChain(chain.DisplayName, chain.Constituents);

                // Wait for some time to pass to see what changes have occurred
                Console.WriteLine("Changes will be displayed if active.  Hit <Enter> to stop and see the updated chain...");
                Console.ReadLine();

                // See if the resulting changes look correct
                DisplayChain(chain.DisplayName, chain.Constituents);

                chain.Close();
            }
        }

        private static void DisplayChain(string name, IList<string> constituents)
        {
            Console.WriteLine($"\nRetrieved Chain RIC: {name}");

            // Display the 30 first elements of the chain
            int idx = 0;
            foreach (string constituent in constituents.Take(30))
                Console.WriteLine($"\t{++idx,2}. {constituent}");

            if (constituents.Count > 30)
                Console.WriteLine($"\t...\n\t<total of {constituents.Count} elements.>");
        }
    }
}
