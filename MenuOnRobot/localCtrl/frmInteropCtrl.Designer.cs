namespace MenuOnRobot
{
    partial class frmInteropCtrl
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInteropCtrl));
        	this.btnExit = new DevComponents.DotNetBar.ButtonX();
        	this.labelX1 = new DevComponents.DotNetBar.LabelX();
        	this.btnPlayer = new PulseButton.PulseButton();
        	this.btnCalculator = new PulseButton.PulseButton();
        	this.btnPainter = new PulseButton.PulseButton();
        	this.btnGame = new PulseButton.PulseButton();
        	this.pictureBox1 = new System.Windows.Forms.PictureBox();
        	this.spGlove = new System.IO.Ports.SerialPort(this.components);
        	this.ofdOpenMedia = new System.Windows.Forms.OpenFileDialog();
        	((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// btnExit
        	// 
        	this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
        	this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
        	this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.btnExit.Location = new System.Drawing.Point(273, 438);
        	this.btnExit.Name = "btnExit";
        	this.btnExit.Size = new System.Drawing.Size(100, 30);
        	this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
        	this.btnExit.TabIndex = 2;
        	this.btnExit.Text = "返回";
        	this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
        	// 
        	// labelX1
        	// 
        	this.labelX1.AutoSize = true;
        	// 
        	// 
        	// 
        	this.labelX1.BackgroundStyle.Class = "";
        	this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
        	this.labelX1.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.labelX1.Location = new System.Drawing.Point(224, 12);
        	this.labelX1.Name = "labelX1";
        	this.labelX1.Size = new System.Drawing.Size(211, 45);
        	this.labelX1.TabIndex = 4;
        	this.labelX1.Text = "体感交互控制";
        	// 
        	// btnPlayer
        	// 
        	this.btnPlayer.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.btnPlayer.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.btnPlayer.Location = new System.Drawing.Point(137, 211);
        	this.btnPlayer.Name = "btnPlayer";
        	this.btnPlayer.PulseSpeed = 0.3F;
        	this.btnPlayer.Size = new System.Drawing.Size(100, 100);
        	this.btnPlayer.TabIndex = 5;
        	this.btnPlayer.Text = " 媒体播放器 ";
        	this.btnPlayer.UseVisualStyleBackColor = true;
        	this.btnPlayer.Click += new System.EventHandler(this.BtnPlayerClick);
        	// 
        	// btnCalculator
        	// 
        	this.btnCalculator.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.btnCalculator.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.btnCalculator.Location = new System.Drawing.Point(163, 49);
        	this.btnCalculator.Name = "btnCalculator";
        	this.btnCalculator.PulseSpeed = 0.3F;
        	this.btnCalculator.Size = new System.Drawing.Size(100, 100);
        	this.btnCalculator.TabIndex = 5;
        	this.btnCalculator.Text = "计算器";
        	this.btnCalculator.UseVisualStyleBackColor = true;
        	this.btnCalculator.Click += new System.EventHandler(this.BtnCalculatorClick);
        	// 
        	// btnPainter
        	// 
        	this.btnPainter.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.btnPainter.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.btnPainter.Location = new System.Drawing.Point(403, 170);
        	this.btnPainter.Name = "btnPainter";
        	this.btnPainter.PulseSpeed = 0.3F;
        	this.btnPainter.Size = new System.Drawing.Size(100, 100);
        	this.btnPainter.TabIndex = 5;
        	this.btnPainter.Text = "画图";
        	this.btnPainter.UseVisualStyleBackColor = true;
        	this.btnPainter.Click += new System.EventHandler(this.BtnPainterClick);
        	// 
        	// btnGame
        	// 
        	this.btnGame.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.btnGame.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.btnGame.Location = new System.Drawing.Point(314, 332);
        	this.btnGame.Name = "btnGame";
        	this.btnGame.PulseSpeed = 0.3F;
        	this.btnGame.Size = new System.Drawing.Size(100, 100);
        	this.btnGame.TabIndex = 5;
        	this.btnGame.Text = "游戏";
        	this.btnGame.UseVisualStyleBackColor = true;
        	this.btnGame.Click += new System.EventHandler(this.BtnGameClick);
        	// 
        	// pictureBox1
        	// 
        	this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
        	this.pictureBox1.Location = new System.Drawing.Point(76, 49);
        	this.pictureBox1.Name = "pictureBox1";
        	this.pictureBox1.Size = new System.Drawing.Size(489, 383);
        	this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        	this.pictureBox1.TabIndex = 6;
        	this.pictureBox1.TabStop = false;
        	// 
        	// spGlove
        	// 
        	this.spGlove.BaudRate = 38400;
        	this.spGlove.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.spGlove_DataReceived);
        	// 
        	// ofdOpenMedia
        	// 
        	this.ofdOpenMedia.FileName = "openFileDialog1";
        	this.ofdOpenMedia.Title = "选择媒体文件";
        	// 
        	// frmInteropCtrl
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
        	this.ClientSize = new System.Drawing.Size(640, 480);
        	this.Controls.Add(this.btnGame);
        	this.Controls.Add(this.btnPainter);
        	this.Controls.Add(this.btnCalculator);
        	this.Controls.Add(this.btnPlayer);
        	this.Controls.Add(this.labelX1);
        	this.Controls.Add(this.btnExit);
        	this.Controls.Add(this.pictureBox1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        	this.Name = "frmInteropCtrl";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "frmInteropCtrl";
        	this.Load += new System.EventHandler(this.frmInteropCtrl_Load);
        	this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInteropCtrl_FormClosed);
        	((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.OpenFileDialog ofdOpenMedia;
        private PulseButton.PulseButton btnGame;
        private PulseButton.PulseButton btnPainter;
        private PulseButton.PulseButton btnCalculator;
        private PulseButton.PulseButton btnPlayer;

        #endregion

        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.IO.Ports.SerialPort spGlove;
    }
}