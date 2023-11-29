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
    public partial class frmLogin : Form
    {
        Service service = new Service();
        MEmployee MLogin = new MEmployee();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void edEmId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && edEmId.Text != "")
            {
                MLogin = service.Login(edEmId.Text);
                if (MLogin != null)
                {

                    if(MLogin.COde != "")
                    {
                        frmTransferLabel MainPage = new frmTransferLabel(MLogin.COde, MLogin.TName, MLogin.TSurn);
                        MainPage.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("กรอกรหัสพนักงานอีกครั้ง !");
                    }

                }
                else
                {
                    MessageBox.Show("กรอกรหัสพนักงานอีกครั้ง !");
                }
            }
        }
    }
}
