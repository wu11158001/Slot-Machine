using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using SlotMachineServer.Tools;
using SlotMachineServer.DAO;
using SlotMachineProtobuf;
using MySql.Data.MySqlClient;

namespace SlotMachineServer.Servers
{
    class Client
    {
        private const string connStr = "database=slot;Data Source=localhost;user=root;password=@Wu19918001;pooling=false;charset=utf8;port=3306";

        private Socket socket;
        private Server server;
        private Message message;

        private UserData userData;
        public UserData GetUserData { get { return userData; } }

        private MySqlConnection mySqlConnection;
        public MySqlConnection GetMySqlConnection { get { return mySqlConnection; } }

        public Game game { get; set; }

        public class UserInfoData
        {
            public string userId { get; set; }
        }
        public UserInfoData UserInfo { get; set; }

        public Client(Socket socket, Server server)
        {
            message = new Message();
            UserInfo = new UserInfoData();
            userData = new UserData();

            mySqlConnection = new MySqlConnection(connStr);
            mySqlConnection.Open();

            game = new Game(mySqlConnection);

            this.server = server;
            this.socket = socket;

            //開始接收消息
            StartReceive();
        }

        /// <summary>
        /// 開始接收消息
        /// </summary>
        void StartReceive()
        {
            socket.BeginReceive(message.GetBuffer, message.GetStartIndex, message.GetRemSize, SocketFlags.None, ReceiveCallBack, null);
        }

        /// <summary>
        /// 接收消息CallBack
        /// </summary>
        void ReceiveCallBack(IAsyncResult iar)
        {
            try
            {
                if (socket == null || !socket.Connected) return;

                int len = socket.EndReceive(iar);
                if (len == 0)
                {
                    //關閉連接
                    Close();
                    return;
                }

                //解析Buffer
                message.ReadBuffer(len, HandleRequest);
                //再次開始接收消息
                StartReceive();
            }
            catch (Exception)
            {
                //關閉連接
                Close();
            }
        }

        /// <summary>
        /// 發送消息
        /// </summary>
        /// <param name="pack"></param>
        public void Send(MainPack pack)
        {
            Console.WriteLine($"{this.UserInfo.userId} 發送消息:{pack.ActionCode}");

            socket.Send(Message.PackData(pack));
        }

        /// <summary>
        /// 解析消息回調方法
        /// </summary>
        void HandleRequest(MainPack pack)
        {
            server.HandleRequest(pack, this);
        }

        /// <summary>
        /// 關閉連接
        /// </summary>
        void Close()
        {
            server.RemoveClient(this);
            socket.Close();
            mySqlConnection.Close();
            Console.WriteLine(this.UserInfo.userId + " => 已斷開連接");
        }
    }
}