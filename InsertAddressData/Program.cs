﻿// See https://aka.ms/new-console-template for more information


using InsertAddressData;
using Newtonsoft.Json;
using System.Text;


var filePath = AppContext.BaseDirectory;
filePath = Utility.GetParentDirectoryPath(filePath,4);
filePath = Path.Combine(filePath, "address.json");
List<AddressModel> data = new List<AddressModel>();
List<string> cityList = new List<string>();

Menu(55);

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
        cityList = data.Select(x => x.City).Distinct().ToList();
    };
};
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

void Menu(int totalCost)
{
    List<Food> toastList = new List<Food>()
    {
        new Food(){ Name = "蔬菜", Price = 30 },
        new Food(){ Name = "火腿", Price = 35 },
        new Food(){ Name = "肉鬆", Price = 35 },
        new Food(){ Name = "鮪魚", Price = 35 },
        new Food(){ Name = "培根", Price = 35 },
    };
    List<Food> drinkList = new List<Food>()
    {
        new Food(){ Name = "薏仁漿", Price = 20 },
        new Food(){ Name = "豆漿", Price = 20 },
        new Food(){ Name = "紅茶", Price = 15 },

    };
    List<Food> otherList = new List<Food>()
    {
        new Food(){ Name = "", Price = 0 },
        new Food(){ Name = "薯餅", Price = 20 },
    };

    Random rand = new Random();
    Food result = new Food(), result2 = new Food(), result3 = new Food();
    while(result.Price + result2.Price + result3.Price != totalCost)
    {
        result = toastList[rand.Next(toastList.Count)];
        result2 = drinkList[rand.Next(drinkList.Count)];
        result3 = otherList[rand.Next(otherList.Count)];
    }
    Console.WriteLine("價值需求:" + totalCost);
    Console.WriteLine(result.Name + "$" + result.Price);
    Console.WriteLine(result2.Name + "$" + result2.Price);
    Console.WriteLine(result3.Name + "$" + result3.Price);
}

class Food
{
    public string Name { get; set; }
    public int Price { get; set; } = 0;
}