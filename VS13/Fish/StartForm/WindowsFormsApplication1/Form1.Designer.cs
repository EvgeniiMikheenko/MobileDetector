namespace WindowsFormsApplication1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ConnectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.отключитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.COM = new System.IO.Ports.SerialPort(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(827, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ConnectMenu
            // 
            this.ConnectMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.отключитьToolStripMenuItem,
            this.настройкаToolStripMenuItem});
            this.ConnectMenu.Name = "ConnectMenu";
            this.ConnectMenu.Size = new System.Drawing.Size(97, 20);
            this.ConnectMenu.Text = "Подключение";
            this.ConnectMenu.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ConnectMenu_DropDownItemClicked);
            this.ConnectMenu.Click += new System.EventHandler(this.ConnectMenu_Click);
            // 
            // отключитьToolStripMenuItem
            // 
            this.отключитьToolStripMenuItem.Name = "отключитьToolStripMenuItem";
            this.отключитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.отключитьToolStripMenuItem.Text = "Отключить";
            // 
            // настройкаToolStripMenuItem
            // 
            this.настройкаToolStripMenuItem.Name = "настройкаToolStripMenuItem";
            this.настройкаToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.настройкаToolStripMenuItem.Text = "Настройка";
            // 
            // COM
            // 
            this.COM.BaudRate = 115200;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 471);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ConnectMenu;
        private System.Windows.Forms.ToolStripMenuItem отключитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкаToolStripMenuItem;
        private System.IO.Ports.SerialPort COM;
    }
}

