namespace LGAR
{
    partial class formDebugConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formDebugConsole));
            this.textOutput = new System.Windows.Forms.RichTextBox();
            this.textInput = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textOutput
            // 
            this.textOutput.BackColor = System.Drawing.SystemColors.Window;
            this.textOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOutput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textOutput.Location = new System.Drawing.Point(0, 0);
            this.textOutput.Name = "textOutput";
            this.textOutput.ReadOnly = true;
            this.textOutput.Size = new System.Drawing.Size(564, 371);
            this.textOutput.TabIndex = 1;
            this.textOutput.Text = "";
            this.textOutput.Click += new System.EventHandler(this.textOutput_Click);
            // 
            // textInput
            // 
            this.textInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textInput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textInput.Location = new System.Drawing.Point(23, 4);
            this.textInput.MaxLength = 256;
            this.textInput.Name = "textInput";
            this.textInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textInput.Size = new System.Drawing.Size(516, 16);
            this.textInput.TabIndex = 0;
            this.textInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textInput_KeyDown);
            this.textInput.Leave += new System.EventHandler(this.textInput_Leave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textInput);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 371);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 30);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = ">";
            // 
            // formDebugConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 401);
            this.Controls.Add(this.textOutput);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "formDebugConsole";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Debug Console";
            this.Activated += new System.EventHandler(this.debugConsole_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.debugConsole_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textOutput;
        private System.Windows.Forms.TextBox textInput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}