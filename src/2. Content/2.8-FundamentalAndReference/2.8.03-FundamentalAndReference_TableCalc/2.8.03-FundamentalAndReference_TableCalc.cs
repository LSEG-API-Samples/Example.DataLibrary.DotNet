using Common_Examples;
using Configuration;
using Refinitiv.Data.Content.Data;
using Refinitiv.Data.Core;

try
{
    // Create a session into the platform
    ISession session = Sessions.GetSession(Sessions.SessionTypeEnum.DESKTOP);

    session.Open();

    var mainRIC = "IBM.N";

    // Get some base values we'll use for calculations
    var baseVals = FundamentalAndReference.Definition().Universe(mainRIC)
                                                       .Fields("TR.SharesOutstanding", "TR.PriceClose")
                                                       .GetData();

    // Broker Detail Summary
    var table = FundamentalAndReference.Definition().Universe(mainRIC)
                                                    .Fields("TR.BrkRecLabel.date", "TR.BrkRecLabel.brokername", "TR.BrkRecLabel.analystname", "TR.BrkRecLabel.analystcode",
                                                            "TR.BrkRecLabel", "TR.TPEstValue", "TR.EPSEstValue")
                                                    .GetData();

    // Perform some table maintenance
    if (table.IsSuccess)
    {
        var dt = table.Data.Table;

        // Rename some columns
        dt.Columns[5].ColumnName = "Broker Rec Descr. = Broker Est";
        dt.Columns[6].ColumnName = "Target Price - Broker Est";
        dt.Columns[7].ColumnName = "EPS - Broker Est";

        // Add the PE ratio as a new column
        var colname = dt.Columns[6].ColumnName;
        var close = Convert.ToDecimal(baseVals.Data.Table.Rows[0][2]);
        dt.Columns.Add("P/E ratio", typeof(decimal), $"[{colname}] / {close}");

        Common.DisplayTable($"Broker Detail Summary - based on closing price: {close}", dt, 0);
    }
    else
    {
        Console.WriteLine($"Failed to execute request: {table.HttpStatus}");
    }
}
catch (Exception e)
{
    Console.WriteLine($"\n**************\nFailed to execute.");
    Console.WriteLine($"Exception: {e.GetType().Name} {e.Message}");
    if (e.InnerException is not null) Console.WriteLine(e.InnerException);
    Console.WriteLine("***************");
}
