using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SlotMachineProtobuf;
using SlotMachineServer.Servers;

namespace SlotMachineServer.DAO
{
    class UserData
    {
        private readonly string listName = "slot", 
                                tableName = "userdata", 
                                userid = "userid", 
                                coin = "coin", 
                                level = "level",
                                exp = "exp",
                                loginDay = "loginday";

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        public bool Logon(MainPack pack, MySqlConnection mySqlConnection)
        {
            //用戶Id
            string userId = pack.LoginPack.Userid;

            try
            {
                //插入數據
                string sql = $"INSERT INTO {this.listName}.{this.tableName} ({this.userid},{this.coin},{this.level},{this.exp},{this.loginDay}) VALUES (@userId,@coin,@level,@exp,@loginday)";

                MySqlCommand comd = new MySqlCommand(sql, mySqlConnection);

                comd.Parameters.AddWithValue("@userId", userId);
                comd.Parameters.AddWithValue("@coin", 10000);
                comd.Parameters.AddWithValue("@level", 1);
                comd.Parameters.AddWithValue("@exp", 0);
                comd.Parameters.AddWithValue("@loginday", 1);
                comd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        public bool Login(MainPack pack, MySqlConnection mySqlConnection)
        {
            //用戶Id
            string userId = pack.LoginPack.Userid;

            string sql = $"SELECT * FROM {this.tableName} WHERE {this.userid} = @userId";
            MySqlCommand cmd = new MySqlCommand(sql, mySqlConnection);

            cmd.Parameters.AddWithValue("@userId", userId);

            MySqlDataReader read = cmd.ExecuteReader();

            bool result = read.HasRows;
            read.Close();

            return result;
        }

        /// <summary>
        /// 獲取用戶訊息
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        public MainPack GetUserInfo(MainPack pack, MySqlConnection mySqlConnection)
        {
            //用戶Id
            string userId = pack.LoginPack.Userid;

            string sql = $"SELECT * FROM {this.tableName} WHERE {this.userid} = @userId";
            MySqlCommand cmd = new MySqlCommand(sql, mySqlConnection);

            cmd.Parameters.AddWithValue("@userId", userId);

            // 使用 MySqlDataReader 來獲取完整的結果集
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    UserInfoPack infoPack = new UserInfoPack();
                    infoPack.Level = Convert.ToInt32(reader["level"]);
                    infoPack.Exp = Convert.ToInt32(reader["exp"]);
                    infoPack.Coin = Convert.ToInt64(reader["coin"]);
                    infoPack.LoginDay = Convert.ToInt32(reader["loginday"]);

                    pack.UserInfoPack = infoPack;

                    return pack;
                }
                else
                {
                    Console.WriteLine($"{userId} => 搜索獲取用戶訊息錯誤");
                    return null;
                }
            }
        }

        /// <summary>
        /// 用戶訊息增減
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <param name="id">用戶ID</param>
        /// <param name="dataKey">更換的key</param>
        /// <param name="val">增減的值</param>
        public long AddUserInfo(MySqlConnection mySqlConnection, string id, string dataKey, long val)
        {
            //取資料庫值
            string getSql = $"SELECT {dataKey} FROM {this.tableName} WHERE userid = @id";
            MySqlCommand cmd = new MySqlCommand(getSql, mySqlConnection);

            cmd.Parameters.AddWithValue("@id", id);

            // 執行查詢並獲取查詢結果
            long getVal = 0;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    getVal = reader.GetInt64(0);
                }
                else
                {
                    Console.WriteLine($"沒有找到對應的ID: {id}");
                }
            }

            //寫入
            long updateVal = getVal + val;
            WriteUserInfo(mySqlConnection, id, dataKey, updateVal);

            return updateVal;
        }

        /// <summary>
        /// 寫入用戶訊息
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <param name="id">用戶ID</param>
        /// <param name="dataKey">更換的key</param>
        /// <param name="val">更換的值</param>
        public void WriteUserInfo(MySqlConnection mySqlConnection, string id, string dataKey, long val)
        {
            string writeSql = $"UPDATE {this.tableName} SET {dataKey} = @newValue WHERE userid = @id";
            MySqlCommand cmd = new MySqlCommand(writeSql, mySqlConnection);

            // 使用参数化查询以防止 SQL 注入
            cmd.Parameters.AddWithValue("@newValue", val);
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine($"{id} => Sql寫入 {dataKey} 錯誤");
            }
        }
    }
}
