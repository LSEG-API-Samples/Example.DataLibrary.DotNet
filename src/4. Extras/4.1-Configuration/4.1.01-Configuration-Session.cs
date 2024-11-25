using Newtonsoft.Json.Linq;
using LSEG.Data.Core;
using System;
using System.IO;
using System.Threading;
using LSEG.Data;

namespace _4._1_._01_Configuration_Session
{
    internal class Program
    {
        // **********************************************************************************************************************
        // 4.1-Configuration-Session
        // The following examples demonstrate some of the ways to utilize configuration within your application. The examples
        // have been broken out into 2 logical sections:
        //
        //     1. Automatically loading a local configuration file (lseg-data.config.json)
        //        Within this section, we demonstrate the following mechanisms:
        //          a) default configuration (Example 1)
        //          b) platform configuration (Example 2)
        //          c) desktop configuration (Example 3)
        //          d) deployed ADS configuration (Example 4)
        //     2. Programatically loading a local configuration file (lseg-data.config.json)
        //          a) default configuration (Example 5)
        //
        // In the above examples (Example 1, Example 5), we are utilizing the default configuration as defined within these 
        // files. You can choose to override the default session if you wish.
        // 
        // Note: To demonstrate, I need to temporarily disable the configuration environment setting (LD_LIB_CONFIG_FILE)
        //       during the run.
        //
        // Refer to the 'Readme.txt' packaged within this example for more details.
        // **********************************************************************************************************************
        static void Main(string[] _)
        {
            Log.Level = NLog.LogLevel.Debug;

            // *************************
            // * Environment management.
            // * Manage the configuration environment variable in the event it is defined.
            // *************************
            const string envVarName = "LD_LIB_CONFIG_FILE";
            string initialValue = Environment.GetEnvironmentVariable(envVarName);

            // For convenience, each example utilizes a cancellation token in the event you do not have
            // access to a specific environment.  With this, you will not be forced to wait for the standard timeout when connecting
            // to an environment you do not have access.
            //
            // Alternatively, you can comment out the code segements below for those sessions you do not have access.

            try
            
            {
                // Disable the environment variable if it is defined
                if (initialValue != null)
                {
                    Environment.SetEnvironmentVariable(envVarName, null);
                    Console.WriteLine($"{envVarName} is temporarily disabled.");
                }

                Console.WriteLine("\nThe following tests demonstrate configuration behavior based on local configuration files.");

                // Example 1 - default session
                DefaultSession();

                // Example 2 - default platform session
                DefaultPlatformSession();

                // Example 3 - default desktop session
                DefaultDesktopSession();

                // Example 4 - specified named session (deployed streaming)
                NamedSession("platform.ads");

                Console.WriteLine("\nThe following test parses the custom configuration file as a JSON object\n");

                // Example 5 - JSON specification
                JsonSession();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
            finally
            {
                // Restore the environment variable if it was initially defined
                if (initialValue != null)
                {
                    Environment.SetEnvironmentVariable(envVarName, initialValue);
                    Console.WriteLine($"{envVarName} has been restored to its original value.");
                }
            }
        }

        // Example 1
        // Use the 'default' setting defined within the 'sessions' stanza.
        private static void DefaultSession()
        {
            try
            {
                Console.Write("<Enter> to open the default session defined in configuration..."); Console.ReadLine();

                using var session = Session.Definition().GetSession().OnState((s, state, msg) => 
                                                                        Console.WriteLine($"State: {state}. {msg}"))
                                                                     .OnEvent((s, eventCode, msg) => 
                                                                        Console.WriteLine($"Event: {eventCode}. {msg}"));
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);
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
                Console.Write("\n<Enter> to open the default platform session defined in configuration..."); Console.ReadLine();

                using var session = PlatformSession.Definition().GetSession().OnState((s, state, msg) => 
                                                                                Console.WriteLine($"State: {state}. {msg}"))
                                                                             .OnEvent((s, eventCode, msg) => 
                                                                                Console.WriteLine($"Event: {eventCode}. {msg}"));
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);
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
                Console.Write("\n<Enter> to open the default desktop session defined in configuration..."); Console.ReadLine();

                using var session = DesktopSession.Definition().GetSession().OnState((s, state, msg) => 
                                                                                Console.WriteLine($"State: {state}. {msg}"))
                                                                            .OnEvent((s, eventCode, msg) => 
                                                                                Console.WriteLine($"Event: {eventCode}. {msg}"));
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);
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
                Console.Write($"\n<Enter> to open a named session: {sessionName} defined in configuration..."); Console.ReadLine();

                using var session = Session.Definition(sessionName).GetSession().OnState((s, state, msg) => 
                                                                                    Console.WriteLine($"State: {state}. {msg}"))
                                                                                .OnEvent((s, eventCode, msg) => 
                                                                                    Console.WriteLine($"Event: {eventCode}. {msg}"));

                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);
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
            // File-based configuration (loaded a JObject)
            Console.Write("<Enter> to load Json configuration from file and open the default session..."); Console.ReadLine();

            var json = JObject.Parse(File.ReadAllText("lseg-data.config.json"));

            // Define Sessions
            var session = Session.Definition(json).GetSession().OnState((state, msg, s) =>
                                                                   Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                               .OnEvent((eventCode, msg, s) =>
                                                                   Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}"));

            using var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            session.Open(source.Token);

            if (session.OpenState == Session.State.Opened)
            {
                Console.Write("\n<Enter> to close session..."); Console.ReadLine();
                session.Close();
            }
        }
    }
}
