using Newtonsoft.Json.Linq;
using Refinitiv.Data.Core;
using System;
using System.IO;
using System.Threading;

namespace _5._1_Session
{
    internal class Program
    {
        // **********************************************************************************************************************
        // 5.1-Session
        // The following example demonstrates how to define session connection details via configuration.  The example includes
        // 2 configuration files, each defining multiple sessions that are used by the code examples below.  Users will be 
        // required to provide the necessary credentials/connection details, as defined in configuration, for the desired session
        // they wish to test.
        //
        // Refer to the 'Readme.txt' packaged within this example for more details.
        // **********************************************************************************************************************
        static void Main(string[] _)
        {
            // For convenience, each example utilizes a cancellation token in the event you do not have
            // access to a specific environment.  With this, you will not be forced to wait for the standard timeout when connecting
            // to an environment you do not have access.
            //
            // Alternatively, you can comment out the code segements below for those sessions you do not have access.

            try
            {
                Console.WriteLine("\nThe following tests will use the default configuration file defined within this project\n");

                // Example 1 - default session
                DefaultSession();

                // Example 2 - default platform session
                DefaultPlatformSession();

                // Example 3 - default desktop session
                DefaultDesktopSession();

                // Example 4 - specified named session (deployed streaming)
                NamedSession("platform.ads");

                Console.WriteLine("\nThe following test parses the custom configuration file as a JSON objectt\n");

                // Example 5 - JSON specification
                JsonSession();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        // Example 1
        // Use the 'default' setting defined within the 'sessions' stanza.
        private static void DefaultSession()
        {
            try
            {
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);

                Console.Write("<Enter> to open the default session defined in configuration..."); Console.ReadLine();
                using var session = Session.Definition().GetSession().OnState((s, state, msg) => 
                                                                        Console.WriteLine($"State: {state}. {msg}"))
                                                                     .OnEvent((s, eventCode, msg) => 
                                                                        Console.WriteLine($"Event: {eventCode}. {msg}"));
                session.Open(source.Token);

                if (session.OpenState == Session.State.Opened)
                {
                    Console.Write("\n<Enter> to close session..."); Console.ReadLine();
                    session.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n***************");
            }
        }

        // Example 2
        // Use the 'default' setting defined within the 'platform' stanza
        private static void DefaultPlatformSession()
        {
            try
            {
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);

                Console.Write("\n<Enter> to open the default platform session defined in configuration..."); Console.ReadLine();
                using var session = PlatformSession.Definition().GetSession().OnState((s, state, msg) => 
                                                                                Console.WriteLine($"State: {state}. {msg}"))
                                                                             .OnEvent((s, eventCode, msg) => 
                                                                                Console.WriteLine($"Event: {eventCode}. {msg}"));
                session.Open(source.Token);

                if (session.OpenState == Session.State.Opened)
                {
                    Console.Write("\n<Enter> to close session..."); Console.ReadLine();
                    session.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n***************");
            }
        }

        // Example 3
        // Use the 'default' setting defined within the 'desktop' stanza
        private static void DefaultDesktopSession()
        {
            try
            {
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);

                Console.Write("\n<Enter> to open the default desktop session defined in configuration..."); Console.ReadLine();
                using var session = DesktopSession.Definition().GetSession().OnState((s, state, msg) => 
                                                                                Console.WriteLine($"State: {state}. {msg}"))
                                                                            .OnEvent((s, eventCode, msg) => 
                                                                                Console.WriteLine($"Event: {eventCode}. {msg}"));
                session.Open(source.Token);

                if (session.OpenState == Session.State.Opened)
                {
                    Console.Write("\n<Enter> to close session..."); Console.ReadLine();
                    session.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n***************");
            }
        }

        // Example 4
        // Attempt to connect into a deployed streaming server ('ads') as referenced within configuration
        private static void NamedSession(string sessionName)
        {
            try
            {
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);

                Console.Write($"\n<Enter> to open a named session: {sessionName} defined in configuration..."); Console.ReadLine();
                using var session = Session.Definition(sessionName).GetSession().OnState((s, state, msg) => 
                                                                                    Console.WriteLine($"State: {state}. {msg}"))
                                                                                .OnEvent((s, eventCode, msg) => 
                                                                                    Console.WriteLine($"Event: {eventCode}. {msg}"));
                session.Open(source.Token);

                if (session.OpenState == Session.State.Opened)
                {
                    Console.Write("\n<Enter> to close session..."); Console.ReadLine();
                    session.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n***************");
            }
        }

        // Example 5
        // Load the JSON configuration from a file and feed into the library. Use the 'default' setting defined
        // within the 'sessions' stanza.
        private static void JsonSession()
        {
            using var source = new CancellationTokenSource();
            source.CancelAfter(10000);

            // File-based configuration (loaded a JObject)
            Console.Write("<Enter> to load Json configuration from file and open the default session..."); Console.ReadLine();
            var json = JObject.Parse(File.ReadAllText("customConfig.json"));

            // Define Sessions
            var session = Session.Definition(json).GetSession().OnState((state, msg, s) =>
                                                                   Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                               .OnEvent((eventCode, msg, s) =>
                                                                   Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}"));
            session.Open(source.Token);

            if (session.OpenState == Session.State.Opened)
            {
                Console.Write("\n<Enter> to close session..."); Console.ReadLine();
                session.Close();
            }
        }
    }
}
