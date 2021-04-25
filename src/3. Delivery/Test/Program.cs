using Refinitiv.Data;
using Refinitiv.Data.Core;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Programmatically override the default log level defined for the Refinitiv Data Library.
            Log.Level = NLog.LogLevel.Debug;

            // Intercept all Refinitiv Data Library log messages within a lambda expression. In our case, the lambda expression 
            // simply echos all log messages generated within the library to the console.
            Log.Output = (loginfo, parms) => Console.WriteLine($"Application: {loginfo.Level} - {loginfo.FormattedMessage}");

            // Create the platform session.
            using (ISession session = Configuration.Sessions.GetSession())
            {
                // Open the session - a number of log messages will be generated.
                session.Open();
            }
        }
    }
}
