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
            try
            {
                // Create a session into the platform
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    session.Open();

                    // Search for all the RICs where 'LSE' is in the name, exclude all derivatives and ensure the state is active (AC)
                    var request = Search.Definition(Search.View.Quotes).Filter("TickerSymbol eq 'LSE' and AssetType ne 'derivative' and AssetState eq 'AC'");
                    DisplayResponse($"\nActive RICs containing 'LSE'.  Ignore derivatives.", request, "RIC");

                    // Search for Top CEOs where apple appears in the document - Display DocumentTitle and its subtype/components.
                    request = Search.Definition(Search.View.People).Query("CEO Apple")
                                                                   .Top(3)
                                                                   .Select("DocumentTitle, DTSubjectName, DTSimpleType, DTCharacteristics");
                    DisplayResponse($"\nTop 3 results for CEOs related with the term 'Apple':", request, "DTSubjectName", "DTSimpleType", "DTCharacteristics");

                    // Oil refineries in Venezula, Especially ones that arzen't currently operational (Boost plant status).
                    request = Search.Definition(Search.View.PhysicalAssets).Filter("RCSAssetTypeLeaf eq 'oil refinery' and RCSRegionLeaf eq 'Venezuela'")
                                                                           .Boost("PlantStatus ne 'Normal Operation'")
                                                                           .Select("DocumentTitle, PlantStatus")
                                                                           .Top(100);
                    DisplayResponse($"\nListing of Oil refineries in Venezula, especially ones that aren't currently operational:", request, "PlantStatus");

                    // The youngest CEO's
                    request = Search.Definition(Search.View.People).Query("ceo")
                                                                   .OrderBy("YearOfBirth desc,LastName,FirstName")
                                                                   .Select("YearOfBirth,DocumentTitle");
                    DisplayResponse($"\nThe youngest CEO's:", request, "YearOfBirth");

                    // Top currencies for Gov Corp Bonds
                    request = Search.Definition(Search.View.GovCorpInstruments).Top(0)
                                                                               .Navigators("Currency(buckets:5,desc:sum_FaceOutstandingUSD,calc:max_CouponRate)");
                    DisplayResponse("\nTop Currencies for Gov Corp Bonds, ranked by Total Outstanding value, along with maximum coupon of each:", request);

                    // Top 2 rate indicators for each central bank
                    request = Search.Definition(Search.View.IndicatorQuotes).Query("rates")
                                                                            .GroupBy("CentralBankName")
                                                                            .GroupCount(2)
                                                                            .Select("CentralBankName,DocumentTitle,RIC");
                    DisplayResponse("\nTop 2 rate indicators for each central bank", request, "CentralBankName", "RIC");

                    // Top 10 Gov Corp instruments for specific range of coupon rates and ratings.  Note: "_" in Select means to include default fields.
                    var filter = "CouponRate gt 4 and RatingsScope(RatingType eq 'FSU' and CurrentRatingRank gt 15) and AssetState eq 'AC'";
                    request = Search.Definition(Search.View.GovCorpInstruments).Top(10)
                                                                               .Filter(filter)
                                                                               .Select("_, RatingsScope(filter:((RatingType xeq 'FSU'))), RatingX1XRatingRank");
                    DisplayResponse($"\n{filter}", request);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void DisplayResponse(string label, ISearchDefinition request, params string[] display)
        {
            Console.WriteLine($"\n{label}\n");

            var response = request.GetData();

            if (response.IsSuccess)
            {
                // Warnings
                foreach (string warning in response.Data.Warnings)
                {
                    Console.WriteLine($"**** {warning}");
                }

                // Total docs in matching set
                Console.WriteLine($"\nMax available hits: {response.Data.Total}.");
                Console.WriteLine($"Requested for {RequestDetails(request.Properties.Top)} hits.  Received: {response.Data.Hits.Count}\n");

                // Echo each hit
                int hitCnt = 0;
                foreach (var hit in response.Data.Hits)
                {
                    Console.WriteLine($"{++hitCnt}. {hit["DocumentTitle"]}");

                    foreach (string val in display)
                    {
                        Console.WriteLine($"\t{val}: {hit[val]}");
                    }
                }

                // Navigators
                if (response.Data.Navigators != null)
                {
                    Console.WriteLine($"Navigators:\n{response.Data.Navigators}");
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

        private static string RequestDetails(uint? top)
        {
            if (top == null)
                return $"10 (Default)";

            return (top == 0 ? "zero" : $"{top}");
        }
    }
}
