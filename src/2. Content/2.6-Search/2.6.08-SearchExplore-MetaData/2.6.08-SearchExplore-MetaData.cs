using Common_Examples;
using Configuration;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using System;

namespace _2._6._08_SearchExplore_MetaData
{
    // **********************************************************************************************************************
    // 2.6.08-SearchExplore-MetaData
    // The following example demonstrates some basic capabilities of the search explore service listing all available 
    // properties within a specified Search View.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            // Note: The Search Explore service is available to all clients.
            using ISession session = Sessions.GetSession();
            if (session.Open() == Session.State.Opened)
            {
                // Request the metadata properties for the default View: Search.ExploreView.Entities
                var response = MetaData.Definition().GetData();

                if (response.IsSuccess)
                {
                    var table = response.Data.Table;
                    Common.DisplayTable($"MetaData display of Explore View: {Search.ExploreView.Entities}", response, 0, 25);

                    // Search for a specific property
                    var result = table.Select("Property = 'RCSAssetCategory'");
                    Console.WriteLine($"Property {result[0]["Property"]}: Navigable: {result[0]["Navigable"]}, Sortable: {result[0]["Sortable"]}");

                    result = table.Select("Property = 'RCSCountryGenealogy'");
                    Console.WriteLine($"Property {result[0]["Property"]}: Navigable: {result[0]["Navigable"]}, Sortable: {result[0]["Sortable"]}");
                }
            }
        }
    }
}
