using Common_Examples;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.IPA;
using Refinitiv.Data.Core;
using System;

namespace _2._7._02_IPA_Option
{
    // **********************************************************************************************************************
    // 2.7.02-IPA-Option
    // The IPA (Instrument Pricing Analytics) Option example demonstrates a number of requests to price Eti, OTC and Fx
    // options within the pricing analytics engine.  The example uses a common method to display the table of data returned.
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

                // Simple request - default fields
                var eti = new JObject()
                {
                    ["universe"] = new JArray(new JObject()
                    {
                        ["instrumentType"] = "Option",
                        ["instrumentDefinition"] = new JObject()
                        {
                            ["InstrumentCode"] = "AAPLM192420500.U",
                            ["underlyingType"] = "Eti"
                        }
                    })
                };

                Common.DisplayTable("Option ETI - Apple", FinancialContracts.Definition(eti).GetData(), 7);

                // OTC Eti Option
                eti = new JObject()
                {
                    ["fields"] = new JArray("InstrumentCode", "ExerciseType", "ValuationDate", "EndDate",
                                            "StrikePrice", "OptionPrice", "UnderlyingRIC", "UnderlyingPrice",
                                            "ExerciseStyle", "ErrorMessage"),
                    ["universe"] = new JArray(new JObject()
                    {
                        ["instrumentType"] = "Option",
                        ["instrumentDefinition"] = new JObject()
                        {
                            ["underlyingType"] = "Eti",
                            ["buySell"] = "Sell",
                            ["callPut"] = "Call",
                            ["endDate"] = $"{DateTime.Now.AddDays(30):O}",
                            ["exerciseStyle"] = "Amer",
                            ["strike"] = 255,
                            ["UnderlyingDefinition"] = new JObject() { ["instrumentCode"] = "AAPL.O" }
                        }
                    })
                };

                Common.DisplayTable("OTC Eti Option", FinancialContracts.Definition(eti).GetData());

                // FX Option (include some pricing and binary properties)
                var fx = new JObject()
                {
                    ["fields"] = new JArray("FxCrossCode", "EndDate", "ForeignCcy", "FxSwap", "ErrorMessage"),
                    ["universe"] = new JArray(new JObject()
                    {
                        ["instrumentType"] = "Option",
                        ["instrumentDefinition"] = new JObject()
                        {
                            ["underlyingType"] = "Fx",
                            ["UnderlyingDefinition"] = new JObject() { ["fxCrossCode"] = "EURUSD" },
                            ["binaryDefinition"] = new JObject()
                            {
                                ["binaryType"] = "OneTouchDeferred",
                                ["trigger"] = 1.2001,
                                ["payoutAmount"] = 1000000
                            },
                            ["settlementType"] = "Cash",
                            ["tenor"] = "1M"
                        }
                    }),
                    ["pricingParameters"] = new JObject()
                    {
                        ["pricingModelType"] = "VannaVolga",
                        ["fxSpotObject"] = new JObject() { ["mid"] = 2.5 },
                        ["cutoffTimeZone"] = "GMT",
                        ["CutoffTime"] = "1500PM"
                    }
                };

                Common.DisplayTable("FX Option", FinancialContracts.Definition(fx).GetData());
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute.");
                Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
                if (e.InnerException is not null) Console.WriteLine(e.InnerException);
                Console.WriteLine("***************");
            }
        }
    }
}
