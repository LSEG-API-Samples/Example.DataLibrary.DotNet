using Common_Examples;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.IPA;
using Refinitiv.Data.Core;
using System;

namespace _2._7._03_IPA_FxCross
{
    // **********************************************************************************************************************
    // 2.7.03-IPA-FxCross
    // The IPA (Instrument Pricing Analytics) FX Cross example demonstrates how to retrieve FX spot pricing and FX Swap
    // analytics available within the pricing analytics engine.The example uses a common method to display the table
    // of data returned.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    internal class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create the platform session.
                using ISession session = Configuration.Sessions.GetSession();

                // Open the session
                session.Open();

                // FX Cross
                var fx = new JObject()
                {
                    ["fields"] = new JArray("FxSpot_BidMidAsk", "Ccy1SpotDate", "Ccy2SpotDate", "ErrorCode"),
                    ["universe"] = new JArray(new JObject()
                    {
                        ["instrumentType"] = "FxCross",
                        ["instrumentDefinition"] = new JObject()
                        {
                            ["fxCrossType"] = "FxSpot",
                            ["fxCrossCode"] = "USDAUD"
                        }
                    }),
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
                Common.DisplayTable("FX Cross", FinancialContracts.Definition(fx).GetData());

                // FX Cross (multiple legs)
                fx = new JObject()
                {
                    ["fields"] = new JArray("InstrumentDescription", "ValuationDate", "EndDate",
                                            "FxSwapsCcy1", "FxSwapsCcy2", "FxSwapsCcy1Ccy2",
                                            "FxOutrightCcy1Ccy2"),
                    ["universe"] = new JArray(new JObject()
                    {
                        ["instrumentType"] = "FxCross",
                        ["instrumentDefinition"] = new JObject()
                        {
                            ["fxCrossType"] = "FxSwap",
                            ["fxCrossCode"] = "CHFJPY",
                            ["legs"] = new JArray(new JObject()
                            {
                                ["dealCcyBuySell"] = "Buy",
                                ["fxLegType"] = "SwapNear",
                                ["dealAmount"] = 1000000,
                                ["contraAmount"] = 897008.3,
                                ["tenor"] = "1M"
                            }, new JObject()
                            {
                                ["dealCcyBuySell"] = "Sell",
                                ["fxLegType"] = "SwapFar",
                                ["dealAmount"] = 1000000,
                                ["contraAmount"] = 900000,
                                ["tenor"] = "1Y"
                            })
                        }
                    }),
                    ["pricingParameters"] = new JObject()
                    {
                        ["valuationDate"] = "2018-02-17T00:00:00Z",
                        ["priceSide"] = "Ask"
                    }
                };

                Common.DisplayTable("FX Cross - multiple legs", FinancialContracts.Definition(fx).GetData());
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
