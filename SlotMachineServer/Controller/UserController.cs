using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlotMachineProtobuf;
using SlotMachineServer.Servers;

namespace SlotMachineServer.Controller
{
    class UserController : BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <returns></returns>
        public MainPack Logon(Server servers, Client client, MainPack pack)
        {
            if (client.GetUserData.Logon(pack, client.GetMySqlConnection))
            {
                pack.ReturnCode = ReturnCode.Succeed;
                client.UserInfo.UserId = pack.LoginPack.Userid;

                Console.WriteLine($"{pack.LoginPack.Userid} : 註冊成功,進入遊戲");
            }
            else
            {
                pack.ReturnCode = ReturnCode.Fail;
                Console.WriteLine($"{pack.LoginPack.Userid} : 註冊失敗");
            }

            return pack;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        public MainPack Login(Server server, Client client, MainPack pack)
        {
            if (server.GetClientList.Any(list => list.UserInfo.UserId == pack.LoginPack.Userid))
            {
                pack.ReturnCode = ReturnCode.DuplicateLogin;
                Console.WriteLine(pack.LoginPack.Userid + " => 重複登入");
                return pack;
            }

            if (client.GetUserData.Login(pack, client.GetMySqlConnection))
            {
                pack.ReturnCode = ReturnCode.Succeed;
                client.UserInfo.UserId = pack.LoginPack.Userid;

                client.Send(server.GetGameData.GetAllBonus(client.GetMySqlConnection));

                Console.WriteLine($"{pack.LoginPack.Userid} => 進入遊戲");
            }
            else
            {
                Console.WriteLine($"{pack.LoginPack.Userid} : 沒有帳號準備註冊");
                return Logon(server, client, pack);
            }

            return pack;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public MainPack Logout(Server server, Client client, MainPack pack)
        {
            Console.WriteLine(client.UserInfo.UserId + " => 用戶登出");
            server.RemoveClient(client);
            return null;
        }

        /// <summary>
        /// 獲取用戶訊息
        /// </summary>
        /// <returns></returns>
        public MainPack GetUserInfo(Server server, Client client, MainPack pack)
        {
            return client.GetUserData.GetUserInfo(pack, client.GetMySqlConnection);
        }
    }
}
