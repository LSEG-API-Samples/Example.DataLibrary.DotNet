﻿using Configuration;
using Refinitiv.Data;
using Refinitiv.Data.Core;
using System;

namespace _1._2___DesktopSession
{
    class Program
    {
        static void Main(string[] _)
        {
            // Programmatically override the default log level defined for the Refinitiv Data Library.
            Log.Level = NLog.LogLevel.Debug;

            try
            {
                // Create a session into the desktop application
                var session = DesktopSession.Definition().AppKey(Credentials.AppKey)
                                                         .GetSession().OnState((state, msg, s) => Console.WriteLine($"State: {state}. {msg}"))
                                                                      .OnEvent((eventCode, msg, s) => Console.WriteLine($"Event: {eventCode}. {msg}"));

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
