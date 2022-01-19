using Common_Examples;
using Refinitiv.Data.Content.Data;
using Refinitiv.Data.Core;
using System;

namespace _2._8._01_DataGrid_Reference
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create the platform session.
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Reference data
                    Common.DisplayDataSet(FundamentalAndReference.Definition().Universe("TRI.N", "IBM.N")
                                                                              .Fields("TR.Revenue", "TR.GrossProfit")
                                                                              .GetData(), "Reference Data");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
