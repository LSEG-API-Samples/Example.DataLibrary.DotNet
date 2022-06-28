using Common_Examples;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.IPA;
using Refinitiv.Data.Core;
using System;

namespace _2._7._05_IPA_CapFloor
{
    // **********************************************************************************************************************
    // 2.7.05-IPA-CapFloor
    // The IPA (Instrument Pricing Analytics) Cap Floor example demonstrates how to retrieve Cap Floor analytics available
    // within the pricing analytics engine. The example uses a common method to display the table of data returned.
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

                var capFloor = new JObject()
                {
                    ["fields"] = new JArray("InstrumentDescription", "StartDate", "EndDate", "InterestPaymentFrequency", "IndexRic",
                                "NotionalCcy", "NotionalAmount", "PremiumBp"),
                    ["universe"] = new JArray(new JObject()
                    {
                        ["instrumentType"] = "CapFloor",
                        ["instrumentDefinition"] = new JObject()
                        {
                            ["stubRule"] = "Maturity",
                            ["notionalCcy"] = "USD",
                            ["startDate"] = "2018-06-15",
                            ["endDate"] = "2022-06-15",
                            ["notionalAmount"] = 1000000,
                            ["indexName"] = "Composite",
                            ["indexTenor"] = "5Y",
                            ["cmsTemplate"] = "USD_SB3L",
                            ["interestCalculationMethod"] = "Dcb_Actual_360",
                            ["interestPaymentFrequency"] = "Quarterly",
                            ["buySell"] = "Buy",
                            ["capStrikePercent"] = 1
                        }
                    }),
                    ["pricingParameters"] = new JObject()
                    {
                        ["skipFirstCapFloorlet"] = false,
                        ["valuationDate"] = "2020-02-07"
                    }
                };

                Common.DisplayTable("Cap Floor", FinancialContracts.Definition(capFloor).GetData());
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
