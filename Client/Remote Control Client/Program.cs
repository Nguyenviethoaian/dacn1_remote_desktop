using System;
using System.Threading;

namespace Remote_Control_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClientController client = new ClientController("192.168.1.2", 5000); // Kết nối đến Server
            
            Thread screenThread = new Thread(new ThreadStart(client.SendScreen));
            screenThread.Start();
            
            //client.GetClientHistory();
            
            Console.ReadLine();
            client.Stop();
        }
    }

}