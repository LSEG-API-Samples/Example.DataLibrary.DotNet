using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using System;

namespace _3._1._10_Streaming_IPAFxCross
{
    internal class Program
    {
        // **********************************************************************************************************************
        // 3.1.10-Streaming-IPAFxCross
        // The following example demonstrates the use of streaming IPA (Instrument Pricing Analytics) using the RDP Streaming
        // services.  Using the IPA Fx Cross services, the example will show realtime updates to changes in the cross rates.
        //
        // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
        //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
        //      2. Configuration.Credentials to define your login credentials for the specified access channel.
        // **********************************************************************************************************************
        static void Main(string[] _)
        {
            try
            {
                // The IPA Streaming service is presently available only on RDP
                using ISession session = Configuration.Sessions.GetSession(Configuration.Sessions.SessionTypeEnum.RDP);

                // Open the session
                session.Open();

                // Define an FX Cross request...
                var universe = new JObject()
                {
                    ["instrumentType"] = "FxCross",
                    ["instrumentDefinition"] = new JObject()
                    {
                        ["fxCrossType"] = "FxSpot",
                        ["fxCrossCode"] = "USDAUD"
                    }
                };

                var parameters = new JObject()
                {
                    ["marketData"] = new JObject()
                    {
                        ["fxSpots"] = new JArray()
                        {
                            new JObject()
                            {
                                ["spotDefinition"] = new JObject()
                                {
                                    ["fxCrossCode"] = "AUDUSD",
                                    ["Source"] = "Composite"
                                }
                            }
                        }
                    }
                };

                // Prepare an RDP Stream definition
                var stream = RDPStream.Definition().Universe(universe)
                                                   .ExtendedParams(parameters)
                                                   .Fields("InstrumentDescription", "FxSpot", "FxSpot_BidMidAsk", "Ccy1SpotDate", "Ccy2SpotDate", "ErrorCode")
                                                   .GetStream().OnResponse((msg, s) => Console.WriteLine($"Response: {msg}"))
                                                               .OnUpdate((msg, s) => DumpMsg(msg))
                                                               .OnAlarm((msg, s) => Console.WriteLine($"Stream alarm: {DateTime.Now:HH:mm:ss}: {msg}"));

                // Open the stream...
                if (stream.Open() == Stream.State.Opened)
                {
                    Console.WriteLine("Waiting for updates..."); Console.ReadLine();
                    Console.WriteLine("Closing stream...");
                    stream.Close();
                    Console.Write($"The stream is now {stream.OpenState}. There should no longer be updates.  Hit <Enter> to exit...");
                    Console.ReadLine();
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
        private static void DumpMsg(JObject msg)
        {
            // The msg contains an array of data updates
            if (msg["data"] is JArray data)
            {
                foreach (var universe in data)
                {
                    if (universe is JArray items)
                    {
                        // Pull out a couple of details:
                        Console.WriteLine($"{DateTime.Now:HH:mm:ss}: Spot rate for {items[0]} => {items[1]}");
                    }
                }
            }
        }
    }
}
