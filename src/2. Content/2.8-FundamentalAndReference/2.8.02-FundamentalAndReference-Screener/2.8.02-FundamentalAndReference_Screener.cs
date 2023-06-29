using Common_Examples;
using Configuration;
using Refinitiv.Data.Content.Data;
using Refinitiv.Data.Core;
using System;

namespace _2._8._02_FundamentalAndReference_Screener
{
    class Program
    {
        static void Main(string[] _)
        {
            Common.ShowUniverse = false;    // Display setting

            try
            {
                // Create the platform session.
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

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
