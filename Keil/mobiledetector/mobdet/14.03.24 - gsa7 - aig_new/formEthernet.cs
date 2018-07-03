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
    public partial class formEthernet : Form
    {
        public formEthernet()
        {
            InitializeComponent();

            txtAddress.Text = "tcp://127.0.0.1:502/";
            txtAddress.Select("tcp://".Length, "127.0.0.1".Length);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Uri ur;
            if (Uri.TryCreate(txtAddress.Text, UriKind.Absolute, out ur))
            {
                uri = ur;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        Uri uri;

        public Uri NetworkLocation
        {
            get { return uri; }
            set { txtAddress.Text = (uri = value).ToString(); }
        }
    }
}
