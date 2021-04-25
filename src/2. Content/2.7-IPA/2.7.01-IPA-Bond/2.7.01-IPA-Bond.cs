using Common_Examples;
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
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Multiples bonds - only specific fields
                    Common.DisplayTable(Bond.Definition("US10YT=RR", "US20YT=RR").Fields("InstrumentCode", "BondType", "EndDate", "CouponRatePercent",
                                                                                         "YieldPercent", "ProcessingInformation")
                                                                                 .GetData(), "Multiple Bonds - limited fields");
                    Console.Write("Enter to continue: "); Console.ReadLine();

                    // Bond with some properties
                    Common.DisplayTable(Bond.Definition("US10YT=RR").IssueDate("2002-02-28")
                                                                    .EndDate("2032-02-28")
                                                                    .NotionalCcy("USD")
                                                                    .InterestPaymentFrequency(Bond.IndexFrequency.Annual)
                                                                    .FixedRatePercent(7)
                                                                    .InterestCalculationMethod(Bond.DayCountMethods.Dcb_30_Actual)
                                                                    .PricingParams(Bond.PricingDefinition().CleanPrice(102))
                                                                    .Fields("InstrumentDescription", "InstrumentCode", "BondType", "Isin", "Ticker",
                                                                            "Cusip", "IssuePrice", "CouponRatePercent", "CouponType", "CouponTypeDescription")
                                                                    .GetData(), "Bond with additional properties");
                    Console.Write("Enter to continue: "); Console.ReadLine();

                    // Single bond - default parameters
                    Common.DisplayTable(Bond.Definition("US10YT=RR").GetData(), "Single Bond - default parameters (all fields)");
                    Console.Write("Enter to continue: "); Console.ReadLine();

                    // Invalid Bond
                    Common.DisplayTable(Bond.Definition("JUNK").Fields("InstrumentCode", "BondType", "EndDate", "CouponRatePercent",
                                                                       "YieldPercent", "ErrorMessage")
                                                               .GetData(), "Invalid Bond");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
