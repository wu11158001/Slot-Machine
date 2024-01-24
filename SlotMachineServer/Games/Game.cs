using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlotMachineProtobuf;
using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using SlotMachineServer.DAO;

namespace SlotMachineServer.Servers
{
    class Game
    {
        public Game(MySqlConnection mySqlConnection)
        {
            RuleData ruleData = new();
            classicAppearRateDic = ruleData.GetClassicAppearRate(mySqlConnection);
            classicLineRateDic = ruleData.GetClassicLineRate(mySqlConnection);
        }

        //經典連線規則
        private List<List<int>> classicLineRuleList = new()
        {
            new List<int>() { 0, 1, 2 },
            new List<int>() { 3, 4, 5 },
            new List<int>() { 6, 7, 8 },
            new List<int>() { 0, 3, 6 },
            new List<int>() { 1, 4, 7 },
            new List<int>() { 2, 5, 8 },
            new List<int>() { 0, 4, 8 },
            new List<int>() { 2, 4, 6 },
        };

        //經典玩法賠率_出現數量(編號,賠率List)
        private Dictionary<int, List<int>> classicAppearRateDic = new Dictionary<int, List<int>>();
        //經典玩法賠率_連線(編號,賠率)
        private Dictionary<int, int> classicLineRateDic = new Dictionary<int, int>();

        /// <summary>
        /// 獲取賠率_經典
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public MainPack GetClassicRate(MainPack pack)
        {
            ClassicRatePack ratePack = new ClassicRatePack();
            //連線
            foreach (var rate in classicLineRateDic)
            {
                KeyIntValue keyValue = new KeyIntValue
                {
                    Key = rate.Key,
                    Value = rate.Value
                };
                ratePack.Line.Add(keyValue);
            }
            //出現數量
            foreach (var rate in classicAppearRateDic)
            {
                IntList intList = new IntList
                {
                    Values = { rate.Value }
                };
                ratePack.Appear.Add(rate.Key, intList);
            }

            pack.ClassicRatePack = ratePack;
            return pack;
        }

        int count = 0;

        /// <summary>
        /// 遊戲結果_經典
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public MainPack ClassicResult(MainPack pack, Client client, Server server)
        {
            //設定結果
            ClassicPack classicPack = new ClassicPack();
            Random random = new Random();
            List<int> resultList = new();

            for (int i = 0; i < 9; i++)
            {
                resultList.Add(random.Next(0, 8));
            }

            //Demo用
            count++;
            switch (count)
            {
                case 2:
                    resultList = new List<int>() { 1, 2, 3, 1, 2, 3, 4, 4, 3 };
                    break;
                case 3:
                    resultList = new List<int>() { 1, 2, 3 , 1, 2, 3, 1, 2, 3};
                    break;
                case 5:
                    resultList = new List<int>() { 7, 7, 7, 7, 7, 7, 7, 7, 7 };
                    break;
            }            

            //獎池更新
            string poolName = "classic";
            long curPoolVal = server.GetGameData.GetBonusInfo(poolName, client.GetMySqlConnection);
            bool isBigWin = false;
            MainPack bonusMainPack = new MainPack();
            bonusMainPack.ActionCode = ActionCode.BonusPoolInfo;
            bonusMainPack.RequestCode = RequestCode.Game;
            BonusPoolPack bonusPoolPack = new BonusPoolPack();
            bonusPoolPack.GameName = poolName;
            if (resultList.Where(x => x == 7).Count() == resultList.Count)
            {
                //獲得全盤7大獎                
                bonusPoolPack.BonusValue = 0;
                bonusPoolPack.WinId = client.UserInfo.UserId;
                bonusPoolPack.WinNickName = client.UserInfo.NickName;
                bonusPoolPack.WinImgUrl = client.UserInfo.ImgUrl;
                bonusPoolPack.WinValue = curPoolVal;
                isBigWin = true;
            }
            else
            {
                //提取部分賭注到獎池                
                long takeVal = Convert.ToInt64(pack.ClassicPack.BetValue * 0.05f);
                bonusPoolPack.BonusValue = curPoolVal + takeVal;
            }
            bonusMainPack.BonusPoolPack = bonusPoolPack;
            server.UpdateBonusPool(bonusMainPack, client.GetMySqlConnection);

            //判斷結果
            int winRate = 0;
            List<int> winNumList = JudgeWinNums(resultList, ref winRate);
            long winCoin = (long)(pack.ClassicPack.BetValue * ((float)winRate / 100));

            //用戶訊息資料庫更新
            long coinUpdate = winCoin - pack.ClassicPack.BetValue;
            long bigCoin = isBigWin ? curPoolVal : 0;
            coinUpdate = client.GetUserData.AddUserInfo(client.GetMySqlConnection, client.UserInfo.UserId, "coin", coinUpdate + bigCoin);
            long expUpdate = pack.ClassicPack.BetValue / 100000;
            if (expUpdate < 1) expUpdate = 1;
            expUpdate = client.GetUserData.AddUserInfo(client.GetMySqlConnection, client.UserInfo.UserId, "exp", expUpdate);
            long lvUpdate = 1;
            long expTemp = 2;
            while (expTemp <= expUpdate)
            {
                expTemp *= 2;
                lvUpdate++;
            }
            client.GetUserData.WriteUserInfo(client.GetMySqlConnection, client.UserInfo.UserId, "level", lvUpdate);

            //回傳
            UserInfoPack userInfoPack = new UserInfoPack();
            userInfoPack.Coin = coinUpdate;
            userInfoPack.Exp = (int)expUpdate;
            userInfoPack.Level = (int)lvUpdate;
            pack.UserInfoPack = userInfoPack;

            classicPack.ResultNums.AddRange(resultList);
            classicPack.WinCoin = winCoin;
            classicPack.WinNums.AddRange(winNumList);
            pack.ClassicPack = classicPack;

            return pack;
        }

        /// <summary>
        /// 判斷贏得編號_經典
        /// </summary>
        /// <param name="resultNums">結果編號</param>
        /// <param name="winRate">用戶贏得賠率</param>
        /// <returns></returns>
        private List<int> JudgeWinNums(List<int> resultNums, ref int winRate)
        {
            HashSet<int> winHs = new();

            //結果(編號,(出現次數,位置))
            Dictionary<int, (int, List<int>)> resultDic = new();
            for (int i = 0; i < resultNums.Count; i++)
            {
                if (resultDic.ContainsKey(resultNums[i]))
                {
                    var dic = resultDic[resultNums[i]];
                    resultDic[resultNums[i]] = (dic.Item1+1, dic.Item2.Append(i).ToList());
                }
                else resultDic.Add(resultNums[i], (1, new List<int>() { i }));
            }

            //判斷數量
            int count = resultNums.Count;
            foreach (var dic in resultDic)
            {      
                if (dic.Value.Item1 >= 3)
                {
                    int rate = classicAppearRateDic[dic.Key][dic.Value.Item1 - 3];
                    if (rate > 0)
                    {
                        winRate += rate;
                        winHs.Add(dic.Key);
                    }                    
                }
            }

            //判斷連線
            foreach (var dic in resultDic)
            {
                if(dic.Value.Item1 >= Math.Sqrt(resultNums.Count))
                {
                    for (int i = 0; i < classicLineRuleList.Count; i++)
                    {
                        if (classicLineRuleList[i].All(item => dic.Value.Item2.Contains(item)))
                        {
                            winRate += classicLineRateDic[dic.Key];
                            winHs.Add(dic.Key);
                        }
                    }
                }
            }

            return winHs.ToList();
        }
    }
}
