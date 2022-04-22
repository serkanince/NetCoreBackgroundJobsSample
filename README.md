# NetCoreBackgroundJobsSample


## Description

This project contains sample code for making calls from a background job (IHostedService) to exchange rates. 

Read More Detail on Blog Post (Turkish): 

[Blog](https://dev.serkanince.com/2022/04/strategy-pattern-ve-background-job.html)
[Medium](https://medium.com/@serkanince/strategy-pattern-ve-background-job-kullanarak-d%C3%B6viz-kurlar%C4%B1n%C4%B1-%C3%A7ekme-53bb312a80)

## Exchange Rate Sources



| Name 	      | URL                                                                           |
|-------------------- | ----------------------------------------------------------------------------- |
| Exchange Rate 	          | https://api.exchangerate.host                                                     |
| Free Forex API 	      | https://www.freeforexapi.com                                                     |
| TCMB 	      | http://www.tcmb.gov.tr                                        |


### Features

* .Net 5 Framework
* Native .Net Hosted Services ( [REF](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio) )
* Strategy Pattern ( [REF](https://refactoring.guru/design-patterns/strategy/csharp/example) )
* Producer and Consumer (Pub/Sub Technique)
* Retry Policy ( [POLLY](https://github.com/App-vNext/Polly) )
* Concurrent Queue List (thread safe)
* Queued Background Tasks 
* Clean Code
* SOLID
* Native DI ( [REF](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0) )

### Build & Run

```ps1
dotnet restore
dotnet build
dotnet run
```

### Sample Console App Response

```ps1
Got a new message: 1 Currency Data : Code : USD , Rate : 14.6897 (4/13/2022 1:18:23 PM). (Queue size: 0)
```

