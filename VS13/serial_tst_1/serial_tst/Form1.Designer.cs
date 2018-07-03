namespace GSAD_net_settings
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
            this.COM1 = new System.IO.Ports.SerialPort(this.components);
            this.connect_btn = new System.Windows.Forms.Button();
            this.save_btn = new System.Windows.Forms.Button();
            this.Baudrate = new System.Windows.Forms.ComboBox();
            this.module_addr1 = new System.Windows.Forms.RichTextBox();
            this.Databits = new System.Windows.Forms.ComboBox();
            this.Parity = new System.Windows.Forms.ComboBox();
            this.Stopbit = new System.Windows.Forms.ComboBox();
            this.baudrate_lbl = new System.Windows.Forms.TextBox();
            this.databits_lbl = new System.Windows.Forms.TextBox();
            this.parity_lbl = new System.Windows.Forms.TextBox();
            this.stopbit_lbl = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.module_addr_lbl = new System.Windows.Forms.TextBox();
            this.status_line = new System.Windows.Forms.TextBox();
            this.Search = new System.Windows.Forms.Button();
            this.COM_sw = new System.Windows.Forms.ComboBox();
            this.baudrate_label = new System.Windows.Forms.Label();
            this.databits_label = new System.Windows.Forms.Label();
            this.parity_label = new System.Windows.Forms.Label();
            this.stopbit_label = new System.Windows.Forms.Label();
            this.addr_label = new System.Windows.Forms.Label();
            this.write_btn = new System.Windows.Forms.Button();
            this.Default = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // COM1
            // 
            this.COM1.PortName = "COM3";
            this.COM1.PinChanged += new System.IO.Ports.SerialPinChangedEventHandler(this.Serial_Port);
            // 
            // connect_btn
            // 
            this.connect_btn.Location = new System.Drawing.Point(179, 57);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(199, 53);
            this.connect_btn.TabIndex = 0;
            this.connect_btn.Text = "Connect";
            this.connect_btn.UseVisualStyleBackColor = true;
            this.connect_btn.Click += new System.EventHandler(this.connect_btn_Click);
            // 
            // save_btn
            // 
            this.save_btn.Location = new System.Drawing.Point(262, 285);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(62, 23);
            this.save_btn.TabIndex = 1;
            this.save_btn.Text = "Save";
            this.save_btn.UseVisualStyleBackColor = true;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // Baudrate
            // 
            this.Baudrate.AutoCompleteCustomSource.AddRange(new string[] {
            "50",
            "75",
            "100",
            "110",
            "150",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "56000",
            "57600",
            "115200",
            ""});
            this.Baudrate.BackColor = System.Drawing.Color.White;
            this.Baudrate.FormattingEnabled = true;
            this.Baudrate.Items.AddRange(new object[] {
            "50",
            "75",
            "100",
            "110",
            "150",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "56000",
            "57600",
            "115200"});
            this.Baudrate.Location = new System.Drawing.Point(97, 133);
            this.Baudrate.Name = "Baudrate";
            this.Baudrate.Size = new System.Drawing.Size(174, 21);
            this.Baudrate.TabIndex = 2;
            this.Baudrate.SelectedIndexChanged += new System.EventHandler(this.Baudrate_SelectedIndexChanged);
            // 
            // module_addr1
            // 
            this.module_addr1.DetectUrls = false;
            this.module_addr1.Location = new System.Drawing.Point(97, 256);
            this.module_addr1.MaxLength = 3;
            this.module_addr1.Multiline = false;
            this.module_addr1.Name = "module_addr1";
            this.module_addr1.Size = new System.Drawing.Size(174, 23);
            this.module_addr1.TabIndex = 3;
            this.module_addr1.Text = "";
            this.module_addr1.TextChanged += new System.EventHandler(this.module_addr_TextChanged);
            // 
            // Databits
            // 
            this.Databits.AutoCompleteCustomSource.AddRange(new string[] {
            "8 bit"});
            this.Databits.FormattingEnabled = true;
            this.Databits.Items.AddRange(new object[] {
            "8 bit"});
            this.Databits.Location = new System.Drawing.Point(97, 159);
            this.Databits.Name = "Databits";
            this.Databits.Size = new System.Drawing.Size(174, 21);
            this.Databits.TabIndex = 4;
            this.Databits.SelectedIndexChanged += new System.EventHandler(this.Databits_SelectedIndexChanged);
            // 
            // Parity
            // 
            this.Parity.AutoCompleteCustomSource.AddRange(new string[] {
            "None",
            "Odd",
            "Even"});
            this.Parity.FormattingEnabled = true;
            this.Parity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.Parity.Location = new System.Drawing.Point(97, 185);
            this.Parity.Name = "Parity";
            this.Parity.Size = new System.Drawing.Size(174, 21);
            this.Parity.TabIndex = 5;
            this.Parity.SelectedIndexChanged += new System.EventHandler(this.Parity_SelectedIndexChanged);
            // 
            // Stopbit
            // 
            this.Stopbit.AutoCompleteCustomSource.AddRange(new string[] {
            "One",
            "Two",
            "One point Five"});
            this.Stopbit.FormattingEnabled = true;
            this.Stopbit.Items.AddRange(new object[] {
            "One",
            "Two",
            "One point Two"});
            this.Stopbit.Location = new System.Drawing.Point(97, 213);
            this.Stopbit.Name = "Stopbit";
            this.Stopbit.Size = new System.Drawing.Size(174, 21);
            this.Stopbit.TabIndex = 6;
            this.Stopbit.SelectedIndexChanged += new System.EventHandler(this.Stopbit_SelectedIndexChanged);
            // 
            // baudrate_lbl
            // 
            this.baudrate_lbl.Cursor = System.Windows.Forms.Cursors.Default;
            this.baudrate_lbl.HideSelection = false;
            this.baudrate_lbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.baudrate_lbl.Location = new System.Drawing.Point(277, 131);
            this.baudrate_lbl.MaxLength = 0;
            this.baudrate_lbl.Name = "baudrate_lbl";
            this.baudrate_lbl.ReadOnly = true;
            this.baudrate_lbl.ShortcutsEnabled = false;
            this.baudrate_lbl.Size = new System.Drawing.Size(80, 20);
            this.baudrate_lbl.TabIndex = 7;
            this.baudrate_lbl.TabStop = false;
            this.baudrate_lbl.Text = "Baudrate";
            this.baudrate_lbl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.baudrate_lbl.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // databits_lbl
            // 
            this.databits_lbl.Cursor = System.Windows.Forms.Cursors.Default;
            this.databits_lbl.Location = new System.Drawing.Point(277, 160);
            this.databits_lbl.Name = "databits_lbl";
            this.databits_lbl.ReadOnly = true;
            this.databits_lbl.Size = new System.Drawing.Size(80, 20);
            this.databits_lbl.TabIndex = 8;
            this.databits_lbl.Text = "Data bits";
            this.databits_lbl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.databits_lbl.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // parity_lbl
            // 
            this.parity_lbl.Cursor = System.Windows.Forms.Cursors.Default;
            this.parity_lbl.Location = new System.Drawing.Point(277, 186);
            this.parity_lbl.Name = "parity_lbl";
            this.parity_lbl.ReadOnly = true;
            this.parity_lbl.Size = new System.Drawing.Size(80, 20);
            this.parity_lbl.TabIndex = 9;
            this.parity_lbl.Text = "Parity";
            this.parity_lbl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // stopbit_lbl
            // 
            this.stopbit_lbl.Cursor = System.Windows.Forms.Cursors.Default;
            this.stopbit_lbl.Location = new System.Drawing.Point(277, 213);
            this.stopbit_lbl.Name = "stopbit_lbl";
            this.stopbit_lbl.ReadOnly = true;
            this.stopbit_lbl.Size = new System.Drawing.Size(80, 20);
            this.stopbit_lbl.TabIndex = 10;
            this.stopbit_lbl.Text = "Stop Bit";
            this.stopbit_lbl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(13, 350);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 17);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "Connected";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // module_addr_lbl
            // 
            this.module_addr_lbl.Cursor = System.Windows.Forms.Cursors.Default;
            this.module_addr_lbl.Location = new System.Drawing.Point(277, 259);
            this.module_addr_lbl.Name = "module_addr_lbl";
            this.module_addr_lbl.ReadOnly = true;
            this.module_addr_lbl.Size = new System.Drawing.Size(80, 20);
            this.module_addr_lbl.TabIndex = 12;
            this.module_addr_lbl.Text = "Адрес модуля";
            this.module_addr_lbl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // status_line
            // 
            this.status_line.Location = new System.Drawing.Point(13, 370);
            this.status_line.Name = "status_line";
            this.status_line.ReadOnly = true;
            this.status_line.Size = new System.Drawing.Size(252, 20);
            this.status_line.TabIndex = 13;
            // 
            // Search
            // 
            this.Search.Location = new System.Drawing.Point(12, 33);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(139, 53);
            this.Search.TabIndex = 14;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = true;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // COM_sw
            // 
            this.COM_sw.AllowDrop = true;
            this.COM_sw.FormattingEnabled = true;
            this.COM_sw.Location = new System.Drawing.Point(179, 33);
            this.COM_sw.Name = "COM_sw";
            this.COM_sw.Size = new System.Drawing.Size(199, 21);
            this.COM_sw.TabIndex = 15;
            this.COM_sw.SelectedIndexChanged += new System.EventHandler(this.COM_sw_SelectedIndexChanged);
            this.COM_sw.Click += new System.EventHandler(this.COM_sw_Click);
            // 
            // baudrate_label
            // 
            this.baudrate_label.AutoSize = true;
            this.baudrate_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.baudrate_label.Location = new System.Drawing.Point(22, 136);
            this.baudrate_label.Name = "baudrate_label";
            this.baudrate_label.Size = new System.Drawing.Size(28, 16);
            this.baudrate_label.TabIndex = 18;
            this.baudrate_label.Text = "Set";
            this.baudrate_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // databits_label
            // 
            this.databits_label.AutoSize = true;
            this.databits_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.databits_label.Location = new System.Drawing.Point(22, 158);
            this.databits_label.Name = "databits_label";
            this.databits_label.Size = new System.Drawing.Size(28, 16);
            this.databits_label.TabIndex = 19;
            this.databits_label.Text = "Set";
            // 
            // parity_label
            // 
            this.parity_label.AutoSize = true;
            this.parity_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.parity_label.Location = new System.Drawing.Point(22, 184);
            this.parity_label.Name = "parity_label";
            this.parity_label.Size = new System.Drawing.Size(28, 16);
            this.parity_label.TabIndex = 20;
            this.parity_label.Text = "Set";
            // 
            // stopbit_label
            // 
            this.stopbit_label.AutoSize = true;
            this.stopbit_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.stopbit_label.Location = new System.Drawing.Point(22, 211);
            this.stopbit_label.Name = "stopbit_label";
            this.stopbit_label.Size = new System.Drawing.Size(28, 16);
            this.stopbit_label.TabIndex = 21;
            this.stopbit_label.Text = "Set";
            // 
            // addr_label
            // 
            this.addr_label.AutoSize = true;
            this.addr_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addr_label.Location = new System.Drawing.Point(22, 257);
            this.addr_label.Name = "addr_label";
            this.addr_label.Size = new System.Drawing.Size(28, 16);
            this.addr_label.TabIndex = 22;
            this.addr_label.Text = "Set";
            // 
            // write_btn
            // 
            this.write_btn.Location = new System.Drawing.Point(167, 285);
            this.write_btn.Name = "write_btn";
            this.write_btn.Size = new System.Drawing.Size(62, 23);
            this.write_btn.TabIndex = 24;
            this.write_btn.Text = "Send";
            this.write_btn.UseVisualStyleBackColor = true;
            this.write_btn.Click += new System.EventHandler(this.write_btn_Click);
            // 
            // Default
            // 
            this.Default.Location = new System.Drawing.Point(73, 285);
            this.Default.Name = "Default";
            this.Default.Size = new System.Drawing.Size(65, 23);
            this.Default.TabIndex = 25;
            this.Default.Text = "Default";
            this.Default.UseVisualStyleBackColor = true;
            this.Default.Click += new System.EventHandler(this.Default_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 403);
            this.Controls.Add(this.Default);
            this.Controls.Add(this.write_btn);
            this.Controls.Add(this.addr_label);
            this.Controls.Add(this.stopbit_label);
            this.Controls.Add(this.parity_label);
            this.Controls.Add(this.databits_label);
            this.Controls.Add(this.baudrate_label);
            this.Controls.Add(this.COM_sw);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.status_line);
            this.Controls.Add(this.module_addr_lbl);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.stopbit_lbl);
            this.Controls.Add(this.parity_lbl);
            this.Controls.Add(this.databits_lbl);
            this.Controls.Add(this.baudrate_lbl);
            this.Controls.Add(this.Stopbit);
            this.Controls.Add(this.Parity);
            this.Controls.Add(this.Databits);
            this.Controls.Add(this.module_addr1);
            this.Controls.Add(this.Baudrate);
            this.Controls.Add(this.save_btn);
            this.Controls.Add(this.connect_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Настройка порта";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button connect_btn;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.ComboBox Baudrate;
        private System.Windows.Forms.RichTextBox module_addr1;
        private System.Windows.Forms.ComboBox Databits;
        private System.Windows.Forms.ComboBox Parity;
        private System.Windows.Forms.ComboBox Stopbit;
        private System.Windows.Forms.TextBox baudrate_lbl;
        private System.Windows.Forms.TextBox databits_lbl;
        private System.Windows.Forms.TextBox parity_lbl;
        private System.Windows.Forms.TextBox stopbit_lbl;
        public System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox module_addr_lbl;
        private System.Windows.Forms.TextBox status_line;
        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.ComboBox COM_sw;
        public System.IO.Ports.SerialPort COM1;
        private System.Windows.Forms.Label baudrate_label;
        private System.Windows.Forms.Label databits_label;
        private System.Windows.Forms.Label parity_label;
        private System.Windows.Forms.Label stopbit_label;
        private System.Windows.Forms.Label addr_label;
        private System.Windows.Forms.Button write_btn;
        private System.Windows.Forms.Button Default;
    }
}

