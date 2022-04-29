using Common_Examples;
using Newtonsoft.Json.Linq;
using Refinitiv.Data.Content.Data;
using Refinitiv.Data.Core;
using System;

namespace _2._8._02_DataGrid_Historical
{
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create the platform session.
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Historical content
                    var response = FundamentalAndReference.Definition().Universe("AMCATMIV.AX", "BTATMIV.L", "4183ATMIV.OS",
                                                                                 "AATMIV.U", "STXEATMIV.EX", "1101ATMIV.TM", "0001ATMIV.HK")
                                                                       .Fields("TR.90DAYATTHEMONEYIMPLIEDVOLATILITYINDEXFORCALLOPTIONS.date",
                                                                               "TR.90DAYATTHEMONEYIMPLIEDVOLATILITYINDEXFORCALLOPTIONS")
                                                                       .Parameters(new JObject()
                                                                       {
                                                                          {"SDATE", "0d" },
                                                                          {"EDATE", "-4d" }
                                                                       }).GetData();
                    Common.DisplayTable("Valid request with historical fields", response);

                    // Last Month End, in Euros
                    response = FundamentalAndReference.Definition().Universe("GOOG.O", "AAPL.O")
                                                                   .Fields("TR.RevenueMean", "TR.NetProfitMean")
                                                                   .Parameters(new JObject()
                                                                   {
                                                                       {"SDate", "0M" },
                                                                       {"Curn", "EUR" }
                                                                   }).GetData();
                    Common.DisplayTable("Last Month End, in Euros", response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }
    }
}
