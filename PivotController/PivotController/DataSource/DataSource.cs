using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;

namespace PivotController.Controllers
{
    public class DataSource
    {

        public class PivotCSVData
        {
            public string Region { get; set; }
            public string Country { get; set; }
            public string ItemType { get; set; }
            public string SalesChannel { get; set; }
            public string OrderPriority { get; set; }
            public string OrderDate { get; set; }
            public int OrderID { get; set; }
            public string ShipDate { get; set; }
            public int UnitsSold { get; set; }
            public double UnitPrice { get; set; }
            public double UnitCost { get; set; }
            public double TotalRevenue { get; set; }
            public double TotalCost { get; set; }
            public double TotalProfit { get; set; }


            public List<string[]> ReadCSVData(string url)
            {
                List<string[]> data = new List<string[]>();
                using (StreamReader reader = new StreamReader(new WebClient().OpenRead(url)))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            data.Add(line.Split(','));
                        }
                    }
                    return data;
                }
            }
        }

        public class PivotJSONData
        {
            public string Date { get; set; }
            public string Sector { get; set; }
            public string EnerType { get; set; }
            public string EneSource { get; set; }
            public int PowUnits { get; set; }
            public int ProCost { get; set; }


            public List<PivotJSONData> ReadJSONData(string url)
            {
                WebClient myWebClient = new WebClient();
                Stream myStream = myWebClient.OpenRead(url);
                StreamReader stream = new StreamReader(myStream);
                string result = stream.ReadToEnd();
                stream.Close();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<PivotJSONData>>(result);
            }
        }

        public class PivotExpandoData
        {
            public List<ExpandoObject> Orders { get; set; } = new List<ExpandoObject>();
            public List<ExpandoObject> GetExpandoData()
            {
                Orders = Enumerable.Range(1, 75).Select((x) =>
                {
                    dynamic d = new ExpandoObject();
                    d.OrderID = 1000 + (x % 100);
                    d.CustomerID = (new string[] { "ALFKI", "ANANTR", "ANTON", "BLONP", "BOLID" })[new Random().Next(5)];
                    d.Freight = (new double[] { 2, 1, 4, 5, 3 })[new Random().Next(5)] * x;
                    d.OrderDate = (new DateTime[] { new DateTime(2010, 11, 5), new DateTime(2018, 10, 3), new DateTime(1995, 9, 9), new DateTime(2012, 8, 2), new DateTime(2015, 4, 11) })[new Random().Next(5)];
                    d.ShipCountry = (new string[] { "USA", "UK" })[new Random().Next(2)];
                    d.Verified = (new bool[] { true, false })[new Random().Next(2)];

                    return d;
                }).Cast<ExpandoObject>().ToList<ExpandoObject>();
                return Orders;
            }
        }

        public class PivotDynamicData
        {
            public List<DynamicDictionary> Orders = new List<DynamicDictionary>() { };
            public List<DynamicDictionary> GetDynamicData()
            {
                Orders = Enumerable.Range(1, 100).Select((x) =>
                {
                    dynamic d = new DynamicDictionary();
                    d.OrderID = 100 + x;
                    d.CustomerID = (new string[] { "ALFKI", "ANANTR", "ANTON", "BLONP", "BOLID" })[new Random().Next(5)];
                    d.Freight = (new double[] { 2, 1, 4, 5, 3 })[new Random().Next(5)] * x;
                    d.OrderDate = (new DateTime[] { new DateTime(2010, 11, 5), new DateTime(2018, 10, 3), new DateTime(1995, 9, 9), new DateTime(2012, 8, 2), new DateTime(2015, 4, 11) })[new Random().Next(5)];
                    d.ShipCountry = (new string[] { "USA", "UK" })[new Random().Next(2)];
                    d.Verified = (new bool[] { true, false })[new Random().Next(2)];
                    return d;
                }).Cast<DynamicDictionary>().ToList<DynamicDictionary>();
                return Orders;
            }

            public class DynamicDictionary : System.Dynamic.DynamicObject
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                public override bool TryGetMember(GetMemberBinder binder, out object result)
                {
                    string name = binder.Name;
                    return dictionary.TryGetValue(name, out result);
                }
                public override bool TrySetMember(SetMemberBinder binder, object value)
                {
                    dictionary[binder.Name] = value;
                    return true;
                }
                //The "GetDynamicMemberNames" method of the "DynamicDictionary" class must be overridden and return the property names to perform data operation and editing while using dynamic objects.
                public override System.Collections.Generic.IEnumerable<string> GetDynamicMemberNames()
                {
                    return this.dictionary?.Keys;
                }
            }
        }

        public class UniversityData
        {
            public string university { get; set; }
            public int year { get; set; }
            public int rank_display { get; set; }
            public string country { get; set; }
            public string city { get; set; }
            public string region { get; set; }
            public string type { get; set; }
            public string research_output { get; set; }
            public int? student_faculty_ratio { get; set; }
            public string international_students { get; set; }
            public string faculty_count { get; set; }
            public string link { get; set; }
            public string logo { get; set; }

            public List<UniversityData> ReadUniversityJSONData(string url)
            {
                WebClient myWebClient = new WebClient();
                Stream myStream = myWebClient.OpenRead(url);
                StreamReader stream = new StreamReader(myStream);
                string result = stream.ReadToEnd();
                stream.Close();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<UniversityData>>(result);
            }
        }

        public class PivotViewData
        {
            public string ProductID { get; set; }
            public string Country { get; set; }
            public string Product { get; set; }
            public double Sold { get; set; }
            public double Price { get; set; }
            public string Year { get; set; }

            public List<PivotViewData> GetVirtualData()
            {
                List<PivotViewData> VirtualData = new List<PivotViewData>();

                for (int i = 1; i <= 100; i++)
                {
                    PivotViewData p = new PivotViewData
                    {
                        ProductID = "PRO-" + (100 + i),
                        //Year = "FY 2015",
                        //Country = "Canada",
                        //Product = "Car",
                        Year = (new string[] { "FY 2015", "FY 2016", "FY 2017", "FY 2018", "FY 2019" })[new Random().Next(5)],
                        Country = (new string[] { "Canada", "France", "Australia", "Germany", "France" })[new Random().Next(5)],
                        Product = (new string[] { "Car", "Van", "Bike", "Flight", "Bus" })[new Random().Next(5)],
                        Price = (3.4 * i) + 500,
                        Sold = (i * 15) + 10
                    };
                    VirtualData.Add(p);
                }
                return VirtualData;
            }
        }

        public class BusinessObjectsDataView
        {
            public DataTable GetDataTable()
            {
                DataTable dt = new DataTable("BusinessObjectsDataTable");
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof(PivotViewData));
                foreach (PropertyDescriptor pd in pdc)
                {
                    dt.Columns.Add(new DataColumn(pd.Name, pd.PropertyType));
                }
                List<PivotViewData> list = new PivotViewData().GetVirtualData();
                foreach (PivotViewData bo in list)
                {
                    DataRow dr = dt.NewRow();
                    foreach (PropertyDescriptor pd in pdc)
                    {
                        dr[pd.Name] = pd.GetValue(bo);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }
    }
}
