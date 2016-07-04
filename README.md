Azure Logic App Monitoring 
=============
[![Build status](https://ci.appveyor.com/api/projects/status/ueq2xq8p9oehjd43?svg=true)](https://ci.appveyor.com/project/Kevin-Bronsdijk/logicapps-monitoring)

***
### Solution which will track failed Logic Apps workflow runs within Azure Applications Insight. 

A blog-post with more details can be found here; [notifications on error within logic apps](http://devslice.net/2016/03/notifications-on-error-logic-apps/)

**Update:** The Logic Apps team recently announced (July 02, 2016) that Logic Apps integrates with Azure Alerts enabling rich alerting scenarios. Therefore I'm not working on new features or other improvements however, this solution still provides some options not covered within Azure alerts; 

1. Azure Application insights integration (central place for diagnostic information)
2. Detailed error log
3. Not required to enable diagnostics for individual Logic Apps.

***

* [Introduction](#introduction)
* [Solution details](#solution-details)
* [Deployment](#deployment)
* [For Developers](#for-developers)
* [Important to know](#important-to-know)

## Introduction

This project was created due to the lacking ability to send notations on error when a Logic App workflow fails for some reason. Part of this has been addressed after the Azure team released an update on July 02, 2016. More details can be found here: https://feedback.azure.com/forums/287593-logic-apps/suggestions/10101393-notifications-on-error

## Solution details

The project consists of a Azure WebJob which constantly monitors for failed Logic App workflow runs. By default, the application only Tracks failed workflow runs however, the project can be extended.

The logging part of this solution depends on the Azure Application Insight monitoring and diagnostics platform. This to keep the implementation simple and avoid mail, SQL server other dependencies. This will also help keeping all diagnostic information into one central place, and the ability to configure alerts.

In addition, the WebJob also writes run details within the WebJob log. This includes the amount of failed runs found for every pull as well as the run name. 

Example:
```
 [03/08/2016 03:52:53 > be04cb: INFO] Found 0 failed workflow runs
 [03/08/2016 03:53:20 > be04cb: INFO] Found 5 failed workflow runs
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: myworkflow run: 08587441976978562759 
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: myworkflow run: 08587441976995362404 
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: myworkflow run: 08587441977007368807 
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: myworkflow run: 08587441977022435721 
 [03/08/2016 03:53:20 > be04cb: INFO] Workflow: myworkflow run: 08587441977041226327 
```

## Deployment

### Requirements

1. Active Directory application and service principal - [details](https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-service-principal-portal/) 
2.	Azure Web App with **Always On** enabled - [details](https://azure.microsoft.com/en-us/documentation/articles/web-sites-configure/)
3.	Azure Application Insights Environment

A prebuild version of the solution can be found within the [releases](releases/) folder of the project. After downloading the release package, make sure to perform the following:

1. Update the ApplicationInsights.config and App.config files within the .zip package with your information. 
2. Create a continuously running Web Job using the portal as in [this article](https://azure.microsoft.com/en-us/documentation/articles/web-sites-create-web-jobs/) or use Powershell.
3. Add the same <connectionStrings> configuration within your Azure Web App or you won’t be able to use the WebJob dashboard within the Azure Management Portal.
4. Within the Azure Applications Insight environment, create dashboards and alerts based on the custom metric called **Workflow-Failed** as desired. 
 
## For Developers

Given that this solution will only be used until the Azure Logic apps team decides to implement logging natively within the product, I’ve decided to keep the solution simple. The solution can be extended if desired by creating a custom implementation of the `IFetcher` interface. If you prefer a different logging platform, feel free to implement a custom version of the `ITracker` interface. 
## Important to know

1. Not required to alter any existing Logic App workflows. 
2. Having multiple Webjob instances active will result in duplicate event registrations. 
3. Logging is not real-time
