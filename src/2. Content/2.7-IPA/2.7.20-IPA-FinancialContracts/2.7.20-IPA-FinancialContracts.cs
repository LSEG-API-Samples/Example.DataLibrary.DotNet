using Common_Examples;
using Refinitiv.Data.Content.IPA;
using Refinitiv.Data.Core;
using System;

namespace _2._7._20_IPA_FinancialContracts
{
    // **********************************************************************************************************************
    // 2.7.20-IPA-FinanicialContracts
    // The IPA (Instrument Pricing Analytics) Financial Contracts example demonstrates how to request for multiple asset 
    // types within the pricing analytics engine.  The example uses a common method to display the table of data returned.
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

                    // Mixed Options
                    var binary = OptionFx.BinaryDefinition(OptionFx.BinaryType.OneTouchDeferred, 1.2001).SettlementType(OptionFx.SettlementType.Cash)
                                                                                                        .PayoutAmount(1000000);
                    Common.DisplayDataSet(FinancialContracts.Definition().Universe(OptionEti.Definition("FCHI560000L1.p"),
                                                                                   OptionFx.Definition().FxCrossCode("EURUSD")
                                                                                                        .Tenor("1M")
                                                                                                        .BinaryDefinition(binary),
                                                                                   OptionEti.Definition("FCHI505000O1.p"))
                                                                         .Fields("InstrumentDescription", "InstrumentCode", "FxCrossCode", "StrikePrice",
                                                                                 "DeltaPercent", "OptionPrice", "UnderlyingRIC")
                                                                         .GetData(), "Mixed Options");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
