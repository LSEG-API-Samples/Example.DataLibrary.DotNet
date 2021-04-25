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
            DEPLOYED,               // PlatformSession          - Deployed ADS streaming services
        };

        // Change the type of Session to switch the access channel
        public static SessionTypeEnum SessionType { get; set; } = SessionTypeEnum.RDP;



        // ** CHANGES BELOW ARE NOT REQUIRED UNLESS YOUR ACCESS REQUIRES ADDITIONAL PARAMETERS **

        // GetSession
        // Based on the above Session Type, retrieve the Session used to define how you want to access the platform.
        //
        public static ISession GetSession()
        {
            switch (SessionType)
            {
                case SessionTypeEnum.RDP:
                    return PlatformSession.Definition().AppKey(Credentials.AppKey)
                                                       .OAuthGrantType(new GrantPassword().UserName(Credentials.RDPUser)
                                                                                          .Password(Credentials.RDPPassword))
                                                       .TakeSignonControl(true)
                                                       .GetSession().OnState((s, state, msg) => Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                                    .OnEvent((s, eventCode, msg) => Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}"));
                case SessionTypeEnum.DESKTOP:
                    return DesktopSession.Definition().AppKey(Credentials.AppKey)
                                                      .GetSession().OnState((s, state, msg) => Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                                   .OnEvent((s, eventCode, msg) => Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}"));
                case SessionTypeEnum.DEPLOYED:
                    return PlatformSession.Definition().Host(Credentials.ADSHost)
                                                       .GetSession().OnState((s, state, msg) => Console.WriteLine($"{DateTime.Now}: State: {state}. {msg}"))
                                                                    .OnEvent((s, eventCode, msg) => Console.WriteLine($"{DateTime.Now}: Event: {eventCode}. {msg}"));
                default:
                    throw new IndexOutOfRangeException($"Unknown Session Type: {SessionType}");
            }
        }
    }
}
