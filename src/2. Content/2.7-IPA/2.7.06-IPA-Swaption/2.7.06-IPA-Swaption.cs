using Common_Examples;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.IPA;
using Refinitiv.Data.Core;
using System;

namespace _2._7._06_IPA_Swaption
{
    internal class Program
    {
        // **********************************************************************************************************************
        // 2.7.06-IPA-Swaption
        // The IPA (Instrument Pricing Analytics) Swaption example demonstrates how to retrieve Swaption analytics available
        // within the pricing analytics engine. The example uses a common method to display the table of data returned.
        //
        // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
        //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
        //      2. Configuration.Credentials to define your login credentials for the specified access channel.
        // **********************************************************************************************************************
        static void Main(string[] _)
        {
            try
            {
                // Create the platform session.
                using ISession session = Configuration.Sessions.GetSession();

                // Open the session
                session.Open();

                // Swaption
                var swaption = new JObject()
                {
                    ["fields"] = new JArray("InstrumentDescription", "ValuationDate", "ExpiryDate",
                                            "OptionType", "ExerciseStyle", "NotionalAmount", "ErrorMessage",
                                            "NotionalCcy", "SettlementType", "SettlementCcy",
                                            "Tenor", "UnderlyingTenor", "StrikePercent",
                                            "MarketValueInDealCcy", "PremiumPercent", "ForwardPremiumInDealCcy",
                                            "ImpliedVolatilityPercent", "DeltaPercent", "DeltaAmountInDealCcy",
                                            "ThetaAmountInDealCcy", "VegaAmountInDealCcy", "GammaAmountInDealCcy",
                                            "DiscountCurveName", "ForwardCurveName"),
                    ["universe"] = new JArray(new JObject()
                    {
                        ["instrumentType"] = "Swaption",
                        ["instrumentDefinition"] = new JObject()
                        {
                            ["settlementType"] = "Cash",
                            ["tenor"] = "7Y",
                            ["strikePercent"] = 2.75,
                            ["buySell"] = "Buy",
                            ["swaptionType"] = "Payer",
                            ["exerciseStyle"] = "BERM",
                            ["bermudanSwaptionDefinition"] = new JObject() 
                            { 
                                ["exerciseScheduleType"] = "FloatLeg",
                                ["notificationDays"] = 0
                            },
                            ["underlyingDefinition"] = new JObject()
                            {
                                ["tenor"] = "3Y",
                                ["template"] = "NOK_AB6O"
                            }
                        },
                        ["pricingParameters"] = new JObject() 
                        { 
                            ["valuationDate"] = "2020-04-24",
                            ["nbIterations"] = 80
                        }
                    })
                };

                Common.DisplayTable("Swaption", FinancialContracts.Definition(swaption).GetData(), 7);
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
