using System;
using SlotMachineServer.Servers;

namespace SlotMachineServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(5501);
            Console.WriteLine("服務端啟動..");
            Console.Read();
        }
    }
}
