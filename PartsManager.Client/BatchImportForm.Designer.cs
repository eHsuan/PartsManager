namespace PartsManager.Client
{
    partial class BatchImportForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnDownloadTemplate = new System.Windows.Forms.Button();
            this.btnImportFile = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDownloadTemplate
            // 
            this.btnDownloadTemplate.Location = new System.Drawing.Point(30, 20);
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.Size = new System.Drawing.Size(180, 50);
            this.btnDownloadTemplate.TabIndex = 0;
            this.btnDownloadTemplate.Tag = "Btn_DownloadTemplate";
            this.btnDownloadTemplate.Text = "產生匯入格式";
            this.btnDownloadTemplate.UseVisualStyleBackColor = true;
            this.btnDownloadTemplate.Click += new System.EventHandler(this.btnDownloadTemplate_Click);
            // 
            // btnImportFile
            // 
            this.btnImportFile.Location = new System.Drawing.Point(230, 20);
            this.btnImportFile.Name = "btnImportFile";
            this.btnImportFile.Size = new System.Drawing.Size(180, 50);
            this.btnImportFile.TabIndex = 1;
            this.btnImportFile.Tag = "Btn_ImportFile";
            this.btnImportFile.Text = "匯入物料檔案";
            this.btnImportFile.UseVisualStyleBackColor = true;
            this.btnImportFile.Click += new System.EventHandler(this.btnImportFile_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Location = new System.Drawing.Point(30, 120);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(740, 300);
            this.txtLog.TabIndex = 2;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(27, 95);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(101, 12);
            this.lblLog.TabIndex = 3;
            this.lblLog.Tag = "Label_ImportLog";
            this.lblLog.Text = "匯入執行訊息：";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft JhengHei", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Yellow;
            this.lblStatus.Location = new System.Drawing.Point(430, 35);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 18);
            this.lblStatus.TabIndex = 4;
            // 
            // BatchImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnImportFile);
            this.Controls.Add(this.btnDownloadTemplate);
            this.Name = "BatchImportForm";
            this.Tag = "BatchImportForm";
            this.Text = "批次新增物料";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDownloadTemplate;
        private System.Windows.Forms.Button btnImportFile;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Label lblStatus;
    }
}
