namespace DCI_TRANSFER_SERIAR
{
    partial class frmUploadExcel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUploadExcel));
            this.btnUpload = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnPreviewExcel = new System.Windows.Forms.Button();
            this.tbFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectExcel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtShowCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SerialOld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelCodeOld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelNameOld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineOld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SerialNew = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelCodeNew = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModelNameNew = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineNew = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnUpload.Enabled = false;
            this.btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnUpload.ForeColor = System.Drawing.Color.DimGray;
            this.btnUpload.Location = new System.Drawing.Point(1012, 422);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(162, 47);
            this.btnUpload.TabIndex = 9;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SerialOld,
            this.ModelCodeOld,
            this.ModelNameOld,
            this.LineOld,
            this.SerialNew,
            this.ModelCodeNew,
            this.ModelNameNew,
            this.LineNew,
            this.ColMessage});
            this.dataGridView1.Location = new System.Drawing.Point(12, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.Size = new System.Drawing.Size(1162, 267);
            this.dataGridView1.TabIndex = 8;
            // 
            // btnPreviewExcel
            // 
            this.btnPreviewExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnPreviewExcel.Location = new System.Drawing.Point(12, 62);
            this.btnPreviewExcel.Name = "btnPreviewExcel";
            this.btnPreviewExcel.Size = new System.Drawing.Size(136, 39);
            this.btnPreviewExcel.TabIndex = 7;
            this.btnPreviewExcel.Text = "Preview";
            this.btnPreviewExcel.UseVisualStyleBackColor = true;
            this.btnPreviewExcel.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbFilePath
            // 
            this.tbFilePath.BackColor = System.Drawing.Color.White;
            this.tbFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.tbFilePath.Location = new System.Drawing.Point(12, 22);
            this.tbFilePath.Multiline = true;
            this.tbFilePath.Name = "tbFilePath";
            this.tbFilePath.ReadOnly = true;
            this.tbFilePath.Size = new System.Drawing.Size(453, 32);
            this.tbFilePath.TabIndex = 6;
            // 
            // btnSelectExcel
            // 
            this.btnSelectExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSelectExcel.Location = new System.Drawing.Point(481, 21);
            this.btnSelectExcel.Name = "btnSelectExcel";
            this.btnSelectExcel.Size = new System.Drawing.Size(130, 34);
            this.btnSelectExcel.TabIndex = 5;
            this.btnSelectExcel.Text = "Browse...";
            this.btnSelectExcel.UseVisualStyleBackColor = true;
            this.btnSelectExcel.Click += new System.EventHandler(this.btnSelectExcel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.txtShowCount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 375);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1162, 41);
            this.panel1.TabIndex = 10;
            // 
            // txtShowCount
            // 
            this.txtShowCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtShowCount.Location = new System.Drawing.Point(970, 0);
            this.txtShowCount.Name = "txtShowCount";
            this.txtShowCount.Size = new System.Drawing.Size(192, 40);
            this.txtShowCount.TabIndex = 1;
            this.txtShowCount.Text = "-";
            this.txtShowCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(806, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "รวมจำนวน";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SerialOld
            // 
            this.SerialOld.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SerialOld.HeaderText = "SERIAL (เก่า)";
            this.SerialOld.Name = "SerialOld";
            this.SerialOld.ReadOnly = true;
            this.SerialOld.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ModelCodeOld
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ModelCodeOld.DefaultCellStyle = dataGridViewCellStyle1;
            this.ModelCodeOld.HeaderText = "MODEL CODE (เก่า)";
            this.ModelCodeOld.Name = "ModelCodeOld";
            this.ModelCodeOld.ReadOnly = true;
            // 
            // ModelNameOld
            // 
            this.ModelNameOld.HeaderText = "MODEL NAME (เก่า)";
            this.ModelNameOld.Name = "ModelNameOld";
            this.ModelNameOld.ReadOnly = true;
            // 
            // LineOld
            // 
            this.LineOld.HeaderText = "LINE (เก่า)";
            this.LineOld.Name = "LineOld";
            this.LineOld.ReadOnly = true;
            // 
            // SerialNew
            // 
            this.SerialNew.HeaderText = "SERIAL (ใหม่)";
            this.SerialNew.Name = "SerialNew";
            this.SerialNew.ReadOnly = true;
            // 
            // ModelCodeNew
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ModelCodeNew.DefaultCellStyle = dataGridViewCellStyle2;
            this.ModelCodeNew.HeaderText = "MODEL CODE (ใหม่)";
            this.ModelCodeNew.Name = "ModelCodeNew";
            this.ModelCodeNew.ReadOnly = true;
            // 
            // ModelNameNew
            // 
            this.ModelNameNew.HeaderText = "MODE NAME (ใหม่)";
            this.ModelNameNew.Name = "ModelNameNew";
            this.ModelNameNew.ReadOnly = true;
            // 
            // LineNew
            // 
            this.LineNew.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LineNew.HeaderText = "LINE (ใหม่)";
            this.LineNew.Name = "LineNew";
            this.LineNew.ReadOnly = true;
            // 
            // ColMessage
            // 
            this.ColMessage.HeaderText = "แจ้งเตือน";
            this.ColMessage.Name = "ColMessage";
            this.ColMessage.ReadOnly = true;
            // 
            // frmUploadExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 477);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnPreviewExcel);
            this.Controls.Add(this.tbFilePath);
            this.Controls.Add(this.btnSelectExcel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUploadExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upload Excel";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnPreviewExcel;
        private System.Windows.Forms.TextBox tbFilePath;
        private System.Windows.Forms.Button btnSelectExcel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label txtShowCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialOld;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelCodeOld;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelNameOld;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineOld;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelCodeNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModelNameNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMessage;
    }
}