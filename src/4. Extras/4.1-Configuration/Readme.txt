Configuration
-------------

The following section demonstrates the use of an optional configuration feature available within the 
library that can be used to simplify and control the behavior of an application.  

**Note** - To configure credentials for the examples within the entire solution, refer to the folder:
				> .Solution/Configuration
		   Within the Configuration folder, the files:
				> Session.cs
				  Indicates the type of connection into the platform
				> Credentials.cs
				  Specification of the access credentials
		   
		   Alternatively, users can utilize their own configuration by setting the 
		   Sessions.SessionType = SessionTypeEnum.CONFIG within the Session.cs file. This setting allows 
		   users to create their own application configuration within their environment.  
		   See configuration details below.

In most cases, applications are not required to utilize configuration when building their solutions. 
The configuration database defined within the library manages a number of features such as user credentials,
API endpoints and session settings. In some cases, there may be situations where users choose to override
default configuration settings.  

For example:

	o Credentials
	  While credentials are typically specified directly within API calls, users can optionally choose to 
	  control user credential settings via a configuration store, eg file.

	o Sessions
	  Library behavior can be modified or tuned via session parameters, such as timeouts, server specifications 
	  or API endpoints.

	o Testing environments
	  Using named stanza's, multiple environments can be setup to control where and how you connect into
	  the environment.

	o API endpoints
	  Endpoint specifications that drive data delivery may change and thus require default settings to 
	  be modified via session-specific stanzas.
	

By default, the LSEG Data Library for .Net uses an internal database defining default configuration 
settings within the library. Users have the ability to utilize their own configuration store to override 
default settings.  For this project, you can refer to the 'lseg-data.config.json' as a reference.

The libary can pull in configuration settings via a configuration file defined within the file system, a
JSON object and an embedded resource via a Stream.

Note: By default, the library will search for the configuration file: lseg-data.config.json

The following steps are enforced when locating a file-based configuration file:

1. Locate the specified configuration file as defined by the environment variable: LD_LIB_CONFIG_FILE
2. Otherwise, locate the default configuration file within your runtime working directory
3. Otherwise, locate the default configuration file within the users HOME directory

If no explicit configuration is defined, the library will utilize default configuration defined
within the library and rely on API parameter settings supported by the interfaces.
