namespace boxes
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btn_show = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblOnline = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_print = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "智能货柜基础服务器运营版（二代）";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_show,
            this.btn_exit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // btn_show
            // 
            this.btn_show.Name = "btn_show";
            this.btn_show.Size = new System.Drawing.Size(100, 22);
            this.btn_show.Text = "显示";
            this.btn_show.Click += new System.EventHandler(this.btn_show_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(100, 22);
            this.btn_exit.Text = "退出";
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(537, 395);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(66, 40);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "停止服务";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 409);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "0";
            // 
            // lblOnline
            // 
            this.lblOnline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOnline.AutoSize = true;
            this.lblOnline.Font = new System.Drawing.Font("宋体", 12F);
            this.lblOnline.Location = new System.Drawing.Point(12, 405);
            this.lblOnline.Name = "lblOnline";
            this.lblOnline.Size = new System.Drawing.Size(136, 16);
            this.lblOnline.TabIndex = 10;
            this.lblOnline.Text = "当前连接主机数：";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(615, 389);
            this.txtLog.TabIndex = 12;
            this.txtLog.Text = "";
            this.txtLog.DoubleClick += new System.EventHandler(this.txtLog_DoubleClick);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(154, 405);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "当前连接客户端：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(279, 409);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "0";
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn_print
            // 
            this.btn_print.Location = new System.Drawing.Point(316, 395);
            this.btn_print.Name = "btn_print";
            this.btn_print.Size = new System.Drawing.Size(111, 37);
            this.btn_print.TabIndex = 15;
            this.btn_print.Text = "打印socket信息";
            this.btn_print.UseVisualStyleBackColor = true;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(433, 397);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(94, 31);
            this.btn_clear.TabIndex = 16;
            this.btn_clear.Text = "清理";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 440);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_print);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblOnline);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "智能货柜基础服务器运营版（二代）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btn_show;
        private System.Windows.Forms.ToolStripMenuItem btn_exit;
        private System.Windows.Forms.Button btnStart;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOnline;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.Button btn_clear;
    }
}

