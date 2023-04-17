// See https://aka.ms/new-console-template for more information


using InsertAddressData;
using Newtonsoft.Json;
using System.Text;


var filePath = AppContext.BaseDirectory;
filePath = Utility.GetParentDirectoryPath(filePath,4);
filePath = Path.Combine(filePath, "address.json");
List<AddressModel> data = new List<AddressModel>();
// 讀取 JSON 檔案
using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
{
    // 將 FileStream 包裝成 StreamReader
    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
    {
        // 讀取 StreamReader 中的所有資料
        string text = reader.ReadToEnd();
        // 將讀取的資料存入變數中
        string fileData = text;
        data = JsonConvert.DeserializeObject<List<AddressModel>>(fileData);
        data.ForEach(x => x.Zip5 = x.Zip5.Substring(0, 3));
        data = data.GroupBy(p => new { p.City, p.Area }).Select(x => x.First()).OrderBy(x => x.Zip5).ToList();
    };
};

Console.WriteLine("Hello, World!");

