using Common_Examples;
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

                // Single ETI Option -default parameters
                var response = OptionEti.Definition("AAPLM212222500.U").GetData();
                Common.DisplayTable("Single ETI Option - Columns truncated", response, 10);

                // Multiples ETI options - default parameters (Buy Call)
                response = OptionEti.Definition("FCHI560000L1.p", "AAPLM212222500.U").Fields("InstrumentCode", "StrikePrice", "EndDate", "ExerciseType",
                                                                                             "OptionPrice", "UnderlyingRIC", "ErrorMessage")
                                                                                     .GetData();
                Common.DisplayTable("Multiple ETI Options", response);

                // OTC Eti Option
                response = OptionEti.Definition().ExcerciseStyle(OptionEti.ExerciseStyle.Amer)
                                                 .Strike(255)
                                                 .EndDate(DateTime.Now.AddDays(30))
                                                 .BuySell(FinancialContracts.BuySell.Sell)
                                                 .CallPut(FinancialContracts.CallPut.Call)
                                                 .UnderlyingInstrument("AAPL.O")
                                                 .Fields("InstrumentCode", "ExerciseType", "ValuationDate", "EndDate", "StrikePrice", "OptionPrice",
                                                         "UnderlyingRIC", "UnderlyingPrice", "ExerciseStyle", "ErrorMessage")
                                                 .GetData();
                Common.DisplayTable("OTC Option", response);

                // FX Option (include some pricing and binary properties)
                var pricing = OptionFx.PricingDefinition().CutoffTimeZone(OptionFx.CutoffTimeZone.GMT)
                                                          .FxSpotObject(OptionFx.BidAskMidDefinition().Mid(2.5))
                                                          .CutoffTime("1500PM")
                                                          .PricingModelType(OptionFx.PricingModelType.VannaVolga);
                var binary = OptionFx.BinaryDefinition(OptionFx.BinaryType.OneTouchDeferred,
                                                       1.2001).PayoutAmount(1000000);

                response = OptionFx.Definition().Fields("FxCrossCode", "EndDate", "ForeignCcy", "FxSwap", "ErrorMessage")
                                                .FxCrossCode("EURUSD")
                                                .SettlementType(OptionFx.SettlementType.Cash)
                                                .Tenor("1M")
                                                .BinaryDefinition(binary)
                                                .PricingParams(pricing)
                                                .GetData();
                Common.DisplayTable("FX Option - Specify some pricing and binary properties", response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
