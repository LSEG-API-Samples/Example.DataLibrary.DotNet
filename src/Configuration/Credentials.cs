using System;

namespace Configuration
{
    // GlobalSettings
    // The following class is a convenient interface used strictly for the Examples within this solution.  Most of the examples,
    // will refer to these global settings allowing the developer to easily test across all provided example projects.
    //
    // Depending on the credentials provided to you, modify the specific section outlined below.
    //
    // **Note**
    // The following settings are optional.  That is, if you choose to control your credentials within a configuration file,
    // you can set the SessionType within the 'Session.cs' file to use your configuration file:
    //
    // public static SessionTypeEnum SessionType { get; set; } = SessionTypeEnum.CONFIG;
    //
    public static class Credentials
    {
        // ********************************************************************
        // RDP/RTO in Cloud Global Authentication parameters
        //
        // Note: Parameters in this section are only applicable if you were
        //       provided RDP (v1 and v2) or ERT in Cloud credentials.
        // ********************************************************************

        // *****************************************
        // v1 (OAuth 2.0 - Password Grant)
        // *****************************************
        //public static string RDPUser { get; } = "<RDP Machine ID>";
        //public static string RDPUser { get; } = "GE-A-01103867-3-5070"; // Historical
        //public static string RDPUser { get; } = "GE-A-00898811-3-1060"; // RealTime
        public static string RDPUser { get; } = "GE-A-01103867-3-2505"; // Wealth

        //public static string RDPPassword { get; } = "<RDP Password>";
        public static string RDPPassword { get; } = "8$3_13thwwepo8%9238llrXB35nskje";

        // AppKey used for both Desktop or Platform v1 sessions.
        //public static string AppKey { get; } = "<Application Key>";
        public static string AppKey { get; } = "f8a6d3b53b5d4b8794d9c1b40ec82ab314d75f4e";

        // *****************************************
        // v2 (OAuth 2.0 - Client Credentials)
        //public static string RDPClientID { get; } = "<RDP Client ID>";
        public static string RDPClientID { get; } = "GE-ZMFC3O4OWFHU";
        public static string RDPClientSecret { get; } = "8f289828-8fbd-4462-9c78-1a5dd14e4a48";

        // Research UserID (Used by example 3.3.03-Queue-Research)
        public static string ResearchID { get; } = "<Research ID>";

        // ********************************************************************
        // ADS (Advanced Distribution Server) Global Authentication parameters
        //
        // Note: Parameters in this section are specific to deployed sessions
        //       and only applicable if you were provided ADS WebSocket
        //       connection details.
        // ********************************************************************
        public static string ADSHost { get; } = "<server>:<port>";          // ADS Host. Eg: "wsserver:15000"
        public static string ADSDacsUser { get; } = Environment.UserName;   // DACs username
        public static string ADSDacsPosition { get; } = "127.0.0.1/net";    // DACs position
        public static string ADSDacsApplicationID { get; } = "256";         // DACs Application ID
    }
}

