using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Syncfusion.Pivot.Engine;

namespace PivotController.Controllers
{
    [Route("api/[controller]")]
    public class PivotController : Controller
    {
        private readonly IMemoryCache _cache;
        private IWebHostEnvironment _hostingEnvironment;
        private bool isRendered;
        private PivotEngine<DataSource.PivotViewData> PivotEngine = new PivotEngine<DataSource.PivotViewData>();
        JsonSerializerOptions customSerializeOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
                {
                    new SortedDictionaryConverter(),
                    new DoubleConverter(),
                    new ObjectToInferredTypesConverter()
                }
        };

        public PivotController(IMemoryCache cache, IWebHostEnvironment environment)
        {
            _cache = cache;
            _hostingEnvironment = environment;
        }

        [Route("/api/pivot/post")]
        [HttpPost]
        public async Task<object> Post([FromBody]object args)
        {
            FetchData param = JsonConvert.DeserializeObject<FetchData>(args.ToString());
            if (param.Action == "fetchFieldMembers")
            {
                return await GetMembers(param);
            }
            else if (param.Action == "fetchRawData")
            {
                return await GetRawData(param);
            }
            else
            {
                return await GetPivotValues(param);
            }
        }
        public async Task<EngineProperties> GetEngine(FetchData param)
        {
            isRendered = false;
            return await _cache.GetOrCreateAsync("engine" + param.Hash,
                async (cacheEntry) =>
                {
                    isRendered = true;
                    cacheEntry.SetSize(1);
                    cacheEntry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(60);
                    PivotEngine.Data = await GetData(param);
                    return await PivotEngine.GetEngine(param);
                });
        }

        public async Task<object> GetData(FetchData param)
        {
            return await _cache.GetOrCreateAsync("dataSource" + param.Hash,
                async (cacheEntry) =>
                {
                    cacheEntry.SetSize(1);
                    cacheEntry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(60);

                    // Here, you can refer different kinds of data sources. We've bound a collection in this illustration.
                    // return new DataSource.UniversityData().ReadUniversityJSONData(_hostingEnvironment.ContentRootPath + "\\DataSource\\universitydata.json");
                    return new DataSource.PivotViewData().GetVirtualData();

                    //return new DataSource.UniversityData().ReadUniversityJSONData(_hostingEnvironment.ContentRootPath + "\\DataSource\\universitydata.json");
                    // EXAMPLE:
                    // Other data sources, such as DataTable, CSV, JSON, etc., can be bound as shown below.
                    // return new DataSource.BusinessObjectsDataView().GetDataTable();
                    // return new DataSource.PivotJSONData().ReadJSONData(_hostingEnvironment.ContentRootPath + "\\DataSource\\sales-analysis.json");
                    // return new DataSource.PivotCSVData().ReadCSVData(_hostingEnvironment.ContentRootPath + "\\DataSource\\sales.csv");
                    // return new DataSource.PivotJSONData().ReadJSONData("http://cdn.syncfusion.com/data/sales-analysis.json");
                    // return new DataSource.PivotCSVData().ReadCSVData("http://cdn.syncfusion.com/data/sales-analysis.csv");
                    // return new DataSource.PivotExpandoData().GetExpandoData();
                    // return new DataSource.PivotDynamicData().GetDynamicData();
                });
        }

        public async Task<object> GetMembers(FetchData param)
        {
            EngineProperties engine = await GetEngine(param);
            Dictionary<string, object> returnValue = new Dictionary<string, object>();
            returnValue["memberName"] = param.MemberName;
            if (engine.FieldList[param.MemberName].IsMembersFilled)
            {
                returnValue["members"] = Serialize(engine.FieldList[param.MemberName].Members, customSerializeOptions);
            }
            else
            {
                await PivotEngine.PerformAction(engine, param);
                returnValue["members"] = Serialize(engine.FieldList[param.MemberName].Members, customSerializeOptions);
            }
            return returnValue;
        }

        public async Task<object> GetRawData(FetchData param)
        {
            EngineProperties engine = await GetEngine(param);
            return PivotEngine.GetRawData(param, engine);
        }

        public async Task<object> GetPivotValues(FetchData param)
        {
            Stopwatch ss = new Stopwatch();
            ss.Start();
            EngineProperties engine = await GetEngine(param);
            if (!isRendered)
            {
                engine = await PivotEngine.PerformAction(engine, param);
            }
            _cache.Remove("engine" + param.Hash);
            _cache.Set("engine" + param.Hash, engine, new MemoryCacheEntryOptions().SetSize(1).SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(60)));
            var pivotValues = PivotEngine.GetPivotValues();
            ss.Stop();
            Console.WriteLine(ss.ElapsedMilliseconds);
            return pivotValues;
        }

        private string Serialize(dynamic fieldItem, JsonSerializerOptions jsonSerializerOptions = null)
        {
            string serializedString;
            serializedString = fieldItem != null ? System.Text.Json.JsonSerializer.Serialize(fieldItem, jsonSerializerOptions ?? new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }) : null;
            return serializedString;
        }

    }
}
