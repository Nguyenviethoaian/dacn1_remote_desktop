using System.Windows.Forms;

namespace Remote_Control_Server
{
    partial class ServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
{
    this.btnViewClientScreen = new System.Windows.Forms.Button();
    this.btnControlCamera = new System.Windows.Forms.Button();
    this.btnViewHistory = new System.Windows.Forms.Button();
    this.pictureBoxScreen = new System.Windows.Forms.PictureBox();
    this.pictureBoxCamera = new System.Windows.Forms.PictureBox();
    this.dataGridViewHistory = new System.Windows.Forms.DataGridView();
    this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
    //this.comboBoxClients = new System.Windows.Forms.ComboBox();  // Thêm ComboBox ở đây

    ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).BeginInit();
    ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
    this.SuspendLayout();

    // 
    // flowLayoutPanel1
    // 
    this.flowLayoutPanel1.Controls.Add(this.btnViewClientScreen);
    this.flowLayoutPanel1.Controls.Add(this.btnControlCamera);
    this.flowLayoutPanel1.Controls.Add(this.btnViewHistory);
    this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 40);
    this.flowLayoutPanel1.TabIndex = 0;

    // 
    // btnViewClientScreen
    // 
    this.btnViewClientScreen.Location = new System.Drawing.Point(3, 3);
    this.btnViewClientScreen.Name = "btnViewClientScreen";
    this.btnViewClientScreen.Size = new System.Drawing.Size(120, 30);
    this.btnViewClientScreen.TabIndex = 0;
    this.btnViewClientScreen.Text = "Xem màn hình Client";
    this.btnViewClientScreen.UseVisualStyleBackColor = true;
    this.btnViewClientScreen.Click += new System.EventHandler(this.btnViewClientScreen_Click);

    // 
    // btnControlCamera
    // 
    this.btnControlCamera.Location = new System.Drawing.Point(129, 3);
    this.btnControlCamera.Name = "btnControlCamera";
    this.btnControlCamera.Size = new System.Drawing.Size(120, 30);
    this.btnControlCamera.TabIndex = 1;
    this.btnControlCamera.Text = "Điều khiển Camera";
    this.btnControlCamera.UseVisualStyleBackColor = true;
    this.btnControlCamera.Click += new System.EventHandler(this.btnControlCamera_Click);

    // 
    // btnViewHistory
    // 
    this.btnViewHistory.Location = new System.Drawing.Point(255, 3);
    this.btnViewHistory.Name = "btnViewHistory";
    this.btnViewHistory.Size = new System.Drawing.Size(120, 30);
    this.btnViewHistory.TabIndex = 2;
    this.btnViewHistory.Text = "Xem lịch sử";
    this.btnViewHistory.UseVisualStyleBackColor = true;
    this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click);

    // 
    // comboBoxClients
    // 
    /*this.comboBoxClients.FormattingEnabled = true;
    this.comboBoxClients.Location = new System.Drawing.Point(380, 3);  // Vị trí ComboBox
    this.comboBoxClients.Name = "comboBoxClients";
    this.comboBoxClients.Size = new System.Drawing.Size(200, 21);  // Kích thước ComboBox
    this.comboBoxClients.TabIndex = 3;*/

    // 
    // pictureBoxScreen
    // 
    this.pictureBoxScreen.Location = new System.Drawing.Point(12, 50);
    this.pictureBoxScreen.Name = "pictureBoxScreen";
    this.pictureBoxScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; // Hoặc PictureBoxSizeMode.Zoom
    this.pictureBoxScreen.Size = new System.Drawing.Size(760, 480);
    this.pictureBoxScreen.TabIndex = 3;
    this.pictureBoxScreen.TabStop = false;

    // 
    // pictureBoxCamera
    // 
    this.pictureBoxCamera.Location = new System.Drawing.Point(12, 50);
    this.pictureBoxCamera.Name = "pictureBoxCamera";
    this.pictureBoxCamera.Size = new System.Drawing.Size(760, 480);
    //this.pictureBoxCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
    this.pictureBoxCamera.TabIndex = 4;
    this.pictureBoxCamera.TabStop = false;
    this.pictureBoxCamera.Visible = false;

    // 
    // richTextBoxHistory
    // 
    
    this.dataGridViewHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this.dataGridViewHistory.Columns.Add("Application", "Ứng Dụng");
    this.dataGridViewHistory.Columns["Application"].Width = 200;
    this.dataGridViewHistory.Columns.Add("Date", "Ngày");
    this.dataGridViewHistory.Columns["Date"].Width = 100;
    this.dataGridViewHistory.Columns.Add("Time", "Thời gian");
    this.dataGridViewHistory.Columns["Time"].Width = 100;
    this.dataGridViewHistory.Location = new System.Drawing.Point(12, 50);
    this.dataGridViewHistory.Name = "dataGridViewHistory";
    this.dataGridViewHistory.Size = new System.Drawing.Size(760, 480);
    this.dataGridViewHistory.TabIndex = 5;
    this.dataGridViewHistory.Visible = false;
    
    //this.richTextBoxHistory.Location = new System.Drawing.Point(12, 50);
    //this.richTextBoxHistory.Name = "richTextBoxHistory";
    //this.richTextBoxHistory.Size = new System.Drawing.Size(760, 480);
    //this.richTextBoxHistory.TabIndex = 5;
    //this.richTextBoxHistory.Text = "";
    //this.richTextBoxHistory.Visible = false;

    // Thêm richTextBoxHistory vào form
    this.Controls.Add(this.dataGridViewHistory);

    // 
    // ServerForm
    // 
    this.ClientSize = new System.Drawing.Size(800, 600);
    //this.Controls.Add(this.comboBoxClients);  // Thêm ComboBox vào form
    this.Controls.Add(this.pictureBoxCamera);
    this.Controls.Add(this.pictureBoxScreen);
    this.Controls.Add(this.flowLayoutPanel1);
    this.Name = "ServerForm";
    this.Text = "Remote Control Server";

    ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).EndInit();
    ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
    this.ResumeLayout(false);
}


        #endregion

        private System.Windows.Forms.Button btnViewClientScreen;
        private System.Windows.Forms.Button btnControlCamera;
        private System.Windows.Forms.Button btnViewHistory;
        private System.Windows.Forms.PictureBox pictureBoxScreen;
        private System.Windows.Forms.PictureBox pictureBoxCamera;
        //private System.Windows.Forms.RichTextBox richTextBoxHistory;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridViewHistory;
        //private System.Windows.Forms.ComboBox comboBoxClients;  // Khai báo ComboBox
    }
}
