using Refinitiv.Data.Content.Symbology;
using Refinitiv.Data.Core;
using System;
using System.Collections.Generic;

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

                    // RICs - convert multiple RICs to pre-defined instrument types
                    var response = SymbolConversion.Definition("IBM", "AAPL.O", "GOOGL.O", "LP60000008").GetData();
                    DisplayResponse($"\nRIC Lookup for 4 valid items:", response);

                    // ISINs - convert to all known symbol types  (Note: Can we detect the Symbol types?)
                    response = SymbolConversion.Definition("US5949181045", "US02079K1079").FromSymbolType(SymbolConversion.SymbolType.ISIN).GetData();
                    DisplayResponse("\nISIN Lookup for 2 valid items:", response);

                    // ISINs - convert to RIC and Ticker only.  Include 1 bad ISIN.
                    response = SymbolConversion.Definition("US5949181045", "JUNK", "US02079K1079").FromSymbolType(SymbolConversion.SymbolType.ISIN)
                                                                                                  .ToSymbolType(SymbolConversion.SymbolType.RIC,
                                                                                                                SymbolConversion.SymbolType.Ticker)
                                                                                                  .GetData();
                    DisplayResponse("\nISIN Lookup for 2 valid items, 1 invalid item - convert to RIC and Ticker only:", response);

                    // SEDOL - convert to all known
                    response = SymbolConversion.Definition("BH4HKS3").FromSymbolType(SymbolConversion.SymbolType.SEDOL).GetData();
                    DisplayResponse("\nSEDOL lookup:", response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void DisplayResponse(string label, ISymbolConversionResponse response)
        {
            Console.WriteLine($"\n{label}\n");
            if (response.IsSuccess)
            {
                // Matches
                foreach (KeyValuePair<string, ISymbolConversionMatches> match in response.Data.Matches)
                {
                    Console.WriteLine($"{match.Key} - {match.Value.Description}");
                    if (match.Value.RIC != null) Console.WriteLine($"\tRIC: {match.Value.RIC}");
                    if (match.Value.ISIN != null) Console.WriteLine($"\tISIN: {match.Value.ISIN}");
                    if (match.Value.CUSIP != null) Console.WriteLine($"\tCUSIP: {match.Value.CUSIP}");
                    if (match.Value.PermID != null) Console.WriteLine($"\tPermID: {match.Value.PermID}");
                    if (match.Value.SEDOL != null) Console.WriteLine($"\tSEDOL: {match.Value.SEDOL}");
                    if (match.Value.LipperID != null) Console.WriteLine($"\tLipperID: {match.Value.LipperID}");
                    if (match.Value.Ticker != null) Console.WriteLine($"\tTicker: {match.Value.Ticker}");
                }

                // Warnings
                foreach (string warning in response.Data.Warnings)
                {
                    Console.WriteLine($"WARNING: {warning}");
                }
            }
            else
            {
                Console.WriteLine(response.HttpStatus);
            }

            Console.Write("\nHit any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
