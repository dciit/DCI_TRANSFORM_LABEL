namespace DCI_TRANSFER_SERIAR
{
    partial class frmSetting
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
            this.cbInsertFinalDataDit = new System.Windows.Forms.CheckBox();
            this.cbInsertFnDataCenter = new System.Windows.Forms.CheckBox();
            this.cbInsertFnCopyLabel = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbInsertFinalDataDit
            // 
            this.cbInsertFinalDataDit.AutoSize = true;
            this.cbInsertFinalDataDit.Location = new System.Drawing.Point(28, 27);
            this.cbInsertFinalDataDit.Name = "cbInsertFinalDataDit";
            this.cbInsertFinalDataDit.Size = new System.Drawing.Size(118, 17);
            this.cbInsertFinalDataDit.TabIndex = 0;
            this.cbInsertFinalDataDit.Text = "Insert FinalDataDIT";
            this.cbInsertFinalDataDit.UseVisualStyleBackColor = true;
            this.cbInsertFinalDataDit.CheckedChanged += new System.EventHandler(this.cbInsertFinalDataDit_CheckedChanged);
            // 
            // cbInsertFnDataCenter
            // 
            this.cbInsertFnDataCenter.AutoSize = true;
            this.cbInsertFnDataCenter.Location = new System.Drawing.Point(28, 54);
            this.cbInsertFnDataCenter.Name = "cbInsertFnDataCenter";
            this.cbInsertFnDataCenter.Size = new System.Drawing.Size(121, 17);
            this.cbInsertFnDataCenter.TabIndex = 1;
            this.cbInsertFnDataCenter.Text = "Insert FnDataCenter";
            this.cbInsertFnDataCenter.UseVisualStyleBackColor = true;
            this.cbInsertFnDataCenter.CheckedChanged += new System.EventHandler(this.cbInsertFnDataCenter_CheckedChanged);
            // 
            // cbInsertFnCopyLabel
            // 
            this.cbInsertFnCopyLabel.AutoSize = true;
            this.cbInsertFnCopyLabel.Location = new System.Drawing.Point(28, 79);
            this.cbInsertFnCopyLabel.Name = "cbInsertFnCopyLabel";
            this.cbInsertFnCopyLabel.Size = new System.Drawing.Size(135, 17);
            this.cbInsertFnCopyLabel.TabIndex = 2;
            this.cbInsertFnCopyLabel.Text = "Insert FnCopyLabelLog";
            this.cbInsertFnCopyLabel.UseVisualStyleBackColor = true;
            this.cbInsertFnCopyLabel.CheckedChanged += new System.EventHandler(this.cbInsertFnCopyLabel_CheckedChanged);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 122);
            this.Controls.Add(this.cbInsertFnCopyLabel);
            this.Controls.Add(this.cbInsertFnDataCenter);
            this.Controls.Add(this.cbInsertFinalDataDit);
            this.Name = "frmSetting";
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbInsertFinalDataDit;
        private System.Windows.Forms.CheckBox cbInsertFnDataCenter;
        private System.Windows.Forms.CheckBox cbInsertFnCopyLabel;
    }
}