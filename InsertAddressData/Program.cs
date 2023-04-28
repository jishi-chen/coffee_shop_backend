// See https://aka.ms/new-console-template for more information


using InsertAddressData;
using Newtonsoft.Json;
using System.Runtime.Serialization;

var filePath = AppContext.BaseDirectory;
filePath = Utility.GetParentDirectoryPath(filePath,4);
filePath = Path.Combine(filePath, "address.json");
List<AddressModel> data = JsonConvert.DeserializeObject<List<AddressModel>>(File.ReadAllText(filePath));
List<string> cityList = new List<string>();

data.ForEach(x => x.Zip5 = x.Zip5.Substring(0, 3));
data = data.GroupBy(p => new { p.City, p.Area }).Select(x => x.First()).OrderBy(x => x.Zip5).ToList();
cityList = data.Select(x => x.City).Distinct().ToList();


DataManipulation db = new DataManipulation();
if (!db.CheckIsExist())
{
    db.BeginTransaction();
    foreach (var city in cityList)
    {
        Guid cityId = Guid.NewGuid();
        db.InsertCity(cityId, city);
        data.Where(x => x.City == city).ToList().ForEach(x => db.InsertArea(cityId, x.Area, x.Zip5));
    }
    db.CommitTransaction();
}

Console.WriteLine("Hello, World!");


//AdditionalData 用法

//foreach (var item in data)
//{
//    if (item.AdditionalData.Keys.Contains("Road"))
//    {
//        Console.WriteLine("道路: " + item.AdditionalData["Road"]);
//    }
//    if (item.AdditionalData.Keys.Contains("Scope"))
//    {
//        Console.WriteLine("Scope: " + item.AdditionalData["Scope"]);
//    }
//}

var day = new Holiday()
{
    Id = 959,
    Date = "2021/1/1",
    IsHoliday = "是"
};

Console.WriteLine(JsonConvert.SerializeObject(day));
day.AdditionalData.Add("name", "中華民國開國紀念日");
day.AdditionalData.Add("description", "全國各機關學校放假一日");
var text = JsonConvert.SerializeObject(day);
Console.WriteLine(text);
var obj = JsonConvert.DeserializeObject<Holiday>(text);
Console.WriteLine(obj);

public class Holiday
{
    [JsonProperty("_id")]
    public long Id { get; set; }

    [JsonProperty("date")]
    public string Date { get; set; }

    // these properties are set in OnDeserialized
    [JsonProperty("dayName")]
    public string DayName { get; set; }

    // [JsonProperty("name")]
    // public string Name { get; set; }

    // [JsonProperty("description")]
    // public string Description { get; set; }

    [JsonProperty("holidayCategory")]
    public string HolidayCategory { get; set; }

    [JsonProperty("isHoliday")]
    public string IsHoliday { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        // SAMAccountName is not deserialized to any property
        // and so it is added to the extension data dictionary
        string sameName = (string)AdditionalData["name"];
        DayName = sameName.Split("")[0];
    }
}