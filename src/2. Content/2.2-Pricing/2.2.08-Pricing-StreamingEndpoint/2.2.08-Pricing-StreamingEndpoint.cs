using Configuration;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.Pricing;
using System;

namespace _2._2._08_Pricing_StreamingEndpoint
{
    // **********************************************************************************************************************
    // 2.2.08-Pricing-StreamingEndpoint
    // The following example demonstrates how to override the default streaming endpoint when connecting to RDP.  The example
    // utilizes the configuration file: refinitiv-data.config.json located within this project.
    //
    // The example demonstrates the same functionality defined within example: 2.2.05-Pricing-StreamingEvents.  However,
    // through configuration, will define the region to control the endpoint driving the streaming data.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // This example requires a platform session to demonstrate how to override the default region
                var session = Sessions.GetSession(Sessions.SessionTypeEnum.RDP);

                // Open the session
                session.Open();

                // Create a streaming price interface for a list of instruments and specify lambda expressions to capture real-time updates
                using var stream = Pricing.Definition("EUR=", "CAD=", "USD=").Fields("DSPLY_NAME", "BID", "ASK")
                                                                             .GetStream().OnRefresh((item, refresh, s) => Console.WriteLine(refresh))
                                                                                         .OnUpdate((item, update, s) => DisplayUpdate(item, update))
                                                                                         .OnStatus((item, status, s) => Console.WriteLine(status))
                                                                                         .OnError((item, err, s) => Console.WriteLine(err));
                stream.Open();

                // Pause on the main thread while updates come in.  Wait for a key press to exit.
                Console.WriteLine("Streaming updates.  Press any key to stop...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }


        }

        // Based on market data events, reach into the message and pull out the fields of interest for our display.
        private static void DisplayUpdate(string item, JObject update)
        {
            var fields = update["Fields"];

            // Display the quote for the asset we're watching
            Console.WriteLine($"{ DateTime.Now.ToString("HH:mm:ss")}: {item} ({fields["BID"],6}/{fields["ASK"],6}) - {fields["DSPLY_NAME"]}");
        }
    }
}
