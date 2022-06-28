using Common_Examples;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.IPA;
using Refinitiv.Data.Core;
using System;

namespace _2._7._01_IPA_Bond
{
    // **********************************************************************************************************************
    // 2.7.01-IPA-Bond
    // The IPA (Instrument Pricing Analytics) Bond example demonstrates a number of requests to price your bonds within 
    // the pricing analytics engine.  The example uses a common method to display the table of data returned.
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

                // Create request object...
                var bond = new JObject()
                {
                    ["fields"] = new JArray("InstrumentTag", "InstrumentDescription", "BondType", "CleanPrice", 
                                            "DirtyPrice", "MarketValueInDealCcy", "IssueDate", "EndDate"),
                    ["universe"] = new JArray(
                        new JObject()
                        {
                            ["instrumentType"] = "Bond",
                            ["instrumentDefinition"] = new JObject()
                            {
                                ["instrumentTag"] = "10Y Treasury Bond",
                                ["instrumentCode"] = "US10YT=RR"
                            }
                        },
                        new JObject()
                        {
                            ["instrumentType"] = "Bond",
                            ["instrumentDefinition"] = new JObject()
                            {
                                ["instrumentTag"] = "20Y Treasury Bond",
                                ["instrumentCode"] = "US20YT=RR"
                            }
                        })
                };
                var response = FinancialContracts.Definition(bond).GetData();

                Common.DisplayTable("10Y/20Y Treasury Bonds", response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
