using NLog;
using Refinitiv.Data.Core;

namespace _4._0._02_LogConfig
{
    // *******************************************************************************************************************************************
    // 4.0.02-Log-Config
    // By default, When using the Refinitiv Data Library for .NET, general information log messages will be sent to a unique log file.  
    // However, application developers can choose to override this behavior.
    //
    // The following tutorial demonstrates the ability for an application to configure and use the built-in logging within the library.  
    // Features demonstrated within this application:
    //
    //      - Configure logging
    //        The library uses the NLog logging framework which defines the NLog.config configuration file to control logging levels, targets,
    //        logging format, etc.  See NLog.config configuration file included with this project for more details.
    //
    //      - Application logging
    //        In addition to configuring logging within the Refinitiv Data library for .NET, applications can piggyback off of the 
    //        logging capability by sending their log messages to their own targets.
    //
    // To demonstrate basic functionality, the application simply opens a session and exits. 
    // *******************************************************************************************************************************************
    class Program
    {
        // Instruct our application that wa want to user the NLog logging system to managing our application logging
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] _)
        {
            // Any application-specific logging will be sent to our application-specific target - see NLog.config
            logger.Info($"Application launched.  Attempting to create our session...");

            // All Library-specific logging will be sent to an RDP target - see NLog.config

            // Create the platform session.
            using (ISession session = Configuration.Sessions.GetSession())
            {
                // Open the session
                session.Open();
            }
            logger.Info("Application execution done.");

            System.Console.WriteLine("\nRefer to the output directory for the 2 log files (*.log) generated.");
        }
    }
}
