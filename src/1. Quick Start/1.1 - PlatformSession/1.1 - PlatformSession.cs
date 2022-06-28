using Configuration;
using Refinitiv.Data;
using Refinitiv.Data.Core;
using System;

namespace _1._1___PlatformSession
{
    class Program
    {
        static void Main(string[] _)
        {
            // Programmatically override the default log level defined for the Refinitiv Data Library.
            Log.Level = NLog.LogLevel.Debug;

            // Create a session into RDP
            var session = PlatformSession.Definition().AppKey(Credentials.AppKey)
                                                      .OAuthGrantType(new GrantPassword().UserName(Credentials.RDPUser)
                                                                                         .Password(Credentials.RDPPassword))
                                                      .TakeSignonControl(true)
                                                      .GetSession().OnState((state, msg, s) => Console.WriteLine($"State: {state}. {msg}"))
                                                                   .OnEvent((eventCode, msg, s) => Console.WriteLine($"Event: {eventCode}. {msg}"));

            if (session.Open() == Session.State.Opened)
                Console.WriteLine("Session successfully opened");
            else
                Console.WriteLine("Session failed to open");
        }
    }
}
