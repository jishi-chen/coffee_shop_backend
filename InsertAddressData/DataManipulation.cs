using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace InsertAddressData
{
    public class DataManipulation
    {
        private SqlConnection Conn { get; set; }
        private SqlTransaction Tran { get; set; }
        private const string connectionString = "Server=(local);Data Source=WILLYCHEN-PC10;Integrated Security=true;Initial Catalog=CoffeeShop;MultipleActiveResultSets=True;App=EntityFramework";

        public DataManipulation()
        {
            Conn = new SqlConnection(connectionString);
            Conn.Open();
        }



        public void InsertCity(Guid id, string cityName)
        {
            StringBuilder sqlStr = new StringBuilder();
            SqlParameter[] parameters = null;

            sqlStr.Append(
                @"
insert into AddressCity
(ID, CityName) 
values 
(@ID, @CityName) 
                ");

            parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier){ Value = id },
                new SqlParameter("@CityName", SqlDbType.NVarChar){ Value = cityName ?? string.Empty },
            };

            ExecuteSql(sqlStr.ToString(), parameters);
        }

        public void InsertArea(Guid cityID, string areaName, string zipCode)
        {
            StringBuilder sqlStr = new StringBuilder();
            SqlParameter[] parameters = null;

            sqlStr.Append(
                @"
insert into AddressArea
(ID, CityID, AreaName, ZipCode) 
values 
(@ID, @CityID, @AreaName, @ZipCode) 
                ");

            parameters = new SqlParameter[]
            {
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier){ Value = Guid.NewGuid() },
                new SqlParameter("@CityID", SqlDbType.UniqueIdentifier){ Value = cityID },
                new SqlParameter("@AreaName", SqlDbType.NVarChar){ Value = areaName ?? string.Empty },
                new SqlParameter("@ZipCode", SqlDbType.VarChar){ Value = zipCode ?? string.Empty },
            };

            ExecuteSql(sqlStr.ToString(), parameters);
        }

        public bool CheckIsExist()
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(
                @"
select * from AddressCity
                ");

            DataTable dt = GetDataTable(sqlStr.ToString());

            return dt.AsEnumerable().ToList().Any();
        }

        /// <summary> 啟用Transaction </summary>
        public void BeginTransaction()
        {
            try
            {
                Tran = Conn.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CommitTransaction()
        {
            Tran.Commit();
        }

        public void RollBackTransaction()
        {
            Tran.Rollback();
        }

        /// <summary> 關閉Transaction </summary>
        public void CloseTransaction()
        {
            if (Tran != null)
            {
                Tran.Dispose();
            }
        }

        /// <summary> 執行DBSQL </summary>
        /// <param name="sqlStr">SQL字串</param>
        /// <param name="parameters">參數</param>
        /// <returns></returns>
        /// <remarks>若要執行有Transaction效果，必需先執行BeginTransaction()</remarks>
        public void ExecuteSql(string sqlStr, SqlParameter[] parameters)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, Conn))
            {
                sqlCmd.CommandType = CommandType.Text;
                //將參數加到參數集合中。
                sqlCmd.Parameters.AddRange(parameters);
                //判斷是否有Transaction
                if (Tran != null)
                {
                    sqlCmd.Transaction = Tran;
                }
                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary> 用DataTable查詢 </summary>
        /// <param name="sqlStr">Sql字串</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sqlStr)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda = GetDataAdapter(sqlStr);
            sda.Fill(dt);
            sda.Dispose();

            return dt;
        }
        /// <summary> 用DataAdapter查詢 </summary>
        /// <param name="sqlStr">Sql字串</param>
        /// <returns></returns>
        public SqlDataAdapter GetDataAdapter(string sqlStr)
        {
            SqlDataAdapter myDa = null;
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, Conn))
            {
                sqlCmd.CommandType = CommandType.Text;

                //判斷是否有Transaction
                if (Tran != null)
                    sqlCmd.Transaction = Tran;

                myDa = new SqlDataAdapter(sqlCmd);
            }

            return myDa;
        }
    }
}
