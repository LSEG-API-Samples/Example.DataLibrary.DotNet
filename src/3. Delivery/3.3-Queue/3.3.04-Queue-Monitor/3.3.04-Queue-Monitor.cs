using Configuration;
using LSEG.Data.Core;
using LSEG.Data.Delivery.Queue;

namespace _3._3._04_Queue_Monitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string newsStoriesEndpoint = "https://api.refinitiv.com/message-services/v1/news-stories/subscriptions";

            try
            {
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.RDPv1);

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

                    // Monitor queue status...
                    //queue.OnStop((q, details) =>
                    //{
                    //    Console.WriteLine("OnStop status called.");
                    //    foreach (var (key, value) in details)
                    //    {
                    //        value((arg1, arg2) =>
                    //        {
                    //            Console.WriteLine($"arg1: {arg1}");
                    //            Console.WriteLine($"arg2: {arg2}");
                    //        });
                    //    }
                    //});

                    // Subscribe to the queue.
                    // Note: The subscriber interface has 2 mechanisms to retrieve data from the queue.  The first mechanism is to selectively
                    //       poll the queue for new messages.  The second mechanism is to define a callback/lambda expression and notify the
                    //       the subscriber to poll for messages as they come in - this mechansim provides a near realtime result.
                    //
                    // The following example demonstrates the second mechanism.
                    IQueueSubscriber subscriber = definition.CreateAWSSubscriber(queue);

                    // Open the subscriber to begin polling for messages. Use Async() as this method is a long running task.
                    var task = subscriber.StartPollingAsync((story, s) => Console.WriteLine($"Story came through: {story.Data.Raw["sourceSeqNo"]}"));
                    Console.ReadKey();

                    // Close the subscription - stops polling for messages
                    //subscriber.StopPolling();
                    //task.GetAwaiter().GetResult();
                    //Console.WriteLine("Stopped polling for messages from the queue.");

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
