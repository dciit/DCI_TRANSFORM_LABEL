using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCI_TRANSFER_SERIAR
{
    public partial class frmUploadExcel : Form
    {
        private string path = @"C:\\temp\";
        List<MSerial> listSerial = new List<MSerial>();
        ConnectDB conDBSCM = new ConnectDB("DBSCM");
        Service service = new Service();
        int[] LengthSerial = { 11, 15, 16 };
        public frmUploadExcel()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            if (tbFilePath.Text.Length > 0)
            {
                uploadFile();
            }
        }


        private void uploadFile()
        {
            bool enableBtnUpload = true;
            string pathFile = tbFilePath.Text;
            string filename = ("AdjustLabelManual-" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                if (File.Exists(path + filename))
                {
                    File.Delete(path + filename);
                }
            }
            File.Copy(pathFile, path + filename);
         
            byte[] bin = File.ReadAllBytes(path + filename);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            listSerial.Clear();
            using (MemoryStream stream = new MemoryStream(bin))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                foreach (ExcelWorksheet xlWorkSheet in excelPackage.Workbook.Worksheets)
                {
                    if (xlWorkSheet.Name == "Sheet1")
                    {
                        for (int row = 2; row <= xlWorkSheet.Dimension.End.Row; row++)
                        {
                            MSerial item = new MSerial();
                            for (int colIndex = 1; colIndex <= xlWorkSheet.Dimension.End.Column; colIndex++)
                            {
                                xlWorkSheet.Cells[row, colIndex].Value = xlWorkSheet.Cells[row, colIndex].Value != null ? xlWorkSheet.Cells[row, colIndex].Value.ToString() : "";
                            }
                            item.SErial_old = xlWorkSheet.Cells[row, 1].Value.ToString();
                            item.MOdelCodeOld = xlWorkSheet.Cells[row, 2].Value.ToString();
                            item.MOdelNameOld = xlWorkSheet.Cells[row, 3].Value.ToString();
                            item.LIneOld = xlWorkSheet.Cells[row, 4].Value.ToString();
                            item.SErial_new = xlWorkSheet.Cells[row, 5].Value.ToString();
                            item.MOdelCodeNew = xlWorkSheet.Cells[row, 6].Value.ToString();
                            item.MOdelNameNew = xlWorkSheet.Cells[row, 7].Value.ToString();
                            item.LIneNew = xlWorkSheet.Cells[row, 8].Value.ToString();
                            item.EMCode = xlWorkSheet.Cells[row,9].Value.ToString();
                            bool isMapping = false;
                            bool haveSerial = false;
                            bool existSerial = false;
                            if (item.SErial_old.Length > 11 && item.SErial_new.Length > 11)
                            {
                                isMapping = service.checkTransferMapping(item.SErial_old.Substring(4, 3), item.SErial_new.Substring(4, 3));
                                List<MSerial> serialDetial = service.getDetailSerial(item.SErial_old);
                                if (serialDetial.Count > 0)
                                {
                                    haveSerial = true;
                                }
                            }
                            if (LengthSerial.Contains(item.SErial_old.Length) && LengthSerial.Contains(item.SErial_new.Length) && isMapping == true && haveSerial == true )
                            {
                                item.ADj_status = "1";
                            }
                            else
                            {
                                if (!LengthSerial.Contains(item.SErial_old.Length) || !LengthSerial.Contains(item.SErial_new.Length))
                                {
                                    item.EXcelRemark.Add("Serial ตัวอักษรไม่ครบตามกำหนด");
                                }
                                if (!isMapping)
                                {
                                    item.EXcelRemark.Add("Mapping Model ไม่ตรงกัน !");
                                }
                                if (!haveSerial)
                                {
                                    item.EXcelRemark.Add("ไม่พบข้อมูลของ Serial !");
                                }
                                if (!existSerial)
                                {
                                    //item.EXcelRemark.Add("Serial มีอยญู่ในร")
                                }
                                item.ADj_status = "0";
                            }
                            listSerial.Add(item);
                        }
                    }
                }
            }
            txtShowCount.Text = listSerial.Count.ToString();
            if (listSerial.Count > 0)
            {
                dataGridView1.Rows.Clear();
                foreach (MSerial item in listSerial)
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    var row = dataGridView1.Rows[rowIndex];
                    row.Cells["SerialOld"].Value = item.SErial_old;
                    row.Cells["ModelCodeOld"].Value = item.MOdelCodeOld;
                    row.Cells["ModelNameOld"].Value = item.MOdelNameOld;
                    row.Cells["LineOld"].Value = item.LIneOld;
                    row.Cells["SerialNew"].Value = item.SErial_new;
                    row.Cells["ModelCodeNew"].Value = item.MOdelCodeNew;
                    row.Cells["ModelNameNew"].Value = item.MOdelNameNew;
                    row.Cells["LineNew"].Value = item.LIneNew;
                    if (item.ADj_status.Equals("0"))
                    {
                        enableBtnUpload = false;
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 128);
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    if (item.EXcelRemark.Count > 0)
                    {
                        foreach(string itemRemarkExcel in item.EXcelRemark)
                        {
                            row.Cells["ColMessage"].Value += itemRemarkExcel;
                        }
                    }
                }
                btnUpload.Enabled = enableBtnUpload;
                if (enableBtnUpload)
                {
                    btnUpload.BackColor = Color.Green;
                    btnUpload.ForeColor = Color.White;
                }
                else
                {
                    btnUpload.BackColor = SystemColors.ActiveBorder;
                    btnUpload.ForeColor = Color.DimGray;
                }
            }
        }

        private void btnSelectExcel_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Excel Files (.xlsx)|*.xlsx;";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            DialogResult dlg = openFileDialog1.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                tbFilePath.Text = openFileDialog1.FileName;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            int countSuccess = 0;
            DialogResult dialog = MessageBox.Show("ต้องการอัพโหลดข้อมูล ใช่หรือไม่ ?", "อัพโหลดข้อมูล", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                foreach (MSerial item in listSerial)
                {
                    if (service.insertAdjust(item) > 0)
                    {
                        countSuccess++;
                        Console.WriteLine("upload ผ่าน");
                    }
                    else
                    {
                        Console.WriteLine("upload ไม่ผ่าน");
                    }
                }
            }
            if (countSuccess == listSerial.Count)
            {
                MessageBox.Show("อัพโหลดข้อมูลสำเร็จ ...");
            }
        }
    }
}
