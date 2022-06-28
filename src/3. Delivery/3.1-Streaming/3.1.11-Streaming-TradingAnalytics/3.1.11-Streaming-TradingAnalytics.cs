using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using System;

namespace _3._1._11_Streaming_TradingAnalytics
{
    internal class Program
    {
        // **********************************************************************************************************************
        // 3.1.11-Streaming-TradingAnalytics
        // The following example demonstrates the use of streaming Trading Analytics via the REDI trading platform.  The service
        // utilizes the RDP Streaming platform. Because many streaming services are built on top of RDP streaming, you will 
        // need to explicitely specify the Trading Analytics service via the Api method.  See below.
        //
        // To execute this example, you must be a licensed Redi customer.
        //
        // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
        //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
        //      2. Configuration.Credentials to define your login credentials for the specified access channel.
        // **********************************************************************************************************************
        static void Main(string[] _)
        {
            try
            {
                using ISession session = Configuration.Sessions.GetSession();

                // Open the session
                session.Open();

                var parameters = new JObject()
                {
                    ["universeType"] = "UserID",
                    ["events"] = "Full",
                    ["finalizedOrders"] = "P1D"
                };

                var stream = RDPStream.Definition().Api(RDPStream.Api.TradingAnalytics)
                                                   .Fields("OrderKey", "OrderTime", "RIC", "Side", "AveragePrice", "OrderStatus", "OrderQuantity")
                                                   .Parameters(parameters)
                                                   .GetStream().OnResponse((msg, s) => Console.WriteLine($"Response: {msg}"))
                                                               .OnUpdate((msg, s) => UpdateMsg(msg))
                                                               .OnAlarm((msg, s) => Console.WriteLine($"Stream alarm: {DateTime.Now:HH:mm:ss}: {msg}"));

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
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void UpdateMsg(JObject update)
        {
            Console.Write($"Update: {DateTime.Now:HH:mm:ss}. Contains: ");
            if (update["data"] is JArray data)
                Console.Write($"{data.Count} data elements. ");
            if (update["messages"] is JArray messages)
                Console.Write($"{messages.Count} message elements.");
            Console.WriteLine();
        }
    }
}
