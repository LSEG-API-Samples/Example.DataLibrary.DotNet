using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._3._05_News_Image
{
    // **********************************************************************************************************************
    // 2.3.05-News-Image
    // The following example fetches news images based on an image ID.
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
                // Create a session into the platform...
                using (ISession session = Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Display Image - hardcoded ID
                    Console.WriteLine("\nImage based on hardcoded ID");
                    RetrieveImage("2022-06-16T224431Z_1_OV4_RTRLXPP_2_LYNXPACKAGER__JPG");

                    // Display Image - based on OnlineReports query
                    string imageId = null;
                    var reportStory = OnlineReports.Definition("OLGBTOPNEWS").GetData();     // US Top News
                    if (reportStory.IsSuccess)
                    {
                        // Walk through the reports until we find an Image
                        foreach (IOnlineReportStory story in reportStory.Data.OnlineReportStories)
                        {
                            // The images are buried within the body of the report
                            var link = story.Raw["newsItem"]?["itemMeta"]?["link"] as JArray;
                            if (link != null)
                            {
                                imageId = (string)link[0]["_residref"];
                                break;
                            }
                        }
                    }

                    if (imageId != null)
                    {
                        Console.WriteLine("\nImage based on US Top News online reports");
                        RetrieveImage(imageId);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void RetrieveImage(string imageId)
        {
            IImageResponse image = Image.Definition(imageId).GetData();
            if (image.IsSuccess)
            {
                Console.WriteLine($"Image represented as a: {image.Data.Image} of length: {image.Data.Image.Length} bytes.");
            }
            else
            {
                Console.WriteLine($"Failed to retrieve image ID: {imageId}\n{image.HttpStatus}");
            }
        }
    }
}
