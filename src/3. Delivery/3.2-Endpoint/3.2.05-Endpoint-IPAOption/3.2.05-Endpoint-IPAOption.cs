using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Request;
using System;

namespace _3._2._05_Endpoint_IPAOption
{
    // **********************************************************************************************************************
    // 3.2.05-Endpoint-IPAOption
    // The following example demonstrates the Instrument Pricing Analytics (IPA) API demonstrating Option Pricing.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            const string intradayEndpoint = "https://api.refinitiv.com/data/quantitative-analytics/v1/financial-contracts";

            try
            {
                // Create the platform session.
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // IPA - Financial Contracts (Option)
                    var response = EndpointRequest.Definition(intradayEndpoint).Method(EndpointRequest.Method.POST)
                                                                               .BodyParameters(new JObject()
                                                                               {
                                                                                   ["fields"] = new JArray("ErrorMessage",
                                                                                                           "UnderlyingRIC",
                                                                                                           "UnderlyingPrice",
                                                                                                           "DeltaPercent",
                                                                                                           "GammaPercent",
                                                                                                           "RhoPercent",
                                                                                                           "ThetaPercent",
                                                                                                           "VegaPercent"),
                                                                                   ["universe"] = new JArray(new JObject()
                                                                                   {
                                                                                       ["instrumentType"] = "Option",
                                                                                       ["instrumentDefinition"] = new JObject()
                                                                                       {
                                                                                           ["instrumentCode"] = "AAPLA192422500.U",
                                                                                           ["underlyingType"] = "Eti"
                                                                                       },
                                                                                       ["pricingParameters"] = new JObject()
                                                                                       {
                                                                                           ["underlyingTimeStamp"] = "Close"
                                                                                       }
                                                                                   })
                                                                               }).GetData();
                    if (response.IsSuccess)
                    {
                        Console.WriteLine(response.Data.Raw["data"]);
                    }
                    else
                    {
                        Console.WriteLine($"Request failed: {response.HttpStatus}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
