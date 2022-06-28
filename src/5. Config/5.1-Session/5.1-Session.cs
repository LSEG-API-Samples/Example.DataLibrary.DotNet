using Refinitiv.Data.Core;
using System;
using System.Threading;

namespace _5._1_Session
{
    internal class Program
    {
        // **********************************************************************************************************************
        // 5.1-Session
        // The following example demonstrates how to define session connection details via configuration.  The example defines
        // a local configuration file defining multiple sessions that are used by the code examples below.  Users will be 
        // required to provide the necessary credentials/connection details, as defined in configuration, for the desired session
        // they wish to test.
        // **********************************************************************************************************************
        static void Main(string[] args)
        {
            // For convenience, each example utilizes a cancellation token in the event you do not have
            // access to a specific environment.  With this, you will not be forced to wait for the standard timeout when connecting
            // to an environment you do not have access.
            //
            // Alternatively, you can comment out the code segements below for those sessions you do not have access.

            try
            {
                // Example 1 - default session
                DefaultSession();

                // Example 2 - default platform session
                DefaultPlatformSession();

                // Example 3 - default desktop session
                DefaultDesktopSession();

                // Example 4 - specified named session (deployed streaming)
                NamedSession();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        // Example 1
        // Use the 'default' setting defined within the 'sessions' stanza.
        static private void DefaultSession()
        {
            try
            {
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);

                Console.Write("<Enter> to open the default session defined in configuration..."); Console.ReadLine();
                using var session = Session.Definition().GetSession().OnState((s, state, msg) => Console.WriteLine($"State: {state}. {msg}"))
                                                                     .OnEvent((s, eventCode, msg) => Console.WriteLine($"Event: {eventCode}. {msg}"));
                session.Open(source.Token);

                Console.Write("<Enter> to close session..."); Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n***************");
            }
        }

        // Example 2
        // Use the 'default' setting defined within the 'platform' stanza
        static private void DefaultPlatformSession()
        {
            try
            {
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);

                Console.Write("\n<Enter> to open the default platform session defined in configuration..."); Console.ReadLine();
                using var session = PlatformSession.Definition().GetSession().OnState((s, state, msg) => Console.WriteLine($"State: {state}. {msg}"))
                                                                             .OnEvent((s, eventCode, msg) => Console.WriteLine($"Event: {eventCode}. {msg}"));
                session.Open(source.Token);

                Console.Write("<Enter> to close session..."); Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n***************");
            }
        }

        // Example 3
        // Use the 'default' setting defined within the 'desktop' stanza
        static private void DefaultDesktopSession()
        {
            try
            {
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);

                Console.Write("\n<Enter> to open the default desktop session defined in configuration..."); Console.ReadLine();
                using var session = DesktopSession.Definition().GetSession().OnState((s, state, msg) => Console.WriteLine($"State: {state}. {msg}"))
                                                               .OnEvent((s, eventCode, msg) => Console.WriteLine($"Event: {eventCode}. {msg}"));
                session.Open(source.Token);

                Console.Write("<Enter> to close session..."); Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n***************");
            }
        }

        // Example 4
        // Attempt to connect into a deployed streaming server ('ads') as referenced within configuration
        static private void NamedSession()
        {
            try
            {
                using var source = new CancellationTokenSource();
                source.CancelAfter(10000);

                Console.Write("\n<Enter> to open a named session defined in configuration..."); Console.ReadLine();
                using var session = Session.Definition("platform.ads").GetSession().OnState((s, state, msg) => Console.WriteLine($"State: {state}. {msg}"))
                                                                                   .OnEvent((s, eventCode, msg) => Console.WriteLine($"Event: {eventCode}. {msg}"));
                session.Open(source.Token);

                Console.Write("<Enter> to close session..."); Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n***************");
            }
        }
    }
}
