namespace serial_tst
{
    partial class GraphForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.COM3 = new System.IO.Ports.SerialPort(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.GetSysTime = new System.Windows.Forms.Button();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PortConfigBtn = new System.Windows.Forms.Button();
            this.PortSelect = new System.Windows.Forms.ComboBox();
            this.TimeDatebtn = new System.Windows.Forms.Button();
            this.setTime = new System.Windows.Forms.MaskedTextBox();
            this.setDate = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // COM3
            // 
            this.COM3.BaudRate = 115200;
            this.COM3.PortName = "COM6";
            this.COM3.ReadTimeout = 500;
            this.COM3.WriteTimeout = 500;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1169, 743);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "CLR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.Location = new System.Drawing.Point(252, 701);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(75, 21);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.AutoSize = true;
            this.button3.Location = new System.Drawing.Point(171, 701);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "START";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(333, 700);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "STOP";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(44, 701);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(1271, 743);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(81, 23);
            this.button6.TabIndex = 10;
            this.button6.Text = "SEND";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // GetSysTime
            // 
            this.GetSysTime.Location = new System.Drawing.Point(945, 741);
            this.GetSysTime.Name = "GetSysTime";
            this.GetSysTime.Size = new System.Drawing.Size(103, 23);
            this.GetSysTime.TabIndex = 7;
            this.GetSysTime.Text = "GetSysTime";
            this.GetSysTime.UseVisualStyleBackColor = true;
            this.GetSysTime.Click += new System.EventHandler(this.button5_Click);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zedGraphControl1.BackColor = System.Drawing.Color.Blue;
            this.zedGraphControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.zedGraphControl1.IsShowPointValues = true;
            this.zedGraphControl1.Location = new System.Drawing.Point(19, 65);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.PointValueFormat = "G";
            this.zedGraphControl1.Size = new System.Drawing.Size(1420, 612);
            this.zedGraphControl1.TabIndex = 11;
            this.zedGraphControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.zedGraphControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.zedGraphControl1_MouseClick);
            this.zedGraphControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mousedown);
            this.zedGraphControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);
            this.zedGraphControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouseup);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1358, 741);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.richTextBox2.Location = new System.Drawing.Point(708, 695);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(71, 23);
            this.richTextBox2.TabIndex = 13;
            this.richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.richTextBox3.Location = new System.Drawing.Point(785, 695);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(71, 23);
            this.richTextBox3.TabIndex = 14;
            this.richTextBox3.Text = "";
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(44, 730);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(279, 45);
            this.trackBar1.TabIndex = 17;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // richTextBox4
            // 
            this.richTextBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox4.Location = new System.Drawing.Point(329, 730);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.ReadOnly = true;
            this.richTextBox4.Size = new System.Drawing.Size(51, 26);
            this.richTextBox4.TabIndex = 18;
            this.richTextBox4.Text = "";
            // 
            // richTextBox5
            // 
            this.richTextBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox5.Location = new System.Drawing.Point(438, 730);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.ReadOnly = true;
            this.richTextBox5.Size = new System.Drawing.Size(52, 26);
            this.richTextBox5.TabIndex = 19;
            this.richTextBox5.Text = "";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(330, 757);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "A";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(435, 757);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "B";
            // 
            // PortConfigBtn
            // 
            this.PortConfigBtn.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.PortConfigBtn.Location = new System.Drawing.Point(1262, 683);
            this.PortConfigBtn.Name = "PortConfigBtn";
            this.PortConfigBtn.Size = new System.Drawing.Size(128, 23);
            this.PortConfigBtn.TabIndex = 22;
            this.PortConfigBtn.Text = "PortConfig";
            this.PortConfigBtn.UseVisualStyleBackColor = true;
            this.PortConfigBtn.Click += new System.EventHandler(this.PortConfigBtn_Click);
            // 
            // PortSelect
            // 
            this.PortSelect.FormattingEnabled = true;
            this.PortSelect.Items.AddRange(new object[] {
            "b"});
            this.PortSelect.Location = new System.Drawing.Point(708, 745);
            this.PortSelect.Name = "PortSelect";
            this.PortSelect.Size = new System.Drawing.Size(121, 21);
            this.PortSelect.TabIndex = 23;
            this.PortSelect.SelectedIndexChanged += new System.EventHandler(this.PortSelect_SelectedIndexChanged);
            this.PortSelect.Click += new System.EventHandler(this.PortSelect_Click);
            // 
            // TimeDatebtn
            // 
            this.TimeDatebtn.Location = new System.Drawing.Point(1262, 713);
            this.TimeDatebtn.Name = "TimeDatebtn";
            this.TimeDatebtn.Size = new System.Drawing.Size(128, 23);
            this.TimeDatebtn.TabIndex = 28;
            this.TimeDatebtn.Text = "TimeDate config";
            this.TimeDatebtn.UseVisualStyleBackColor = true;
            this.TimeDatebtn.Click += new System.EventHandler(this.TimeDatebtn_Click);
            // 
            // setTime
            // 
            this.setTime.Location = new System.Drawing.Point(945, 685);
            this.setTime.Mask = "00:00:00";
            this.setTime.Name = "setTime";
            this.setTime.Size = new System.Drawing.Size(100, 20);
            this.setTime.TabIndex = 29;
            // 
            // setDate
            // 
            this.setDate.Location = new System.Drawing.Point(945, 703);
            this.setDate.Mask = "00/00/0000";
            this.setDate.Name = "setDate";
            this.setDate.Size = new System.Drawing.Size(100, 20);
            this.setDate.TabIndex = 30;
            this.setDate.ValidatingType = typeof(System.DateTime);
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1450, 778);
            this.Controls.Add(this.setDate);
            this.Controls.Add(this.setTime);
            this.Controls.Add(this.TimeDatebtn);
            this.Controls.Add(this.PortSelect);
            this.Controls.Add(this.PortConfigBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox5);
            this.Controls.Add(this.richTextBox4);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.GetSysTime);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button2);
            this.Name = "GraphForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button GetSysTime;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button PortConfigBtn;
        private System.Windows.Forms.ComboBox PortSelect;
        private System.Windows.Forms.Button TimeDatebtn;
        private System.Windows.Forms.MaskedTextBox setTime;
        private System.Windows.Forms.MaskedTextBox setDate;
        public System.IO.Ports.SerialPort COM3;
    }
}

