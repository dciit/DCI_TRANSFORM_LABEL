using MultiSkillCer;
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
    public partial class frmSetting : Form
    {

        public IniFile initFile = new IniFile("Config.ini");
        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            bool checkedInsertFinalDataDIT = initFile.GetString("MODE","FINALDATADIT", "FLASE") == "TRUE" ? true : false;
            bool checkedInsertFnDataCenter = initFile.GetString("MODE", "FNDATACENTER", "FLASE") == "TRUE" ? true : false;
            bool checkedInsertFnCopyLabel = initFile.GetString("MODE", "FNCOPYLABELLOG", "FLASE") == "TRUE" ? true : false;
            if (checkedInsertFinalDataDIT)
            {
                cbInsertFinalDataDit.Checked = checkedInsertFinalDataDIT;
            }
            if (checkedInsertFnDataCenter)
            {
                cbInsertFnDataCenter.Checked = checkedInsertFnDataCenter;
            }
            if (checkedInsertFnCopyLabel)
            {
                cbInsertFnCopyLabel.Checked = checkedInsertFnCopyLabel;
            }
        }

        private void cbInsertFinalDataDit_CheckedChanged(object sender, EventArgs e)
        {
            cbChangeChecked("FINALDATADIT",cbInsertFinalDataDit.Checked);
        }

        private void cbInsertFnDataCenter_CheckedChanged(object sender, EventArgs e)
        {
            cbChangeChecked("FNDATACENTER", cbInsertFnDataCenter.Checked);
        }

        private void cbInsertFnCopyLabel_CheckedChanged(object sender, EventArgs e)
        {
            cbChangeChecked("FNCOPYLABELLOG", cbInsertFnCopyLabel.Checked);
        }

        private void cbChangeChecked(string key,bool check)
        {
            initFile.WriteValue("MODE", key, check ? "TRUE" : "FALSE");
        }

    }
}
