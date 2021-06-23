using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.News;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Queue;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2._3._07_News_Analyze
{
    // **********************************************************************************************************************
    // 2.3.07-News-Analyze
    // The News Analyze service converts a textual news query into a JSON tree expression used by other news services as a
    // means to filter the news of interest.  For example, the queue-based services, news headlines or headlines and stories,
    // utilize a tree expression that filter data delivered to message queues within the cloud.
    //
    // The following example demonstrates how to request for news headlines from the platforms' message services using a
    // simple news query expression. The Analyze service will perform the task of converting the query into a JSON tree
    // expression - see CreateQueue() below.  The example will (retrieve/create/delete) a queue to deliver news headlines.
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
            const string newsHeadlinesEndpoint = "https://api.refinitiv.com/message-services/v1/news-headlines/subscriptions";

            try
            {
                // Create the platform session.
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Create a queue definition
                    var definition = Queue.Definition(newsHeadlinesEndpoint);

                    // Create a QueueManager to actively manage our queues
                    IQueueManager manager = definition.CreateQueueManager().OnError((err, qm) => Console.WriteLine(err));

                    // First, check to see if we have any news headline queues active in the cloud...
                    List<IQueueNode> queues = manager.GetAllQueues();

                    // Determine if we retrieved an active headline queue...create one if not.
                    IQueueNode queue;
                    if (queues.Count > 0)
                        queue = queues[0];
                    else
                        queue = CreateQueue(manager, "AA");      // Create a Queue with the new query expression ("AA" - alerts only)

                    // Ensure our queue is created
                    if (queue != null)
                    {
                        Console.WriteLine($"{Environment.NewLine}{(queues.Count > 0 ? "Using existing" : "Created a new")} queue.  Waiting for headlines...");

                        // Subscribe to the queue.
                        // Note: The subscriber interface has 2 mechanisms to retrieve data from the queue.  The first mechanism is to selectively
                        //       poll the queue for new messages.  The second mechanism is to define a callback/lambda expression and notify the
                        //       the subscriber to poll for messages as they come in - this mechansim provides a near realtime result.
                        //
                        // The following example demonstrates the first mechanism.
                        var subscriber = definition.CreateAWSSubscriber(queue);

                        // Poll the queue until we hit any key on the keyboard.
                        // Each poll will timeout after 2 seconds if no messages arrive.
                        while (!Console.KeyAvailable)
                        {
                            subscriber.GetNextMessage(2, (headline, s) => DisplayHeadline(headline));
                        }
                        Console.ReadKey();

                        // Prompt the user to delete the queue
                        Console.Write("\nDelete the queue (Y/N) [N]: ");
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


        // CreateQueue
        // The IQueueManager interface can accept a news expression to filter the type of news based on a JSON interface that
        // contains complicated expressions.  To simplify the specification of an expresion, RDP offers a news Analyze endpoint
        // that will take a familar desktop news expression and turn it into a JSON representation required by the queue manager.
        private static IQueueNode CreateQueue(IQueueManager manager, string newsExpression)
        {
            // Analyze the following news query string
            var analyze = Analyze.Definition(newsExpression).GetData();
            if (analyze.IsSuccess)
            {
                return manager.CreateQueue(analyze.Data);
            }
            else
            {
                Console.WriteLine($"Issues analyzing newsExpression [{newsExpression}\n{analyze.HttpStatus}\nCreating queue with no criteria.");
                return manager.CreateQueue();
            }
        }

        // DisplayHeadline
        // The raw message from the platform is interrogated to pull out the headline.  The 'pubStatus' indicator determines
        // if the headline is 'usable'.  A non-usable headline (canceled or withdrawn) will not carry any headline title to display.
        private static void DisplayHeadline(IQueueResponse response)
        {
            try
            {
                if (response.IsMessageAvailable)
                {
                    var msg = response.Data.Raw;

                    // Determine if the headline is usable, i.e. if we want to display it
                    var pubStatus = msg["payload"]?["newsItem"]?["itemMeta"]?["pubStatus"]?["_qcode"]?.ToString();
                    if (pubStatus is string && pubStatus.Contains("usable"))
                    {
                        DateTime local = DateTime.Parse(msg["distributionTimestamp"].ToString()).ToLocalTime();

                        // Determine if this is an actual headline
                        JArray headline = msg["payload"]?["newsItem"]?["contentMeta"]?["headline"] as JArray;
                        if (headline?.Count > 0 && headline[0]["$"] is JToken)
                            Console.WriteLine($"{local}: {headline[0]["$"]}".Indent(110));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("*************************************************");
                Console.WriteLine(e);
                Console.WriteLine(response.Data.Raw);
                Console.WriteLine("*************************************************");
            }
        }
    }

    public static class StringExtensions
    {
        public static string Indent(this string value, int maxSize)
        {
            StringBuilder sb = new StringBuilder();
            string str = value;
            int indent = 0;
            while (str.Length > maxSize)
            {
                sb.Append(new string(' ', indent)).Append(str.Substring(0, maxSize));
                sb.Append(Environment.NewLine);
                indent = 23;
                str = str.Substring(maxSize);
            }

            sb.Append(new string(' ', indent)).Append(str);

            return sb.ToString();
        }
    }
}
