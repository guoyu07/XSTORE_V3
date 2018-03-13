namespace boxes
{
    partial class BoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoxForm));
            this.button2 = new System.Windows.Forms.Button();
            this.DevConnect = new System.Windows.Forms.Label();
            this.DevConnectCount = new System.Windows.Forms.Label();
            this.WeChartConnect = new System.Windows.Forms.Label();
            this.WeChartConnectCount = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.CountTicket = new System.Windows.Forms.Timer(this.components);
            this.BoxContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MinWindow = new System.Windows.Forms.NotifyIcon(this.components);
            this.clear = new System.Windows.Forms.Button();
            this.BoxContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(655, 431);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "停用服务";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.StopService_Click);
            // 
            // DevConnect
            // 
            this.DevConnect.AutoSize = true;
            this.DevConnect.Location = new System.Drawing.Point(12, 436);
            this.DevConnect.Name = "DevConnect";
            this.DevConnect.Size = new System.Drawing.Size(101, 12);
            this.DevConnect.TabIndex = 2;
            this.DevConnect.Text = "当前设备连接数：";
            // 
            // DevConnectCount
            // 
            this.DevConnectCount.AutoSize = true;
            this.DevConnectCount.Location = new System.Drawing.Point(108, 436);
            this.DevConnectCount.Name = "DevConnectCount";
            this.DevConnectCount.Size = new System.Drawing.Size(11, 12);
            this.DevConnectCount.TabIndex = 3;
            this.DevConnectCount.Text = "0";
            // 
            // WeChartConnect
            // 
            this.WeChartConnect.AutoSize = true;
            this.WeChartConnect.Location = new System.Drawing.Point(151, 436);
            this.WeChartConnect.Name = "WeChartConnect";
            this.WeChartConnect.Size = new System.Drawing.Size(113, 12);
            this.WeChartConnect.TabIndex = 4;
            this.WeChartConnect.Text = "当前客户端链接数：";
            // 
            // WeChartConnectCount
            // 
            this.WeChartConnectCount.AutoSize = true;
            this.WeChartConnectCount.Location = new System.Drawing.Point(263, 436);
            this.WeChartConnectCount.Name = "WeChartConnectCount";
            this.WeChartConnectCount.Size = new System.Drawing.Size(11, 12);
            this.WeChartConnectCount.TabIndex = 5;
            this.WeChartConnectCount.Text = "0";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtLog.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtLog.Location = new System.Drawing.Point(0, -1);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(741, 426);
            this.txtLog.TabIndex = 6;
            this.txtLog.Text = "";
            // 
            // CountTicket
            // 
            this.CountTicket.Enabled = true;
            this.CountTicket.Tick += new System.EventHandler(this.CountTicket_Tick);
            // 
            // BoxContextMenuStrip
            // 
            this.BoxContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.BoxContextMenuStrip.Name = "BoxContextMenuStrip";
            this.BoxContextMenuStrip.Size = new System.Drawing.Size(101, 48);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.Show_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.Exit_Click);
            // 
            // MinWindow
            // 
            this.MinWindow.Icon = ((System.Drawing.Icon)(resources.GetObject("MinWindow.Icon")));
            this.MinWindow.Text = "XSTORE";
            this.MinWindow.Visible = true;
            this.MinWindow.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MinWindow_MouseDoubleClick);
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(559, 431);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 23);
            this.clear.TabIndex = 7;
            this.clear.Text = "清理日志";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // BoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(742, 460);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.WeChartConnectCount);
            this.Controls.Add(this.WeChartConnect);
            this.Controls.Add(this.DevConnectCount);
            this.Controls.Add(this.DevConnect);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BoxForm";
            this.Text = "X-Store开箱程序";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BoxForm_FormClosing);
            this.BoxContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Label DevConnect;
        public System.Windows.Forms.Label DevConnectCount;
        public System.Windows.Forms.Label WeChartConnect;
        public System.Windows.Forms.Label WeChartConnectCount;
        public System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Timer CountTicket;
        private System.Windows.Forms.ContextMenuStrip BoxContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon MinWindow;
        private System.Windows.Forms.Button clear;
    }
}