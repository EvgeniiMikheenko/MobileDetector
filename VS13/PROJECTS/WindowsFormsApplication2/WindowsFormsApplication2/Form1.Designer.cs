namespace WindowsFormsApplication2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.COM4 = new System.IO.Ports.SerialPort(this.components);
            this.com4_open = new System.Windows.Forms.Button();
            this.com4_close = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.textbox = new System.Windows.Forms.RichTextBox();
            this.status = new System.Windows.Forms.RichTextBox();
            this.stop = new System.Windows.Forms.Button();
            this.clr = new System.Windows.Forms.Button();
            this.counter = new System.Windows.Forms.RichTextBox();
            this.adc = new System.Windows.Forms.RichTextBox();
            this.yn = new System.Windows.Forms.RichTextBox();
            this.number = new System.Windows.Forms.RichTextBox();
            this.Graph = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // COM4
            // 
            this.COM4.BaudRate = 115200;
            this.COM4.PortName = "COM4";
            this.COM4.ReadTimeout = 1000;
            this.COM4.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.COM4_DataReceived);
            // 
            // com4_open
            // 
            this.com4_open.Location = new System.Drawing.Point(12, 265);
            this.com4_open.Name = "com4_open";
            this.com4_open.Size = new System.Drawing.Size(51, 23);
            this.com4_open.TabIndex = 1;
            this.com4_open.Text = "OPEN";
            this.com4_open.UseVisualStyleBackColor = true;
            this.com4_open.Click += new System.EventHandler(this.com4_open_Click);
            // 
            // com4_close
            // 
            this.com4_close.Location = new System.Drawing.Point(12, 305);
            this.com4_close.Name = "com4_close";
            this.com4_close.Size = new System.Drawing.Size(51, 23);
            this.com4_close.TabIndex = 2;
            this.com4_close.Text = "CLOSE";
            this.com4_close.UseVisualStyleBackColor = true;
            this.com4_close.Click += new System.EventHandler(this.button2_Click);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(12, 6);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(51, 23);
            this.start.TabIndex = 3;
            this.start.Text = "start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.button1_Click);
            // 
            // textbox
            // 
            this.textbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textbox.Location = new System.Drawing.Point(69, 6);
            this.textbox.Name = "textbox";
            this.textbox.Size = new System.Drawing.Size(52, 464);
            this.textbox.TabIndex = 4;
            this.textbox.Text = "";
            this.textbox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(12, 346);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(51, 32);
            this.status.TabIndex = 5;
            this.status.Text = "";
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(12, 35);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(51, 23);
            this.stop.TabIndex = 6;
            this.stop.Text = "stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // clr
            // 
            this.clr.Location = new System.Drawing.Point(12, 64);
            this.clr.Name = "clr";
            this.clr.Size = new System.Drawing.Size(51, 23);
            this.clr.TabIndex = 7;
            this.clr.Text = "clr";
            this.clr.UseVisualStyleBackColor = true;
            this.clr.Click += new System.EventHandler(this.clr_Click);
            // 
            // counter
            // 
            this.counter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.counter.Location = new System.Drawing.Point(241, 6);
            this.counter.Name = "counter";
            this.counter.Size = new System.Drawing.Size(163, 464);
            this.counter.TabIndex = 8;
            this.counter.Text = "";
            // 
            // adc
            // 
            this.adc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.adc.Location = new System.Drawing.Point(127, 6);
            this.adc.Name = "adc";
            this.adc.Size = new System.Drawing.Size(54, 464);
            this.adc.TabIndex = 9;
            this.adc.Text = "";
            // 
            // yn
            // 
            this.yn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.yn.Location = new System.Drawing.Point(187, 6);
            this.yn.Name = "yn";
            this.yn.Size = new System.Drawing.Size(48, 464);
            this.yn.TabIndex = 10;
            this.yn.Text = "";
            // 
            // number
            // 
            this.number.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.number.Location = new System.Drawing.Point(69, 476);
            this.number.Name = "number";
            this.number.Size = new System.Drawing.Size(166, 26);
            this.number.TabIndex = 11;
            this.number.Text = "";
            // 
            // Graph
            // 
            this.Graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Graph.IsShowPointValues = false;
            this.Graph.Location = new System.Drawing.Point(410, 6);
            this.Graph.Name = "Graph";
            this.Graph.PointValueFormat = "G";
            this.Graph.Size = new System.Drawing.Size(798, 496);
            this.Graph.TabIndex = 12;
            this.Graph.Load += new System.EventHandler(this.Graph_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1286, 520);
            this.Controls.Add(this.Graph);
            this.Controls.Add(this.number);
            this.Controls.Add(this.yn);
            this.Controls.Add(this.adc);
            this.Controls.Add(this.counter);
            this.Controls.Add(this.clr);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.status);
            this.Controls.Add(this.textbox);
            this.Controls.Add(this.start);
            this.Controls.Add(this.com4_close);
            this.Controls.Add(this.com4_open);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.IO.Ports.SerialPort COM4;
        private System.Windows.Forms.Button com4_open;
        private System.Windows.Forms.Button com4_close;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.RichTextBox textbox;
        private System.Windows.Forms.RichTextBox status;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Button clr;
        private System.Windows.Forms.RichTextBox counter;
        private System.Windows.Forms.RichTextBox adc;
        private System.Windows.Forms.RichTextBox yn;
        private System.Windows.Forms.RichTextBox number;
        private ZedGraph.ZedGraphControl Graph;
    }
}

