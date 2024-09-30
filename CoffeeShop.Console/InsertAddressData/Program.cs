using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using static InsertAddressData.Program;

namespace InsertAddressData
{
    class Program
    {
        static void Main(string[] args)
        {
            // 專案根目錄路徑
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            projectDirectory = projectDirectory.Replace("\\bin\\Debug", "");
            // 指定 JSON 檔案路徑
            string jsonFilePath = Path.Combine(projectDirectory, "taiwan_districts.json");

            if (File.Exists(jsonFilePath))
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
                string connectionString = "Server=(local);Data Source=UTRUST-WILLY;Integrated Security=true;Initial Catalog=CoffeeShop;MultipleActiveResultSets=True;App=EntityFramework;TrustServerCertificate=true";
                var root = JsonConvert.DeserializeObject<List<Root>>(jsonContent);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var cityId = new object();
                    foreach (var city in root)
                    {
                        string query = @"INSERT INTO AddressCity (CityName, SortIndex)
                                     VALUES (@CityName, @SortIndex);
                                     SELECT SCOPE_IDENTITY();";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@CityName", city.name);
                            cmd.Parameters.AddWithValue("@SortIndex", root.IndexOf(city));
                            cityId = cmd.ExecuteScalar();
                        }

                        foreach (var district in city.districts)
                        {
                            query = "INSERT INTO AddressArea (CityId, AreaName, ZipCode, SortIndex) VALUES (@CityId, @AreaName, @ZipCode, @SortIndex)";
                            using (SqlCommand cmd = new SqlCommand(query, connection))
                            {
                                cmd.Parameters.AddWithValue("@CityId", cityId);
                                cmd.Parameters.AddWithValue("@AreaName", district.name);
                                cmd.Parameters.AddWithValue("@ZipCode", district.zip);
                                cmd.Parameters.AddWithValue("@SortIndex", city.districts.IndexOf(district));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    connection.Close();
                }
            }
        }

        // 用來映射 JSON 的模型類別
        public class District
        {
            public string zip { get; set; }
            public string name { get; set; }
        }

        public class Root
        {
            public List<District> districts { get; set; }
            public string name { get; set; }
        }
    }
}
