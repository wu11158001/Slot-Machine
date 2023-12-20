using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlotMachineProtobuf;
using Google.Protobuf.Collections;

namespace SlotMachineServer.Servers
{
    class Game
    {
        /// <summary>
        /// 經典遊戲結果
        /// </summary>
        /// <returns></returns>
        public MainPack ClassicResult(MainPack pack)
        {
            //設定結果
            ClassicPack classicPack = new ClassicPack();
            Random random = new Random();
            int[] result = new int[9];
            for (int i = 0; i < 9; i++)
            {
                result[i] = random.Next(0, 8);
            }
            classicPack.ResultNums.AddRange(result);
            classicPack.WinCoin = 10;
            pack.ClassicPack = classicPack;

            return pack;
        }
    }
}
