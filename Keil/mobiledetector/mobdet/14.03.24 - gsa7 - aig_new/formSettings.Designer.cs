namespace LGAR
{
    partial class formSettings
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
            this.yoTab = new System.Windows.Forms.TabControl();
            this.yoPage = new System.Windows.Forms.TabPage();
            this.yoGo = new System.Windows.Forms.Button();
            this.yoCancel = new System.Windows.Forms.Button();
            this.chkJoke = new System.Windows.Forms.CheckBox();
            this.yoTab.SuspendLayout();
            this.yoPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // yoTab
            // 
            this.yoTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yoTab.Controls.Add(this.yoPage);
            this.yoTab.Location = new System.Drawing.Point(12, 12);
            this.yoTab.Name = "yoTab";
            this.yoTab.SelectedIndex = 0;
            this.yoTab.Size = new System.Drawing.Size(425, 334);
            this.yoTab.TabIndex = 0;
            // 
            // yoPage
            // 
            this.yoPage.Controls.Add(this.chkJoke);
            this.yoPage.Location = new System.Drawing.Point(4, 22);
            this.yoPage.Name = "yoPage";
            this.yoPage.Padding = new System.Windows.Forms.Padding(3);
            this.yoPage.Size = new System.Drawing.Size(417, 308);
            this.yoPage.TabIndex = 1;
            this.yoPage.Text = "Общие";
            this.yoPage.UseVisualStyleBackColor = true;
            // 
            // yoGo
            // 
            this.yoGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.yoGo.Location = new System.Drawing.Point(195, 352);
            this.yoGo.Name = "yoGo";
            this.yoGo.Size = new System.Drawing.Size(118, 23);
            this.yoGo.TabIndex = 1;
            this.yoGo.Text = "Ok";
            this.yoGo.UseVisualStyleBackColor = true;
            this.yoGo.Click += new System.EventHandler(this.yoGo_Click);
            // 
            // yoCancel
            // 
            this.yoCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.yoCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.yoCancel.Location = new System.Drawing.Point(319, 352);
            this.yoCancel.Name = "yoCancel";
            this.yoCancel.Size = new System.Drawing.Size(118, 23);
            this.yoCancel.TabIndex = 2;
            this.yoCancel.Text = "Отмена";
            this.yoCancel.UseVisualStyleBackColor = true;
            // 
            // chkJoke
            // 
            this.chkJoke.AutoSize = true;
            this.chkJoke.Checked = true;
            this.chkJoke.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkJoke.Enabled = false;
            this.chkJoke.Location = new System.Drawing.Point(65, 140);
            this.chkJoke.Name = "chkJoke";
            this.chkJoke.Size = new System.Drawing.Size(166, 17);
            this.chkJoke.TabIndex = 0;
            this.chkJoke.Text = "Автоматические настройки";
            this.chkJoke.UseVisualStyleBackColor = true;
            // 
            // formSettings
            // 
            this.AcceptButton = this.yoGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.yoCancel;
            this.ClientSize = new System.Drawing.Size(449, 387);
            this.Controls.Add(this.yoCancel);
            this.Controls.Add(this.yoGo);
            this.Controls.Add(this.yoTab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.yoTab.ResumeLayout(false);
            this.yoPage.ResumeLayout(false);
            this.yoPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl yoTab;
        private System.Windows.Forms.TabPage yoPage;
        private System.Windows.Forms.Button yoGo;
        private System.Windows.Forms.Button yoCancel;
        private System.Windows.Forms.CheckBox chkJoke;
    }
}