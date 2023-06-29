using Refinitiv.Data;
using Refinitiv.Data.Core;
using System;

namespace _4._0._01_LogAPI
{
    // *******************************************************************************************************************************************
    // 4.0.01-Log-Api
    // By default, the Refinitiv Data Library for .NET will send general information log messages to a unique log file.  For applications that
    // utilize their own logging services, the Data Library for .NET log messages can be captured and rerouted for application use.
    // 
    // The following tutorial demonstrates the ability for an application to programmatically manage and control the library logs.
    // *******************************************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            // Programmatically override the default log level defined for the Refinitiv Data Library.
            Log.Level = NLog.LogLevel.Debug;

            // Intercept all Refinitiv Data Library log messages within a lambda expression. In our case, the lambda expression 
            // simply echos all log messages generated within the library to the console.
            Log.Output = (loginfo, parms) => Console.WriteLine($"Application: {loginfo.Level} - {loginfo.FormattedMessage}");

            // Create the platform session.
            using ISession session = Configuration.Sessions.GetSession();

            // Open the session - a number of log messages will be generated.
            session.Open();
        }
    }
}
