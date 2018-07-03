#define LUA

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#if LUA
using LuaInterface;
using System.Text.RegularExpressions;
#else

#endif

namespace LGAR
{
    public partial class formDebugConsole : Form
    {

#if LUA
        Lua Interpreter = null;
        Regex reLuaReturner = new Regex
            (@"^(if|do|function|while|repeat|for|return|local|;)\b|^[^=]*=([^=]+.*|$)");

        public formDebugConsole()
        {
            InitializeComponent();
            textOutput.Text = "Version " +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void SetInterpreter(Lua li)
        {
            Interpreter = li;
        }

#else
        object Interpreter;

        public formDebugConsole(object engine)
        {
            InitializeComponent();
            Interpreter = engine;
            textOutput.Text = "Version " +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void SetInterpreter(object li)
        {
            Interpreter = li;
        }

#endif

        List<string> History = new List<string>();
        int hiCounter = 0;
        string hiCurrent = "";

        private void textInput_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            switch (e.KeyCode)
            {
                case Keys.Return:
                    string i = textInput.Text;
                    if (string.IsNullOrWhiteSpace(i)) return;
                    i = i.Trim();
                    Print("> " + i.Trim(), Color.Blue);
                    //
                    ResetConsole();
                    History.Remove(i);
                    History.Insert(0, i);
                    textInput.Refresh();
                    //
                    if (Interpreter == null)
                    {
                        Print(" Interpreter disabled.", Color.Magenta);
                        return;
                    }
                    exec(i);
                    break;

                case Keys.Up:
                    if (hiCounter < History.Count)
                    {
                        if (hiCounter == 0) hiCurrent = textInput.Text;
                        ++hiCounter;
                        textInput.Text = History[hiCounter - 1];
                        textInput.Select(textInput.TextLength, 0);
                    }
                    break;

                case Keys.Down:
                    if (hiCounter > 0)
                    {
                        --hiCounter;
                        if (hiCounter == 0 || hiCounter >= History.Count)
                            textInput.Text = hiCurrent;
                        else textInput.Text = History[hiCounter - 1];
                        textInput.Select(textInput.TextLength, 0);
                    }
                    break;

                case Keys.Escape:
                    ResetConsole();
                    break;

                case Keys.D:
                    if (!e.Control) goto default;
                    ResetConsole();
                    Hide();
                    break;

                default:
                    e.SuppressKeyPress = false;
                    break;
            }
        }

        public void ResetConsole()
        {
            hiCounter = 0;
            hiCurrent = "";
            textInput.Text = "";
        }

        void exec(string i)
        {
            try
            {
#if LUA
                if (i == "?")
                {
                    Trace(string.Join(" ", new List<string>(Interpreter.Globals).ToArray()));
                    return;
                }
                if (!reLuaReturner.IsMatch(i)) i = "=" + i;
                var obj = Eval(i.StartsWith("=") ? "return " + i.Substring(1) : i);
                if (obj is LuaFunction)
                {
                    Print(" function ", Color.DarkCyan);
                }
                else if (obj is LuaTable)
                {
                    var tab = obj as LuaTable;
                    int cnt = tab.Keys.Count;
                    Print(" table[" + cnt + "]", Color.DarkCyan);
                    if (cnt < 16)
                    {
                        var sb = new StringBuilder(" {\r\n");
                        foreach(object kv in tab.Keys)
                        {
                            sb.AppendFormat("\t[{0}] = {1}\r\n", kv, tab[kv]);
                        }
                        sb.Append(" }");
                        Print(sb.ToString(), Color.DarkCyan);
                    }
                }
                else if(obj != null) Print(" " + obj);
            }
            catch (LuaScriptException ex)
            {
                Print(ex.Message, Color.OrangeRed);
            }
#else
                Print(" Interpreter disabled", Color.OrangeRed);
            }
#endif
            catch (Exception ex)
            {
                Print(ex.ToString(), Color.Red);
            }
        }

        private void textOutput_Click(object sender, EventArgs e)
        {
            textInput.Focus();
        }

        private void textInput_Leave(object sender, EventArgs e)
        {
            //textInput.Focus(); // force
        }

        private void debugConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing) e.Cancel = true;
            Hide();
        }

        private void debugConsole_Activated(object sender, EventArgs e)
        {
            textInput.Focus();
        }


        public void Print(string Message, Color Color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, Color>(Print), Message, Color);
                return;
            }

            var o = textOutput;
            o.Select(o.TextLength, 0);
            o.SelectionColor = Color;
            o.SelectionFont = o.Font;
            o.SelectedText = "\r\n" + Message;
            o.Select(o.TextLength, 0);
            o.ScrollToCaret();
        }

        public void Print(string Message)
        {
            Print(Message, textOutput.ForeColor);
        }

        public void Trace(string Message)
        {
            Print(" " + Message, Color.FromKnownColor(KnownColor.GrayText));
        }

        public void Error(Exception ex)
        {
            if(ex == null) return;
            Print(" " + 
#if DEBUG
                ex.ToString()
#else
                ex.Message
#endif
            , Color.Red);
        }

        public object Eval(string Command)
        {
            if(InvokeRequired)
                return Invoke(new Func<string, object>(Eval), Command);
#if LUA
            Object[] res = Interpreter.DoString(Command);
#else
            Object[] res = null;
#endif
            if (res != null && res.Length > 0) return res[0];
            return null;
        }

        public void LuaPrint(params object[] s)
        {
            if(s == null || s.Length == 0) return;
            string[] ss = new string[s.Length];
            for (int i = 0, l = s.Length; i < l; ++i)
                ss[i] = (s[i] ?? "nil").ToString();
            Print(string.Join("\t", ss), Color.Green);
        }
    }
}
