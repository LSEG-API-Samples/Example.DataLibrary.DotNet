using Configuration;
using LSEG.Data.Content.Pricing;
using LSEG.Data.Core;
using System;

namespace _2._2._02_Pricing_Chains
{
    // **********************************************************************************************************************
    // 2.2.02-Pricing-Chains
    // The following example demonstrates a basic use case of the retrieve chains via the Pricing interface from the platform.
    //
    // Suggestion: This example uses the pricing chain endpoint service within RDP. For users that do not have access
    //             to this endpoint, but do have access to the streaming services, you can instead retrieve chains through
    //             the streaming interfaces.  Refer to example 2.2.06-Pricing-StreamingChain.
    //
    // Note: The service is only available within RDP. Refer to the .Solutions folder Configuration.Credentials file to
    //       specify the required RDP credentials. Alternatively, users to specify credentials within their own
    //       configuration file.
    // **********************************************************************************************************************
    internal class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session into the platform...
                using ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.RDPv1);

                // Open the session
                session.Open();

                var chain = ".AV.O";            // Nasdaq Top 25

                Console.WriteLine($"\nRetrieving chain RIC: {chain}...");
                var response = Chain.Definition(chain).GetData();

                if (response.IsSuccess)
                {
                    Console.WriteLine($"\nRetrieved Chain RIC: {response.Data.DisplayName}");

                    // Display the 30 first elements of the chain
                    int idx = 0;
                    foreach (var constituent in response.Data.Constituents)
                        Console.WriteLine($"\t{++idx,2}. {constituent}");
                }
                else
                {
                    Console.WriteLine(response.HttpStatus);
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
