using Common_Examples;
using Configuration;
using LSEG.Data.Content.Data;
using LSEG.Data.Core;
using System;

namespace _2._8._02_FundamentalAndReference_Screener
{
    class Program
    {
        // **********************************************************************************************************************
        // 2.8.01-FundamentalAndReference-Basics
        // The following example demonstrates the use of the Screener feature services offered within the desktop.
        //
        // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
        //      1. Configuration.Session to specify the access channel into the platform. Default: Desktop
        //      2. Configuration.Credentials to define your login credentials for the specified access channel.
        // **********************************************************************************************************************
        static void Main(string[] _)
        {
            Common.ShowUniverse = false;    // Display setting

            try
            {
                // Create a session into the platform
                using ISession session = Sessions.GetSession();

                // Open the session
                session.Open();

                // Note:
                // When applying a screener expression, it is important to ensure any embedded strings maintain there proper quotes
                var universe = "SCREEN(U(IN(Equity(active,public,private,primary))/*UNV:PublicPrivate*/), Contains(TR.BusinessSummary,\"polymer\"), CURN=USD)";
                var response = FundamentalAndReference.Definition(universe).Fields("TR.CommonName", "TR.HeadquartersCountry",
                                                                                   "TR.GICSSector", "TR.OrganizationStatusCode",
                                                                                   "TR.Revenue")
                                                                           .GetData();
                Common.DisplayTable("Screener", response, 0, 25);
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
