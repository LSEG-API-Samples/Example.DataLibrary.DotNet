using Configuration;
using Refinitiv.Data;
using Refinitiv.Data.Core;
using System;

namespace _1._3___DeployedSession
{
    class Program
    {
        static void Main(string[] _)
        {
            // Programmatically override the default log level defined for the Refinitiv Data Library.
            Log.Level = NLog.LogLevel.Debug;

            // Create a session into a deployed ADS providing steaming data only
            var session = PlatformSession.Definition().Host(Credentials.ADSHost)
                                                      .GetSession().OnState((state, msg, s) => Console.WriteLine($"State: {state}. {msg}"))
                                                                   .OnEvent((eventCode, msg, s) => Console.WriteLine($"Event: {eventCode}. {msg}"));

            if (session.Open() == Session.State.Opened)
                Console.WriteLine("Session successfully opened");
            else
                Console.WriteLine("Session failed to open");
        }
    }
}
