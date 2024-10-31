## Content (LSEG.Data.Content) Release Notes

### 2.0.12
- Fixed issue when processing FidValues for Summaries streaming data
- Improved CPU usage when processing Summaries streaming data
 
### 2.0.11
- Fix service assigning for Summaries Definition
- Fix websocket outstanding operation problem when opening multiple streams using Task.WhenAll

### 2.0.10
- Improved library memory usage and processing speed for Summaries streaming of price data

### 2.0.9
- Fixed current bar timestamping for intraday periods

### 2.0.8
- Fixed cloud credentials refresh for messaging services at session reconnect
 
### 2.0.7
- Fixed corner case bug related to qualifier actions filtering for Summaries streaming price data

### 2.0.6
- Fixed corner case bug for Chain Streams on stream reconnect 
- Implemented additional callbacks to Chain Streams to detect complete refresh and update flows

### 2.0.5
- Fixed minor bug when handling columns of DBNull type
- Fixed minor bug when filtering data table fields based on qualifier actions

### 2.0.4
- Fixed minor bug about incorrect column types when retrieving summaries data

### 2.0.3 
- Optimized memory usage of InstrumentMeta objects 
- Fixed minor issues

### 2.0.0 
- Rebranding
	- All namespaces are updated from "Refinitiv..." to "LSEG..."
	- Updated configurations : all files and folders that contain "Refinitiv" in their name or content, will now contain "LSEG"
	- Updated DataGrid (Data.Table) to include column data types
	- QoS (Quality Of Service) has been moved from Events and Summaries to HistoricalPricing context. 

### 1.0.0-beta5.1
- Fixed issue with .Net Framework 4.8 processing TSI updates
- Add support for Historical Pricing interfaces to prepend a forward slash ('/') to the instrument.  This will automatically apply a delayed Qos to the request.

### 1.0.0-beta5
- Added new TimeSeries (TS) capabilities to the Historical Pricing service to request for streaming TimeSeries 
  bars based on the request frequencies including: Events, Intraday and Interday intervals
	- Desktop session
	- Requires minumum Proxy ver: 3.6.0
- Updated Historical Pricing Summaries.Sessions to HistoricalPricing.Sessions.  The property is now available
  for Intraday, events and single-events interfaces.
- Fixed issue when specifying 'EventTypes', 'Adjustments' or 'Sessions' for Historical Events requesting for 
  multiple instruments
- Fixed issue when specifying 'Adjustments' or 'Sessions' for Historical Interday Summaries requesting for 
  multiple instruments
- Provided configuration options to control the differences within RDP news story services available within
  the desktop vs the platform implementation
- Updated DataTable data type properties for each defined cell to respect the native data types returned by 
  the backend service.

### 1.0.0-beta4
- IPA
	- Standardized Financial-Contracts interface to support all available assets allowing the specification
	  of a JSON request
	- Added streaming interface to IPA Financial Contracts
- Added Qos setting for all HistoricalPricing interfaces
- Fixed column ordering of Search results to match property list within Select statement
- Updated Chain interface to support new RDP endpoint service (supports .GetData())
- Updated SymbolConversion to automatically choose the appropriate Search variant based on the type of session
- Updated News Story processing logic to account for changes in the structure and data returned
	- removed Html specification on Definition - stories now support both HTML and text in response

### 1.0.0-beta3
- Minimum support for .Net Framework 4.8, .Net Standard 2.0.  Applications can target .Net Core, .Net 6.
- Fixed issue when adding items within Streaming interfaces that were initially not listed but become available.
- Fixed issue when reporting OpenState within Pricing interface to reflect state of the connection.
- Fixed issue with streaming chains not updating.
- Updated ESG endpoint to v2.
- Renamed DataGrid interface to FundamentalAndReference
- Added TimestampLabel specification to HistoricalPricing Summaries definition
- Added CancellationToken to all relevant interfaces
- Updated ESG interfaces to include dir-scores, scores (full/standard), measures (full/standard)
- Updated HistoricalPricing interfaces to include 'Events' and 'Interday-summaries' new multi Universe APIs
- Replaced response.Data.Universe property with response.Data.Records within the interfaces:
	HistoricalPricing, ESG, IPA and FundamentalAndReference
- Fixed News OnlineReports processing to access short form version of the story
- Added new Search services: SearchLight (Wealth clients); Explore (Available to all RDP, Desktop clients)
	- Changed default View to Search.ExploreView.Entities for Search and Metadata interfaces
- In IPA OptionFx, moved OptionFx.BinaryDefinition().SettlementType() to OptionFx.Definition()

### 1.0.0-beta2
- Moved Pricing .Streaming() specifications from Definition into Stream interface.
- Pricing.AddItems() can be invoked on a stream that is closed.
- Ensure null values in updates properly updates the Pricing cache.

### 1.0.0-beta1
- Renamed Namespace and NuGet package.
- Support for .Net Framework 4.52, .Net Standard 2.0.  Applications can target .Net Core, .Net 5.
- Added new interfaces including Search/Metadata, News/TopNews, DataGrid.
- For those data services that offer both request/reply and streaming semantics, the library interfaces have been standardized to provide consistent definitions.  
  A single definition can support both GetData() and GetStream() capabilities.  See the Pricing interfaces.
- Standardized callback signatures within Stream and Request interfaces.
- Updated News/MRN interface to map to new datafeeds (Analytics Assets and Events).