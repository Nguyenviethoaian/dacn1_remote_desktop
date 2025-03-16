using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Remote_Control_Server
{
    public class ServerController
    {
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private Bitmap fullScreenImage;
        public List<HistoryItem> clientHistory;
        private int totalParts = 4;
        public ServerController(int port)
        {
            listener = new TcpListener(IPAddress.Parse("192.168.1.2"), port);
            clientHistory = new List<HistoryItem>();
            fullScreenImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }
        public void StartScreenStream()
        {
            listener.Start();
            Console.WriteLine("Server started...");
            client = listener.AcceptTcpClient();
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            Console.WriteLine("Client connected...");
        }
        public void StartCameraStream()
        {
            if (client == null || !client.Connected)
            {
                Console.WriteLine("No client connected.");
                return;
            }
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
        public Image GetClientScreen()
        {
            if (reader == null || !client.Connected)
            {
                Console.WriteLine("No client connected.");
                return null;
            }

            try
            {
                int imageLength = reader.ReadInt32(); 
                byte[] imageBytes = reader.ReadBytes(imageLength);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    return Image.FromStream(ms);  
                }
            }
            catch
            {
                return null;
            }
        }
        /*public Image GetClientScreen()
        {
            if (reader == null || !client.Connected)
            {
                Console.WriteLine("No client connected.");
                return null; 
            }
            try
            {
                List<Bitmap> sections = new List<Bitmap>();
                for (int i = 0; i < totalParts * totalParts; i++)
                {
                    int imageLength = reader.ReadInt32(); 
                    byte[] imageBytes = reader.ReadBytes(imageLength); 
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        Bitmap section = (Bitmap)Image.FromStream(ms); 
                        sections.Add(section);
                    }
                }
                int width = sections[0].Width * totalParts;
                int height = sections[0].Height * totalParts;

                Bitmap fullImage = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(fullImage))
                {
                    int x = 0, y = 0;
                    foreach (var section in sections)
                    {
                        g.DrawImage(section, new Rectangle(x, y, section.Width, section.Height));
                        x += section.Width;

                        if (x >= fullImage.Width)
                        {
                            x = 0;
                            y += section.Height;
                        }
                    }
                }

                return fullImage;
            }
            catch
            {
                return null;
            }
        }*/
        public Image GetCameraFrame()
        {
            try
            {
                int imageLength = reader.ReadInt32();
                byte[] imageBytes = reader.ReadBytes(imageLength);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }
        public void SendMouseMove(int x, int y)
        {
            if (writer == null || !client.Connected)
            {
                Console.WriteLine("Connection is not established.");
                return; 
            }
            writer.Write(1); 
            writer.Write(x);
            writer.Write(y);
        }
        public void SendMouseClickLeft()
        {
            writer.Write(2); 
        }
        public void SendMouseClickRight()
        {
            writer.Write(3); 
        }
        public void SendKeyPress(string key)
        {
            writer.Write(4); 
            writer.Write(key);
        }
        
        public void RequestMousePosition()
        {
            writer.Write(5); 
        }
        public void ReceiveMousePosition()
        {
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            Console.WriteLine($"Current Mouse Position: X={x}, Y={y}");
        }
        public void RequestCameraStream()
        {
            if (writer != null && client.Connected)
            {
                writer.Write(7); 
            }
        }
        
        public void RequestClientHistory()
        {
            if (writer != null && client.Connected)
            {
                writer.Write(10); // Mã lệnh yêu cầu lịch sử
            }
        }
        
        public void ReceiveClientHistory()
        {
            Console.WriteLine("đã vào history");
            if (reader == null || !client.Connected)
            {
                Console.WriteLine("No client connected.");
                return;
            }

            try
            {
                    Console.WriteLine("đã vào history ===10");
                    while (true)
                    {
                        string applicationName = reader.ReadString();
                        if (applicationName == "END")
                        {
                            break; 
                        }
                        string timestamp = reader.ReadString();
                        clientHistory.Add(new HistoryItem
                        {
                            ApplicationName = applicationName,
                            DateTime = DateTime.Parse(timestamp)
                        });
                        Console.WriteLine("hihiiiii");
                        Console.WriteLine($"Received history: {applicationName} at {timestamp}");
                    }
            }
            catch
            {
                Console.WriteLine("Error receiving history.");
            }
        }
        
    }

}

public class HistoryItem
{
    public string ApplicationName { get; set; }
    public DateTime DateTime { get; set; }
            
    public string Date => DateTime.ToShortDateString(); // Thuộc tính cho ngày
    public string Time => DateTime.ToShortTimeString();

    public override string ToString()
    {
        return $"{ApplicationName} - {DateTime}";
    }
}