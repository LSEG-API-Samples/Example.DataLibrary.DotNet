using Refinitiv.Data.Core;
using Refinitiv.Data.Delivery.Request;

// **********************************************************************************************************************
// 3.2.06-Endpoint-TSIMetadata
// The following example demonstrates the retrieval of TimeSeries Interface (TSI) metadata.
//
// Note: The metadata service is only avalable via the Desktop session.
// **********************************************************************************************************************
try
{
    // Capability only available using a Desktop session
    using ISession session = Configuration.Sessions.GetSession(Configuration.Sessions.SessionTypeEnum.DESKTOP);

    if (session.Open() == Session.State.Opened)
    {
        // Instrument Metadata URL. Note: 'types' is optional.
        const string instrumentMetadata = "/data/historical-pricing/v1/metadata/instrument-metadata?version=v2&types=MarketRulesData,Qualifiers,RuleReferenceData,TimeZone,TradingSessions&ric=VOD.L";

        // Global Metadata URL.
        const string globalMetadata = "/data/historical-pricing/v1/metadata/global-metadata";

        // Global metadata
        DisplayData(EndpointRequest.Definition(globalMetadata).GetData());

        // Instrument metadata
        DisplayData(EndpointRequest.Definition(instrumentMetadata).GetData());
    }
}
catch (Exception e)
{
    Console.WriteLine($"\n**************\nFailed to execute.");
    Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
    if (e.InnerException is not null) Console.WriteLine(e.InnerException);
    Console.WriteLine("***************");
}

static void DisplayData(IEndpointResponse response)
{
    if (response.IsSuccess)
        Console.WriteLine(response.Data.Raw);
    else
        Console.WriteLine(response.HttpStatus);

    Console.Write("\nEnter to continue..."); Console.ReadLine();
}

