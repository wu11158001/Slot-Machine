using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SlotMachineServer.DAO
{
    class RuleData
    {
        /// <summary>
        /// 獲取經典玩法賠率_出現數量
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetClassicAppearRate(MySqlConnection mySqlConnection)
        {
            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();

            for (int i = 0; i < 8; i++)
            {
                string sql = $"SELECT * FROM classic_appear WHERE rateindex = @rateindex";
                MySqlCommand cmd = new MySqlCommand(sql, mySqlConnection);

                cmd.Parameters.AddWithValue("@rateindex", i);

                // 使用 MySqlDataReader 來獲取完整的結果集
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        List<int> list = new List<int>();
                        for (int appear = 3; appear <= 9; appear++)
                        {
                            list.Add(Convert.ToInt32(reader[appear.ToString()]));
                        }
                        
                        dic.Add(i, list);
                    }
                    else
                    {
                        Console.WriteLine($"搜索獲取經典賠率錯誤");
                        return null;
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 獲取經典玩法賠率_連線
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetClassicLineRate(MySqlConnection mySqlConnection)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();

            for (int i = 0; i < 8; i++)
            {
                string sql = $"SELECT * FROM classic_line WHERE rateindex = @rateindex";
                MySqlCommand cmd = new MySqlCommand(sql, mySqlConnection);

                cmd.Parameters.AddWithValue("@rateindex", i);

                // 使用 MySqlDataReader 來獲取完整的結果集
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int rate = Convert.ToInt32(reader["rate"]);

                        dic.Add(i, rate);
                    }
                    else
                    {
                        Console.WriteLine($"搜索獲取經典賠率錯誤");
                        return null;
                    }
                }
            }

            return dic;
        }
    }
}
