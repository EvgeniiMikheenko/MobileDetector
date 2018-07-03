namespace serialPortLib
{
	partial class LogForm
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
            this.lbxLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxLog
            // 
            this.lbxLog.BackColor = System.Drawing.SystemColors.WindowText;
            this.lbxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxLog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbxLog.ForeColor = System.Drawing.Color.Lime;
            this.lbxLog.FormattingEnabled = true;
            this.lbxLog.ItemHeight = 16;
            this.lbxLog.Location = new System.Drawing.Point(0, 0);
            this.lbxLog.Name = "lbxLog";
            this.lbxLog.Size = new System.Drawing.Size(603, 545);
            this.lbxLog.TabIndex = 0;
            this.lbxLog.MouseEnter += new System.EventHandler(this.lbxLog_MouseEnter);
            this.lbxLog.MouseLeave += new System.EventHandler(this.lbxLog_MouseLeave);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(125, 22);
            this.toolStripMenuItem1.Text = "Свернуть";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 545);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.lbxLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "LogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LogForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbxLog;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
	}
}