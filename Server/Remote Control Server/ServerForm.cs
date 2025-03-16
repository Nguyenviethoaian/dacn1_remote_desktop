using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace Remote_Control_Server
{
    public partial class ServerForm : Form
    {
        private ServerController server;
        private Thread listenThread;
        private Thread cameraThread;
        private bool isCameraActive = false;
        

        public ServerForm()
        {
            InitializeComponent();
            server = new ServerController(5000);
            
            listenThread = new Thread(new ThreadStart(ViewClientScreen));
            listenThread.Start();
            
            pictureBoxScreen.MouseMove += pictureBoxScreen_MouseMove;
            pictureBoxScreen.MouseClick += pictureBoxScreen_MouseClick;
            
            
            this.KeyPreview = true; 
            pictureBoxScreen.KeyPress += pictureBoxScreen_KeyPress;
        }
        private async void btnViewClientScreen_Click(object sender, EventArgs e)
        {
            StopCameraThread();
            StopHistoryThread();
            isCameraActive = false;
            if (listenThread == null || !listenThread.IsAlive) // Kiểm tra xem luồng đã chạy chưa
            {
                listenThread = new Thread(new ThreadStart(ViewClientScreen));
                listenThread.Start(); 
            }
            //await Task.Run(() => ViewClientScreen());
            dataGridViewHistory.Visible = false;
            pictureBoxScreen.Visible = true; 
            pictureBoxCamera.Visible = false; 
        }
        
        private async void btnControlCamera_Click(object sender, EventArgs e)
        {
            StopScreenThread();
            StopHistoryThread();
            isCameraActive = true;
            server.RequestCameraStream(); 
            //StopCameraThread(); // Đảm bảo dừng luồng camera trước khi khởi động mới
            //cameraThread = new Thread(new ThreadStart(ViewClientCamera));
            //cameraThread.Start();
            await Task.Run(() => ViewClientCamera());
            dataGridViewHistory.Visible = false;
            pictureBoxScreen.Visible = false; 
            pictureBoxCamera.Visible = true;
        }
        private async void btnViewHistory_Click(object sender, EventArgs e)
        {
            StopScreenThread();
            StopCameraThread();
            isCameraActive = false;
            server.RequestClientHistory();
            await Task.Run(() => server.ReceiveClientHistory());
            //await Task.Run(() => ViewClientHistory());
            pictureBoxScreen.Visible = false;
            pictureBoxCamera.Visible = false;
            dataGridViewHistory.Visible = true;
        }

        private async void ViewClientScreen()
        {
            server.StartScreenStream();
            while (true)
            {
                if (!isCameraActive)
                {
                    Image screenImage = await Task.Run(() => server.GetClientScreen()); 
                    if (screenImage != null)
                    {
                        Invoke(new Action(() =>
                        {
                            pictureBoxScreen.Image = screenImage;
                            pictureBoxScreen.Visible = true; 
                            pictureBoxCamera.Visible = false;
                        }));
                    }
                }
                await Task.Delay(10);
            }
        }
        
        private void pictureBoxScreen_MouseMove(object sender, MouseEventArgs e)
        {
            server.SendMouseMove(e.X, e.Y);
        }
        
        private void pictureBoxScreen_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                server.SendMouseClickLeft();
            }
            else if (e.Button == MouseButtons.Right)
            {
                server.SendMouseClickRight();
            }
        }
        private void pictureBoxScreen_KeyPress(object sender, KeyPressEventArgs e)
        {
            string key = e.KeyChar.ToString(); 
            server.SendKeyPress(key);
        }

        private async void ViewClientCamera()
        {
            server.StartCameraStream();
            while (isCameraActive)
            {
                Image cameraFrame = await Task.Run(() => server.GetCameraFrame());
                if (cameraFrame != null)
                {
                    Invoke(new Action(() =>
                    {
                        pictureBoxCamera.Image = cameraFrame;
                        pictureBoxCamera.SizeMode = PictureBoxSizeMode.Zoom; 
                        pictureBoxCamera.Visible = true;
                        pictureBoxScreen.Visible = false;
                    }));
                }
                await Task.Delay(10);
            }
        }

        private async void ViewClientHistory()
        {
            await Task.Run(() => server.ReceiveClientHistory());
            Invoke(new Action(() =>
            {
                dataGridViewHistory.Rows.Clear(); 
                foreach (var item in server.clientHistory) 
                {
                    dataGridViewHistory.Rows.Add(item.ApplicationName, item.Date, item.Time);
                }
                dataGridViewHistory.Visible = true; 
                pictureBoxScreen.Visible = false; 
                pictureBoxCamera.Visible = false;
            }));
        }
        
        private void StopScreenThread()
        {
            if (listenThread != null && listenThread.IsAlive)
            {
                listenThread.Abort();
            }
        }
        private void StopCameraThread()
        {
            if (cameraThread != null && cameraThread.IsAlive)
            {
                cameraThread.Abort();
            }
        }
        private void StopHistoryThread()
        {
            if (listenThread != null && listenThread.IsAlive)
            {
                listenThread.Abort();
            }
        }
    }

}