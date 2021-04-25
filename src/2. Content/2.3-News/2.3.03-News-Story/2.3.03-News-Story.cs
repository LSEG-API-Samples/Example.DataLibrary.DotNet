using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using System;

namespace _2._3._03_News_Story
{
    // **********************************************************************************************************************
    // 2.3.03-News-Story
    // The following example presents a news story based on a specific headline.
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
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    if (session.Open() == Session.State.Opened)
                    {

                        // Retrieve the most recent headline about Apple
                        var headline = Headlines.Definition().Query("L:EN and Apple")
                                                             .Count(1)
                                                             .GetData();

                        if (headline.IsSuccess)
                        {
                            // Retrieve the story based on the story ID
                            var story = Story.Definition(headline.Data.Headlines[0].StoryId).GetData();

                            Console.WriteLine($"\nHeadline: {headline.Data.Headlines[0].HeadlineTitle}");

                            if (story.IsSuccess)
                                Console.WriteLine($"\nStory: {story.Data.NewsStory}");
                            else
                                Console.WriteLine($"Problem retrieving the story: {story.HttpStatus}");
                        }
                        else
                            Console.WriteLine($"Problem retrieving the headline: {headline.HttpStatus}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
