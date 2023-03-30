# server-side-pivot-engine-for-blazor-pivot-table
Introduced a server-side engine where all the pivot calculations, filtering, sorting, etc. are done. Then, the information to be displayed in the viewport will be passed to the client side. This prevents transferring the entire data source to a browser, which reduces network traffic and increases rendering performance of the Pivot Table especially where there are millions of data. It works best when virtual scrolling is enabled. The engine supports different kinds of data sources such as collections, JSON, CSV, DataTable, and dynamic types.

The following steps demonstrate how to run the application,
* Open the [PivotController](./PivotController/) project in Visual Studio 2022.
* Dependent packages will be downloaded automatically from the nuget.org site.
* After downloading the packages, run the project, it will be hosted in the localhost URL `http://localhost:61379`.
* Now, open the [PivotTable](./Sample/PivotTable/) project in Visual Studio 2022, map the hosted URL in the **index.razor** file, and run the project. The pivot table will be populated in the browser.
