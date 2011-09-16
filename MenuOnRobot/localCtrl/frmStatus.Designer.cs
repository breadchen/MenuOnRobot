namespace MenuOnRobot
{
    partial class frmStatus
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
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.tlpLocation = new System.Windows.Forms.TableLayoutPanel();
            this.pulseButton1 = new PulseButton.PulseButton();
            this.pulseButton3 = new PulseButton.PulseButton();
            this.pulseButton2 = new PulseButton.PulseButton();
            this.btnCurrentPos = new PulseButton.PulseButton();
            this.btnRobotPos = new PulseButton.PulseButton();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.grpLocationInfo = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.grpEnvironmentInfo = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblAdvice = new DevComponents.DotNetBar.LabelX();
            this.lblAverageBrightness = new DevComponents.DotNetBar.LabelX();
            this.lblAverageHumidity = new DevComponents.DotNetBar.LabelX();
            this.lblAverageTemp = new DevComponents.DotNetBar.LabelX();
            this.grpSafty = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblGuardStatus = new DevComponents.DotNetBar.LabelX();
            this.lblSmoke = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnInfoControl = new DevComponents.DotNetBar.ButtonX();
            this.tlpLocation.SuspendLayout();
            this.grpLocationInfo.SuspendLayout();
            this.grpEnvironmentInfo.SuspendLayout();
            this.grpSafty.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007Blue;
            // 
            // tlpLocation
            // 
            this.tlpLocation.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpLocation.ColumnCount = 6;
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLocation.Controls.Add(this.pulseButton1, 1, 4);
            this.tlpLocation.Controls.Add(this.pulseButton3, 2, 1);
            this.tlpLocation.Controls.Add(this.pulseButton2, 4, 4);
            this.tlpLocation.Controls.Add(this.btnCurrentPos, 1, 1);
            this.tlpLocation.Controls.Add(this.btnRobotPos, 3, 3);
            this.tlpLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLocation.Location = new System.Drawing.Point(0, 0);
            this.tlpLocation.Name = "tlpLocation";
            this.tlpLocation.RowCount = 6;
            this.tlpLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.64392F));
            this.tlpLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.48751F));
            this.tlpLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.48751F));
            this.tlpLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.48751F));
            this.tlpLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.63566F));
            this.tlpLocation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.25788F));
            this.tlpLocation.Size = new System.Drawing.Size(403, 394);
            this.tlpLocation.TabIndex = 0;
            // 
            // pulseButton1
            // 
            this.pulseButton1.ButtonColorBottom = System.Drawing.Color.Green;
            this.pulseButton1.ButtonColorTop = System.Drawing.Color.YellowGreen;
            this.pulseButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pulseButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButton1.Location = new System.Drawing.Point(70, 261);
            this.pulseButton1.Name = "pulseButton1";
            this.pulseButton1.PulseSpeed = 0.2F;
            this.pulseButton1.PulseWidth = 3;
            this.pulseButton1.ShapeType = PulseButton.PulseButton.Shape.Rectangle;
            this.pulseButton1.Size = new System.Drawing.Size(59, 62);
            this.pulseButton1.TabIndex = 7;
            this.pulseButton1.Text = "客厅";
            this.pulseButton1.UseVisualStyleBackColor = true;
            // 
            // pulseButton3
            // 
            this.pulseButton3.ButtonColorBottom = System.Drawing.Color.Green;
            this.pulseButton3.ButtonColorTop = System.Drawing.Color.YellowGreen;
            this.pulseButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pulseButton3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButton3.Location = new System.Drawing.Point(136, 69);
            this.pulseButton3.Name = "pulseButton3";
            this.pulseButton3.PulseSpeed = 0.2F;
            this.pulseButton3.PulseWidth = 3;
            this.pulseButton3.ShapeType = PulseButton.PulseButton.Shape.Rectangle;
            this.pulseButton3.Size = new System.Drawing.Size(59, 57);
            this.pulseButton3.TabIndex = 4;
            this.pulseButton3.Text = "主卧";
            this.pulseButton3.UseVisualStyleBackColor = true;
            // 
            // pulseButton2
            // 
            this.pulseButton2.ButtonColorBottom = System.Drawing.Color.Green;
            this.pulseButton2.ButtonColorTop = System.Drawing.Color.YellowGreen;
            this.pulseButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pulseButton2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButton2.Location = new System.Drawing.Point(268, 261);
            this.pulseButton2.Name = "pulseButton2";
            this.pulseButton2.PulseSpeed = 0.2F;
            this.pulseButton2.PulseWidth = 3;
            this.pulseButton2.ShapeType = PulseButton.PulseButton.Shape.Rectangle;
            this.pulseButton2.Size = new System.Drawing.Size(59, 62);
            this.pulseButton2.TabIndex = 3;
            this.pulseButton2.Text = "洗手间";
            this.pulseButton2.UseVisualStyleBackColor = true;
            // 
            // btnCurrentPos
            // 
            this.btnCurrentPos.ButtonColorBottom = System.Drawing.Color.Maroon;
            this.btnCurrentPos.ButtonColorTop = System.Drawing.Color.Red;
            this.btnCurrentPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCurrentPos.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCurrentPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCurrentPos.Location = new System.Drawing.Point(70, 69);
            this.btnCurrentPos.Name = "btnCurrentPos";
            this.btnCurrentPos.PulseSpeed = 0.2F;
            this.btnCurrentPos.PulseWidth = 6;
            this.btnCurrentPos.Size = new System.Drawing.Size(59, 57);
            this.btnCurrentPos.TabIndex = 0;
            this.btnCurrentPos.Text = "当前位置";
            this.btnCurrentPos.UseVisualStyleBackColor = true;
            this.btnCurrentPos.Click += new System.EventHandler(this.btnCurrentPos_Click);
            // 
            // btnRobotPos
            // 
            this.btnRobotPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRobotPos.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRobotPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRobotPos.Location = new System.Drawing.Point(202, 197);
            this.btnRobotPos.Name = "btnRobotPos";
            this.btnRobotPos.PulseSpeed = 0.3F;
            this.btnRobotPos.PulseWidth = 5;
            this.btnRobotPos.Size = new System.Drawing.Size(59, 57);
            this.btnRobotPos.TabIndex = 2;
            this.btnRobotPos.Text = "服务管家";
            this.btnRobotPos.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(323, 446);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 30);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "返回";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // grpLocationInfo
            // 
            this.grpLocationInfo.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpLocationInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpLocationInfo.Controls.Add(this.tlpLocation);
            this.grpLocationInfo.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpLocationInfo.Location = new System.Drawing.Point(12, 12);
            this.grpLocationInfo.Name = "grpLocationInfo";
            this.grpLocationInfo.Size = new System.Drawing.Size(409, 429);
            // 
            // 
            // 
            this.grpLocationInfo.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpLocationInfo.Style.BackColorGradientAngle = 90;
            this.grpLocationInfo.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpLocationInfo.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpLocationInfo.Style.BorderBottomWidth = 1;
            this.grpLocationInfo.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpLocationInfo.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpLocationInfo.Style.BorderLeftWidth = 1;
            this.grpLocationInfo.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpLocationInfo.Style.BorderRightWidth = 1;
            this.grpLocationInfo.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpLocationInfo.Style.BorderTopWidth = 1;
            this.grpLocationInfo.Style.Class = "";
            this.grpLocationInfo.Style.CornerDiameter = 4;
            this.grpLocationInfo.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpLocationInfo.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpLocationInfo.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpLocationInfo.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpLocationInfo.StyleMouseDown.Class = "";
            this.grpLocationInfo.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpLocationInfo.StyleMouseOver.Class = "";
            this.grpLocationInfo.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpLocationInfo.TabIndex = 2;
            this.grpLocationInfo.Text = "位置信息";
            // 
            // grpEnvironmentInfo
            // 
            this.grpEnvironmentInfo.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpEnvironmentInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpEnvironmentInfo.Controls.Add(this.lblAdvice);
            this.grpEnvironmentInfo.Controls.Add(this.lblAverageBrightness);
            this.grpEnvironmentInfo.Controls.Add(this.lblAverageHumidity);
            this.grpEnvironmentInfo.Controls.Add(this.lblAverageTemp);
            this.grpEnvironmentInfo.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpEnvironmentInfo.Location = new System.Drawing.Point(440, 60);
            this.grpEnvironmentInfo.Name = "grpEnvironmentInfo";
            this.grpEnvironmentInfo.Size = new System.Drawing.Size(183, 234);
            // 
            // 
            // 
            this.grpEnvironmentInfo.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpEnvironmentInfo.Style.BackColorGradientAngle = 90;
            this.grpEnvironmentInfo.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpEnvironmentInfo.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpEnvironmentInfo.Style.BorderBottomWidth = 1;
            this.grpEnvironmentInfo.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpEnvironmentInfo.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpEnvironmentInfo.Style.BorderLeftWidth = 1;
            this.grpEnvironmentInfo.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpEnvironmentInfo.Style.BorderRightWidth = 1;
            this.grpEnvironmentInfo.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpEnvironmentInfo.Style.BorderTopWidth = 1;
            this.grpEnvironmentInfo.Style.Class = "";
            this.grpEnvironmentInfo.Style.CornerDiameter = 4;
            this.grpEnvironmentInfo.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpEnvironmentInfo.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpEnvironmentInfo.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpEnvironmentInfo.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpEnvironmentInfo.StyleMouseDown.Class = "";
            this.grpEnvironmentInfo.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpEnvironmentInfo.StyleMouseOver.Class = "";
            this.grpEnvironmentInfo.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpEnvironmentInfo.TabIndex = 3;
            this.grpEnvironmentInfo.Text = "环境信息";
            // 
            // lblAdvice
            // 
            // 
            // 
            // 
            this.lblAdvice.BackgroundStyle.Class = "";
            this.lblAdvice.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblAdvice.Location = new System.Drawing.Point(3, 114);
            this.lblAdvice.Name = "lblAdvice";
            this.lblAdvice.Size = new System.Drawing.Size(171, 82);
            this.lblAdvice.TabIndex = 1;
            this.lblAdvice.Text = "建议：打开加湿器,拉上一些窗帘";
            this.lblAdvice.TextAlignment = System.Drawing.StringAlignment.Center;
            this.lblAdvice.WordWrap = true;
            // 
            // lblAverageBrightness
            // 
            // 
            // 
            // 
            this.lblAverageBrightness.BackgroundStyle.Class = "";
            this.lblAverageBrightness.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblAverageBrightness.Location = new System.Drawing.Point(3, 77);
            this.lblAverageBrightness.Name = "lblAverageBrightness";
            this.lblAverageBrightness.Size = new System.Drawing.Size(171, 31);
            this.lblAverageBrightness.TabIndex = 0;
            this.lblAverageBrightness.Text = "平均亮度：明亮";
            this.lblAverageBrightness.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblAverageHumidity
            // 
            // 
            // 
            // 
            this.lblAverageHumidity.BackgroundStyle.Class = "";
            this.lblAverageHumidity.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblAverageHumidity.Location = new System.Drawing.Point(3, 40);
            this.lblAverageHumidity.Name = "lblAverageHumidity";
            this.lblAverageHumidity.Size = new System.Drawing.Size(171, 31);
            this.lblAverageHumidity.TabIndex = 0;
            this.lblAverageHumidity.Text = "平均湿度：16%";
            this.lblAverageHumidity.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblAverageTemp
            // 
            // 
            // 
            // 
            this.lblAverageTemp.BackgroundStyle.Class = "";
            this.lblAverageTemp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblAverageTemp.Location = new System.Drawing.Point(3, 3);
            this.lblAverageTemp.Name = "lblAverageTemp";
            this.lblAverageTemp.Size = new System.Drawing.Size(171, 31);
            this.lblAverageTemp.TabIndex = 0;
            this.lblAverageTemp.Text = "平均温度：30C";
            this.lblAverageTemp.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // grpSafty
            // 
            this.grpSafty.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpSafty.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpSafty.Controls.Add(this.lblGuardStatus);
            this.grpSafty.Controls.Add(this.lblSmoke);
            this.grpSafty.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpSafty.Location = new System.Drawing.Point(440, 301);
            this.grpSafty.Name = "grpSafty";
            this.grpSafty.Size = new System.Drawing.Size(183, 140);
            // 
            // 
            // 
            this.grpSafty.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpSafty.Style.BackColorGradientAngle = 90;
            this.grpSafty.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpSafty.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSafty.Style.BorderBottomWidth = 1;
            this.grpSafty.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpSafty.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSafty.Style.BorderLeftWidth = 1;
            this.grpSafty.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSafty.Style.BorderRightWidth = 1;
            this.grpSafty.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSafty.Style.BorderTopWidth = 1;
            this.grpSafty.Style.Class = "";
            this.grpSafty.Style.CornerDiameter = 4;
            this.grpSafty.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpSafty.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpSafty.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpSafty.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpSafty.StyleMouseDown.Class = "";
            this.grpSafty.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpSafty.StyleMouseOver.Class = "";
            this.grpSafty.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpSafty.TabIndex = 3;
            this.grpSafty.Text = "安保信息";
            // 
            // lblGuardStatus
            // 
            // 
            // 
            // 
            this.lblGuardStatus.BackgroundStyle.Class = "";
            this.lblGuardStatus.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblGuardStatus.Location = new System.Drawing.Point(8, 71);
            this.lblGuardStatus.Name = "lblGuardStatus";
            this.lblGuardStatus.Size = new System.Drawing.Size(161, 31);
            this.lblGuardStatus.TabIndex = 0;
            this.lblGuardStatus.Text = "门禁状态：设防";
            this.lblGuardStatus.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblSmoke
            // 
            this.lblSmoke.AutoSize = true;
            // 
            // 
            // 
            this.lblSmoke.BackgroundStyle.Class = "";
            this.lblSmoke.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSmoke.Location = new System.Drawing.Point(8, 4);
            this.lblSmoke.Name = "lblSmoke";
            this.lblSmoke.Size = new System.Drawing.Size(161, 55);
            this.lblSmoke.TabIndex = 0;
            this.lblSmoke.Text = "可燃气体水平：\r\n正常";
            this.lblSmoke.TextAlignment = System.Drawing.StringAlignment.Center;
            this.lblSmoke.WordWrap = true;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(436, 13);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(192, 41);
            this.labelX2.TabIndex = 11;
            this.labelX2.Text = "室内信息监控";
            // 
            // btnInfoControl
            // 
            this.btnInfoControl.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnInfoControl.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnInfoControl.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnInfoControl.Location = new System.Drawing.Point(217, 446);
            this.btnInfoControl.Name = "btnInfoControl";
            this.btnInfoControl.Size = new System.Drawing.Size(100, 30);
            this.btnInfoControl.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnInfoControl.TabIndex = 1;
            this.btnInfoControl.Text = "设置环境信息";
            this.btnInfoControl.Click += new System.EventHandler(this.btnInfoControl_Click);
            // 
            // frmStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.grpSafty);
            this.Controls.Add(this.grpEnvironmentInfo);
            this.Controls.Add(this.grpLocationInfo);
            this.Controls.Add(this.btnInfoControl);
            this.Controls.Add(this.btnExit);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "状态信息";
            this.Load += new System.EventHandler(this.frmStatus_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStatus_FormClosed);
            this.tlpLocation.ResumeLayout(false);
            this.grpLocationInfo.ResumeLayout(false);
            this.grpEnvironmentInfo.ResumeLayout(false);
            this.grpSafty.ResumeLayout(false);
            this.grpSafty.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private DevComponents.DotNetBar.LabelX lblSmoke;
        private DevComponents.DotNetBar.LabelX lblGuardStatus;

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private System.Windows.Forms.TableLayoutPanel tlpLocation;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.GroupPanel grpLocationInfo;
        private DevComponents.DotNetBar.Controls.GroupPanel grpEnvironmentInfo;
        private PulseButton.PulseButton pulseButton1;
        private PulseButton.PulseButton btnCurrentPos;
        private PulseButton.PulseButton btnRobotPos;
        private PulseButton.PulseButton pulseButton2;
        private PulseButton.PulseButton pulseButton3;
        private DevComponents.DotNetBar.LabelX lblAverageTemp;
        private DevComponents.DotNetBar.LabelX lblAverageHumidity;
        private DevComponents.DotNetBar.LabelX lblAverageBrightness;
        private DevComponents.DotNetBar.Controls.GroupPanel grpSafty;
        private DevComponents.DotNetBar.LabelX lblAdvice;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btnInfoControl;


    }
}