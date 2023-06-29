using Configuration;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Queue;
using System;
using System.Collections.Generic;

namespace _3._3._03_Queue_Research
{
    // **********************************************************************************************************************
    // 3.3.03-Queue-Research
    // The Refinitiv Data Platform defines a service that utilizes Cloud-based Queuing to deliver realtime messages.
    // The following example demonstrates how to manage (retrieve/create/delete) a queue to deliver real-time research.
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
            const string researchEndpoint = "https://api.refinitiv.com/message-services/v1/research/subscriptions";

            try
            {
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.RDP);

                if (session.Open() == Session.State.Opened)
                {
                    // Create our Research definition
                    var definition = Queue.Definition(researchEndpoint);

                    // Create a QueueManager to actively manage our queues
                    IQueueManager manager = definition.CreateQueueManager().OnError((err, qm) => Console.WriteLine(err));

                    // First, check to see if we have any research queues active in the cloud...
                    var queues = manager.GetAllQueues();

                    // Prepare Research criteria if we plan to create a new AWS queue - we must supply a research ID.                
                    var criteria = new JObject()
                    {
                        ["transport"] = new JObject()
                        {
                            ["transportType"] = "AWS-SQS"
                        },
                        ["payloadVersion"] = "2.0",
                        ["userID"] = Configuration.Credentials.ResearchID
                    };

                    // If no existing queue exists, create one.
                    IQueueNode queue = (queues.Count > 0 ? queues[0] : manager.CreateQueue(criteria));

                    if (queue != null)
                    {
                        Console.WriteLine($"{Environment.NewLine}{(queues.Count > 0 ? "Using existing" : "Created a new")} queue...");

                        // Subscribe to the queue.
                        // Note: The subscriber interface has 2 mechanisms to retrieve data from the queue.  The first mechanism is to selectively
                        //       poll the queue for new messages.  The second mechanism is to define a callback/lambda expression and notify the
                        //       the subscriber to poll for messages as they come in - this mechansim provides a near realtime result.
                        //
                        // The following example demonstrates the first mechanism.
                        var subscriber = definition.CreateAWSSubscriber(queue);

                        // Poll the queue until we hit any key on the keyboard.
                        // Each poll will timeout after 2 seconds if not messages arrive.
                        Console.WriteLine("Attempt to retrieve research messages.  Hit any key to interrupt fetching...");
                        while (!Console.KeyAvailable)
                        {
                            subscriber.GetNextMessage(2, (research, s) => DisplayResearch(research));
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
                Console.WriteLine($"\n**************\nFailed to execute.");
                Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
                if (e.InnerException is not null) Console.WriteLine(e.InnerException);
                Console.WriteLine("***************");
            }
        }

        private static void DisplayResearch(IQueueResponse response)
        {
            try
            {
                var msg = response.Data.Raw;

                DateTime distributed = DateTime.Parse(msg["distributionTimestamp"].ToString()).ToLocalTime();

                Console.WriteLine($"\nDocument distributed: {distributed}");

                // Headline
                var headline = msg["payload"]?["Headline"]?["DocumentHeadlineValue"]?.ToString();
                if (headline != null)
                    Console.WriteLine($"\tHeadline:\t{headline}");

                // Synopsis
                var synopsis = msg["payload"]?["Synopsis"]?["DocumentSynopsisValue"]?.ToString();
                if (synopsis != null)
                    Console.WriteLine($"\tSynopsis:\t{synopsis}");

                // Document name
                var document = msg["payload"]?["DocumentFileName"]?.ToString();
                if (document != null)
                    Console.WriteLine($"\tDocument:\t{document} ({msg["payload"]?["DocumentFileType"]})");
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
}
