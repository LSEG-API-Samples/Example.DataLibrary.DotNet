using Common_Examples;
using Configuration;
using Refinitiv.Data.Content.SearchService;
using Refinitiv.Data.Core;
using System;

namespace _2._6._03_Search_MetaData
{
    // **********************************************************************************************************************
    // 2.6.03-SearchService-MetaData
    // The following example demonstrates some basic capabilities of the metaata service in search listing all available 
    // properties within a specified Search View.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Credentials to define your login credentials for the desktop.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Note: The full Search service is only available on the desktop.
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

                if (session.Open() == Session.State.Opened)
                {
                    // Request the metadata properties for the GovCorpInstruments View
                    var response = MetaData.Definition(Search.View.GovCorpInstruments).GetData();

                    if (response.IsSuccess)
                    {
                        var table = response.Data.Table;
                        Common.DisplayTable($"MetaData display of Light View: {Search.LightView.GovCorpInstruments}", response, 0, 25);

                        // Search for a specific property
                        var result = table.Select("Property = 'RCSAssetCategory'");
                        Console.WriteLine($"Property {result[0]["Property"]}: Navigable: {result[0]["Navigable"]}, Sortable: {result[0]["Sortable"]}");

                        result = table.Select("Property = 'RCSCountryGenealogy'");
                        Console.WriteLine($"Property {result[0]["Property"]}: Navigable: {result[0]["Navigable"]}, Sortable: {result[0]["Sortable"]}");

                        result = table.Select("Property = 'SPsRatingsScope.CurrentRatingRank'");
                        Console.WriteLine($"Property {result[0]["Property"]}: Navigable: {result[0]["Navigable"]}, Sortable: {result[0]["Sortable"]}");

                        // Search for a specific nested property and all its sub properties
                        result = table.Select("Property LIKE 'RatingsScope.*'");
                        Common.DisplayRows(table.Columns, result);
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
