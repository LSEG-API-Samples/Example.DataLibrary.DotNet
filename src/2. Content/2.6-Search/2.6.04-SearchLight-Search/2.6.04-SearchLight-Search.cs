using Common_Examples;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using Configuration;
using System;

namespace _2._6._04_SearchService_Search
{
    // **********************************************************************************************************************
    // 2.6.04-SearchLight-Search
    // The following example demonstrates some basic capabilities of the search light service showing the different views and
    // how you can filter and query within a view to pull out results.
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
                // Note: The Search Light service is available to Wealth clients.
                using ISession session = Sessions.GetSession();
                session.Open();

                // Search for all the RICs where 'LSE' is in the name, exclude all derivatives and ensure the state is active (AC)
                var request = Search.Definition().View(Search.LightView.EquityQuotes)
                                                 .Filter("TickerSymbol eq 'LSE' and AssetType ne 'derivative' and AssetState eq 'AC'")
                                                 .Select("DocumentTitle, AssetState, RIC, SearchAllCategoryv3, AssetType");
                Common.DisplayTable($"Active RICs containing 'LSE'.  Ignore derivatives.", request.GetData());

                request = Search.Definition().View(Search.LightView.SearchAllLight)
                                             .Filter("IssuerName eq 'US Treasury' and Currency eq 'USD' and SearchAllCategoryv3 eq 'Bonds' and IsActive eq true")
                                             .Select("RIC,ISIN,IssuerName,MaturityDate,CouponRate");
                Common.DisplayTable("Active U.S Treasury Bonds", request.GetData());

                // Top currencies for Gov Corp Bonds
                request = Search.Definition(Search.LightView.GovCorpInstruments).Top(0)
                                                                                .Navigators("Currency(buckets:5,calc:max_CouponRate)");
                Common.DisplayTable("Top Currencies for Gov Corp Bonds, ranked by Total Outstanding value, along with maximum coupon of each:", request.GetData());

                // Top 2 rate indicators for each central bank
                request = Search.Definition(Search.LightView.IndicatorQuotes).Query("rates")
                                                                             .GroupCount(2)
                                                                             .Select("DocumentTitle,RIC");
                Common.DisplayTable("Top 2 rate indicators for each central bank", request.GetData());

                // Find any data on Bill Gates
                request = Search.Definition(Search.LightView.SearchAllLight).Query("Bill Gates");
                Common.DisplayTable("Any data on Bill Gates", request.GetData());
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
