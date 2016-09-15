Skybrud.Umbraco.FetchFeed
==================

**Skybrud.Umbraco.FetchFeed** is a small Umbraco package for periodically downloading files to disk by adding a scheduled task in Umbraco.

## Links

- <a href="#installation">Installation</a>
- <a href="#configuration">Configuration</a>

## Installation

1. [**NuGet Package**][NuGetPackage]  
Install this NuGet package in your Visual Studio project. Makes updating easy.

## Configuration
In order to use the package, you must create a new configuration file at `~/config/FetchFeed.config`. The content should look something like:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <feeds>
        <!-- interval in minutes -->
        <feed alias="SkybrudRss" url="http://www.skybrud.dk/rss" path="~/App_Data/FetchFeed/SkybrudRss.xml" interval="5" />
    </feeds>
</configuration>
```

Also, in `~/config/umbracoSettings.config` you must find the `scheduledTasks` element, and append a new child element:

```xml
<task log="true" alias="FetchFeed" interval="60" url="http://localhost/umbraco/api/FetchFeed/Update" />
```

Make sure that the domain matches your Umbraco installation.




[NuGetPackage]: https://www.nuget.org/packages/Skybrud.Umbraco.FetchFeed