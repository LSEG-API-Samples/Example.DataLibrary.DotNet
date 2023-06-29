using Common_Examples;
using Refinitiv.Data.Content.Data;
using Refinitiv.Data.Core;
using Configuration;
using System;

namespace _2._8._01_FundamentalAndReference_Basics
{
    class Program
    {
        static void Main(string[] _)
        {
            Common.ShowUniverse = false;    // Display setting

            try
            {
                // Create a session into the desktop
                // Note: The Fundamental and Reference API is only available on the desktop.
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

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
