﻿## Delivery (Refinitiv.Data) Release Notes

### 1.0.0-beta5
- Added support for new OAuth version 2 (Client Credentials) authentication
	o Support for Client ID/Client Secret
- Fixed URL endpoint mapping issue on Linux-based systems
- Updated configuration discovery to recognize the users HOME directory for all platforms (Windows/MacOS/Linux)
- Apply escaped representation of string values applied to any GET endpoint request within the PATH parameter
- Resolved directory issue locating desktop .portInUse file within MacOS/Linux-based environments
- Sessions now support an optional specification of either a Stream or Json object (JObject) as a way to configure
  applications. This replaces the DataLibraryConfig.SetConfigStream() and DataLibraryConfig.SetConfigFile().
  See example: 5.1-Session

### 1.0.0-beta4
- New streaming services
	o Added RDPStream to support new RDP streaming services (Quantitative Analytics, Custom Instruments and Benchmarks)
	o Added capability to dynamically discover and request OMM and RDP streams
	o Added OMMStream.Definition.Api to support additional OMM streaming services
	o Streaming services have been made available in both Platform and Desktop sessions
- Updated IPlatformSession.OnStreamingEndpoint signature to include additional streaming services 
- Fixed issue as reported in case: 11209155 to handle duplicate endpoints reported in stream discovery

### 1.0.0-beta3
- Minimum support for .Net Framework 4.8, .Net Standard 2.0.  Applications can target .Net Core, .Net 6.
- Added StreamingState property to ISession interface to capture the streaming connection state.
- Fixed issue when reporting OpenState within OMMStream interface to reflect state of the connection.
- Fixed issue clearing item cache upon close for specified item(s).
- Fixed issue closing multiple streams for desktop session.
- Fixed NullReferenceException attempting to establish a desktop session.
- Changed Endpoint Callback signature - order changed to place data as the 1st parameter
- Adding duplicate items from different client (OMMStream) definitions are now observed and no longer rejected as duplicates
- Added CancellationToken to all relevant interfaces
- Session.Open() properly throws Exceptions as opposed to catching and reporting in Session callback
- Updated Desktop initialization to utilize 'status' request instead of 'ping'
- Updated IDesktopSessionDefinition.Port() to support multiple port specifications

### 1.0.0-beta2
- Moved OMMStream .Streaming() specification from Definition into Stream interface.

### 1.0.0-beta1
- Renamed Namespace and NuGet package.
- Support for .Net Framework 4.52, .Net Standard 2.0.  Applications can target .Net Core, .Net 5.
- Addressed disconnect and other issues when using the streaming interfaces.
- Standardized callback signatures within Session, Stream, Queue and Endpoint interfaces.
- Standardized Delivery and Core layer interfaces providing consistent definitions.
- New OMMStream interface supports batch requests.
- Re-designed WebSocket integration allowing external NuGet packages to be registered within the library.  
  - By default, the native Microsoft WebSocket implementation is registered.
  - Optionally, developers can register any of the currently available implementations including: NinjaWebSockets, 
    WebSocket4Net, Websocket-client and WebSocketSharp.