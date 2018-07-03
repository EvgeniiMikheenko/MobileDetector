namespace LGAR
{
    partial class formSerial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formSerial));
            this.prop = new System.Windows.Forms.PropertyGrid();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // prop
            // 
            this.prop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prop.Location = new System.Drawing.Point(12, 12);
            this.prop.Name = "prop";
            this.prop.Size = new System.Drawing.Size(315, 361);
            this.prop.TabIndex = 0;
            // 
            // cmdConnect
            // 
            this.cmdConnect.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdConnect.Location = new System.Drawing.Point(69, 383);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(97, 23);
            this.cmdConnect.TabIndex = 1;
            this.cmdConnect.Text = "&Подключить";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(172, 383);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(97, 23);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Text = "&Закрыть";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // formSerial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(339, 418);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdConnect);
            this.Controls.Add(this.prop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 400);
            this.Name = "formSerial";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Подключение по последовательному порту";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid prop;
        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.Button cmdClose;
    }
}