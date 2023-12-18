using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SlotMachineProtobuf;

namespace SlotMachineServer.DAO
{
    class UserData
    {
        private readonly string listName = "slot", 
                                tableName = "userdata", 
                                userid = "userid", 
                                coin = "coin", 
                                level = "level",
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
                string sql = $"INSERT INTO {this.listName}.{this.tableName} ({this.userid},{this.coin},{this.level}, {this.loginDay}) VALUES (@userId,@coin,@level,@loginday)";

                MySqlCommand comd = new MySqlCommand(sql, mySqlConnection);

                comd.Parameters.AddWithValue("@userId", userId);
                comd.Parameters.AddWithValue("@coin", 10000);
                comd.Parameters.AddWithValue("@level", 1);
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
    }
}
