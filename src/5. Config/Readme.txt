Configuration
-------------

The following section demonstrates the use of an optional configuration feature available within the 
library that can be used to simplify and control the behavior of an application.  In most cases, 
applications are not required to utilize configuration when building their solutions.  However, in 
some cases, configuration can be used to simplify and control different testing environments, override 
default behavior as well as define user credentials when connecting into the environment.

By default, the Refinitiv Data Library for .Net utilizes an internal database that defines default 
configuration settings within the library. Users can provide their own file-based or embedded resource 
defining application configuration settings.

The library will attempt to locate a default configuration file called: refinitiv-data.config.json. 
However, applications can override the default configuration using the following:

1. DataLibraryConfig.SetConfigFile() or
2. Defining the environment variable: RD_LIB_CONFIG_FILE

The libary will attempt to locate configuration based on the following criteria:

1. If an embedded resource has been loaded containing configuration settings
2. Otherwise, if a configuration file has been defined via DataLibraryConfig.SetConfigFile()
3. Otherwise, locate the config file set by the RD_LIB_CONFIG_FILE environment variable
3. Otherwise, locate the default configuration file witin your runtime working directory
4. Otherwise, locate the default configuration file within the %USERPROFILE% directory
5. Otherwise, locate the default configuration file within the %HOMEPATH% directory

If no explicit configuration is defined, the library will utilize default configuration defined
within the library. While configuration can allow applications to define user credentials and connection
details, the library offers interfaces to programmatically define these details. This is the 
primary use case within the example package.
