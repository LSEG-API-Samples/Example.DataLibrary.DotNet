using Refinitiv.Data.Core;
using System;

namespace Configuration
{
    public static class Sessions
    {
        // ***********************************************************************************************
        // Determine the type of connection below based on your access.
        //
        // Note: Users can utilize their own configuration by setting the connection type as: CONFIG.
        //       This setting allows users to define their own application configuration within their
        //       environment.  Refer to section 5 - Optional Configuration for more details.
        // ***********************************************************************************************
        public enum SessionTypeEnum
        {
            DESKTOP,                // DesktopSession           - Eikon/Refintiv Workspace
            RDP,                    // PlatformSession          - Refinitiv Data Platform
            DEPLOYED,               // PlatformSession          - Deployed ADS streaming services only
            CONFIG                  // Session                  - Configuration-based session
        };

        // Change the type of Session to switch the access channel
        public static SessionTypeEnum SessionType { get; set; } = SessionTypeEnum.RDP;



        // ** CHANGES BELOW ARE NOT REQUIRED UNLESS YOUR ACCESS REQUIRES ADDITIONAL PARAMETERS **

        // GetSession
        // Based on the above Session Type, retrieve the Session used to define how you want to access the platform.
        //
        public static ISession GetSession(SessionTypeEnum? session = default, string sessionName = default)
        {
            session ??= SessionType;
            return session switch
            {
                SessionTypeEnum.RDP => PlatformSession.Definition().AppKey(Credentials.AppKey)
                                                                   .OAuthGrantType(new GrantPassword().UserName(Credentials.RDPUser)
                                                                                                      .Password(Credentials.RDPPassword))
                                                                   .TakeSignonControl(true)
                                                                   .GetSession().OnState((state, msg, s) => 
                                                                                        Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                                                .OnEvent((eventCode, msg, s) => 
                                                                                        Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}")),
                SessionTypeEnum.DESKTOP => DesktopSession.Definition().AppKey(Credentials.AppKey)
                                                                      .GetSession().OnState((state, msg, s) => 
                                                                                        Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                                      .OnEvent((eventCode, msg, s) => 
                                                                                        Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}")),
                SessionTypeEnum.DEPLOYED => PlatformSession.Definition().Host(Credentials.ADSHost)
                                                                        .DacsUserName(Credentials.ADSDacsUser)
                                                                        .DacsPosition(Credentials.ADSDacsPosition)
                                                                        .DacsApplicationID(Credentials.ADSDacsApplicationID)
                                                                        .GetSession().OnState((state, msg, s) => 
                                                                                        Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                                        .OnEvent((eventCode, msg, s) => 
                                                                                        Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}")),
                _ => Session.Definition(sessionName).GetSession().OnState((state, msg, s) =>
                                                                    Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                                 .OnEvent((eventCode, msg, s) =>
                                                                    Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}"))
            };
        }
    }
}
