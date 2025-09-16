using Configuration;
using Newtonsoft.Json.Linq;
using LSEG.Data.Content.Pricing;
using System;

namespace _2._2._08_Pricing_StreamingEndpoint
{
    // **********************************************************************************************************************
    // 2.2.08-Pricing-StreamingEndpoint
    // The following example demonstrates how to override the default streaming endpoint when connecting to RDP.  The example
    // utilizes the locally-defined configuration file: lseg-data.config.json.
    //
    // The example demonstrates the same functionality defined within example: 2.2.05-Pricing-StreamingEvents.  However,
    // through configuration, overrides the streaming region to control the endpoint driving the streaming data.
    //
    // Note: The service is only available within RDP. Refer to the .Solutions folder Configuration.Credentials file to
    //       specify the required RDP credentials. Alternatively, users to specify credentials within their own
    //       configuration file.
    // **********************************************************************************************************************
    internal class Program
    {
        static void Main(string[] _)
        {
            // *************************
            // * Environment management.
            // * Manage the configuration environment variable in the event it is defined.
            // *************************
            const string envVarName = "LD_LIB_CONFIG_FILE";
            string initialValue = Environment.GetEnvironmentVariable(envVarName);

            try
            {
                // Disable the environment variable if it is defined
                if (initialValue != null)
                {
                    Environment.SetEnvironmentVariable(envVarName, null);
                    Console.WriteLine($"{envVarName} is temporarily disabled.");
                }

                // This example requires a platform session to demonstrate how to override the default region
                var session = Sessions.GetSession(Sessions.SessionTypeEnum.RDPv1);

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
                Console.WriteLine($"\n**************\nFailed to execute.");
                Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
                if (e.InnerException is not null) Console.WriteLine(e.InnerException);
                Console.WriteLine("***************");
            }
            finally
            {
                // Restore the environment variable if it was initially defined
                if (initialValue != null)
                {
                    Environment.SetEnvironmentVariable(envVarName, initialValue);
                    Console.WriteLine($"{envVarName} has been restored to its original value.");
                }
            }
        }

        // Based on market data events, reach into the message and pull out the fields of interest for our display.
        private static void DisplayUpdate(string item, JObject update)
        {
            var fields = update["Fields"];

            // Display the quote for the asset we're watching
            Console.WriteLine($"{ DateTime.Now:HH:mm:ss}: {item} ({fields["BID"],6}/{fields["ASK"],6}) - {fields["DSPLY_NAME"]}");
        }
    }
}
