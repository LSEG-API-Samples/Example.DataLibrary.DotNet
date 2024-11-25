## LSEG.Data and LSEG.Data.Content Release Notes

## 2.0.0
- **Rebranding**
	- All namespaces have been updated from "Refinitiv..." to "LSEG...".
	- Updated configurations: all files and folders containing "Refinitiv" in their names or content have been changed to "LSEG".
- **Default Session Update**
	- The default session has been changed from "platform" to "desktop".
- **.NET8 Support**
	- The library now supports .NET 4.8, .NET 6, and .NET 8.
- **Performance Enhancements**
	- Improved hardware resource utilization and overall performance when streaming price data.
- **Bug Fixes and Improvements**
	- Resolved an issue with the Chain Stream Update Complete callback when swapping instruments in different chain segments.
	- Fixed a bug when processing FidValues for Summaries streaming data.
	- Addressed service assignment issues for Summaries Definition.
	- Corrected WebSocket outstanding operation exceptions occurring when opening multiple streams using Task.WhenAll.
	- Fixed timestamping for the current bar in intraday periods.
	- Resolved cloud credentials refresh issues for messaging services during session reconnects.
	- Fixed a bug related to qualifier actions filtering for Summaries streaming data.
- **New Features for Chain Streams**
	- Extended Chain Streams with additional callbacks for complete refresh/update events for a given chain.
- **Minor Fixes**
	- Corrected a minor bug when handling columns of DBNull type.
	- Fixed a minor issue when filtering data table fields based on qualifier actions.
	- Resolved a bug related to incorrect column types when retrieving summaries data.
- **LSEG.Data**  
  - Fixed issue disposing of OMM streaming connection.  
- **LSEG.Data.Content**  
  - Fixed issue with .NET Framework 4.8 processing TSI updates.  
  - Added support for Historical Pricing interfaces to prepend a forward slash (`/`) to instruments. This applies a delayed QoS to the request.  

## 1.0.0-beta5  
- **Refinitiv.Data**  
  - Added support for new OAuth 2.0 (Client Credentials) authentication:  
    - Client ID/Client Secret support.  
  - Fixed URL endpoint mapping issue on Linux-based systems.  
  - Updated configuration discovery to recognize the user's HOME directory across platforms (Windows/MacOS/Linux).  
  - Escaped representation of string values applied to any GET endpoint request within the PATH parameter.  
  - Resolved directory issue locating the `.portInUse` file on MacOS/Linux-based systems.  
  - Sessions now support optional specification of either a `Stream` or `Json` object (`JObject`) for application configuration. This replaces `DataLibraryConfig.SetConfigStream()` and `DataLibraryConfig.SetConfigFile()`.  

- **Refinitiv.Data.Content**  
  - Added new TimeSeries (TS) capabilities for the Historical Pricing service, enabling streaming TimeSeries bars based on various request frequencies (Events, Intraday, and Interday intervals). Requires Proxy version 3.6.0.  
  - Updated `Historical Pricing Summaries.Sessions` to `HistoricalPricing.Sessions`.  
  - Fixed issues with:  
    - Specifying `EventTypes`, `Adjustments`, or `Sessions` for Historical Events requesting multiple instruments.  
    - Specifying `Adjustments` or `Sessions` for Historical Interday Summaries with multiple instruments.  
  - Updated DataTable properties to align with native backend service data types.  
  - Enhanced configuration options for RDP news story services to handle desktop vs. platform implementations.  

## 1.0.0-beta4  
- **Refinitiv.Data**  
  - Introduced new streaming services:  
    - Added `RDPStream` to support Quantitative Analytics, Custom Instruments, and Benchmarks.  
    - Dynamic discovery and request capabilities for OMM and RDP streams.  
    - `OMMStream.Definition.Api` supports additional OMM streaming services.  
  - Updated `IPlatformSession.OnStreamingEndpoint` signature to include more streaming services.  
  - Fixed duplicate endpoint handling in stream discovery.  

- **Refinitiv.Data.Content**  
  - Standardized Financial Contracts interface in IPA to support all assets with JSON requests.  
  - Added a streaming interface to IPA Financial Contracts.  
  - Enhanced QoS settings for all HistoricalPricing interfaces.  
  - Updated SymbolConversion to auto-select appropriate Search variants based on the session type.  
  - Updated the Chain interface for compatibility with the new RDP endpoint service.  

## 1.0.0-beta3  
- **Shared Features (Refinitiv.Data & Refinitiv.Data.Content)**  
  - Minimum support for .NET Framework 4.8, .NET Standard 2.0, .NET Core, and .NET 6.  
  - Fixed issues related to OpenState reporting, item caching, and stream closures.  
  - Added `CancellationToken` support across relevant interfaces.  
  - Introduced new APIs and properties for improved session and pricing state handling.  

- **Unique Updates for Refinitiv.Data.Content**  
  - Updated ESG endpoint to v2, adding new scoring and measurement interfaces.  
  - Renamed `DataGrid` interface to `FundamentalAndReference`.  
  - Introduced TimestampLabel to `HistoricalPricing Summaries`.  

## 1.0.0-beta2  
- **Refinitiv.Data**  
  - Moved `OMMStream.Streaming()` specification from Definition into Stream interface.  

- **Refinitiv.Data.Content**  
  - Moved Pricing `.Streaming()` specifications to Stream interface.  
  - Enhanced Pricing cache updates for null values.  

## 1.0.0-beta1  
- **Shared Updates (Refinitiv.Data & Refinitiv.Data.Content)**  
  - Renamed Namespace and NuGet package.  
  - Standardized callback signatures across Stream, Request, Queue, and Endpoint interfaces.  

- **Unique Updates for Refinitiv.Data.Content**  
  - Introduced new interfaces, including `Search/Metadata`, `News/TopNews`, and `DataGrid`.  
  - Standardized library interfaces to support both request/reply and streaming semantics.  
