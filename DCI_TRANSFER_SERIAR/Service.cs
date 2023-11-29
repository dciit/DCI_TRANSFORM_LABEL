using DCI_TRANSFER_SERIAR.Models;
using MultiSkillCer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace DCI_TRANSFER_SERIAR
{
    internal class Service
    {
        private IniFile initFile = new IniFile("Config.ini");
        private ConnectDB conDBDCI = new ConnectDB("DBDCI");
        private ConnectDB conDBSCM = new ConnectDB("DBSCM");
        private ConnectDB conDBIOT = new ConnectDB("DBIOT");
        private ConnectDB conDBIOT122103 = new ConnectDB("DBIOT122103");
        private string _ADJ_SERIAL = "ADJ_SERIAL";



        internal bool addAdjust(MSerial model)
        {
            SqlCommand cmd = new SqlCommand();
            int lastAdjId = getRowCount();
            cmd.CommandText = @"INSERT INTO [" + _ADJ_SERIAL + "] (ADJ_ID,SERIAL_OLD,MODEL_CODE_OLD,MODEL_NAME_OLD,LINE_OLD,SERIAL_NEW,MODEL_CODE_NEW,MODEL_NAME_NEW,LINE_NEW,UPDATE_DT,EM_CODE,Remark) VALUES (@ADJ_ID,@SERIAL_OLD,@MODEL_CODE_OLD,@MODEL_NAME_OLD,@LINE_OLD,@SERIAL_NEW,@MODEL_CODE_NEW,@MODEL_NAME_NEW,@LINE_NEW,@UPDATE_DT,@EM_CODE,'" + model.REmark + "')";
            cmd.Parameters.Add(new SqlParameter("@ADJ_ID", lastAdjId + 1));
            cmd.Parameters.Add(new SqlParameter("@SERIAL_OLD", model.SErial_old.ToUpper()));
            cmd.Parameters.Add(new SqlParameter("@MODEL_CODE_OLD", model.MOdelCodeOld));
            cmd.Parameters.Add(new SqlParameter("@MODEL_NAME_OLD", model.MOdelNameOld));
            cmd.Parameters.Add(new SqlParameter("@LINE_OLD", model.LIneOld));
            cmd.Parameters.Add(new SqlParameter("@SERIAL_NEW", model.SErial_new.ToUpper()));
            cmd.Parameters.Add(new SqlParameter("@MODEL_CODE_NEW", model.MOdelCodeNew));
            cmd.Parameters.Add(new SqlParameter("@MODEL_NAME_NEW", model.MOdelNameNew));
            cmd.Parameters.Add(new SqlParameter("@LINE_NEW", model.LIneNew));
            cmd.Parameters.Add(new SqlParameter("@UPDATE_DT", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@EM_CODE", model.EMCode));
            DataTable dt = conDBSCM.Query(cmd);
            if (initFile.GetString("MODE", "FINALDATADIT", "") == "TRUE")
            {
                InsertFinalData(model);
            }
            if (initFile.GetString("MODE", "FNCOPYLABELLOG", "") == "TRUE")
            {
                InsertToLabel(model);
            }
            if (initFile.GetString("MODE", "FNDATACENTER", "") == "TRUE")
            {
                InsertToFnDataCenter(model);
            }
            return true;
        }

        private string getPipeNo(string newSerial)
        {
            string pipeno = "";
            SqlCommand sqlGetPipe = new SqlCommand();
            sqlGetPipe.CommandText = @"SELECT  fn.PipeNo FROM [dbDCI].[dbo].[FN_CopyLabelLog] lb LEFT JOIN [dbDCI].[dbo].[FN_DataCenter] fn on lb.to_serial = fn.Serial  WHERE lb.to_serial = '" + newSerial + "'";
            DataTable dt = conDBDCI.Query(sqlGetPipe);
            if (dt.Rows.Count > 0)
            {
                pipeno = dt.Rows[0]["PipeNo"].ToString();
            }
            return pipeno;
        }
        private void InsertToLabel(MSerial model)
        {
            Random rnd = new Random(5);
            int nxt = rnd.Next(1, 99999);

            SqlCommand sqlInsertCopyLabel = new SqlCommand();
            DateTime dtNow = DateTime.Now;
            string nbr = $"{dtNow.Year}{dtNow.Month}{dtNow.Day}{dtNow.Hour}{dtNow.Minute}{dtNow.Second}{nxt.ToString("00000")}";
            sqlInsertCopyLabel.CommandText = @"INSERT INTO [dbo].[FN_CopyLabelLog]
           ([nbr],[from_serial],[to_serial],[from_line],[to_line],[dataDate],[CreateBy],[Remark1],[Remark2],[Remark3]) VALUES (@nbr,@from_serial,@to_serial,@from_line,@to_line,@dataDate,@CreateBy,@Remark1,@Remark2,@Remark3)";
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("nbr", nbr));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("from_serial", model.SErial_old.ToUpper()));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("to_serial", model.SErial_new.ToUpper()));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("from_line", "LINE" + model.LIneOld));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("to_line", "LINE" + model.LIneNew));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("dataDate", DateTime.Now));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("CreateBy", model.EMCode));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("Remark1", ""));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("Remark2", ""));
            sqlInsertCopyLabel.Parameters.Add(new SqlParameter("Remark3", ""));
            int insert = conDBDCI.ExecuteNonQuery(sqlInsertCopyLabel);
        }

        private void InsertToFnDataCenter(MSerial model)
        {
            string serialNew = model.SErial_new.ToUpper();
            SqlCommand sqlGetSerialBefore = new SqlCommand();
            sqlGetSerialBefore.CommandText = @"SELECT Serial,PipeNo FROM [dbDCI].[dbo].[FN_DataCenter] WHERE Serial = @SERIAL";
            sqlGetSerialBefore.Parameters.Add(new SqlParameter("SERIAL", model.SErial_old.ToUpper()));
            DataTable dt = conDBDCI.Query(sqlGetSerialBefore);
            int count = dt.Rows.Count;
            if (count == 0)
            {
                SqlCommand sqlInsertFnDataCenter = new SqlCommand();
                sqlInsertFnDataCenter.CommandText = @"INSERT INTO [dbDCI].[dbo].[FN_DataCenter] (Serial,ModelCode,PipeNo,Line,LB_InsertDate,LB_MFGDate,OC_Judgement,ReLabel_Status) 
                  SELECT @Serial_New,ModelCode,@PipeNo,Line,LB_InsertDate,LB_MFGDate,OC_Judgement,@ReLabel_Status FROM [dbDCI].[dbo].[FN_DataCenter] WHERE Serial = @Serial_Old ";
                sqlInsertFnDataCenter.Parameters.Add(new SqlParameter("@Serial_New", serialNew.ToUpper()));
                sqlInsertFnDataCenter.Parameters.Add(new SqlParameter("@Serial_Old", model.SErial_old.ToUpper()));
                sqlInsertFnDataCenter.Parameters.Add(new SqlParameter("@ReLabel_Status", "re_new"));
                int insertFn = conDBDCI.ExecuteNonQuery(sqlInsertFnDataCenter);
                if (insertFn == 0)
                {
                    MessageBox.Show($"ไม่สามารถเพิ่มข้อมูลได้ {model.SErial_new} ใน FN_DATA_CENTER ติดต่อ IT (115)");
                }

                //==========================================================
                //      INSERT LABEL PRINT DATA IN IoT
                //==========================================================
                SqlCommand sqlChkLB = new SqlCommand();
                sqlChkLB.CommandText = @"
                    SELECT [MFGDate],[ModelName],[ModelNo],[LabelRunning],[PipeDate_SN],[Destination],[Class],[PipeNumber],[Supplier_PartNumber],[Goodman_PartNumber],[Supplier_SerialNumber],[Goodman_DNX],[SerialNumber],[InsertDate] FROM (
	                    SELECT [MFGDate],[ModelName],[ModelNo],[LabelRunning],[PipeDate_SN],[Destination],[Class],[PipeNumber],[Supplier_PartNumber],[Goodman_PartNumber],[Supplier_SerialNumber],[Goodman_DNX],[SerialNumber],[InsertDate]
	                    FROM [dbIoT].[dbo].[L8_FN_LabelPrinting] 
	                    WHERE SerialNumber = @SerialOld 

	                    UNION ALL

	                    SELECT [MFGDate],[ModelName],[ModelNo],[LabelRunning],[PipeDate_SN],[Destination],[Class],[PipeNumber],[Supplier_PartNumber],[Goodman_PartNumber],[Supplier_SerialNumber],[Goodman_DNX],[SerialNumber],[InsertDate]
	                    FROM [dbIoT].[dbo].[L8_FN_LabelPrinting_2022] 
	                    WHERE SerialNumber = @SerialOld 
                    ) as t  ";
                sqlChkLB.Parameters.Add(new SqlParameter("@SerialOld", model.SErial_old.ToUpper()));
                DataTable dtChkLB = conDBIOT122103.Query(sqlChkLB);
                int countChkLB = dtChkLB.Rows.Count;
                if (countChkLB > 0)
                {
                    SqlCommand sqlCheckPrinter = new SqlCommand();
                    sqlCheckPrinter.CommandText = @"SELECT [SerialNumber] ,[InsertDate] FROM [dbIoT].[dbo].[L8_FN_LabelPrinting] WHERE SerialNumber  = '" + serialNew + "'";
                    DataTable dtPrinter = conDBIOT122103.Query(sqlCheckPrinter);
                    string pipeno = getPipeNo(serialNew);
                    if (dtPrinter.Rows.Count > 0)
                    {
                        updatePrinter(model.SErial_new, pipeno);
                    }
                    else
                    {
                        SqlCommand sqlInstrLB = new SqlCommand();
                        sqlInstrLB.CommandText = @"INSERT INTO [dbIoT].[dbo].[L8_FN_LabelPrinting] ([MFGDate],[ModelName],[ModelNo],[LabelRunning],[PipeDate_SN],[Destination],[Class],[PipeNumber],[Supplier_PartNumber],[Goodman_PartNumber],[Supplier_SerialNumber],[Goodman_DNX],[SerialNumber],[InsertDate])
                                SELECT [MFGDate],[ModelName],[ModelNo],substring(@SerialNew, 8,8),[PipeDate_SN],[Destination],[Class],@PipeNo,substring(@SerialNew, 8,8),[Goodman_PartNumber],[Supplier_SerialNumber],[Goodman_DNX],@SerialNew,[InsertDate] FROM (
	                                SELECT [MFGDate],[ModelName],[ModelNo],[LabelRunning],[PipeDate_SN],[Destination],[Class],[PipeNumber],[Supplier_PartNumber],[Goodman_PartNumber],[Supplier_SerialNumber],[Goodman_DNX],[SerialNumber],[InsertDate]
	                                FROM [dbIoT].[dbo].[L8_FN_LabelPrinting] 
	                                WHERE SerialNumber = @SerialOld 

	                                UNION ALL

	                                SELECT [MFGDate],[ModelName],[ModelNo],[LabelRunning],[PipeDate_SN],[Destination],[Class],[PipeNumber],[Supplier_PartNumber],[Goodman_PartNumber],[Supplier_SerialNumber],[Goodman_DNX],[SerialNumber],[InsertDate]
	                                FROM [dbIoT].[dbo].[L8_FN_LabelPrinting_2022] 
	                                WHERE SerialNumber = @SerialOld 
                                ) as t ";
                        sqlInstrLB.Parameters.Add(new SqlParameter("@SerialOld", model.SErial_old));
                        sqlInstrLB.Parameters.Add(new SqlParameter("@SerialNew", serialNew));
                        sqlInstrLB.Parameters.Add(new SqlParameter("@PipeNo", pipeno));
                        int insertLB = conDBIOT122103.ExecuteNonQuery(sqlInstrLB);
                        if (insertLB == 0)
                        {
                            MessageBox.Show($"ไม่สามารถเพิ่มข้อมูลได้ {model.SErial_new} ใน L8_FN_LabelPrinting ติดต่อ IT");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"w {model.SErial_new} ใน L8_FN_LabelPrinting ติดต่อ IT");
                }
                //==========================================================
                //     END INSERT LABEL PRINT DATA IN IoT
                //==========================================================

            }
            else
            {
                if (dt.Rows[0]["PipeNo"].ToString() == "")
                {
                    string pipeno = getPipeNo(serialNew);
                    updateFnCenter(serialNew, pipeno);
                    updatePrinter(serialNew, pipeno);
                }
                //else
                //{
                //    MessageBox.Show($"มีข้อมูลเดิมของ Serial :  {model.SErial_old} ใน FN_DATA_CENTER ติดต่อ IT");
                //}
            }
        }

        private void updateFnCenter(string serialNew, string pipeno)
        {
            SqlCommand sqlUpdateFnDataCenter = new SqlCommand();
            sqlUpdateFnDataCenter.CommandText = @"UPDATE [dbDCI].[dbo].[FN_DataCenter] SET PipeNo = '" + pipeno + "' WHERE Serial = '" + serialNew + "' AND PipeNo = ''";
            int updateFnDataCenter = conDBDCI.ExecuteNonQuery(sqlUpdateFnDataCenter);
            if (updateFnDataCenter == 0)
            {
                MessageBox.Show($"มีข้อมูลเดิมของ FnDataCenter Serial :  {serialNew} แต่นำ PipeNo ไปอัพเดรตไม่สำเร็จ (182) ติดต่อ IT " + sqlUpdateFnDataCenter.CommandText);
            }
        }

        private void updatePrinter(string serialNew, string pipeno)
        {
            SqlCommand sqlUpdateL8FnLabelPrint = new SqlCommand();
            sqlUpdateL8FnLabelPrint.CommandText = @" UPDATE [dbIoT].[dbo].[L8_FN_LabelPrinting] SET PipeNumber = '" + pipeno + "' WHERE SerialNumber = '" + serialNew.ToUpper() + "' AND PipeNumber = ''";
            int updatePrinter = conDBIOT122103.ExecuteNonQuery(sqlUpdateL8FnLabelPrint);
            if (updatePrinter == 0)
            {
                MessageBox.Show($"มีข้อมูลเดิมของ Printer Serial :  {serialNew} แต่นำ PipeNo ไปอัพเดรตไม่สำเร็จ (182) ติดต่อ IT " + sqlUpdateL8FnLabelPrint.CommandText);
            }
        }

        private void InsertFinalData(MSerial model)
        {

            SqlCommand sqlCheck = new SqlCommand();
            sqlCheck.CommandText = @"SELECT ID FROM [dbIoT].[dbo].[FinalDataDIT]  WHERE SerialNo = @SERIALNO";
            sqlCheck.Parameters.Add(new SqlParameter("@SERIALNO", model.SErial_new));
            DataTable dtCheckExist = conDBIOT.Query(sqlCheck);
            if (dtCheckExist.Rows.Count == 0)
            {
                SqlCommand sqlInsert = new SqlCommand();
                sqlInsert.CommandText = @"INSERT INTO [dbo].[FinalDataDIT] ([Model],[SerialNo],[PDDate],[PDTime],[LineNo],[Send],[SendDate],[InsertDate])
                    VALUES (@MODEL,@SERIALNO,@PDATE,@PDTIME,@LINENO,@SEND,@SENDDATE,@INSERTDATE)";
                sqlInsert.Parameters.Add(new SqlParameter("@MODEL", model.MOdelNameNew));
                sqlInsert.Parameters.Add(new SqlParameter("@SERIALNO", model.SErial_new));
                sqlInsert.Parameters.Add(new SqlParameter("@PDATE", "1900-01-01"));
                sqlInsert.Parameters.Add(new SqlParameter("@PDTIME", "00:00:00"));
                sqlInsert.Parameters.Add(new SqlParameter("@LINENO", model.LIneNew != "" ? model.LIneNew : "999"));
                sqlInsert.Parameters.Add(new SqlParameter("@SEND", "1"));
                sqlInsert.Parameters.Add(new SqlParameter("@SENDDATE", DateTime.Now));
                sqlInsert.Parameters.Add(new SqlParameter("@INSERTDATE", DateTime.Now));
                int a = conDBIOT.ExecuteNonQuery(sqlInsert);
                if (a == 0)
                {
                    MessageBox.Show("ไม่สามารถบันทึกข้อมูลเข้า IOT FinalDataDIT !!! ติดต่อ IT ");
                }
            }
            else
            {
                Console.WriteLine("sadds");
            }


        }

        internal int getRowCount()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT TOP 1 * FROM " + _ADJ_SERIAL + " ORDER BY UPDATE_DT DESC";
            DataTable dt = conDBSCM.Query(cmd);

            if (dt.Rows.Count > 0)
            {
                int adj_id = 0;
                foreach (DataRow dw in dt.Rows)
                {
                    adj_id = dw["ADJ_ID"] != System.DBNull.Value ? Convert.ToInt32(dw["ADJ_ID"]) : 0;
                }
                return adj_id;
            }
            else
            {
                return 0;
            }
        }


        internal List<MSerial> getDetailSerial(string serial)
        {
            serial = serial.Trim();
            List<MSerial> list = new List<MSerial>();
            List<string> dbFnName = listTbFn();
            foreach (string dbName in dbFnName)
            {
                if (list.Count > 0)
                {
                    break;
                }
                SqlCommand _cmd = new SqlCommand();
                _cmd.CommandText = @"SELECT Line,serial, SUBSTRING(serial,5,3)  ModelCode , M.[Model] as ModelName FROM [dbDCI].[dbo].[" + dbName + "]  F  LEFT JOIN (SELECT [ModelCode],[Model] FROM [192.168.226.86].[dbSCM].[dbo].[PN_Compressor] WHERE [Status] = 'ACTIVE' GROUP BY [ModelCode],[Model]) M ON M.[ModelCode] = SUBSTRING(serial,5,3)  WHERE Serial = @SERIAL ";
                _cmd.Parameters.Add(new SqlParameter("@SERIAL", serial));
                DataTable _dt = conDBDCI.Query(_cmd);

                foreach (DataRow dr21 in _dt.Rows)
                {
                    MSerial item = new MSerial();
                    item.SErial = dr21["serial"].ToString();
                    item.MOdelCode = dr21["ModelCode"].ToString();
                    item.MOdelName = dr21["ModelName"].ToString();
                    item.LIne = dr21["line"].ToString();
                    list.Add(item);
                }
            }
            //         SqlCommand cmd = new SqlCommand();
            //         cmd.CommandText = @"SELECT Line,serial, SUBSTRING(serial,5,3)  ModelCode , M.[Model] as ModelName FROM [dbDCI].[dbo].[FN_DataCenter]  F LEFT JOIN (SELECT [ModelCode],[Model] FROM [192.168.226.86].[dbSCM].[dbo].[PN_Compressor] WHERE [Status] = 'ACTIVE'
            //GROUP BY [ModelCode],[Model]) M ON M.[ModelCode] = SUBSTRING(serial,5,3)  WHERE Serial = @SERIAL ";
            //         cmd.Parameters.Add(new SqlParameter("@SERIAL", serial));
            //         DataTable dt = conDBDCI.Query(cmd);
            //         if (dt.Rows.Count > 0)
            //         {
            //             foreach (DataRow dr in dt.Rows)
            //             {
            //                 MSerial item = new MSerial();
            //                 item.SErial = dr["serial"].ToString();
            //                 item.MOdelCode = dr["ModelCode"].ToString();
            //                 item.MOdelName = dr["ModelName"].ToString();
            //                 item.LIne = dr["line"].ToString();
            //                 list.Add(item);
            //             }
            //         }
            //         else
            //         {


            //             SqlCommand cmd21 = new SqlCommand();
            //             cmd21.CommandText = @"SELECT Line,serial, SUBSTRING(serial,5,3)  ModelCode , M.[Model] as ModelName 
            //                                   FROM [dbDCI].[dbo].[FN_DataCenter_2021]  F 
            //                                   LEFT JOIN (SELECT [ModelCode],[Model] FROM [192.168.226.86].[dbSCM].[dbo].[PN_Compressor] WHERE [Status] = 'ACTIVE'
            //                                        GROUP BY [ModelCode],[Model]) M ON M.[ModelCode] = SUBSTRING(serial,5,3)  WHERE Serial = @SERIAL ";
            //             cmd21.Parameters.Add(new SqlParameter("@SERIAL", serial));
            //             DataTable dt21 = conDBDCI.Query(cmd21);

            //             if (dt21.Rows.Count > 0)
            //             {
            //                 foreach (DataRow dr21 in dt21.Rows)
            //                 {
            //                     MSerial item = new MSerial();
            //                     item.SErial = dr21["serial"].ToString();
            //                     item.MOdelCode = dr21["ModelCode"].ToString();
            //                     item.MOdelName = dr21["ModelName"].ToString();
            //                     item.LIne = dr21["line"].ToString();
            //                     list.Add(item);
            //                 }
            //             }
            //             else
            //             {
            //                 SqlCommand cmd20 = new SqlCommand();
            //                 cmd20.CommandText = @"SELECT Line,serial, SUBSTRING(serial,5,3)  ModelCode , M.[Model] as ModelName 
            //                                       FROM [dbDCI].[dbo].[FN_DataCenter_2020]  F 
            //                                       LEFT JOIN (SELECT [ModelCode],[Model] FROM [192.168.226.86].[dbSCM].[dbo].[PN_Compressor] WHERE [Status] = 'ACTIVE'
            //                                         GROUP BY [ModelCode],[Model]) M ON M.[ModelCode] = SUBSTRING(serial,5,3)  WHERE Serial = @SERIAL ";
            //                 cmd20.Parameters.Add(new SqlParameter("@SERIAL", serial));
            //                 DataTable dt20 = conDBDCI.Query(cmd20);

            //                 if (dt20.Rows.Count > 0)
            //                 {
            //                     foreach (DataRow dr20 in dt20.Rows)
            //                     {
            //                         MSerial item = new MSerial();
            //                         item.SErial = dr20["serial"].ToString();
            //                         item.MOdelCode = dr20["ModelCode"].ToString();
            //                         item.MOdelName = dr20["ModelName"].ToString();
            //                         item.LIne = dr20["line"].ToString();
            //                         list.Add(item);
            //                     }
            //                 }
            //                 else
            //                 {
            //                     SqlCommand cmd19 = new SqlCommand();
            //                     cmd19.CommandText = @"SELECT Line,serial, SUBSTRING(serial,5,3)  ModelCode , M.[Model] as ModelName 
            //                                 FROM [dbDCI].[dbo].[FN_DataCenter_2019]  F 
            //                                 LEFT JOIN (SELECT [ModelCode],[Model] FROM [192.168.226.86].[dbSCM].[dbo].[PN_Compressor] WHERE [Status] = 'ACTIVE'
            //                                    GROUP BY [ModelCode],[Model]) M ON M.[ModelCode] = SUBSTRING(serial,5,3)  WHERE Serial = @SERIAL ";
            //                     cmd19.Parameters.Add(new SqlParameter("@SERIAL", serial));
            //                     DataTable dt19 = conDBDCI.Query(cmd19);

            //                     if (dt19.Rows.Count > 0)
            //                     {
            //                         foreach (DataRow dr19 in dt19.Rows)
            //                         {
            //                             MSerial item = new MSerial();
            //                             item.SErial = dr19["serial"].ToString();
            //                             item.MOdelCode = dr19["ModelCode"].ToString();
            //                             item.MOdelName = dr19["ModelName"].ToString();
            //                             item.LIne = dr19["line"].ToString();
            //                             list.Add(item);
            //                         }
            //                     }
            //                 }
            //             }
            //         }
            return list;
        }


        internal List<MSerial> getDetailSerialNew(string serial)
        {
            serial = serial.Trim();
            List<MSerial> list = new List<MSerial>();
            SqlCommand cmd = new SqlCommand();
            //         cmd.CommandText = @"SELECT Line,serial, SUBSTRING(serial,5,3)  ModelCode , M.[Model] as ModelName FROM [dbDCI].[dbo].[FN_DataCenter]  F LEFT JOIN (SELECT [ModelCode],[Model] FROM [192.168.226.86].[dbSCM].[dbo].[PN_Compressor] WHERE [Status] = 'ACTIVE'
            //GROUP BY [ModelCode],[Model]) M ON M.[ModelCode] = SUBSTRING(serial,5,3)  WHERE Serial = @SERIAL ";
            cmd.CommandText = @"SELECT [ModelCode],[Model] 
                                FROM [192.168.226.86].[dbSCM].[dbo].[PN_Compressor] 
                                WHERE [Status] = 'ACTIVE' AND [ModelCode] = SUBSTRING(@SERIAL,5,3) 
                                GROUP BY [ModelCode],[Model] ";
            cmd.Parameters.Add(new SqlParameter("@SERIAL", serial));
            DataTable dt = conDBDCI.Query(cmd);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MSerial item = new MSerial();
                    item.SErial = serial;
                    item.MOdelCode = dr["ModelCode"].ToString();
                    item.MOdelName = dr["Model"].ToString();
                    item.LIne = "";
                    list.Add(item);
                }
            }
            return list;
        }

        internal MEmployee Login(string emId)
        {
            SqlCommand cmd = new SqlCommand();
            MEmployee result = new MEmployee();
            cmd.CommandText = @"SELECT * FROM [dbDCI].[dbo].[Employee] WHERE CODE = @EMID";
            cmd.Parameters.Add(new SqlParameter("@EMID", emId));
            DataTable dt = conDBDCI.Query(cmd);
            if (dt.Rows.Count > 0)
            {
                Properties.Settings.Default.EM_CODE = emId;
                foreach (DataRow dw in dt.Rows)
                {
                    result.COde = dw["CODE"].ToString();
                    result.TName = dw["TNAME"].ToString();
                    result.TSurn = dw["TSURN"].ToString();
                }
            }
            return result;
        }

        internal bool existSerial(string serial)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT ADJ_ID FROM " + _ADJ_SERIAL + " WHERE SERIAL_OLD = @SERIAL OR SERIAL_NEW = @SERIAL";
            cmd.Parameters.Add(new SqlParameter("@SERIAL", serial));
            DataTable dt = conDBSCM.Query(cmd);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal List<MSerial> getTransferLabel()
        {
            List<MSerial> list = new List<MSerial>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT TOP (20) * FROM " + _ADJ_SERIAL + " ORDER BY UPDATE_DT DESC";
            DataTable dt = conDBSCM.Query(cmd);
            foreach (DataRow dr in dt.Rows)
            {
                MSerial item = new MSerial();
                item.ADj_id = dr["ADJ_ID"] != System.DBNull.Value ? Convert.ToInt32(dr["ADJ_ID"]) : 0;
                item.SErial_old = dr["SERIAL_OLD"].ToString();
                item.MOdelCodeOld = dr["MODEL_CODE_OLD"].ToString();
                item.MOdelNameOld = dr["MODEL_NAME_OLD"].ToString();
                item.LIneOld = dr["LINE_OLD"].ToString();
                item.SErial_new = dr["SERIAL_NEW"].ToString();
                item.MOdelCodeNew = dr["MODEL_CODE_NEW"].ToString();
                item.MOdelNameNew = dr["MODEL_NAME_NEW"].ToString();
                item.LIneNew = dr["LINE_NEW"].ToString();
                try { item.UPdate_dt = Convert.ToDateTime(dr["UPDATE_DT"].ToString()); } catch { item.UPdate_dt = new DateTime(1900, 1, 1, 0, 0, 0); }
                item.ADj_status = dr["ADJ_STATUS"].ToString();
                list.Add(item);
            }
            return list;
        }


        internal List<MSummaryTransfer> getSummaryTransferByDate(DateTime _date)
        {

            List<MSummaryTransfer> aryData = new List<MSummaryTransfer>();
            DataTable dtData = new DataTable();
            string strSQL = @"SELECT [MODEL_CODE_OLD],[MODEL_NAME_OLD],[MODEL_CODE_NEW],[MODEL_NAME_NEW], COUNT([SERIAL_OLD]) cnt FROM [dbSCM].[dbo].[" + _ADJ_SERIAL + "] WHERE UPDATE_DT BETWEEN @dateStart AND  DATEADD(day, 1, @dateStart) GROUP BY [MODEL_CODE_OLD],[MODEL_NAME_OLD],[MODEL_CODE_NEW],[MODEL_NAME_NEW] ";
            SqlCommand cmdData = new SqlCommand();
            cmdData.CommandText = strSQL;
            cmdData.Parameters.Add(new SqlParameter("@dateStart", new DateTime(_date.Year, _date.Month, _date.Day, 8, 0, 0)));
            dtData = conDBSCM.Query(cmdData);

            if (dtData.Rows.Count > 0)
            {
                foreach (DataRow drData in dtData.Rows)
                {
                    MSummaryTransfer mItem = new MSummaryTransfer();
                    mItem.ModelCodeOld = drData["MODEL_CODE_OLD"].ToString();
                    mItem.ModelNameOld = drData["MODEL_NAME_OLD"].ToString();
                    mItem.ModelCodeNew = drData["MODEL_CODE_NEW"].ToString();
                    mItem.ModelNameNew = drData["MODEL_NAME_NEW"].ToString();
                    mItem.TransferQty = Convert.ToInt32(drData["cnt"].ToString());
                    aryData.Add(mItem);
                }
            }
            return aryData;

        }


        internal bool checkTransferMapping(string ModeCodeOld, string ModelCodeNew)
        {
            bool _isMapping = false;
            DataTable dtData = new DataTable();
            string strSQL = @"SELECT [MODEL_CODE_OLD],[MODEL_NAME_OLD],[MODEL_CODE_NEW],[MODEL_NAME_NEW]
                              FROM [dbSCM].[dbo].[ADJ_MAPPING] 
                              WHERE MODEL_CODE_OLD=@MODEL_CODE_OLD AND MODEL_CODE_NEW=@MODEL_CODE_NEW   ";
            SqlCommand cmdData = new SqlCommand();
            cmdData.CommandText = strSQL;
            cmdData.Parameters.Add(new SqlParameter("@MODEL_CODE_OLD", ModeCodeOld));
            cmdData.Parameters.Add(new SqlParameter("@MODEL_CODE_NEW", ModelCodeNew));
            dtData = conDBSCM.Query(cmdData);

            if (dtData.Rows.Count > 0)
            {
                _isMapping = true;
            }
            return _isMapping;

        }

        internal List<MSerial> getSerialHistoryBySerial(string serial)
        {
            List<MSerial> list = new List<MSerial>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT * FROM " + _ADJ_SERIAL + " WHERE SERIAL_OLD = @SERIAL OR SERIAL_NEW = @SERIAL ORDER BY ADJ_ID DESC";
            cmd.Parameters.Add(new SqlParameter("@SERIAL", serial));
            DataTable dt = conDBSCM.Query(cmd);
            foreach (DataRow dr in dt.Rows)
            {
                MSerial item = new MSerial();

                try { item.ADj_id = Convert.ToInt32(dr["ADJ_ID"]); } catch { item.ADj_id = 0; }
                item.SErial_old = dr["SERIAL_OLD"].ToString();
                item.MOdelCodeOld = dr["MODEL_CODE_OLD"].ToString();
                item.MOdelNameOld = dr["MODEL_NAME_OLD"].ToString();
                item.LIneOld = dr["LINE_OLD"].ToString();
                item.SErial_new = dr["SERIAL_NEW"].ToString();
                item.MOdelCodeNew = dr["MODEL_CODE_NEW"].ToString();
                item.MOdelNameNew = dr["MODEL_NAME_NEW"].ToString();
                item.LIneNew = dr["LINE_NEW"].ToString();
                try { item.UPdate_dt = Convert.ToDateTime(dr["UPDATE_DT"].ToString()); } catch { item.UPdate_dt = new DateTime(1900, 1, 1, 0, 0, 0); }
                item.ADj_status = dr["ADJ_STATUS"].ToString();
                list.Add(item);
            }
            return list;
        }

        internal bool removeSerial(string serialOld, string serialNew)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"DELETE FROM " + _ADJ_SERIAL + " WHERE SERIAL_OLD = @SERIAL_OLD AND SERIAL_NEW = @SERIAL_NEW";
            cmd.Parameters.Add(new SqlParameter("@SERIAL_OLD", serialOld));
            cmd.Parameters.Add(new SqlParameter("@SERIAL_NEW", serialNew));
            conDBSCM.ExecuteCommand(cmd);
            return true;
        }

        internal int insertAdjust(MSerial item)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"INSERT INTO " + _ADJ_SERIAL + "(SERIAL_OLD,MODEL_CODE_OLD,MODEL_NAME_OLD,LINE_OLD,SERIAL_NEW,MODEL_CODE_NEW,MODEL_NAME_NEW,LINE_NEW,UPDATE_DT,EM_CODE) VALUES (@SERIAL_OLD,@MODEL_CODE_OLD,@MODEL_NAME_OLD,@LINE_OLD,@SERIAL_NEW,@MODEL_CODE_NEW,@MODEL_NAME_NEW,@LINE_NEW,@UPDATE_DT,@EM_CODE)";
            cmd.Parameters.Add(new SqlParameter("@SERIAL_OLD", item.SErial_old));
            cmd.Parameters.Add(new SqlParameter("@MODEL_CODE_OLD", item.MOdelCodeOld.ToUpper()));
            cmd.Parameters.Add(new SqlParameter("@MODEL_NAME_OLD", item.MOdelNameOld));
            cmd.Parameters.Add(new SqlParameter("@LINE_OLD", item.LIneOld));
            cmd.Parameters.Add(new SqlParameter("@SERIAL_NEW", item.SErial_new));
            cmd.Parameters.Add(new SqlParameter("@MODEL_CODE_NEW", item.MOdelCodeNew.ToUpper()));
            cmd.Parameters.Add(new SqlParameter("@MODEL_NAME_NEW", item.MOdelNameNew));
            cmd.Parameters.Add(new SqlParameter("@LINE_NEW", item.LIneNew));
            cmd.Parameters.Add(new SqlParameter("@UPDATE_DT", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@EM_CODE", Properties.Settings.Default.EM_CODE));
            return conDBSCM.ExecuteNonQuery(cmd);
        }

        internal MSerial getLineByModel(string ModelCode)
        {
            MSerial item = new MSerial();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT Model,Line FROM[dbSCM].[dbo].[PN_Compressor] WHERE ModelCode = @MODEL_CODE";
            cmd.Parameters.Add(new SqlParameter("@MODEL_CODE", ModelCode));
            DataTable dt = conDBSCM.Query(cmd);
            foreach (DataRow dr in dt.Rows)
            {
                item.MOdelName = dr["Model"].ToString();
                item.LIne = dr["Line"].ToString();
            }
            return item;
        }

        internal List<string> listTbFn()
        {
            List<string> listTbFn = new List<string>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select table_name from dbDCI.INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'";
            DataTable dt = conDBDCI.Query(cmd);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["table_name"].ToString().Contains("FN_DataCenter"))
                {
                    if (dr["table_name"].ToString().Length >= 18)
                    {
                        listTbFn.Add(dr["table_name"].ToString().Substring(0, 18));
                    }
                    else if (dr["table_name"].ToString().Length == 13)
                    {
                        listTbFn.Add(dr["table_name"].ToString());
                    }
                    else
                    {
                        listTbFn.Add(dr["table_name"].ToString());
                    }
                }
            }
            IEnumerable<string> distinctAges = listTbFn.Distinct();
            List<string> listTb = new List<string>();
            foreach (string item in distinctAges)
            {

                if (item.Length > 13)
                {
                    if (item.Substring(13, 1).Equals("_"))
                    {
                        int myInt;
                        if (int.TryParse(item.Substring(14, 1), out myInt))
                        {
                            listTb.Add(item);
                        }

                    }
                }
                if (item.Length == 13)
                {
                    listTb.Add(item);
                }
            }
            listTb.Sort();
            listTb.Reverse();
            return listTb;
        }
    }
}
