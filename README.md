# Refinitiv Data Library for .NET

## Summary

The following series of examples demonstrate how to programmatically access content residing within the **Refinitiv Data Platform (RDP)** using a single, ease of use API called the **Refinitiv Data Library for .NET**.  The platform refers to the layer of data services providing both streaming and non-streaming content serving different clients, from the simple desktop interface to the enterprise application. Specifically, the library supports applications written for:

- **Desktop**

  - Whether written for Eikon or Refinitiv Workspace, applications can access both streaming and non-streaming (RDP endpoints) content

- **Cloud**

  - Direct access to both streaming and non-streaming services defined within the RDP

- **Deployed Streaming**

  - Access to locally deployed streaming services

  

The **Refinitiv Data Library for .NET** is a community-based API and will be managed as an open-source project.

The Refinitiv Data Library for .NET is structured as a stack of interfaces and libraries designed to foster the adoption of our platform by both financial coders and professional developers to programmatically access financial content.  Based on this stack of interfaces, the examples defined within this section have been organized as follows:

### **Content**

The *Content* examples target higher-level abstractions representing financial items like Pricing, News, Historical Data, etc. The *Content* layer can easily be used by both professional developers and financial coders. It provides great flexibility for commonly used financial objects.

### **Delivery**

The *Delivery* examples target the interfaces defined within the lowest abstraction layer of the library.  The examples provide core level services including logging and WebSocket customization.  In addition, different delivery service examples such as Request/Reply data endpoints, real-time streaming and interfaces supporting queuing capabilities offered for news headlines, stories and research message services.

