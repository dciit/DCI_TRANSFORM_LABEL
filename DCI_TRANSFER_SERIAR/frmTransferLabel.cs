using DCI_TRANSFER_SERIAR.Models;
using MultiSkillCer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCI_TRANSFER_SERIAR
{
    public partial class frmTransferLabel : Form
    {

        //1.04 เช็ค Table Service >  Method:listTbFn
        string _KEY = "XXX";
        string _VER = "1.00";
        string _EMCODE = "";
        Service service = new Service();
        Timer timer = new Timer();
        public IniFile initFile = new IniFile("Config.ini");
        public string _remark;
        public frmTransferLabel(String emId, String TName, String TSurn)
        {
            InitializeComponent();
            LabelNameSystem.Text = LabelNameSystem.Text + " ( " + emId + " : " + TName + " " + TSurn + ")";
            this.Text = this.Text + " (" + _KEY + ")" + " (" + _VER + ")";
            _EMCODE = emId;
        }

        public frmTransferLabel()
        {

        }

        private void DCI_TRANSFER_LABEL_Load(object sender, EventArgs e)
        {
            dpkDate.Value = DateTime.Now;
            tbKeySerialOld.Focus();
            this.ActiveControl = tbKeySerialOld;
            setHistory();
            btnSearch_Click(sender, e);
            initSettingInsert();
            //*** Sound Ready ***
            PlayWav(@"Audio/ready.wav");
        }
        public void initSettingInsert()
        {
            bool checkedInsertFinalDataDIT = initFile.GetString("MODE", "FINALDATADIT", "FLASE") == "TRUE" ? true : false;
            bool checkedInsertFnDataCenter = initFile.GetString("MODE", "FNDATACENTER", "FLASE") == "TRUE" ? true : false;
            bool checkedInsertFnCopyLabel = initFile.GetString("MODE", "FNCOPYLABELLOG", "FLASE") == "TRUE" ? true : false;

            initSettingDisplay(insertFinalDataDITToolStripMenuItem, checkedInsertFinalDataDIT);
            initSettingDisplay(fNDataCenterToolStripMenuItem, checkedInsertFnDataCenter);
            initSettingDisplay(fNCopyLAbelToolStripMenuItem, checkedInsertFnCopyLabel);

        }

        private void initSettingDisplay(ToolStripMenuItem menu, bool _checked)
        {
            if (_checked)
            {
                menu.BackColor = Color.Green;
                menu.ForeColor = Color.White;
            }
            else
            {
                menu.BackColor = Color.Red;
                menu.ForeColor = Color.White;
            }
        }

        private void txtOldSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbKeySerialOld.Text.Trim().Length > 0)
                {
                    if (tbKeySerialOld.Text.Trim() != tbSerialNew.Text.Trim())
                    {
                        MCheckBit checkBit = service.validateBit(tbKeySerialOld.Text.Trim());
                        txtBitOld.Text = checkBit.bitRaw;
                        if (checkBit.bitCheck == false)
                        {
                            callAlert(7000, "Serial Check Bit Not Incorrect !", Color.Red, Color.White);
                            PlayWav(@"Audio/no_pass.wav");
                            tbSerialOld.Text = "";
                            tbLineOld.Text = "";
                            tbModelCodeOld.Text = "";
                            tbModelNameOld.Text = "";
                            btnTransfer.Enabled = false;
                            btnTransfer.BackColor = SystemColors.ControlLight;
                            btnTransfer.ForeColor = Color.Black;
                            return;
                        }
                        else
                        {
                            initRow(tbKeySerialOld, e, "Old");
                        }
                    }
                    else
                    {
                        tbKeySerialOld.Text = "";
                        tbSerialOld.Text = "";
                        tbModelCodeOld.Text = "";
                        tbModelNameOld.Text = "";
                        tbLineOld.Text = "";
                        PlayWav(@"Audio/no_pass.wav");
                    }
                }
                setViewBtnTransfer();
            }
        }

        private void txtNewSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbKeySerialNew.Text.Trim().Length > 0)
                {
                    if (tbKeySerialNew.Text.Trim() != tbSerialOld.Text.Trim())
                    {
                        MCheckBit checkBit = service.validateBit(tbKeySerialNew.Text.Trim());
                        txtBitNew.Text = checkBit.bitRaw;
                        if (checkBit.bitCheck == false)
                        {
                            callAlert(7000, "Serial Check Bit Not Incorrect !", Color.Red, Color.White);
                            PlayWav(@"Audio/no_pass.wav");
                            tbSerialNew.Text = "";
                            tbLineNew.Text = "";
                            tbModelCodeNew.Text = "";
                            tbModelNameNew.Text = "";
                            btnTransfer.Enabled = false;
                            btnTransfer.BackColor = SystemColors.ControlLight;
                            btnTransfer.ForeColor = Color.Black;
                            return;
                        }
                        else
                        {
                            initRow(tbKeySerialNew, e, "New");
                        }
                    }
                    else
                    {
                        tbKeySerialNew.Text = "";
                        tbSerialNew.Text = "";
                        tbModelCodeNew.Text = "";
                        tbModelNameNew.Text = "";
                        tbLineNew.Text = "";
                        PlayWav(@"Audio/no_pass.wav");

                    }
                }
                setViewBtnTransfer();
            }
        }

        private void initRow(TextBox txtSerial, KeyEventArgs e, string type)
        {
            string serial = txtSerial.Text.Trim();
            if (txtSerial.Text.Trim().Length == 11 ||
                txtSerial.Text.Trim().Length == 15 ||
                txtSerial.Text.Trim().Length == 16)
            {

            }
            else
            {
                txtSerial.Text = "";
                PlayWav(@"Audio/no_pass.wav");
                callAlert(2500, "Serial Incorrect !", Color.Red, Color.White);
                return;
            }


            if (tbKeySerialOld.Text.Length >= 15)
            {
                tbKeySerialOld.Text = tbKeySerialOld.Text.Trim().Substring(0, 15);
                serial = tbKeySerialOld.Text.Trim().Substring(0, 15);
            }
            if (tbKeySerialNew.Text.Length >= 15)
            {
                tbKeySerialNew.Text = tbKeySerialNew.Text.Trim().Substring(0, 15);
                serial = tbKeySerialNew.Text.Trim().Substring(0, 15);
            }
            //serial = serial.Substring(0, serial.Length-1);
            if (tbKeySerialOld.Text.Trim() != tbKeySerialNew.Text.Trim())
            {
                bool existSerial = service.existSerial(serial);
                if (!existSerial)
                {
                    txtSerial.Text = "";
                    PlayWav(@"Audio/dup.wav");
                    callAlert(2500, "Serial Already Exists !", Color.Red, Color.White);
                    return;

                }
                txtSerial.BackColor = Color.White;
                List<MSerial> itemDetail = new List<MSerial>();
                if (type == "Old")
                {
                    if (tbModelCodeOld.Text != "" && tbModelCodeNew.Text != "")
                    {
                        if (!service.checkTransferMapping(serial.Substring(4, 3), tbModelCodeNew.Text))
                        {
                            txtSerial.Text = "";
                            PlayWav(@"Audio/no_have.wav");
                            callAlert(2500, "Model Old and Model New Not Match!", Color.Red, Color.White);
                            return;
                        }
                    }
                    itemDetail = service.getDetailSerial(serial);
                    //**************************************
                    // Check Serial Have In Standard (FN_DATA) 
                    //**************************************
                    if (itemDetail.Count == 0)
                    {
                        using (frmRemark fn = new frmRemark(serial))
                        {
                            PlayWav(@"Audio/no_pass.wav");
                            fn.ShowDialog();
                            _remark = "";
                            _remark = fn.getRemark();
                            if (_remark.Length > 0)
                            {
                                Console.WriteLine(_remark + " " + _remark.Length);
                                string cutModelCode = serial.Substring(4, 3);
                                MSerial getDetialByModelCode = service.getLineByModel(cutModelCode);
                                MSerial item = new MSerial();
                                item.SErial = serial;
                                item.MOdelCode = cutModelCode;
                                item.MOdelName = getDetialByModelCode.MOdelName;
                                item.LIne = getDetialByModelCode.LIne;
                                item.REmark = _remark;
                                itemDetail.Add(item);
                            }
                        }
                    }
                    else
                    {
                        _remark = "";
                    }
                }
                else
                {
                    //**************************************
                    // Check Model Old / New is Mapping 
                    //**************************************
                    if (!service.checkTransferMapping(tbModelCodeOld.Text, serial.Substring(4, 3)))
                    {
                        txtSerial.Text = "";
                        PlayWav(@"Audio/no_have.wav");
                        callAlert(2500, "Model Old and Model New Not Match!", Color.Red, Color.White);
                        return;
                    }
                    itemDetail = service.getDetailSerialNew(serial);
                }

                if (itemDetail.Count == 0)
                {
                    txtSerial.Text = "";
                    if (type == "Old")
                    {
                        tbSerialOld.Text = "";
                        tbModelCodeOld.Text = "";
                        tbModelNameOld.Text = "";
                        tbLineOld.Text = "";
                    }
                    else
                    {
                        tbSerialNew.Text = "";
                        tbModelCodeNew.Text = "";
                        tbModelNameNew.Text = "";
                        tbLineNew.Text = "";
                    }

                    PlayWav(@"Audio/no_pass.wav");

                    return;
                }


                foreach (MSerial item in itemDetail)
                {
                    if (type == "Old")
                    {
                        tbSerialOld.Text = item.SErial.ToString();
                        tbModelCodeOld.Text = item.MOdelCode.ToString();
                        tbModelNameOld.Text = item.MOdelName.ToString();
                        tbLineOld.Text = item.LIne.ToString();
                    }
                    else
                    {
                        tbSerialNew.Text = item.SErial.ToString();
                        tbModelCodeNew.Text = item.MOdelCode.ToString();
                        tbModelNameNew.Text = item.MOdelName.ToString();
                        tbLineNew.Text = item.LIne.ToString();
                    }

                    PlayWav(@"Audio/pass.wav");

                }
                txtSerial.Text = "";
                if (type == "New")
                {
                    tbKeySerialOld.Text = "";
                    tbKeySerialOld.Focus();
                }
                else
                {
                    tbKeySerialNew.Text = "";
                    tbKeySerialNew.Focus();
                }
            }
        }



        private void setViewBtnTransfer()
        {
            tbSerialOld.Text = tbSerialOld.Text;
            tbSerialNew.Text = tbSerialNew.Text;
            if (tbSerialOld.Text.Trim() != "" && tbSerialNew.Text.Trim() != "")
            {
                btnTransfer.Enabled = true;
                btnTransfer.BackColor = Color.Green;
                btnTransfer.ForeColor = Color.White;
            }
            else
            {
                btnTransfer.Enabled = false;
                btnTransfer.BackColor = SystemColors.ControlLight;
                btnTransfer.ForeColor = Color.Black;
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            //tbSerialOld.Text = tbSerialOld.Text.ToUpper();
            //tbSerialNew.Text = tbSerialNew.Text.ToUpper();
            //MSerial ModelOld = setModel(dtgOldSerial, _OLD);
            //MSerial ModelNew = setModel(dtgNewSerial, _NEW);

            //bool DigitIsTrue = service.validateBit(tbSerialOld.Text);

            if (tbSerialOld.Text != "" && tbSerialNew.Text != "")
            {
                MSerial serialForInsert = new MSerial();
                serialForInsert.SErial_old = tbSerialOld.Text;
                serialForInsert.MOdelCodeOld = tbModelCodeOld.Text;
                serialForInsert.MOdelNameOld = tbModelNameOld.Text;
                serialForInsert.LIneOld = tbLineOld.Text;
                serialForInsert.REmark = _remark;

                serialForInsert.SErial_new = tbSerialNew.Text;
                serialForInsert.MOdelCodeNew = tbModelCodeNew.Text;
                serialForInsert.MOdelNameNew = tbModelNameNew.Text;
                serialForInsert.LIneNew = tbLineNew.Text;
                serialForInsert.EMCode = _EMCODE;
                bool addAdjust = service.addAdjust(serialForInsert);
                if (addAdjust)
                {
                    tbKeySerialOld.Focus();
                    tbKeySerialOld.Text = "";
                    tbKeySerialNew.Text = "";

                    tbSerialNew.Text = "";
                    tbModelCodeNew.Text = "";
                    tbModelNameNew.Text = "";
                    tbLineNew.Text = "";

                    tbSerialOld.Text = "";
                    tbModelCodeOld.Text = "";
                    tbModelNameOld.Text = "";
                    tbLineOld.Text = "";
                    btnTransfer.Enabled = false;

                    callAlert(5000, "Transfer Serial Success", Color.Green, Color.White);
                    setHistory();
                    int gvFirstIndex = dtgHistoryAdjust.FirstDisplayedScrollingRowIndex;
                    dtgHistoryAdjust.FirstDisplayedScrollingRowIndex = gvFirstIndex;



                    //SoundPlayer pass = new SoundPlayer(@"Audio\pass.wav");
                    //pass.Play();

                    PlayWav(@"Audio/ok2.wav");
                }
            }
            setViewBtnTransfer();
            btnSearch_Click(sender, e);
        }

        private void callAlert(int time, string msg, Color bgColor, Color fgColor)
        {
            lbShowStatusTransfer.Text = msg;
            lbShowStatusTransfer.BackColor = bgColor;
            lbShowStatusTransfer.ForeColor = fgColor;
            timer.Interval = time;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        //private MSerial setModel(DataGridView gv,string type)
        //{
        //MSerial model = new MSerial();
        //foreach (DataGridViewRow row in gv.Rows)
        //{
        //    model.SErial = row.Cells["Serial" + type].Value.ToString();
        //    model.MOdelCode = row.Cells["ModelCode" + type].Value.ToString();
        //    model.MOdelName = row.Cells["ModelName" + type].Value.ToString();
        //    model.LIne = row.Cells["Line" + type].Value.ToString();
        //}
        //return model;
        //}

        private void timer_Tick(object sender, EventArgs e)
        {
            lbShowStatusTransfer.Text = "";
            lbShowStatusTransfer.BackColor = SystemColors.Control;
            lbShowStatusTransfer.ForeColor = Color.White;
            timer.Stop();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("คุณต้องการออกจากระบบใช่หรือไม่ ?", "ออกจากระบบ", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                this.Hide();
                frmLogin login = new frmLogin();
                login.Show();
            }
        }



        private void setHistory()
        {
            List<MSerial> list = service.getTransferLabel();
            dtgHistoryAdjust.Rows.Clear();
            foreach (MSerial item in list)
            {
                int rowIndex = dtgHistoryAdjust.Rows.Add();
                var row = dtgHistoryAdjust.Rows[rowIndex];
                row.Cells["ColRemove"].Value = (item.ADj_status != "999") ? " ลบ " : "";
                row.Cells["ADJ_ID"].Value = item.ADj_id.ToString();
                row.Cells["SerialOld"].Value = item.SErial_old.ToString();
                row.Cells["ModelCodeOld"].Value = item.MOdelCodeOld.ToString();
                row.Cells["ModelNameOld"].Value = item.MOdelNameOld.ToString();
                row.Cells["LineOld"].Value = item.LIneOld.ToString();
                row.Cells["SerialNew"].Value = item.SErial_new.ToString();
                row.Cells["ModelCodeNew"].Value = item.MOdelCodeNew.ToString();
                row.Cells["ModelNameNew"].Value = item.MOdelNameNew.ToString();
                row.Cells["LineNew"].Value = item.LIneNew.ToString();
                row.Cells["ColDate"].Value = item.UPdate_dt.ToString("dd/MM/yyyy HH:mm:ss");
                row.Cells["ColStatus"].Value = item.ADj_status;
            }
        }

        private void tbKeySerialOld_Enter(object sender, EventArgs e)
        {
            //setFocus(tbKeySerialOld,true);
            tbKeySerialOld.BackColor = Color.LemonChiffon;
        }

        private void tbKeySerialOld_Leave(object sender, EventArgs e)
        {
            tbKeySerialOld.BackColor = Color.LightCoral;
        }

        private void tbKeySerialNew_Leave(object sender, EventArgs e)
        {
            tbKeySerialNew.BackColor = Color.LightCoral;
        }

        private void tbKeySerialNew_Enter(object sender, EventArgs e)
        {
            tbKeySerialNew.BackColor = Color.LemonChiffon;
        }

        private void setFocus(TextBox tb, bool act)
        {
            if (act)
            {
                //tb.BackColor = Color.Red;
            }
            else
            {

            }
        }



        private void tbKeySerialOld_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbKeySerialNew_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tbKeySerialOld.Focus();

            tbKeySerialOld.Text = "";
            tbKeySerialNew.Text = "";

            tbSerialNew.Text = "";
            tbModelCodeNew.Text = "";
            tbModelNameNew.Text = "";
            tbLineNew.Text = "";

            tbSerialOld.Text = "";
            tbModelCodeOld.Text = "";
            tbModelNameOld.Text = "";
            tbLineOld.Text = "";
            btnTransfer.Enabled = false;
            txtBitNew.Text = "BIT";
            txtBitOld.Text = "BIT";
            setViewBtnTransfer();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<MSummaryTransfer> oAryData = service.getSummaryTransferByDate(dpkDate.Value);
            dgvSummaryAdjust.Rows.Clear();
            if (oAryData.Count > 0)
            {
                int _count = 0;
                foreach (MSummaryTransfer item in oAryData)
                {
                    dgvSummaryAdjust.Rows.Add(item.ModelNameOld + " (" + item.ModelCodeOld + ")",
                                              " -> ",
                                              item.ModelNameNew + " (" + item.ModelCodeNew + ")",
                                              item.TransferQty.ToString("N0"));

                    _count += item.TransferQty;
                }

                // Total 
                dgvSummaryAdjust.Rows.Add("  ", "  ", " ***** รวมทั้งหมด *****", _count.ToString("N0"));

            }

        }


        private static void PlayWav(string wavFile)
        {
            var player = new System.Media.SoundPlayer { SoundLocation = wavFile };
            player.PlaySync();

        }

        private void tbSearchSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtgHistoryAdjust.Rows.Clear();

                string serial = "";
                if (tbSearchSerial.Text.Trim().Length == 0)
                {
                    setHistory();
                    return;
                }
                else if (tbSearchSerial.Text.Trim().Length == 11 || tbSearchSerial.Text.Trim().Length == 15)
                {
                    serial = tbSearchSerial.Text.Trim();

                }
                else if (tbSearchSerial.Text.Trim().Length == 16)
                {
                    serial = tbSearchSerial.Text.Trim().Substring(0, 15);

                }
                else
                {
                    tbSearchSerial.Text = "";
                    return;

                }
                tbSearchSerial.Text = "";

                List<MSerial> list = service.getSerialHistoryBySerial(serial);

                if (list.Count > 0)
                {
                    foreach (MSerial item in list)
                    {
                        int rowIndex = dtgHistoryAdjust.Rows.Add();
                        var row = dtgHistoryAdjust.Rows[rowIndex];
                        row.Cells["ColRemove"].Value = (item.ADj_status != "999") ? " ลบ " : "";
                        row.Cells["ADJ_ID"].Value = item.ADj_id.ToString();
                        row.Cells["SerialOld"].Value = item.SErial_old.ToString();
                        row.Cells["ModelCodeOld"].Value = item.MOdelCodeOld.ToString();
                        row.Cells["ModelNameOld"].Value = item.MOdelNameOld.ToString();
                        row.Cells["LineOld"].Value = item.LIneOld.ToString();
                        row.Cells["SerialNew"].Value = item.SErial_new.ToString();
                        row.Cells["ModelCodeNew"].Value = item.MOdelCodeNew.ToString();
                        row.Cells["ModelNameNew"].Value = item.MOdelNameNew.ToString();
                        row.Cells["LineNew"].Value = item.LIneNew.ToString();
                        row.Cells["ColDate"].Value = item.UPdate_dt.ToString("dd/MM/yyyy HH:mm:ss");
                        row.Cells["ColStatus"].Value = item.ADj_status;
                    }
                }


            }

        }

        private void btnSearchSerial_Click(object sender, EventArgs e)
        {
            dtgHistoryAdjust.Rows.Clear();

            string serial = "";
            if (tbSearchSerial.Text.Trim().Length == 0)
            {
                setHistory();
                return;
            }
            else if (tbSearchSerial.Text.Trim().Length == 11 || tbSearchSerial.Text.Trim().Length == 15)
            {
                serial = tbSearchSerial.Text.Trim();

            }
            else if (tbSearchSerial.Text.Trim().Length == 16)
            {
                serial = tbSearchSerial.Text.Trim().Substring(0, 15);

            }
            else
            {
                tbSearchSerial.Text = "";
                return;

            }
            tbSearchSerial.Text = "";

            List<MSerial> list = service.getSerialHistoryBySerial(serial);

            if (list.Count > 0)
            {
                foreach (MSerial item in list)
                {
                    int rowIndex = dtgHistoryAdjust.Rows.Add();
                    var row = dtgHistoryAdjust.Rows[rowIndex];
                    row.Cells["ColRemove"].Value = (item.ADj_status != "999") ? " ลบ " : "";
                    row.Cells["ADJ_ID"].Value = item.ADj_id.ToString();
                    row.Cells["SerialOld"].Value = item.SErial_old.ToString();
                    row.Cells["ModelCodeOld"].Value = item.MOdelCodeOld.ToString();
                    row.Cells["ModelNameOld"].Value = item.MOdelNameOld.ToString();
                    row.Cells["LineOld"].Value = item.LIneOld.ToString();
                    row.Cells["SerialNew"].Value = item.SErial_new.ToString();
                    row.Cells["ModelCodeNew"].Value = item.MOdelCodeNew.ToString();
                    row.Cells["ModelNameNew"].Value = item.MOdelNameNew.ToString();
                    row.Cells["LineNew"].Value = item.LIneNew.ToString();
                    row.Cells["ColDate"].Value = item.UPdate_dt.ToString("dd/MM/yyyy HH:mm:ss");
                    row.Cells["ColStatus"].Value = item.ADj_status;

                }
            }
        }

        private void dtgHistoryAdjust_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                string serialOld = dtgHistoryAdjust.Rows[e.RowIndex].Cells["SerialOld"].Value.ToString();
                string serialNew = dtgHistoryAdjust.Rows[e.RowIndex].Cells["SerialNew"].Value.ToString();
                string adjStatus = dtgHistoryAdjust.Rows[e.RowIndex].Cells["ColStatus"].Value.ToString();

                if (adjStatus != "999")
                {
                    DialogResult dlgCF = MessageBox.Show("ยืนยันการลบข้อมูล \"" + serialOld + "\" (เก่า) -> \"" + serialNew + "\" (ใหม่)  ? ", "ยืนยันการลบข้อมูล ?", MessageBoxButtons.OKCancel);
                    if (dlgCF.Equals(DialogResult.OK))
                    {
                        bool remove = service.removeSerial(serialOld, serialNew);
                        if (remove)
                        {
                            setHistory();
                            btnSearch_Click(sender, e);

                            PlayWav(@"Audio/ok2.wav");
                        }
                        else
                        {
                            MessageBox.Show("ลบไม่สำเร็จ !");
                        }
                    }
                } // end if


            }
        }

        private void uploadExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUploadExcel frmUpload = new frmUploadExcel();
            frmUpload.ShowDialog();
        }

        private void insertFinalDataDITToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void การบนทกขอมลToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSetting frmSetting = new frmSetting();
            frmSetting.FormClosed += frmSettingClosed;
            frmSetting.StartPosition = FormStartPosition.CenterParent;
            frmSetting.MaximizeBox = false;
            frmSetting.MinimizeBox = false;
            frmSetting.ShowDialog();
        }

        private void frmSettingClosed(object sender, FormClosedEventArgs e)
        {
            initSettingInsert();
        }
    }
}
