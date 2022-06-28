using Common_Examples;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.IPA;
using Refinitiv.Data.Core;
using System;

namespace _2._7._04_IPA_Swap
{
    // **********************************************************************************************************************
    // 2.7.04-IPA-Swap
    // The IPA (Instrument Pricing Analytics) Swap example demonstrates how to retrieve Swap analytics available within the
    // pricing analytics engine. The example uses a common method to display the table of data returned.
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
                // Create the platform session.
                using ISession session = Configuration.Sessions.GetSession();

                // Open the session
                session.Open();

                var swap = new JObject()
                {
                    ["fields"] = new JArray("InstrumentDescription", "LegTag", "ValuationDate", "Tenor",
                                            "InterestType", "DiscountCurveName"),
                    ["universe"] = new JArray(new JObject()
                    {
                        ["instrumentType"] = "Swap",
                        ["instrumentDefinition"] = new JObject()
                        {
                            ["tenor"] = "2Y",
                            ["legs"] = new JArray(new JObject()
                            {
                                ["interestType"] = "Fixed",
                                ["interestPaymentFrequency"] = "Quarterly",
                                ["direction"] = "Paid",
                                ["notionalAmount"] = 1000000,
                                ["notionalCcy"] = "EUR"
                            }, new JObject()
                            {
                                ["indexTenor"] = "5Y",
                                ["cmsTemplate"] = "EUR_AB6E",
                                ["interestType"] = "Float",
                                ["interestPaymentFrequency"] = "Quarterly",
                                ["direction"] = "Received",
                                ["notionalCcy"] = "EUR"
                            })
                        }
                    }),
                    ["pricingParameters"] = new JObject()
                    {
                        ["indexConvexityAdjustmentIntegrationMethod"] = "RiemannSum",
                        ["indexConvexityAdjustmentMethod"] = "BlackScholes",
                        ["valuationDate"] = "2020-06-01"
                    }
                };

                Common.DisplayTable("Swap", FinancialContracts.Definition(swap).GetData());
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
