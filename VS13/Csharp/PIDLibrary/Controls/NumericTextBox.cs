using System.Windows.Controls;
using System.Windows.Input;


namespace PIDLibrary.Controls
{
  class NumericTextBox: TextBox
  {
    public NumericTextBox()
    {
      Value = 1.0f;
      TextAlignment = System.Windows.TextAlignment.Center;
      this.PreviewKeyDown += new KeyEventHandler(NumericTextBox_PreviewKeyDown);
      this.PreviewTextInput += new TextCompositionEventHandler(NumericTextBox_PreviewTextInput);
    }

    float value;
    public float Value
    {
      get { return this.value; }
      set 
      { 
        this.value = value;
        Text = value.ToString();
      }
    }

		bool isInteger = false;
		public bool IsInteger
		{
			get { return isInteger; }
			set { isInteger = value; }
		}

    void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if ((e.Key == Key.Back) || (e.Key == Key.Return) || (e.Key == Key.Enter) || (e.Key == Key.Left) || (e.Key == Key.Right) || (e.Key == Key.Delete) || (e.Key == Key.Tab))
        return;
      //
      char c = KeyToChar(e.Key);
			if (isInteger)
			{
				if (!char.IsDigit(c))
				{
					e.Handled = true;
					return;
				}
				return;
			}
			//
      if (!char.IsDigit(c))
      {
        if ((c != '.') && (c != ','))
        {
          e.Handled = true;
          return;
        }
        else if ((FindChar(Text, ',')) || (FindChar(Text, '.')))
        {
          e.Handled = true;
          return;
        }
      }
      //
    }

    void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      string txt = GetString(Text, SelectionStart, SelectionLength, e.Text);
      if ((txt[txt.Length - 1] == '.') || (txt[txt.Length - 1] == ','))
        txt += "0";
      float f = 0;
      if (!float.TryParse(ReplaceDot(txt), out f))
      {
        e.Handled = true;
        return;
      }
      value = f;
    }

    bool FindChar(string source, char c)
    {
      bool result = false;
      foreach (char ch in source)
      {
        if (ch == c)
        {
          result = true;
          break;
        }
      }
      return result;
    }

    string ReplaceDot(string source)
    {
      if (string.IsNullOrEmpty(source))
        return "";
      //
      string result = "";
      for (int i = 0; i < source.Length; i++)
      {
        if (source[i] == '.')
          result += ',';
        else
          result += source[i];
      }
      return result;
    }

    char KeyToChar(Key key)
    {
      char result = ' ';
      switch (key)
      {
        case Key.A:
          result = 'A';
          break;
        case Key.B:
          result = 'B';
          break;
        case Key.C:
          result = 'C';
          break;
        case Key.D:
          result = 'D';
          break;
        case Key.D0:
          result = '0';
          break;
        case Key.D1:
          result = '1';
          break;
        case Key.D2:
          result = '2';
          break;
        case Key.D3:
          result = '3';
          break;
        case Key.D4:
          result = '4';
          break;
        case Key.D5:
          result = '5';
          break;
        case Key.D6:
          result = '6';
          break;
        case Key.D7:
          result = '7';
          break;
        case Key.D8:
          result = '8';
          break;
        case Key.D9:
          result = '9';
          break;
        case Key.E:
          result = 'E';
          break;
        case Key.F:
          result = 'F';
          break;
        case Key.G:
          result = 'G';
          break;
        case Key.H:
          result = 'H';
          break;
        case Key.I:
          result = 'I';
          break;
        case Key.J:
          result = 'J';
          break;
        case Key.K:
          result = 'K';
          break;
        case Key.L:
          result = 'L';
          break;
        case Key.M:
          result = 'M';
          break;
        case Key.N:
          result = 'N';
          break;
        case Key.O:
          result = 'O';
          break;
        case Key.P:
          result = 'P';
          break;
        case Key.Q:
          result = 'Q';
          break;
        case Key.R:
          result = 'R';
          break;
        case Key.S:
          result = 'S';
          break;
        case Key.T:
          result = 'T';
          break;
        case Key.U:
          result = 'U';
          break;
        case Key.V:
          result = 'V';
          break;
        case Key.W:
          result = 'W';
          break;
        case Key.X:
          result = 'X';
          break;
        case Key.Y:
          result = 'Y';
          break;
        case Key.Z:
          result = 'Z';
          break;
        case Key.NumPad0:
          result = '0';
          break;
        case Key.NumPad1:
          result = '1';
          break;
        case Key.NumPad2:
          result = '2';
          break;
        case Key.NumPad3:
          result = '3';
          break;
        case Key.NumPad4:
          result = '4';
          break;
        case Key.NumPad5:
          result = '5';
          break;
        case Key.NumPad6:
          result = '6';
          break;
        case Key.NumPad7:
          result = '7';
          break;
        case Key.NumPad8:
          result = '8';
          break;
        case Key.NumPad9:
          result = '9';
          break;
        case Key.OemPeriod:
          result = '.';
          break;
        case Key.OemComma:
          result = ',';
          break;
        default:
          result = ' ';
          break;
      }
      //
      return result;
    }

    string GetString(string source, int selectionStart, int selectionLengtch, string insertString)
    {
      string result = "";
      if ((selectionLengtch == 0))
      { // текст не выделен
        if (selectionStart == 0)
        { // курсор находится в начале слова
          result = insertString + source;
        }
        else if (selectionStart < source.Length)
        { // курсор в середине слова
          result = source.Substring(0, selectionStart) + insertString + source.Substring(selectionStart, source.Length - selectionStart);
        }
        else// if (selectionStart = source.Length)
        { // курсор в конце слова
          result = source + insertString;
        }
      }
      else // в исходном слове выделена какая-то часть
      {
        if (selectionLengtch != source.Length)
        { // Выделено не все слово
          if (selectionStart == 0)
          { // курсор находится в начале слова
            result = insertString + source.Substring(selectionLengtch, source.Length - selectionLengtch);
          }
          else// if (selectionStart < source.Length)
          { // курсор в середине слова
            result = source.Substring(0, selectionStart) + insertString + source.Substring(selectionStart + selectionLengtch, source.Length - (selectionStart + selectionLengtch));
          }
          //else// if (selectionStart = source.Length)
          //{ // курсор в конце слова

          //}
        }
        else
        { // Выделено все слово, поэтому заменяем исходное слово вставленным
          result = insertString;
        }
      }
      //
      return result;
    }
  }
}
