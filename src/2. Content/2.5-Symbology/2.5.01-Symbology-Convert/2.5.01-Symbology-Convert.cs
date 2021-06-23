using Common_Examples;
using Refinitiv.Data.Content.Symbology;
using Refinitiv.Data.Core;
using System;

namespace _2._5._01_Symbology_Convert
{
    // **********************************************************************************************************************
    // 2.5.01-Symbology-Convert
    // The following example demostrates conversions of instrument types such as RICs, SEDOLs, ISINs, PermIDs, etc from 1
    // type to 1 or many primary types.
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
                // Create a session into the platform
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    session.Open();

                    // Convert 4 symbol types (ticker, ISIN, CUSIP, SEDOL)
                    var response = SymbolConversion.Definition().Symbols("IBM", "US5949181045", "037833100", "BH4HKS3")
                                                                .GetData();
                    Common.DisplaySymbology(response, "Detect and convert symbols of mixed type");

                    // ISIN to RIC conversion
                    response = SymbolConversion.Definition().Symbols("US5949181045", "US02079K1079")
                                                            .FromSymbolType(SymbolConversion.SymbolType.ISIN)
                                                            .ToSymbolType(SymbolConversion.SymbolType.RIC)
                                                            .GetData();
                    Common.DisplaySymbology(response, "ISIN to RIC conversion for 2 items:");

                    // ISINs - convert to RIC and Ticker only.  Include 1 bad ISIN.
                    response = SymbolConversion.Definition().Symbols("US5949181045", "JUNK", "US02079K1079")
                                                            .FromSymbolType(SymbolConversion.SymbolType.ISIN)
                                                            .ToSymbolType(SymbolConversion.SymbolType.RIC, SymbolConversion.SymbolType.Ticker)
                                                            .GetData();
                    Common.DisplaySymbology(response, "ISIN Lookup for 2 valid items, 1 invalid item - convert to RIC and Ticker only:");

                    // LipperID conversion
                    response = SymbolConversion.Definition("68384554").FromSymbolType(SymbolConversion.SymbolType.LipperID)
                                                                      .GetData();
                    Common.DisplaySymbology(response, "Lipper ID conversion:");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
