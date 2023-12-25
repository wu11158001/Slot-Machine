using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SlotMachineProtobuf;

namespace SlotMachineServer.DAO
{
    class GameData
    {
        /// <summary>
        /// 獲取獎池訊息
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        public long GetBonusInfo(string poolName, MySqlConnection mySqlConnection)
        {
            //取資料庫值
            string getSql = $"SELECT bonus FROM bonuspool WHERE poolname = @poolname";
            MySqlCommand cmd = new MySqlCommand(getSql, mySqlConnection);

            cmd.Parameters.AddWithValue("@poolname", poolName);

            // 執行查詢並獲取查詢結果
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {                   
                    return reader.GetInt64(0); ;
                }
                else
                {
                    Console.WriteLine($"沒有找到對應的獎池訊息: {poolName}");
                    return 0;
                }
            }
        }

        /// <summary>
        /// 獎池更新
        /// </summary>
        /// <param name="poolName">獎池名稱</param>
        /// <param name="changeVal">更換值</param>
        /// <param name="mySqlConnection"></param>
        public void UpdateBonusPool(string poolName, long changeVal, MySqlConnection mySqlConnection)
        {
            string writeSql = $"UPDATE bonuspool SET bonus = @newValue WHERE poolname = @poolname";
            MySqlCommand cmd = new MySqlCommand(writeSql, mySqlConnection);

            // 使用参数化查询以防止 SQL 注入
            cmd.Parameters.AddWithValue("@newValue", changeVal);
            cmd.Parameters.AddWithValue("@poolname", poolName);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine($"{poolName} => Sql獎池寫入 錯誤!!!");
            }
        }
    }
}
