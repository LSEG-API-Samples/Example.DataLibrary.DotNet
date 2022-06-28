using Refinitiv.Data.Core;
using System;

namespace Configuration
{
    public static class Sessions
    {
        public enum SessionTypeEnum
        {
            DESKTOP,                // DesktopSession           - Eikon/Refintiv Workspace
            RDP,                    // PlatformSession          - Refinitiv Data Platform
            DEPLOYED,               // PlatformSession          - Deployed ADS streaming services only
        };

        // Change the type of Session to switch the access channel
        public static SessionTypeEnum SessionType { get; set; } = SessionTypeEnum.RDP;



        // ** CHANGES BELOW ARE NOT REQUIRED UNLESS YOUR ACCESS REQUIRES ADDITIONAL PARAMETERS **

        // GetSession
        // Based on the above Session Type, retrieve the Session used to define how you want to access the platform.
        //
        public static ISession GetSession(SessionTypeEnum? session = null)
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
                                                                        .GetSession().OnState((state, msg, s) => 
                                                                                        Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                                        .OnEvent((eventCode, msg, s) => 
                                                                                        Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}")),
                _ => throw new IndexOutOfRangeException($"Unknown Session Type: {SessionType}"),
            };
        }
    }
}
