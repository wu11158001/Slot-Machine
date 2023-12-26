using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlotMachineProtobuf;
using SlotMachineServer.Servers;

namespace SlotMachineServer.Controller
{
    class GameController : BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        /// <summary>
        /// 獲取經典玩法賠率
        /// </summary>
        /// <param name="servers"></param>
        /// <param name="client"></param>
        /// <param name="pack"></param>
        /// <returns></returns>
        public MainPack GetClassicRate(Server servers, Client client, MainPack pack)
        {
            return client.GetGame.GetClassicRate(pack);
        }

        /// <summary>
        /// 經典遊戲結果請求
        /// </summary>
        /// <param name="servers"></param>
        /// <param name="client"></param>
        /// <param name="pack"></param>
        /// <returns></returns>
        public MainPack ClassicResult(Server servers, Client client, MainPack pack)
        {
            return client.GetGame.ClassicResult(pack, client, servers);
        }

        /// <summary>
        /// 獎池訊息請求
        /// </summary>
        /// <param name="servers"></param>
        /// <param name="client"></param>
        /// <param name="pack"></param>
        /// <returns></returns>
        public MainPack BonusPoolInfo(Server servers, Client client, MainPack pack)
        {
            pack.BonusPoolPack.BonusValue = servers.GetGameData.GetBonusInfo(pack.BonusPoolPack.GameName, client.GetMySqlConnection); ;
            return pack;
        }
    }
}
