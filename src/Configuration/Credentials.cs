namespace Configuration
{
    // GlobalSettings
    // The following class is a convenient interface used strictly for the Examples within this solution.  Most of the examples,
    // will refer to these global settings allowing the developer to easily test across all provided example projects.
    //
    // Depending on the credentials provided to you, modify the specific section outlined below.
    //
    public static class Credentials
    {
        // ********************************************************************
        // RDP/RTO in Cloud Global Authentication parameters
        //
        // Note: Parameters in this section are only applicable if you were
        //       provided RDP or ERT in Cloud credentials.
        // ********************************************************************
        public static string RDPUser { get; } = "<RDP Machine ID>";
        public static string RDPPassword { get; } = "<RDP Password>";

        // AppKey used for both Desktop or Platform session types.
        public static string AppKey { get; } = "<Application Key>";

        // Research UserID (Used by example 3.3.03-Queue-Research)
        public static string ResearchID { get; } = "<Research ID>";

        // ********************************************************************
        // ADS (Advanced Distribution Server) Global Authentication parameters
        //
        // Note: Parameters in this section are only applicable if you were
        //       provided ADS WebSocket connection details.
        // ********************************************************************
        public static string ADSHost { get; } = "<server>:<port>";   // Eg: "wsserver:15000"
    }
}

