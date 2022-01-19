## Content Refinitiv.Data.Content) Release Notes

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
- Updated HIstoricalPricing interfaces to include 'Events' and 'Interday-summaries' new multi Universe APIs
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
- For those data services that offer both request/reply and streaming semantics, the library interfaces have been standardized to provide consistent definitions.  A single definition can support both GetData() and GetStream() capabilities.  See the Pricing interfaces.
- Standardized callback signatures within Stream and Request interfaces.
- Updated News/MRN interface to map to new datafeeds (Analytics Assets and Events).
