using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace common.plugins
{
    public class StringFileLogger : IDisposable
    {
        public StringFileLogger(string fName)
        {
            if (string.IsNullOrEmpty(fName))
                return;
            //
            mFileName = fName;
            try
            {
                mFstream = new FileStream(mFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
            }
            catch (Exception ex)
            {
                mFstream = null;
                MessageBox.Show("Не удалось создать/открыть файл... \r\n" + ex.Message, "Ошибка !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //
            if (mFstream != null)
            {
                try
                {
                    mWriter = new StreamWriter(mFstream);
                    //
                    Log("");
                    Log("");
                    WriteHeader();
                }
                catch (Exception ex2)
                {
                    mWriter = null;
                    try
                    {
                        mFstream.Dispose();
                        mFstream.Close();
                    }
                    catch { }
                    mFstream = null;
                    MessageBox.Show(ex2.Message, "Ошибка !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //
            GC.Collect();
        }

        ~StringFileLogger()
        {
            CloseLogger();
        }
        //-------------------------------------------------------------------------
        #region Data
        //
        const string Separator = "================================================================================";
        const char Splitter = '|';
        const string EmptyLine = "|                                                                              |";
        //
        const int LineWidth = 80;
        //
        readonly string mFileName;
        readonly FileStream mFstream;
        readonly StreamWriter mWriter;
        //
        #endregion //Data
        //-------------------------------------------------------------------------
        #region Public
        //
        public void Dispose()
        {
            CloseLogger();
        }

        public void Log(params string[] str)
        {
            if ((mWriter == null) || (str == null) || (str.Length <= 0))
                return;
            //
            try
            {
                for (int i = 0; i < str.Length; i++)
                {
                    mWriter.WriteLine(str[i]);
                    mWriter.Flush();
                }
            }
            catch { }
        }
        //
        #endregion // Public
        //-------------------------------------------------------------------------
        #region Private
        //
        void CloseLogger()
        {
            if (mWriter != null)
            {
                try
                {
                    Log(Separator);
                    mWriter.Close();
                }
                catch { }
            }
        }

        string CreateHeader(string str)
        {
            if (string.IsNullOrEmpty(str))
                return EmptyLine;
            //
            int addCh = LineWidth - str.Length;
            if (addCh <= 0)
                return str;
            //
            int leftCount = (addCh / 2) - 1;
            string leftStr = "" + Splitter;
            for (int i = 0; i < leftCount; i++)
            {
                leftStr += " ";
            }
            //
            int rightCount = LineWidth - (leftStr.Length + str.Length) - 1;
            string rightStr = "";
            for (int i = 0; i < rightCount; i++)
            {
                rightStr += "";
            }
            rightStr += Splitter;
            //
            return leftStr + str + rightStr;
        }

        public static string DateToString()
        {
            DateTime dt = DateTime.Now;
            string dayStr = ((dt.Day >= 10) ? dt.Day.ToString() : ("0" + dt.Day.ToString()));
            string monthStr = ((dt.Month >= 10) ? dt.Month.ToString() : ("0" + dt.Month.ToString()));
            //
            string hoursStr = ((dt.Hour >= 10) ? dt.Hour.ToString() : ("0" + dt.Hour.ToString()));
            string minStr = ((dt.Minute >= 10) ? dt.Minute.ToString() : ("0" + dt.Minute.ToString()));
            string secStr = ((dt.Second >= 10) ? dt.Second.ToString() : ("0" + dt.Second.ToString()));
            //
            string date = dayStr + "_" + monthStr + "_" + dt.Year.ToString();
            string time = hoursStr + "_" + minStr + "_" + secStr;
            return date + "__" + time;
        }

        void WriteHeader()
        {
            Log(Separator);
            string hdr = "Запуск новой сессии лога: " + DateToString();
            Log(CreateHeader(hdr));
            Log(Separator);
        }
        //
        #endregion // Private
        //-------------------------------------------------------------------------
        #region Events
        //

        //
        #endregion // Events
        //-------------------------------------------------------------------------
    }
}
