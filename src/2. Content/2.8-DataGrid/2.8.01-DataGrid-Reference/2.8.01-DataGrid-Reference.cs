using Common_Examples;
using Refinitiv.Data.Content.Data;
using Refinitiv.Data.Core;
using System;

namespace _2._8._01_DataGrid_Reference
{
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session into the desktop
                // Note: The Fundamental and Reference API is only available on the desktop.
                using (ISession session = Configuration.Sessions.GetSession(Configuration.Sessions.SessionTypeEnum.DESKTOP))
                {
                    // Open the session
                    session.Open();

                    // Reference data
                    var response = FundamentalAndReference.Definition().Universe("TRI.N", "IBM.N")
                                                                       .Fields("TR.Revenue", "TR.GrossProfit")
                                                                       .GetData();
                    Common.DisplayTable("Reference Data", response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
