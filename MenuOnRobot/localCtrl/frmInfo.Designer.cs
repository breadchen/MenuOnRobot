namespace MenuOnRobot
{
    partial class frmInfo
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
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnSwitch = new DevComponents.DotNetBar.ButtonX();
            this.grpContral = new System.Windows.Forms.GroupBox();
            this.btnMenu = new DevComponents.DotNetBar.ButtonX();
            this.btnAVTV = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.btnMute = new DevComponents.DotNetBar.ButtonX();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnChannelDown = new DevComponents.DotNetBar.ButtonX();
            this.btnChannelUp = new DevComponents.DotNetBar.ButtonX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnVolDown = new DevComponents.DotNetBar.ButtonX();
            this.btnVolUp = new DevComponents.DotNetBar.ButtonX();
            this.lblCategory = new DevComponents.DotNetBar.LabelX();
            this.cbxCategory = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.lblName = new DevComponents.DotNetBar.LabelX();
            this.cbxName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.btnModifyButton = new DevComponents.DotNetBar.ButtonX();
            this.btnAddRemoteController = new DevComponents.DotNetBar.ButtonX();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.btnDelete = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnConfig = new DevComponents.DotNetBar.ButtonX();
            this.grpContral.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(331, 438);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 30);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "返回";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSwitch
            // 
            this.btnSwitch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSwitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSwitch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSwitch.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSwitch.Location = new System.Drawing.Point(32, 43);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Shape = new DevComponents.DotNetBar.EllipticalShapeDescriptor();
            this.btnSwitch.Size = new System.Drawing.Size(50, 50);
            this.btnSwitch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSwitch.TabIndex = 0;
            this.btnSwitch.Text = "开关";
            this.btnSwitch.Click += new System.EventHandler(this.RemoteControllerButton_Click);
            // 
            // grpContral
            // 
            this.grpContral.Controls.Add(this.btnMenu);
            this.grpContral.Controls.Add(this.btnAVTV);
            this.grpContral.Controls.Add(this.buttonX1);
            this.grpContral.Controls.Add(this.btnMute);
            this.grpContral.Controls.Add(this.groupBox2);
            this.grpContral.Controls.Add(this.groupBox1);
            this.grpContral.Controls.Add(this.btnSwitch);
            this.grpContral.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpContral.Location = new System.Drawing.Point(63, 74);
            this.grpContral.Name = "grpContral";
            this.grpContral.Size = new System.Drawing.Size(200, 342);
            this.grpContral.TabIndex = 4;
            this.grpContral.TabStop = false;
            this.grpContral.Text = "客厅电视遥控器";
            // 
            // btnMenu
            // 
            this.btnMenu.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMenu.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnMenu.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMenu.Location = new System.Drawing.Point(114, 242);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(57, 33);
            this.btnMenu.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnMenu.TabIndex = 3;
            this.btnMenu.Text = "菜单";
            // 
            // btnAVTV
            // 
            this.btnAVTV.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAVTV.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAVTV.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAVTV.Location = new System.Drawing.Point(29, 242);
            this.btnAVTV.Name = "btnAVTV";
            this.btnAVTV.Size = new System.Drawing.Size(57, 33);
            this.btnAVTV.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAVTV.TabIndex = 3;
            this.btnAVTV.Text = "AV/TV";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonX1.Location = new System.Drawing.Point(29, 242);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(57, 33);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 3;
            this.buttonX1.Text = "静音";
            // 
            // btnMute
            // 
            this.btnMute.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMute.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnMute.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMute.Location = new System.Drawing.Point(114, 60);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(57, 33);
            this.btnMute.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnMute.TabIndex = 3;
            this.btnMute.Text = "静音";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnChannelDown);
            this.groupBox2.Controls.Add(this.btnChannelUp);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(111, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(63, 98);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "频道";
            // 
            // btnChannelDown
            // 
            this.btnChannelDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChannelDown.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnChannelDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnChannelDown.Location = new System.Drawing.Point(3, 62);
            this.btnChannelDown.Name = "btnChannelDown";
            this.btnChannelDown.Size = new System.Drawing.Size(57, 33);
            this.btnChannelDown.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnChannelDown.TabIndex = 2;
            this.btnChannelDown.Text = "-";
            // 
            // btnChannelUp
            // 
            this.btnChannelUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChannelUp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnChannelUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnChannelUp.Location = new System.Drawing.Point(3, 26);
            this.btnChannelUp.Name = "btnChannelUp";
            this.btnChannelUp.Size = new System.Drawing.Size(57, 33);
            this.btnChannelUp.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnChannelUp.TabIndex = 1;
            this.btnChannelUp.Text = "+";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnVolDown);
            this.groupBox1.Controls.Add(this.btnVolUp);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(26, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(63, 98);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "音量";
            // 
            // btnVolDown
            // 
            this.btnVolDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnVolDown.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnVolDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnVolDown.Location = new System.Drawing.Point(3, 62);
            this.btnVolDown.Name = "btnVolDown";
            this.btnVolDown.Size = new System.Drawing.Size(57, 33);
            this.btnVolDown.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnVolDown.TabIndex = 1;
            this.btnVolDown.Text = "-";
            // 
            // btnVolUp
            // 
            this.btnVolUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnVolUp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnVolUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnVolUp.Location = new System.Drawing.Point(3, 26);
            this.btnVolUp.Name = "btnVolUp";
            this.btnVolUp.Size = new System.Drawing.Size(57, 33);
            this.btnVolUp.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnVolUp.TabIndex = 0;
            this.btnVolUp.Text = "+";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            // 
            // 
            // 
            this.lblCategory.BackgroundStyle.Class = "";
            this.lblCategory.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblCategory.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCategory.Location = new System.Drawing.Point(13, 30);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(74, 31);
            this.lblCategory.TabIndex = 5;
            this.lblCategory.Text = "类别：";
            // 
            // cbxCategory
            // 
            this.cbxCategory.DisplayMember = "Text";
            this.cbxCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCategory.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbxCategory.FormattingEnabled = true;
            this.cbxCategory.ItemHeight = 25;
            this.cbxCategory.Items.AddRange(new object[] {
            this.comboItem1});
            this.cbxCategory.Location = new System.Drawing.Point(93, 30);
            this.cbxCategory.Name = "cbxCategory";
            this.cbxCategory.Size = new System.Drawing.Size(144, 31);
            this.cbxCategory.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxCategory.TabIndex = 6;
            this.cbxCategory.SelectedIndexChanged += new System.EventHandler(this.cbxCategory_SelectedIndexChanged);
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "电视遥控器";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            // 
            // 
            // 
            this.lblName.BackgroundStyle.Class = "";
            this.lblName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblName.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.Location = new System.Drawing.Point(13, 78);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(74, 31);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "名称：";
            // 
            // cbxName
            // 
            this.cbxName.DisplayMember = "Text";
            this.cbxName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxName.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbxName.FormattingEnabled = true;
            this.cbxName.ItemHeight = 25;
            this.cbxName.Items.AddRange(new object[] {
            this.comboItem2});
            this.cbxName.Location = new System.Drawing.Point(93, 78);
            this.cbxName.Name = "cbxName";
            this.cbxName.Size = new System.Drawing.Size(144, 31);
            this.cbxName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxName.TabIndex = 6;
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "客厅";
            // 
            // btnModifyButton
            // 
            this.btnModifyButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnModifyButton.AutoSize = true;
            this.btnModifyButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnModifyButton.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnModifyButton.Location = new System.Drawing.Point(88, 141);
            this.btnModifyButton.Name = "btnModifyButton";
            this.btnModifyButton.Size = new System.Drawing.Size(75, 33);
            this.btnModifyButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnModifyButton.TabIndex = 7;
            this.btnModifyButton.Text = "修改";
            this.btnModifyButton.Click += new System.EventHandler(this.btnModifyButton_Click);
            // 
            // btnAddRemoteController
            // 
            this.btnAddRemoteController.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddRemoteController.AutoSize = true;
            this.btnAddRemoteController.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddRemoteController.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddRemoteController.Location = new System.Drawing.Point(7, 141);
            this.btnAddRemoteController.Name = "btnAddRemoteController";
            this.btnAddRemoteController.Size = new System.Drawing.Size(75, 33);
            this.btnAddRemoteController.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAddRemoteController.TabIndex = 7;
            this.btnAddRemoteController.Text = "添加";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbxCategory);
            this.groupBox3.Controls.Add(this.lblCategory);
            this.groupBox3.Controls.Add(this.lblName);
            this.groupBox3.Controls.Add(this.cbxName);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(328, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(250, 126);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "选择遥控器";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblTips);
            this.groupBox4.Controls.Add(this.btnAddRemoteController);
            this.groupBox4.Controls.Add(this.btnDelete);
            this.groupBox4.Controls.Add(this.btnModifyButton);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(328, 236);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(250, 180);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "管理遥控器";
            // 
            // lblTips
            // 
            // 
            // 
            // 
            this.lblTips.BackgroundStyle.Class = "";
            this.lblTips.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTips.Location = new System.Drawing.Point(6, 30);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(237, 104);
            this.lblTips.TabIndex = 8;
            this.lblTips.Text = "显示操作提示";
            this.lblTips.TextAlignment = System.Drawing.StringAlignment.Center;
            this.lblTips.WordWrap = true;
            // 
            // btnDelete
            // 
            this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDelete.AutoSize = true;
            this.btnDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDelete.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.Location = new System.Drawing.Point(169, 141);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 33);
            this.btnDelete.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除";
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
            this.labelX1.Location = new System.Drawing.Point(198, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(244, 45);
            this.labelX1.TabIndex = 10;
            this.labelX1.Text = "家电中央控制器";
            // 
            // btnConfig
            // 
            this.btnConfig.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfig.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnConfig.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnConfig.Location = new System.Drawing.Point(209, 438);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(100, 30);
            this.btnConfig.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnConfig.TabIndex = 2;
            this.btnConfig.Text = "高级";
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // frmInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grpContral);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmInfo";
            this.Load += new System.EventHandler(this.frmInfo_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInfo_FormClosed);
            this.grpContral.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnSwitch;
        private System.Windows.Forms.GroupBox grpContral;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.ButtonX btnChannelDown;
        private DevComponents.DotNetBar.ButtonX btnChannelUp;
        private DevComponents.DotNetBar.ButtonX btnVolDown;
        private DevComponents.DotNetBar.ButtonX btnVolUp;
        private DevComponents.DotNetBar.ButtonX btnMenu;
        private DevComponents.DotNetBar.ButtonX btnAVTV;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX btnMute;
        private DevComponents.DotNetBar.LabelX lblCategory;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxCategory;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.DotNetBar.LabelX lblName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxName;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.DotNetBar.ButtonX btnModifyButton;
        private DevComponents.DotNetBar.ButtonX btnAddRemoteController;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevComponents.DotNetBar.ButtonX btnDelete;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.DotNetBar.ButtonX btnConfig;

    }
}