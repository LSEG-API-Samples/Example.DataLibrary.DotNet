using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Request;
using System;

namespace _3._2._02_Endpoing_News_Headline_and_Story
{
    // **********************************************************************************************************************
    // 3.2.02-Endpoint-News Headline and Story
    // The following example demonstrates the news headline and story API endpoints.  The code segment retrieves the first
    // headline and story text from the response.
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
                // Create the platform session.
                using ISession session = Configuration.Sessions.GetSession();

                // Open the session
                session.Open();

                // ********************************************************************************************
                // Define the news headline URL - we need this to retrieve the story ID.
                // ********************************************************************************************
                var headlineUrl = "https://api.refinitiv.com/data/news/v1/headlines";

                // Request for the headline based on the following query
                var query = "Tesla searchIn:HeadlineOnly";
                var response = EndpointRequest.Definition(headlineUrl).QueryParameter("query", query).GetData();

                // The headline response will carry the story ID.
                var storyId = GetStoryId(response);
                if (storyId != null)
                {
                    Console.WriteLine($"\nFirst headline returned: {response.Data?.Raw["data"]?[0]["newsItem"]?["itemMeta"]?["title"]?[0]?["$"]}");
                    Console.Write($"Hit <Enter> to retrieve story [{storyId}]: ");
                    Console.ReadLine();

                    // Display the headline and story.  First, define the story endpoint Url.
                    // The URL contains a path token {storyId} which will be replaced by the story ID extracted.
                    var storyUrl = "https://api.refinitiv.com/data/news/v1/stories/{storyId}";

                    // Retrieve and display the story based on the ID we retrieved from the headline
                    DisplayStory(session, EndpointRequest.Definition(storyUrl).PathParameter("storyId", storyId).GetData());
                }
                else
                    Console.WriteLine($"Problems retrieving the story ID:{Environment.NewLine}{response.HttpStatus}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute.");
                Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
                if (e.InnerException is not null) Console.WriteLine(e.InnerException);
                Console.WriteLine("***************");
            }
        }

        // Retrieve the story ID from the first headline
        private static string GetStoryId(IEndpointResponse response)
        {
            string storyId = null;
            if (response.IsSuccess)
            {
                var data = response.Data?.Raw["data"] as JArray;

                // Retrieve the first headline, if available
                storyId = data?[0]?["storyId"]?.ToString();
            }

            return storyId;
        }

        // DisplayHeadlineAndStory
        // Interrogate the response to pull out the story text.
        private static void DisplayStory(ISession session, IEndpointResponse response)
        {
            if (response.IsSuccess)
            {
                Console.Write($"{Environment.NewLine}Story: ");
                if (session is IPlatformSession)
                    Console.WriteLine(response.Data.Raw.SelectToken("newsItem.contentSet.inlineData[0].$"));
                else
                    Console.WriteLine(response.Data.Raw.SelectToken("newsItem.contentSet.inlineData.$"));
            }
            else
                Console.WriteLine($"Failed to retrieve data: {response.HttpStatus}");
        }
    }
}
