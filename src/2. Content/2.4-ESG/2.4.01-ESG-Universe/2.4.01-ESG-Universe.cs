﻿using Common_Examples;
using Refinitiv.Data.Content.ESG;
using Refinitiv.Data.Core;
using System;
using Configuration;

namespace _2._4._01_ESG_Universe
{
    // **********************************************************************************************************************
    // 2.4.01-ESG-Universe
    // The following example retrieves the list of all organziations that have Environmental coverage.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session into the platform...
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.RDP);

                // Open the session
                if (session.Open() == Session.State.Opened)
                {
                    Console.WriteLine("\nRequesting for the entire ESG Universe...");

                    // List all organizations that have ESG coverage
                    var response = Universe.Definition().GetData();
                    Common.DisplayTable("ESG Universe", response, 0, 25);
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
