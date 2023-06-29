using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using System;

namespace _3._1._12_Streaming_Wealth
{
    internal class Program
    {
        // **********************************************************************************************************************
        // 3.1.12-Streaming-Wealth
        // The following example demonstrates the use of streaming services for our Wealth clients accessing content via our
        // Knowledge Direct data services.  The service utilizes the OMM Pricing platform but defines a specific service called:
        // "FRT_FD3_LF1" referencing the services available to Wealth clients.  See below.
        //
        // To execute this example, you must be a licensed Wealth customer accessing the Refinitiv Knowledge Direct service.
        //
        // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
        //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
        //      2. Configuration.Credentials to define your login credentials for the specified access channel.
        // **********************************************************************************************************************
        static void Main(string[] _)
        {
            try
            {
                // The Wealth service is presently only available to RDP customers
                using ISession session = Configuration.Sessions.GetSession(Configuration.Sessions.SessionTypeEnum.RDP);

                // Open the session
                session.Open();

                var stream = OMMStream.Definition().Name("EUR=")
                                                       .Service("ERT_FD3_LF1")
                                                       .Fields("BID", "ASK", "DSPLY_NAME")
                                                       .GetStream().OnRefresh((item, msg, s) => Console.WriteLine(msg))
                                                                   .OnUpdate((item, msg, s) => DumpMsg(item, msg))
                                                                   .OnComplete(s => Console.WriteLine("Request completed."))
                                                                   .OnStatus((item, msg, s) => Console.WriteLine($"{DateTime.Now} => Status: {item} => {msg}"))
                                                                   .OnError((item, msg, s) => Console.WriteLine($"Stream error: {DateTime.Now}:{item} => {msg}"));

                if (stream.Open() == Stream.State.Opened)
                {
                    Console.Write("Waiting for updates..."); Console.ReadLine();
                    Console.WriteLine("Closing all items within stream...");
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

        private static void DumpMsg(string item, JObject msg)
        {
            JToken fields = msg["Fields"];

            // Quote or trade
            if (fields["BID"] != null)
                Console.WriteLine($"{DateTime.Now:HH:mm:ss}: {item} Quote (Bid/Ask) => ({fields["BID"]}/{fields["ASK"]})");
            else if (fields["TRDPRC_1"] != null)
                Console.WriteLine($"{DateTime.Now:HH:mm:ss}: {item} Trade (Price/Vol) => ({fields["TRDPRC_1"]}/{fields["TRDVOL_1"]})");
        }
    }
}
