using LSEG.Data.Content.Pricing;
using LSEG.Data.Core;
using LSEG.Data.Delivery.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using Configuration;

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
        static bool InTransaction = false;

        static void Main(string[] _)
        {
            try
            {
                // Create a session into the platform
                using ISession session = Sessions.GetSession();

                session.Open();

                // Creating a streaming chain and manage updates
                ProcessChain(Chain.Definition(".AV.O").GetStream().Streaming(true)
                                                                  .OnAdd((index, newv, c) => AddConstituent(index, newv))
                                                                  .OnRemove((index, oldv, c) => RemoveConstituent(index, oldv))
                                                                  .OnUpdate((index, oldv, newv, c) => UpdateConstituent(index, oldv, newv))
                                                                  .OnRefreshComplete(c => UpdateTransactionState("}", false))
                                                                  .OnUpdateComplete(c => UpdateTransactionState("}", false))
                                                                  .OnStatus((item, status, c) =>
                                                                      Console.WriteLine($"Status for item: {item} {status}"))
                                                                  .OnError((item, error, c) =>
                                                                      Console.WriteLine($"Error for item: {item} {error}")));
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute.");
                Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
                if (e.InnerException is not null) Console.WriteLine(e.InnerException);
                Console.WriteLine("***************");
            }
        }

        private static void AddConstituent(int index, string constituent)
        {
            if (!InTransaction) 
                UpdateTransactionState("{", true);
            Console.WriteLine($"\tNew constituent {constituent} added at index: {index}");
        }

        private static void RemoveConstituent(int index, string constituent)
        {
            if (!InTransaction)
                UpdateTransactionState("{", true);
            Console.WriteLine($"\tRemoved constituent {constituent} added at index: {index}");
        }

        private static void UpdateConstituent(int index, string oldConstituent, string newConstituent)
        {
            if (!InTransaction)
                UpdateTransactionState("{", true);
            Console.WriteLine($"\tUpdate Index {index} within our Chain from {oldConstituent} => {newConstituent}");
        }

        private static void UpdateTransactionState(string brace, bool transaction)
        {
            Console.WriteLine(brace);
            InTransaction = transaction;
        }


        // ProcessChain
        // Based on the chain request parameters, 
        private static void ProcessChain(IChainStream chain)
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
