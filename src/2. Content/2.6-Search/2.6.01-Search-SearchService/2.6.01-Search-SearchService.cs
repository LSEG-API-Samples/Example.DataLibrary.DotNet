using Common_Examples;
using Refinitiv.Data;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using System;

namespace _2._6._01_Search_SearchService
{
    // **********************************************************************************************************************
    // 2.6.01-Search-SearchService
    // The following example demonstrates some basic capabilities of search showing the different views and how you can 
    // filter and query within a view to pull out results.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            Log.Level = NLog.LogLevel.Trace;
            try
            {
                // Create a session into the platform
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    session.Open();

                    // Search for IBM bonds - basic query
                    var response = Search.Definition().Query("IBM bonds").GetData();
                    Common.DisplaySearch(response, "IBM Bonds - Basic query");

                    // Search for active RICs on LSE - ignore derivatives
                    response = Search.Definition().Filter("TickerSymbol eq 'LSE' and AssetType ne 'derivative' and AssetState eq 'AC'")
                                                  .Select("AssetState, DTSubjectName, DTSimpleType, DTSource, DTCharacteristics, RIC, SearchAllCategoryv3, AssetType")
                                                  .GetData();
                    Common.DisplaySearch(response, "Active RICs containing 'LSE'.  Ignore derivatives.");

                    // Search for IBM bonds - Select a View within the full service
                    response = Search.Definition().View(Search.View.GovCorpInstruments)
                                                  .Filter("IssuerTicker eq 'IBM' and IsActive eq true and AssetStatus ne 'MAT'")
                                                  .Select("ISIN,RIC,IssueDate,Currency,FaceIssuedTotal,CouponRate,MaturityDate")
                                                  .GetData();
                    Common.DisplaySearch(response, "IBM Bonds - Active bonds that have not matured");

                    // Top 2 rate indicators for each central bank - Select a View within the Light service
                    response = Search.Definition(Search.LightView.IndicatorQuotes).Query("rates")
                                                                                  .GroupCount(2)
                                                                                  .Select("DocumentTitle,RIC")
                                                                                  .GetData();
                    Common.DisplaySearch(response, "Top 2 rate indicators for each central bank");

                    // Search for Top CEOs where apple appears in the document - Display DocumentTitle and its subtype/components.
                    response = Search.Definition(Search.View.People).Query("CEO Apple")
                                                                    .Top(3)
                                                                    .Select("DocumentTitle, DTSubjectName, DTSimpleType, DTCharacteristics")
                                                                    .GetData();
                    Common.DisplaySearch(response, "Top 3 results for CEOs related with the term 'Apple'");

                    // Oil refineries in Venezula, Especially ones that arzen't currently operational (Boost plant status).
                    response = Search.Definition(Search.View.PhysicalAssets).Filter("RCSAssetTypeLeaf eq 'oil refinery' and RCSRegionLeaf eq 'Venezuela'")
                                                                            .Boost("PlantStatus ne 'Normal Operation'")
                                                                            .Select("DocumentTitle, PlantStatus")
                                                                            .Top(100)
                                                                            .GetData();
                    Common.DisplaySearch(response, "Listing of Oil refineries in Venezula, highlight those currently operational");

                    // The youngest CEO's
                    response = Search.Definition(Search.View.People).Query("ceo")
                                                                    .OrderBy("YearOfBirth desc,LastName,FirstName")
                                                                    .Select("YearOfBirth,DocumentTitle")
                                                                    .GetData();
                    Common.DisplaySearch(response, "The youngest CEO's");

                    // Navigators - basic usage
                    response = Search.Definition().Top(0)
                                                  .Navigators("RCSTRBC2012Leaf")
                                                  .GetData();
                    DisplayNavigators(response, "List the industry sectors available within Search");

                    // Navigators - advanced usage
                    response = Search.Definition(Search.View.GovCorpInstruments).Top(0)
                                                                                .Navigators("Currency(buckets:5,desc:sum_FaceOutstandingUSD,calc:max_CouponRate)")
                                                                                .GetData();
                    DisplayNavigators(response, "Top Currencies for Gov Corp Bonds, ranked by Total Outstanding value, along with maximum coupon of each:");

                    // Top 2 rate indicators for each central bank
                    response = Search.Definition(Search.View.IndicatorQuotes).Query("rates")
                                                                             .GroupBy("CentralBankName")
                                                                             .GroupCount(2)
                                                                             .Select("CentralBankName,DocumentTitle,RIC")
                                                                             .GetData();
                    Common.DisplaySearch(response, "Top 2 rate indicators for each central bank");

                    // Top 10 Gov Corp instruments for specific range of coupon rates and ratings.  Note: "_" in Select means to include default fields.
                    var filter = "CouponRate gt 4 and RatingsScope(RatingType eq 'FSU' and CurrentRatingRank gt 15) and AssetState eq 'AC'";
                    response = Search.Definition(Search.View.GovCorpInstruments).Top(10)
                                                                                .Filter(filter)
                                                                                .Select("_, RatingsScope(filter:((RatingType xeq 'FSU'))), RatingX1XRatingRank")
                                                                                .GetData();
                    Common.DisplaySearch(response, $"{filter}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void DisplayNavigators(ISearchResponse response, string label)
        {
            Console.WriteLine($"\n{label}\n");

            if (response.IsSuccess)
            {
                // Navigators
                if (response.Data.Navigators != null)
                {
                    Console.WriteLine($"Navigators:\n{response.Data.Navigators}");
                }
            }
            else
            {
                Console.WriteLine($"IsSuccess: {response.IsSuccess}\n{response.HttpStatus}");
            }
            Console.Write("\n<Enter> to continue..."); Console.ReadLine();
        }
    }
}
