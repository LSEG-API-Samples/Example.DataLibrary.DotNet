# LSEG Data Library for .NET

LSEG is excited to announce the release of our newly rebranded library, previously known as [Refinitiv Data Library for .Net](../main), which is now fully supported as an LSEG-managed project.  Refer to the **Migration** section below for more details related to moving to this fully-supported version.

## Summary

The following series of examples demonstrate how to programmatically access content residing within the **Refinitiv Data Platform (RDP)** as well as the LSEG desktop environment using a single, ease of use API called the **LSEG Data Library for .NET**.  The platform refers to the layer of data services providing both streaming and non-streaming content serving different clients, from the popular desktop application to enterprise data cloud services. 

The library supports applications written for:

- **Desktop**

  - Written for LSEG Workspace, applications can access both streaming and non-streaming content

- **Cloud**

  - Direct access to both streaming and non-streaming services defined within the RDP

- **Deployed Streaming**

  - Access to locally deployed streaming services


The LSEG Data Library for .NET is structured as a stack of interfaces and libraries designed to foster the adoption of our platform by both financial coders and professional developers to programmatically access financial content.  

Based on this stack of interfaces, the examples defined within this package have been organized as follows:

### **Content**

The *Content* examples target higher-level abstractions representing financial items like Pricing, News, Historical Data, etc. The *Content* layer can easily be used by both professional developers and financial coders. It provides ease-of-use and intuitive interfaces for commonly used financial objects.

### **Delivery**

The *Delivery* examples target the interfaces defined within the lowest abstraction layer of the library.  The examples provide core level services including logging, library configuration and session management.  In addition, different delivery service examples such as Request/Reply data endpoints, real-time streaming and interfaces supporting queuing capabilities offered for news headlines, stories and research message services.

### **Migration**

The **LSEG Data Library for .Net** is a newly branded and LSEG-supported set of APIs repackaged as a new set of NuGet libraries.  Other than optional configuration specifications and code namspaces, the entire library has identical interfaces to the **Refinitiv Data Library for .Net** thus minimizing migration efforts.

The following table outlines migration details:

| Feature | Refinitiv Data Library for .Net | LSEG Data Library for .Net |
|----------|----------|----------|
| Configuration    | **File**: refinitiv-data.config.json<br>**Env Var**: RD_LIB_CONFIG_FILE | **File**: lseg-data.config.json<br>**Env Var**: LD_LIB_CONFIG_FILE |
| Namespace    | Refinitiv.&lt;package&gt;<br> **e.g.**: Refinitiv.Data.Core   | LSEG.&lt;package&gt;<br>**e.g.**: LSEG.Data.Core   |

Refer to the examples within this project as a demonstration of the above changes.




