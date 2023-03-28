# server-side-pivot-engine-for-blazor-pivot-table
Introduced a server-side engine where all the pivot calculations, filtering, sorting, etc. are done. Then, the information to be displayed in the viewport will be passed to the client side. This prevents transferring the entire data source to a browser, which reduces network traffic and increases rendering performance of the Pivot Table especially where there are millions of data. It works best when virtual scrolling is enabled. The engine supports different kinds of data sources such as collections, JSON, CSV, DataTable, and dynamic types.

The following steps demonstrate how to run the application,
* Open the "PivotController" application in Visual Studio.
* Dependent packages will be downloaded automatically from the nuget.org site.
* Run the application once the packages are downloaded.
* Once the application is hosted in the localhost, copy and paste its URL in the pivot table sample in the index.razor file of the blazor wasm application in Sample folder.
* Then build and run the apllication. The pivot table will be populated in the browser.
