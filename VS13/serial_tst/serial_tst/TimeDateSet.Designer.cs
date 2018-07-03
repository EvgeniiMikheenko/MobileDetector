namespace serial_tst
{
    partial class TimeDateSet
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
            this.GetSysTime = new System.Windows.Forms.Button();
            this.setDate = new System.Windows.Forms.MaskedTextBox();
            this.setTime = new System.Windows.Forms.MaskedTextBox();
            this.Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetSysTime
            // 
            this.GetSysTime.Location = new System.Drawing.Point(12, 12);
            this.GetSysTime.Name = "GetSysTime";
            this.GetSysTime.Size = new System.Drawing.Size(75, 23);
            this.GetSysTime.TabIndex = 0;
            this.GetSysTime.Text = "Текущие";
            this.GetSysTime.UseVisualStyleBackColor = true;
            this.GetSysTime.Click += new System.EventHandler(this.GetSysTime_Click);
            // 
            // setDate
            // 
            this.setDate.Location = new System.Drawing.Point(13, 60);
            this.setDate.Mask = "00/00/0000";
            this.setDate.Name = "setDate";
            this.setDate.Size = new System.Drawing.Size(100, 20);
            this.setDate.TabIndex = 1;
            this.setDate.ValidatingType = typeof(System.DateTime);
            this.setDate.TextChanged += new System.EventHandler(this.setDate_TextChanged);
            // 
            // setTime
            // 
            this.setTime.Location = new System.Drawing.Point(13, 95);
            this.setTime.Mask = "00:00:00";
            this.setTime.Name = "setTime";
            this.setTime.Size = new System.Drawing.Size(100, 20);
            this.setTime.TabIndex = 2;
            this.setTime.TextChanged += new System.EventHandler(this.setTime_TextChanged);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(13, 137);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 3;
            this.Save.Text = "Запись";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // TimeDateSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.setTime);
            this.Controls.Add(this.setDate);
            this.Controls.Add(this.GetSysTime);
            this.Name = "TimeDateSet";
            this.Text = "TimeDateSet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetSysTime;
        private System.Windows.Forms.MaskedTextBox setDate;
        private System.Windows.Forms.MaskedTextBox setTime;
        private System.Windows.Forms.Button Save;
    }
}