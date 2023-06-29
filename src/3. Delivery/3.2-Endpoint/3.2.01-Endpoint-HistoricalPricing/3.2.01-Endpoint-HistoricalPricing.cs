using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Request;
using System;

namespace _3._2._01_Endpoint_HistoricalPricing
{
    // **********************************************************************************************************************
    // 3.2.01-Endpoint-HistoricalPricing
    // The following example demonstrates basic usage of the Data Library for .NET to request data using the Delivery Endpoint 
    // interface. The example will refer to the Historical Pricing Events API to retrieve time series pricing events such as 
    // trades, quotes and corrections.
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
                // Create the platform session.
                using ISession session = Configuration.Sessions.GetSession();

                // Open the session
                session.Open();

                // ********************************************************************************************
                // Basic Endpoint retrieval.
                // The specific endpoint URL below contains all required parameters.
                // ********************************************************************************************
                var endpointUrl = "https://api.refinitiv.com/data/historical-pricing/v1/views/events/VOD.L";

                // Request and display the timeseries data using the endpoint default parameters.
                DisplayResult(EndpointRequest.Definition(endpointUrl).GetData());

                // Apply the same request but override the default 'count' which returns the number of hits.
                DisplayResult(EndpointRequest.Definition(endpointUrl).QueryParameter("count", "5").GetData());
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute.");
                Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
                if (e.InnerException is not null) Console.WriteLine(e.InnerException);
                Console.WriteLine("***************");
            }
        }

        // DisplayResult
        // The following code segment interrogates the response returned from the platform.  If the request was successful, details
        // related to the pricing data is displayed.  Otherwise, status details outlining failure reasons are presented. 
        static void DisplayResult(IEndpointResponse response)
        {
            if (response.IsSuccess)
            {
                // Retrieve the data elements from the response and display how many rows of historical pricing content came back.
                var data = response.Data?.Raw[0]?["data"] as JArray;
                Console.WriteLine($"Timeseries data response: {response.Data?.Raw}{Environment.NewLine}A total of {data?.Count} elements returned.");
            }
            else
                Console.WriteLine($"Failed to retrieve data: {response.HttpStatus}");

            Console.Write("Hit enter to continue..."); Console.ReadLine();
        }
    }
}
