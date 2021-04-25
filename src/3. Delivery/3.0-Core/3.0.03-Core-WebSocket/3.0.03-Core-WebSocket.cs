using Newtonsoft.Json.Linq;
using Refinitiv.Data;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using Refinitiv.Data.WebSockets;
using System;

namespace _3._0._03_Core_WebSocket
{
    // **********************************************************************************************************************
    // 3.0.03-Core-WebSocket
    // The following example demonstrates how to register an alternative .Net WebSocket implementation within the library. 
    // The Refinitiv Data Library for .Net provides the ability to register a Refinitiv WebSocket NuGet implementation
    // the a common interface allowing developers to choose their own preferred WebSocket package.
    //
    // This specific example has installed 2 alternative implementations from NuGet and registers them to be used when
    // performing any streaming activity from the platform.  The following interface will discover, via extension methods
    // defined within the Refinitiv WebSocket implementation, the available implementations:
    //
    //      RDPWebSocket.Register
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
                Log.Level = NLog.LogLevel.Debug;
                
                // Choose a WebSocket Implementation
                Console.WriteLine("\nChoose a WebSocket Implementation:");
                Console.WriteLine("\t1 - ClientWebSocket (Microsoft implementation)");
                Console.WriteLine("\t2 - NinjaWebSockets");
                Console.WriteLine("\t3 - WebSocket4Net (originated from SuperWebSocket)");

                Console.Write("\t==> ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int ver))
                {
                    switch (ver)
                    {
                        case 1:
                            // The default in the library
                            break;
                        case 2:
                            RDPWebSocket.Register.NinjaWebSockets();
                            break;
                        case 3:
                            RDPWebSocket.Register.WebSocket4Net();
                            break;
                        default:
                            return;
                    }

                    using (ISession session = Configuration.Sessions.GetSession())
                    {
                        if (session.Open() == Session.State.Opened)
                            TestStreaming();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void TestStreaming()
        {
            string item1 = "EUR=";
            string item2 = "CAD=";

            try
            {
                var stream1 = OMMStream.Definition(item1).GetStream().OnRefresh((item, msg, s) => Console.WriteLine($"{DateTime.Now}:{msg}"))
                                                                     .OnUpdate((item, msg, s) => DumpMsg(item, msg))
                                                                     .OnStatus((item, msg, s) => Console.WriteLine($"{DateTime.Now} => Status1: {msg}"))
                                                                     .OnError((item, msg, s) => Console.WriteLine($"Stream1 error: {DateTime.Now}:{msg}"));
                if (stream1.Open() != Stream.State.Opened)
                {
                    Console.WriteLine($"Stream did not open: {stream1.OpenState}");
                }

                var stream2 = OMMStream.Definition(item2).GetStream().OnRefresh((item, msg, s) => Console.WriteLine($"{DateTime.Now}:{msg}"))
                                                                     .OnUpdate((item, msg, s) => DumpMsg(item, msg))
                                                                     .OnStatus((item, msg, s) => Console.WriteLine($"{DateTime.Now} => Status2: {msg}"))
                                                                     .OnError((item, msg, s) => Console.WriteLine($"Stream2 error: {DateTime.Now}:{msg}"));
                stream2.Open();

                Console.ReadKey();
                stream1.Close();
                Console.WriteLine($"Stream {item1} has been closed.  Hit any key to close the {item2} stream...");
                Console.ReadKey();
                stream2.Close();
            }
            catch (PlatformNotSupportedException e)
            {
                Console.WriteLine($"\n******{e.Message} Choose an alternative WebSocket implementation.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void DumpMsg(string item, JObject msg)
        {
            JObject fields = (JObject)msg["Fields"];

            // Detect if we have a quote
            if (fields != null && fields["DSPLY_NAME"] != null)
            {
                double bid = (double)fields["BID"];
                double ask = (double)fields["ASK"];

                // Display the trade for the asset we're watching
                Console.WriteLine($"{ DateTime.Now:HH:mm:ss}: {item} ({bid:0.###}/{ask:0.###}) - {fields["DSPLY_NAME"]}");
            }
        }
    }
}
