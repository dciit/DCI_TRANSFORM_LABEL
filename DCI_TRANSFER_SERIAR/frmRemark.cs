using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCI_TRANSFER_SERIAR
{
    public partial class frmRemark : Form
    {
        private string _serial;
        private frmTransferLabel fTransfer;

        public frmRemark(string serial)
        {
            InitializeComponent();
            _serial = serial;
        }

        public string getRemark()
        {
            return tbRemark.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tbRemark.Text = "";
            this.Hide();
        }

        private void frmRemark_Load(object sender, EventArgs e)
        {
            txtSerial.Text = _serial;
        }

        private void btnConfSerial_Click(object sender, EventArgs e)
        {
            if (tbRemark.Text.Trim().Length > 0)
            {
                this.Hide();
            }
            else
            {
                MessageBox.Show("กรุณากรอกหมายเหตุ ...");
            }
        }
    }
}
