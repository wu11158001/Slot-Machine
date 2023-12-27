using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SlotMachineServer.DAO
{
    class RewardData
    {
        private const string tableName = "reward";

        /// <summary>
        /// 獲取獎勵資料
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        public long GetRewardInfo(string dataKey, MySqlConnection mySqlConnection)
        {
            //取資料庫值
            string sql = $"SELECT value FROM {tableName} WHERE rewardname = @rewardname";
            MySqlCommand cmd = new MySqlCommand(sql, mySqlConnection);

            cmd.Parameters.AddWithValue("@rewardname", dataKey);

            // 使用 MySqlDataReader 來獲取完整的結果集
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Convert.ToInt32(reader["value"]); ;
                }
                else
                {
                    Console.WriteLine($"{dataKey} => 搜索獲取獎勵資料錯誤");
                    return 0;
                }
            }
        }
    }
}
