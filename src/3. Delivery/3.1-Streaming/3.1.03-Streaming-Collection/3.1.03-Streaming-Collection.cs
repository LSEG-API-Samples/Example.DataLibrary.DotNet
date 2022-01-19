using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3._1._3_Streaming_Collection
{
    // **********************************************************************************************************************
    // 3.1.03-Streaming-Collection
    // The following example demonstrates the use of retrieving multiple instruments and waiting for the entire collection
    // to be retrieved using the API.  
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create the platform session.
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session and test the state...
                    if (session.Open() == Session.State.Opened)
                    {
                        // *******************************************************************************************************************************
                        // Requesting for multiple instruments.
                        // The following code segment demonstrates the usage of the library and .NET's asynchronous capabilities to send a collection of
                        // requests and monitor the whole collection for completion.
                        // *******************************************************************************************************************************
                        List<Task<Stream.State>> tasks = new List<Task<Stream.State>>();

                        // First, prepare our item stream definition, defining the fields of interest and where to capture events...
                        var itemDef = OMMStream.Definition().Fields("DSPLY_NAME", "BID", "ASK");

                        // Next, iterate through the collection of items, applying each to our parameters specification.  Send each request asynchronously...
                        foreach (var item in new[] { "EUR=", "GBP=", "CAD=" })
                        {
                            // Create our stream
                            IStream stream = itemDef.Name(item).GetStream().OnRefresh((item, msg, s) => DumpMsg(item, msg))
                                                                           .OnUpdate((item, msg, s) => DumpMsg(item, msg))
                                                                           .OnStatus((item, msg, s) => Console.WriteLine(msg))
                                                                           .OnError((item, err, s) => Console.WriteLine(err));

                            // Open the stream asynchronously and keep track of the task
                            tasks.Add(stream.OpenAsync());
                        }

                        // Monitor the collection for completion.  We are intentionally blocking here waiting for the whole collection to complete.
                        Task.WhenAll(tasks).GetAwaiter().GetResult();
                        Console.WriteLine("\nInitial response for all instruments complete.  Updates will follow based on changes in the market...");

                        // Wait for updates...
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
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
                Console.WriteLine($"{ DateTime.Now.ToString("HH:mm:ss")}: {item} ({bid,6}/{ask,6}) - {fields["DSPLY_NAME"]}");
            }
        }
    }
}
