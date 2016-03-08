Azure Logic App Monitoring 
=============
Solution which will track failed Logic Apps workflow runs within Azure Applications Insight.

* [Introduction](#introduction)
* [Solution details](#solution-details)
* [Important to know](#important-to-know)
* [Deployment](#deployment)

## Introduction

I’ve created this project due to the lacking ability to send notations on error, or other events if desired, when a Logic App workflow fails for some reason. As many businesses, I favor a proactive approach when it comes to monitoring application assets and therefore require a flexible automated notifications system.

This project will be rendered absolute as soon as the Azure Logic Apps team implements the following feature request: https://feedback.azure.com/forums/287593-logic-apps/suggestions/10101393-notifications-on-error

## Solution details

The project was created as a Azure WebJob which constantly monitors for failed Logic App workflow runs. The interval can be adjusted within the application configuration file. By default, the application only Tracks failed workflow runs however, It’s possible to provide additional logic by creating a custom implementation of the IFetcher interface. 

The logging part of this solution depends on the Azure Application Insight monitoring and diagnostics platform. This to keep the implementation simple and avoid mail, SQL server and other dependencies. This will also help keeping all diagnostic information into one central place, and the ability to configure alerts. If you prefer a different way of logging information, feel free to implement a custom version of the ITracker interface. 

In addition, the WebJob also writes run details within the WebJob log. This includes the amount of failed runs found for every pull as well as the run name. 

Example:
```
 [03/08/2016 03:52:53 > be04cb: INFO] Found 0 failed workflow runs
 [03/08/2016 03:53:20 > be04cb: INFO] Found 5 failed workflow runs
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: test12345 run: 08587441976978562759 
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: test12345 run: 08587441976995362404 
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: test12345 run: 08587441977007368807 
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: test12345 run: 08587441977022435721 
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: test12345 run: 08587441977041226327 
```

## Important to know

Several things you will need to be aware of before using this solution:
 
1. Only errors for workflow runs with a failure date greater that the start date if the WeJob will be reported. 
2. The Free and Shared plans don’t include the Always On feature. This may result in the Webjob being unloaded. https://azure.microsoft.com/en-us/documentation/articles/web-sites-configure/
3. The Webjob only keeps track of custom events and therefore not summiting any other type of WebJob related diagnostic information.
4. Logging is not real-time
 
## Deployment

Deployment is simple given that this is just a WebJob however, will include some pointers asap. If you have knowledge of Azure WebJobs (Deployment) and Applications Insight, this should be a walk in the park.

1. Create or use an existing WebApplication / Website 
2. Create a new Application Insights environment use an existing environment 
2. Download the project and make sure that the ApplicationInsights.config and App.config file are updated with your information
3. Deploy the project using the Azure Deployment Wizard 
4. Open Application Insights and Create a new Alert rule based on the custom metric called "Failed" which might not be there unless a failed run has been logged. (this label will likely change in the future)


 
