using Common_Examples;
using LSEG.Data.Content.Data;
using LSEG.Data.Core;
using Configuration;
using System;

namespace _2._8._01_FundamentalAndReference_Basics
{
    class Program
    {
        // **********************************************************************************************************************
        // 2.8.01-FundamentalAndReference-Basics
        // The following example provides some basic examples of using the Fundamental and References services to retrieve data.
        //
        // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
        //      1. Configuration.Session to specify the access channel into the platform. Default: Desktop
        //      2. Configuration.Credentials to define your login credentials for the specified access channel.
        // **********************************************************************************************************************
        static void Main(string[] _)
        {
            Common.ShowUniverse = false;    // Display setting

            try
            {
                // Create a session into the platform
                using ISession session = Sessions.GetSession();

                // Open the session
                session.Open();

                // Reference data
                var response = FundamentalAndReference.Definition().Universe("TRI.N", "IBM.N")
                                                                   .Fields("TR.Revenue", "TR.GrossProfit")
                                                                   .GetData();
                Common.DisplayTable("Reference Data", response);

                // Fundamental fields with parameters
                response = FundamentalAndReference.Definition().Universe("LSEG.L", "VOD.L")
                                                               .Fields("TR.PriceTargetMean(Source=ThomsonReuters)")
                                                               .GetData();
                Common.DisplayTable("Reference Data with parameters", response);

                // Chain expansion (S&P 500 - sorted in descending order based on the market cap)
                var mktCap = new FRField("TR.CompanyMarketCap").SortDirection(FRField.SortDirectionType.Desc);
                response = FundamentalAndReference.Definition("0#.SPX").Fields(mktCap).GetData();
                Common.DisplayTable("S&P 500", response, 0, 25);

                // S&P for a defined financial period
                var period = "2010-06-01";
                response = FundamentalAndReference.Definition().Universe($"0#.SPX({period})")
                                                               .Fields(mktCap)
                                                               .Parameters(new Newtonsoft.Json.Linq.JObject()
                                                               {
                                                                   ["SDATE"] = period
                                                               }).GetData();
                Common.DisplayTable($"S&P 500 for the financial period: {period}", response, 0, 25);

                // Average close price over last 5 days
                response = FundamentalAndReference.Definition("IBM.N").Fields("TR.PriceClose(SDate=-5D).date", "TR.PriceClose(SDate=-5D)",
                                                                              "AVG(TR.TSVWAP(SDate=0,EDate=-5,Frq=D).value)")
                                                                       .GetData();
                Common.DisplayTable("Avg Price last 5 days", response);

                // Pricing information
                response = FundamentalAndReference.Definition("AAPL.O").Fields("BID", "CF_BID", "CF_ASK", "CF_LAST").GetData();
                Common.DisplayTable("Pricing", response);

                // Peers
                var name = new FRField("TR.CommonName");
                response = FundamentalAndReference.Definition("Peers(VOD.L)").Fields(name, mktCap)
                                                                             .GetData();
                Common.DisplayTable("Peers", response);

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
