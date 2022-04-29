using Common_Examples;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using Configuration;
using System;

namespace _2._6._07_SearchExplore_Search
{
    // **********************************************************************************************************************
    // 2.6.07-SearchExplore-Search
    // The following example demonstrates capabilities of the basic search explore service.
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
                // Note: The Search Explore service is available to all clients.
                using ISession session = Sessions.GetSession();
                session.Open();

                // Note: The default Search View is Search.ExploreView.Entities

                // Search for all the RICs where 'LSE' is in the name, exclude all derivatives and ensure the state is active (AC)
                var request = Search.Definition().Filter("TickerSymbol eq 'LSE' and AssetType ne 'derivative' and AssetState eq 'AC'")
                                                 .Select("AssetState, DTSubjectName, DTSimpleType, DTSource, DTCharacteristics, RIC, SearchAllCategoryv3, AssetType");
                Common.DisplayTable($"Active RICs containing 'LSE'.  Ignore derivatives.", request.GetData());

                // Oil refineries in Venezula.
                request = Search.Definition().Query("oil refinery Venezuela")
                                             .Select("DocumentTitle, RIC, AssetState")
                                             .Top(100);
                Common.DisplayTable($"Oil refineries in Venezula", request.GetData());

                // Currencies
                request = Search.Definition().Top(0)
                                             .Navigators("RCSCurrencyLeaf(buckets:5)");
                Common.DisplayTable("Top Currencies:", request.GetData());

                // Top 2 rate indicators for each central bank
                request = Search.Definition().Query("rates")
                                             .Select("DTSource,DocumentTitle,RIC");
                Common.DisplayTable("Top 2 rate indicators for each central bank", request.GetData());

                // General listing of the Catalog Items 
                request = Search.Definition(Search.ExploreView.CatalogItems);
                Common.DisplayTable("General listing of the Catalog Items", request.GetData());
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
