using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Queue;
using System;

namespace _3._3._02_Queue_NewsStories
{
    // **********************************************************************************************************************
    // 3.3.02-Queue-NewsStories
    // The Refinitiv Data Platform defines a service that utilizes Cloud-based Queuing to deliver realtime messages.
    // The following example demonstrates how to manage (retrieve/create/delete) a queue to deliver news headlines and stories.  
    // When creating the queue, the example will use the default criteria to filter no stories.  
    // Prior to exit, the user is presented with the decision to delete the queue.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            const string newsStoriesEndpoint = "https://api.refinitiv.com/message-services/v1/news-stories/subscriptions";

            try
            {
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Create our stories definition
                    var definition = Queue.Definition(newsStoriesEndpoint);

                    // Create a QueueManager to actively manage our queues
                    IQueueManager manager = definition.CreateQueueManager().OnError((err, qm) => Console.WriteLine(err));

                    // First, check to see if we have any news headline queues active in the cloud...
                    var queues = manager.GetAllQueues();

                    // If no existing queue exists, create one.  
                    IQueueNode queue = (queues.Count > 0 ? queues[0] : manager.CreateQueue());

                    // Ensure our queue is created
                    if (queue != null)
                    {
                        Console.WriteLine($"{Environment.NewLine}{(queues.Count > 0 ? "Using existing" : "Created a new")} queue.  Waiting for stories...");

                        // Subscribe to the queue.
                        // Note: The subscriber interface has 2 mechanisms to retrieve data from the queue.  The first mechanism is to selectively
                        //       poll the queue for new messages.  The second mechanism is to define a callback/lambda expression and notify the
                        //       the subscriber to poll for messages as they come in - this mechansim provides a near realtime result.
                        //
                        // The following example demonstrates the second mechanism.
                        IQueueSubscriber subscriber = definition.CreateAWSSubscriber(queue);

                        // Open the subscriber to begin polling for messages. Use Async() as this method is a long running task.
                        var task = subscriber.StartPollingAsync((story, s) => DisplayStory(story));
                        Console.ReadKey();

                        // Close the subscription - stops polling for messages
                        subscriber.StopPolling();
                        task.GetAwaiter().GetResult();
                        Console.WriteLine("Stopped polling for messages from the queue.");

                        // Prompt the user to delete the queue
                        Console.Write("Delete the queue (Y/N) [N]: ");
                        var delete = Console.ReadLine();
                        if (delete?.ToUpper() == "Y")
                        {
                            if (manager.DeleteQueue(queue))
                                Console.WriteLine("Successfully deleted queue.");
                            else
                                Console.WriteLine($"Issues deleting queue.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        // Display Story
        // The news msg is a very rich, complex data structure containing news metadata and other elements related to
        // a news story.  This routine simply pulls out content that can be easily displayed on the console.
        private static void DisplayStory(IQueueResponse response)
        {
            try
            {
                if (response.IsSuccess)
                {
                    var msg = response.Data.Raw;

                    // Determine if the headline is usable, i.e. if we want to display it
                    var pubStatus = msg["payload"]?["newsItem"]?["itemMeta"]?["pubStatus"]?["_qcode"]?.ToString();
                    if (pubStatus is string && pubStatus.Contains("usable"))
                    {
                        DateTime local = DateTime.Parse(msg["distributionTimestamp"].ToString()).ToLocalTime();

                        // Pull out the headline
                        var headline = msg["payload"]?["newsItem"]?["contentMeta"]?["headline"] as JArray;

                        if (headline?.Count > 0 && headline[0]["$"] is JToken)
                            Console.WriteLine($"Headline: {local} => {headline[0]["$"]}");

                        // Pull out the story
                        var story = msg["payload"]?["newsItem"]?["contentSet"]?["inlineData"] as JArray;
                        if (story?.Count > 0 && story[0]["$"] is JToken)
                        {
                            var type = story[0]["_contenttype"];
                            if (type != null && type.ToString().Equals("text/plain"))
                            {
                                JToken lang = story[0]["_xml:lang"];
                                var text = story[0]["$"].ToString();
                                var max = text.Length >= 40 ? 40 : text.Length;
                                Console.WriteLine($"\tContent Type: [{type}]\tLang: [{lang ?? "No language"}]. Story: {text.Substring(0, max)}...");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Response failed: {response.Error}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("*************************************************");
                Console.WriteLine(e);
                Console.WriteLine($"Response: [{response}]");
                Console.WriteLine(response.Data?.Raw);
                Console.WriteLine("*************************************************");
                Environment.FailFast("Exiting with exception");
            }
        }
    }
}
