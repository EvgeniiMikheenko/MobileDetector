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
    public partial class formBulbulator : Form
    {
        public formBulbulator()
        {
            InitializeComponent();
        }

        double _value_ = double.NaN;

        public double VALUE
        {
            get
            {
                return _value_;
            }

            set
            {
                _value_ = value;
                textNumber.Text = _value_.ToString();
            }
        }

        public double MinValue = double.NaN;
        public double MaxValue = double.NaN;

        private void cmdOK_Click(object sender, EventArgs e)
        {
            // check
            double R;
            if(!double.TryParse(textNumber.Text, out R))
            {
                MessageBox.Show("Неверное число.");
                return;
            }
            if(R < MinValue)
            {
                MessageBox.Show("Значение меньше допустимого.\r\n"+MinValue);
                return;
            }
            if(R > MaxValue)
            {
                MessageBox.Show("Значение больше допустимого.\r\n"+MaxValue);
                return;
            }
            // done
            _value_ = R;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

    }
}
