namespace MenuOnRobot
{
    partial class frmStarter
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStarter));
        	this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
        	this.btnExit = new DevComponents.DotNetBar.ButtonX();
        	this.btnRemoteCam = new PulseButton.PulseButton();
        	this.btnLocalCtrl = new PulseButton.PulseButton();
        	this.labelX1 = new DevComponents.DotNetBar.LabelX();
        	this.btnIntroduce = new PulseButton.PulseButton();
        	this.btnRecognition = new PulseButton.PulseButton();
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
        	this.btnExit.Text = "退出";
        	this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
        	// 
        	// btnRemoteCam
        	// 
        	this.btnRemoteCam.BackColor = System.Drawing.Color.White;
        	this.btnRemoteCam.ButtonColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
        	this.btnRemoteCam.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.btnRemoteCam.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.btnRemoteCam.Location = new System.Drawing.Point(18, 141);
        	this.btnRemoteCam.Name = "btnRemoteCam";
        	this.btnRemoteCam.PulseSpeed = 0.3F;
        	this.btnRemoteCam.ShapeType = PulseButton.PulseButton.Shape.Rectangle;
        	this.btnRemoteCam.Size = new System.Drawing.Size(180, 198);
        	this.btnRemoteCam.TabIndex = 2;
        	this.btnRemoteCam.Text = "远程聊天";
        	this.btnRemoteCam.UseVisualStyleBackColor = false;
        	this.btnRemoteCam.Click += new System.EventHandler(this.btnRemoteCam_Click);
        	// 
        	// btnLocalCtrl
        	// 
        	this.btnLocalCtrl.BackColor = System.Drawing.Color.White;
        	this.btnLocalCtrl.ButtonColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
        	this.btnLocalCtrl.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.btnLocalCtrl.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.btnLocalCtrl.Location = new System.Drawing.Point(204, 141);
        	this.btnLocalCtrl.Name = "btnLocalCtrl";
        	this.btnLocalCtrl.PulseSpeed = 0.3F;
        	this.btnLocalCtrl.ShapeType = PulseButton.PulseButton.Shape.Rectangle;
        	this.btnLocalCtrl.Size = new System.Drawing.Size(180, 198);
        	this.btnLocalCtrl.TabIndex = 2;
        	this.btnLocalCtrl.Text = "本地控制";
        	this.btnLocalCtrl.UseVisualStyleBackColor = false;
        	this.btnLocalCtrl.Click += new System.EventHandler(this.btnLocalCtrl_Click);
        	// 
        	// labelX1
        	// 
        	this.labelX1.BackColor = System.Drawing.Color.White;
        	// 
        	// 
        	// 
        	this.labelX1.BackgroundStyle.Class = "";
        	this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
        	this.labelX1.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.labelX1.Location = new System.Drawing.Point(248, 30);
        	this.labelX1.Name = "labelX1";
        	this.labelX1.Size = new System.Drawing.Size(380, 83);
        	this.labelX1.TabIndex = 3;
        	this.labelX1.Text = "欢迎使用新概念网络型\r\n室内体感服务系统";
        	this.labelX1.TextAlignment = System.Drawing.StringAlignment.Center;
        	this.labelX1.WordWrap = true;
        	// 
        	// btnIntroduce
        	// 
        	this.btnIntroduce.BackColor = System.Drawing.Color.White;
        	this.btnIntroduce.ButtonColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
        	this.btnIntroduce.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.btnIntroduce.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.btnIntroduce.Location = new System.Drawing.Point(499, 141);
        	this.btnIntroduce.Name = "btnIntroduce";
        	this.btnIntroduce.PulseSpeed = 0.3F;
        	this.btnIntroduce.Size = new System.Drawing.Size(69, 69);
        	this.btnIntroduce.TabIndex = 2;
        	this.btnIntroduce.Tag = "1";
        	this.btnIntroduce.Text = "介绍";
        	this.btnIntroduce.UseVisualStyleBackColor = false;
        	this.btnIntroduce.Click += new System.EventHandler(this.BtnIntroduceClick);
        	// 
        	// btnRecognition
        	// 
        	this.btnRecognition.BackColor = System.Drawing.Color.White;
        	this.btnRecognition.ButtonColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
        	this.btnRecognition.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.btnRecognition.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.btnRecognition.Location = new System.Drawing.Point(424, 141);
        	this.btnRecognition.Name = "btnRecognition";
        	this.btnRecognition.PulseSpeed = 0.3F;
        	this.btnRecognition.Size = new System.Drawing.Size(69, 69);
        	this.btnRecognition.TabIndex = 2;
        	this.btnRecognition.Tag = "1";
        	this.btnRecognition.Text = "开启语音识别";
        	this.btnRecognition.UseVisualStyleBackColor = false;
        	this.btnRecognition.Click += new System.EventHandler(this.BtnRecognitionClick);
        	// 
        	// frmStarter
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
        	this.CancelButton = this.btnExit;
        	this.ClientSize = new System.Drawing.Size(640, 480);
        	this.Controls.Add(this.labelX1);
        	this.Controls.Add(this.btnRecognition);
        	this.Controls.Add(this.btnIntroduce);
        	this.Controls.Add(this.btnLocalCtrl);
        	this.Controls.Add(this.btnRemoteCam);
        	this.Controls.Add(this.btnExit);
        	this.DoubleBuffered = true;
        	this.EnableGlass = false;
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        	this.Name = "frmStarter";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Load += new System.EventHandler(this.frmStarter_Load);
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmStarterFormClosing);
        	this.ResumeLayout(false);
        }
        private PulseButton.PulseButton btnRecognition;
        private PulseButton.PulseButton btnIntroduce;

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private PulseButton.PulseButton btnRemoteCam;
        private PulseButton.PulseButton btnLocalCtrl;
        private DevComponents.DotNetBar.LabelX labelX1;

    }
}

