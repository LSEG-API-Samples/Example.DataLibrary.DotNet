using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Request;
using System;

namespace _3._2._04_Endpoint_DataGrid
{
    // **********************************************************************************************************************
    // 3.2.04-Endpoint-DataGrid
    // The following example demonstrates the DataGrid (ADC) API showcasing a wide variety of conversions.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            const string dataGridEndpoint = "https://api.refinitiv.com/data/datagrid/beta1/";

            try
            {
                // Create the platform session.
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Create the DataGrid endpoint definition
                    var endpoint = EndpointRequest.Definition(dataGridEndpoint).Method(EndpointRequest.Method.POST);

                    // Simple request
                    var response = endpoint.BodyParameters(new JObject()
                                           {
                                               {"universe", new JArray("TRI.N", "IBM.N") },
                                               {"fields", new JArray("TR.Revenue", "TR.GrossProfit") }
                                           })
                                           .GetData();
                    DisplayResponse(response);

                    // Global parameters
                    response = endpoint.BodyParameters(new JObject()
                                        {
                                            {"universe", new JArray("GOOG.O", "AAPL.O") },
                                            {"fields", new JArray("TR.RevenueMean", "TR.NetProfitMean") },
                                            {"parameters", new JObject()
                                                {
                                                    {"SDate", "0M" },
                                                    {"Curn", "EUR" }
                                                }
                                            }
                                        }).GetData();
                    DisplayResponse(response);

                    // Historical data with specific date range
                    response = endpoint.BodyParameters(new JObject()
                                        {
                                            { "universe", new JArray("BHP.AX") },
                                            { "fields", new JArray("TR.AdjmtFactorAdjustmentDate", "TR.AdjmtFactorAdjustmentFactor") },
                                            { "parameters", new JObject()
                                                  {
                                                      {"SDate", "1980-01-01" },
                                                      {"EDate", "2018-09-29" }
                                                  }
                                            }
                                        }).GetData();
                    DisplayResponse(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        static void DisplayResponse(IEndpointResponse response)
        {
            if (response.IsSuccess)
            {
                Console.WriteLine(response.Data.Raw);
                Console.WriteLine(response.Data.Raw["universe"]);
                Console.WriteLine(response.Data.Raw["headers"]);
                Console.WriteLine(response.Data.Raw["data"]);
            }
            else
                Console.WriteLine(response.HttpStatus);

            Console.WriteLine("Enter to continue...");
            Console.ReadLine();
        }
    }
}
