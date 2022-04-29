using Common_Examples;
using Refinitiv.Data;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using Configuration;
using System;

namespace _2._6._01_Search_SearchService
{
    // **********************************************************************************************************************
    // 2.6.01-SearchService-Search
    // The following example demonstrates some basic capabilities of search showing the different views and how you can 
    // filter and query within a view to pull out results.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Credentials to define your login credentials for the desktop.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            Log.Level = NLog.LogLevel.Trace;
            try
            {
                // Create a session into the desktop
                // Note: The full Search service is only available on the desktop.
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);
                session.Open();

                // Search for IBM bonds - basic query
                var request = Search.Definition().View(Search.View.SearchAll)
                                                 .Query("IBM bonds");
                Common.DisplayTable("IBM Bonds - Basic query", request.GetData());

                // Search for active RICs on LSE - ignore derivatives
                request = Search.Definition().View(Search.View.SearchAll)
                                             .Filter("TickerSymbol eq 'LSE' and AssetType ne 'derivative' and AssetState eq 'AC'")
                                             .Select("AssetState, DTSubjectName, DTSimpleType, DTSource, DTCharacteristics, RIC, SearchAllCategoryv3, AssetType");
                Common.DisplayTable("Active RICs containing 'LSE'.  Ignore derivatives.", request.GetData());

                // Search for IBM bonds - Select a View within the full service
                request = Search.Definition().View(Search.View.GovCorpInstruments)
                                             .Filter("IssuerTicker eq 'IBM' and IsActive eq true and AssetStatus ne 'MAT'")
                                             .Select("ISIN,RIC,IssueDate,Currency,FaceIssuedTotal,CouponRate,MaturityDate");
                Common.DisplayTable("IBM Bonds - Active bonds that have not matured", request.GetData());

                // Search for Top CEOs where apple appears in the document - Display DocumentTitle and its subtype/components.
                request = Search.Definition(Search.View.People).Query("CEO Apple")
                                                               .Top(3)
                                                               .Select("DocumentTitle, DTSubjectName, DTSimpleType, DTCharacteristics");
                Common.DisplayTable("Top 3 results for CEOs related with the term 'Apple'", request.GetData());

                // Oil refineries in Venezula, Especially ones that arzen't currently operational (Boost plant status).
                request = Search.Definition(Search.View.PhysicalAssets).Filter("RCSAssetTypeLeaf eq 'oil refinery' and RCSRegionLeaf eq 'Venezuela'")
                                                                        .Boost("PlantStatus ne 'Normal Operation'")
                                                                        .Select("DocumentTitle, PlantStatus")
                                                                        .Top(100);
                Common.DisplayTable("Listing of Oil refineries in Venezula, highlight those currently operational", request.GetData());

                // The youngest CEO's
                request = Search.Definition(Search.View.People).Query("ceo")
                                                               .OrderBy("YearOfBirth desc,LastName,FirstName")
                                                               .Select("YearOfBirth,DocumentTitle");
                Common.DisplayTable("The youngest CEO's", request.GetData());

                // Navigators - basic usage
                request = Search.Definition().View(Search.View.SearchAll)
                                             .Top(0)
                                             .Navigators("RCSTRBC2012Leaf");
                Common.DisplayTable("List the industry sectors available within Search", request.GetData());

                // Navigators - advanced usage
                request = Search.Definition(Search.View.GovCorpInstruments).Top(0)
                                                                           .Navigators("Currency(buckets:5,desc:sum_FaceOutstandingUSD,calc:max_CouponRate)");
                Common.DisplayTable("Top Currencies for Gov Corp Bonds, ranked by Total Outstanding value, along with maximum coupon of each:", request.GetData());

                // Top 2 rate indicators for each central bank
                request = Search.Definition(Search.View.IndicatorQuotes).Query("rates")
                                                                        .GroupBy("CentralBankName")
                                                                        .GroupCount(2)
                                                                        .Select("CentralBankName,DocumentTitle,RIC");
                Common.DisplayTable("Top 2 rate indicators for each central bank", request.GetData());

                // Top 10 Gov Corp instruments for specific range of coupon rates and ratings.  Note: "_" in Select means to include default fields.
                var filter = "CouponRate gt 4 and RatingsScope(RatingType eq 'FSU' and CurrentRatingRank gt 15) and AssetState eq 'AC'";
                request = Search.Definition(Search.View.GovCorpInstruments).Top(10)
                                                                           .Filter(filter)
                                                                           .Select("_, RatingsScope(filter:((RatingType xeq 'FSU'))), RatingX1XRatingRank");
                var response = request.GetData();
                if (response.IsSuccess)
                    Console.WriteLine(response.Data.Raw);
                else
                    Console.WriteLine(response.HttpStatus);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
