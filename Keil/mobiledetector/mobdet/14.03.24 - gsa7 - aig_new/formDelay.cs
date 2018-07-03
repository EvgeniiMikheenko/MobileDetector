using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LGAR
{
    public partial class formDelay : Form
    {
        public formDelay(int delay)
        {
            InitializeComponent();

            numDelay.Value = delay;
        }

        public int Delay
        {
            get { return (int)numDelay.Value; }
            set { numDelay.Value = value; }
        }
    }
}
