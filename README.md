Azure Logic App Monitoring 
=============
Solution which will track failed Logic Apps workflow runs within Azure Applications Insight.

* [Introduction](#introduction)
* [Solution details](#solution-details)
* [Deployment](#deployment)
* [For Developers](#for-developers)
* [Important to know](#important-to-know)

## Introduction

This project was created due to the lacking ability to send notations on error, or other events if desired, when a Logic App workflow fails for some reason. 

This project will be rendered absolute as soon as the Azure Logic Apps team implements the following feature request: https://feedback.azure.com/forums/287593-logic-apps/suggestions/10101393-notifications-on-error

## Solution details

The project consists of a Azure WebJob which constantly monitors for failed Logic App workflow runs. By default, the application only Tracks failed workflow runs however, the project can be extended. More details on this can be found within the “Developers” section

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

###### Admins

For Admins, I’m expecting that they just want to deploy the solution without the need of having Visual Studio or MSBuild. Therefore, a prebuild version of the solution can be found within the **releases** folder of the project.

1. Update the ApplicationInsights.config and App.config files within the .zip package with your information. 
2. Create a continuously running Web Job using the portal as in [this article](https://azure.microsoft.com/en-us/documentation/articles/web-sites-create-web-jobs/) or use Powershell.
3. Add the same <connectionStrings> configuration within your Azure Web App or you won’t be able to use the WebJob dashboard within the Azure Management Portal.
4. Within the Azure Applications Insight environment, create dashboards and alerts based on the custom metric called **Workflow-Failed** as desired. 
 
###### Developers

Make sure you meet the same requirements as cover in the Admins section, using custom build and deployment options instead. 

The root directory contains a build script which in addition creates a .zip deployment package as well. I’ve removed all references to the Azure deployment/publish options available within Visual Studio, because I was having issues with the output of the packages. Will include Nuget Package **Microsoft WebJobs Publish** within the near future.

## For Developers

Given that this solution will only be used until the Azure Logic apps team decides to implement logging natively within the product, I’ve decided to keep the solution simple. The solution can be extended if desired by creating a custom implementation of the IFetcher interface. If you prefer a different logging platform, feel free to implement a custom version of the ITracker interface. 
In addition, I’m open to suggestions and changes.

Things I would like to implement (soon):

* Embedding DLLs in a compiled executable .
* Including a summary run every x minutes within the WebJob log.
* Configuration templates
* Deployment script
* Git Deployment options

## Important to know

1. Not required to alter any Logic App workflows. 
2. Only errors for workflow runs with a failure date greater that the start date if the WeJob will be reported. 
3. The Webjob only keeps track of custom events and not summiting diagnostic information.
4. Logging is not real-time
