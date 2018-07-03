using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace serialPortLib
{
	public partial class LogForm : Form
	{
		public LogForm()
		{
			InitializeComponent();
			ClearLog();
            //
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		int m_rowNum = 1;
		object m_syncObj = new object();

		bool m_autoScrolEn = true;
		//
		#endregion //Data
		//-------------------------------------------------------------------------
		#region Public
		//
		public void ClearLog()
		{
			lbxLog.Items.Clear();
			m_rowNum = 1;
		}

		public void Log(params string[] src)
		{
			if ((src == null) || (src.Length <= 0))
				return;
			//
			try
			{
				lbxLog.BeginInvoke(new Action<string[]>((string[] param) =>
					{
						for (int i = 0; i < param.Length; i++)
						{
							string str = ConvertToString(m_rowNum, 5) + ": \t" + param[i];
							lbxLog.Items.Add(str);
							m_rowNum++;
							//
							if (m_autoScrolEn)
								lbxLog.SelectedIndex = lbxLog.Items.Count - 1;
						}
						//
					}), new object[] { src });
			}
			catch { }
		}

		public void CloseWindow()
		{
			Close();
		}
		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//
		string ConvertToString(int src, int minLen)
		{
			string result = System.Convert.ToString(src);
			while (result.Length < minLen)
			{
				result = "0" + result;
			}
			//
			return (result).ToUpper();
		}
		//
		#endregion // Private
		//-------------------------------------------------------------------------
		#region Events
		//
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}

		private void lbxLog_MouseEnter(object sender, EventArgs e)
		{
			m_autoScrolEn = false;
		}

		private void lbxLog_MouseLeave(object sender, EventArgs e)
		{
			m_autoScrolEn = true;
		}

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }
		//
		#endregion // Events
		//-------------------------------------------------------------------------
	}
}
