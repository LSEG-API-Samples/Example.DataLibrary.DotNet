using Configuration;
using LSEG.Data;
using LSEG.Data.Core;
using System;

namespace _1._1___PlatformSession
{
    class Program
    {
        static void Main(string[] _)
        {
            // Programmatically override the default log level defined for the LSEG Data Library.
            Log.Level = NLog.LogLevel.Debug;

            try
            {
                ISession session = null;

                // Prompt the user to select authentication credentials
                Console.Write("\nChoose v1 (1) or v2 (2) authentication credentials (1,2) => ");
                var cred = Console.ReadLine();

                switch (cred)
                {
                    case "1":
                        // Create a session into RDP using v1 credentials
                        session = PlatformSession.Definition().AppKey(Credentials.AppKey)
                                                              .OAuthGrantType(new GrantPassword().UserName(Credentials.RDPUser)
                                                                                                 .Password(Credentials.RDPPassword))
                                                              .TakeSignonControl(true)
                                                              .GetSession().OnState((state, msg, s) => Console.WriteLine($"State: {state}. {msg}"))
                                                                           .OnEvent((eventCode, msg, s) => Console.WriteLine($"Event: {eventCode}. {msg}"));
                        break;

                    case "2":
                        // Create a session into RDP using v2 credentials
                        session = PlatformSession.Definition().OAuthGrantType(new ClientCredentials().ClientID(Credentials.RDPClientID)
                                                                                                     .ClientSecret(Credentials.RDPClientSecret))
                                                              .GetSession().OnState((state, msg, s) => Console.WriteLine($"State: {state}. {msg}"))
                                                                           .OnEvent((eventCode, msg, s) => Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}"));
                        break;

                    default:
                        Console.WriteLine("Invalid selection. Exiting.");
                        Environment.Exit(0);
                        break;
                }

                if (session.Open() == Session.State.Opened)
                    Console.WriteLine("Session successfully opened");
                else
                    Console.WriteLine("Session failed to open");
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
