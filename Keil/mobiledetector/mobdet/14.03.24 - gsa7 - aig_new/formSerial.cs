using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace LGAR
{
    public partial class formSerial : Form
    {
        public formSerial(SerialPort ser)
        {
            InitializeComponent();

            prop.SelectedObject = ser;
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {

        }
    }
}
