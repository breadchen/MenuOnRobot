namespace MenuOnRobot
{
    partial class frmLocalCtrl
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLocalCtrl));
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnStatus = new PulseButton.PulseButton();
            this.btnInfo = new PulseButton.PulseButton();
            this.btnInteropCtrl = new PulseButton.PulseButton();
            this.balloonTips = new DevComponents.DotNetBar.BalloonTip();
            this.btnShowTime = new PulseButton.PulseButton();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007Blue;
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(270, 438);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 30);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "返回";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStatus
            // 
            this.btnStatus.BackColor = System.Drawing.Color.White;
            this.btnStatus.ButtonColorBottom = System.Drawing.Color.PowderBlue;
            this.btnStatus.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStatus.Location = new System.Drawing.Point(39, 200);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.PulseSpeed = 0.3F;
            this.btnStatus.PulseWidth = 12;
            this.btnStatus.Size = new System.Drawing.Size(140, 140);
            this.btnStatus.TabIndex = 6;
            this.btnStatus.Text = " 室内信息\r\n   监控";
            this.btnStatus.UseVisualStyleBackColor = false;
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.BackColor = System.Drawing.Color.White;
            this.btnInfo.ButtonColorBottom = System.Drawing.Color.PowderBlue;
            this.btnInfo.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnInfo.Location = new System.Drawing.Point(196, 72);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.PulseSpeed = 0.3F;
            this.btnInfo.PulseWidth = 12;
            this.btnInfo.Size = new System.Drawing.Size(140, 140);
            this.btnInfo.TabIndex = 7;
            this.btnInfo.Text = " 家电中央\r\n  控制器";
            this.btnInfo.UseVisualStyleBackColor = false;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnInteropCtrl
            // 
            this.btnInteropCtrl.BackColor = System.Drawing.Color.White;
            this.btnInteropCtrl.ButtonColorBottom = System.Drawing.Color.PowderBlue;
            this.btnInteropCtrl.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnInteropCtrl.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInteropCtrl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnInteropCtrl.Location = new System.Drawing.Point(391, 19);
            this.btnInteropCtrl.Name = "btnInteropCtrl";
            this.btnInteropCtrl.PulseSpeed = 0.3F;
            this.btnInteropCtrl.PulseWidth = 12;
            this.btnInteropCtrl.Size = new System.Drawing.Size(140, 140);
            this.btnInteropCtrl.TabIndex = 8;
            this.btnInteropCtrl.Text = " 体感交互\r\n   控制";
            this.btnInteropCtrl.UseVisualStyleBackColor = false;
            this.btnInteropCtrl.Click += new System.EventHandler(this.btnInteropCtrl_Click);
            // 
            // balloonTips
            // 
            this.balloonTips.AntiAlias = true;
            this.balloonTips.AutoCloseTimeOut = 50;
            this.balloonTips.InitialDelay = 100;
            this.balloonTips.ShowCloseButton = false;
            this.balloonTips.Style = DevComponents.DotNetBar.eBallonStyle.Office2007Alert;
            // 
            // btnShowTime
            // 
            this.btnShowTime.BackColor = System.Drawing.Color.White;
            this.btnShowTime.ButtonColorBottom = System.Drawing.Color.Maroon;
            this.btnShowTime.ButtonColorTop = System.Drawing.Color.Red;
            this.btnShowTime.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShowTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnShowTime.Location = new System.Drawing.Point(313, 260);
            this.btnShowTime.Name = "btnShowTime";
            this.btnShowTime.PulseSpeed = 0.3F;
            this.btnShowTime.PulseWidth = 9;
            this.btnShowTime.Size = new System.Drawing.Size(80, 80);
            this.btnShowTime.TabIndex = 7;
            this.btnShowTime.Text = "表演时刻";
            this.btnShowTime.UseVisualStyleBackColor = false;
            this.btnShowTime.Click += new System.EventHandler(this.BtnShowTimeClick);
            // 
            // frmLocalCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.btnInteropCtrl);
            this.Controls.Add(this.btnShowTime);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.btnStatus);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLocalCtrl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLocalCtrl";
            this.Load += new System.EventHandler(this.frmLocalCtrl_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLocalCtrl_FormClosed);
            this.ResumeLayout(false);

        }
        private PulseButton.PulseButton btnShowTime;

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private PulseButton.PulseButton btnStatus;
        private PulseButton.PulseButton btnInfo;
        private PulseButton.PulseButton btnInteropCtrl;
        private DevComponents.DotNetBar.BalloonTip balloonTips;
    }
}