using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Stream;
using System;

namespace _3._1._01_MarketPrice
{
    // **********************************************************************************************************************
    // 3.1.01-Streaming-MarketPrice
    // The following example demonstrates a basic use case of the API to retrieve streaming content from the platform.
    // MarketPrice refers to level 1 content such as trades and quotes.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ISession session = Configuration.Sessions.GetSession())
                {

                    // Open the session
                    session.Open();

                    // Define a stream to retrieve level 1 content...
                    using var stream = OMMStream.Definition("EUR=").GetStream().OnRefresh((item, msg, s) => Console.WriteLine(msg))
                                                                               .OnUpdate((item, msg, s) => Console.WriteLine(msg))
                                                                               .OnError((item, err, s) => Console.WriteLine(err))
                                                                               .OnStatus((item, msg, s) => Console.WriteLine(msg));
                    // Open the stream...
                    stream.Open();

                    // Wait for data to come in then hit any key to close the stream...
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
