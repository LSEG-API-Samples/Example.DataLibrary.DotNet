using Common_Examples;
using LSEG.Data.Content.Symbology;
using LSEG.Data.Core;
using System;
using Configuration;

namespace _2._5._01_Symbology_Convert
{
    // **********************************************************************************************************************
    // 2.5.01-Symbology-Convert
    // The following example demostrates conversions of instrument types such as RICs, SEDOLs, ISINs, PermIDs from 1 type
    // to 1 or many primary types.
    //
    // Symbol conversion is based on Search.  Clients on the desktop have full access to Search features and thus all symbol
    // conversion capabilities. Clients on the platform (RDP) do not have access to the same services and when using symbol
    // lookup will not have access to symbol auto-detection. Auto-detection is the ability for the service to automatically
    // detect the symbol type based on the symbol format, i.e. no need to specify the .FromSymbolType specification. To
    // demonstrate this feature, refer to the last example below.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: Desktop
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session

                //
                using ISession session = Sessions.GetSession();

                // Open the session
                if (session.Open() == Session.State.Opened)
                {
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
                    // Note: Auto-detection is only available using a Desktop session.
                    if (session is IDesktopSession)
                    {
                        response = SymbolConversion.Definition().Symbols("IBM", "US5949181045", "037833100", "BH4HKS3")
                                                                .GetData();
                        Common.DisplayTable("Detect and convert symbols of mixed type", response);
                    }
                }
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
