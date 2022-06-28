using Common_Examples;
using Refinitiv.Data.Content.Symbology;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._5._01_Symbology_Convert
{
    // **********************************************************************************************************************
    // 2.5.01-Symbology-Convert
    // The following example demostrates conversions of instrument types such as RICs, SEDOLs, ISINs, PermIDs, etc from 1
    // type to 1 or many primary types.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Credentials to define your login credentials for the desktop.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session
                // Note: Symbol conversion is based on Search.  Clients on the desktop have access to full conversion
                //       capabilities.  Clients connecting directly to the platform are licensed as Wealth clients. The
                //       only distinquishable difference between the 2-flavours of conversion is that the
                //       full version provides a way to detect the symbol types automatically.  See the last example.
                //       All examples will work if the session is on the desktop.  For our Wealth clients connecting
                //       directly into RDP, see the first example. Feel free to change the Service Type if connecting
                //       to RDP.
                using ISession session = Sessions.GetSession(); session.Open();

                // ISIN to RIC conversion (
                var response = SymbolConversion.Definition().Symbols("US5949181045", "US02079K1079")
                                                            .FromSymbolType(SymbolConversion.SymbolType.ISIN)
                                                            .ToSymbolType(SymbolConversion.SymbolType.RIC)
                                                            .GetData();
                Common.DisplayTable("ISIN to RIC conversion for 2 items:", response);

                // ISINs - convert to RIC and Ticker only.  Include 1 bad ISIN.
                response = SymbolConversion.Definition().Symbols("US5949181045", "JUNK", "US02079K1079")
                                                        .FromSymbolType(SymbolConversion.SymbolType.ISIN)
                                                        .ToSymbolType(SymbolConversion.SymbolType.RIC, SymbolConversion.SymbolType.Ticker)
                                                        .GetData();
                Common.DisplayTable("ISIN Lookup for 2 valid items, 1 invalid item - convert to RIC and Ticker only:", response);

                // LipperID conversion - convert ID to all available types
                response = SymbolConversion.Definition("68384554").FromSymbolType(SymbolConversion.SymbolType.LipperID)
                                                                  .GetData();
                Common.DisplayTable("Lipper ID conversion:", response);

                // Detect and Convert 4 symbol types (ticker, ISIN, CUSIP, SEDOL)
                // Note: Auto-detection is only available in the Desktop service type
                if (session is IDesktopSession)
                {
                    response = SymbolConversion.Definition().Symbols("IBM", "US5949181045", "037833100", "BH4HKS3")
                                                            .GetData();
                    Common.DisplayTable("Detect and convert symbols of mixed type", response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
