namespace PartsManager.Client
{
    partial class InboundForm
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
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.btnManualInput = new System.Windows.Forms.Button();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.cmbWarehouse = new System.Windows.Forms.ComboBox();
            this.btnPrintLabel = new System.Windows.Forms.Button();
            this.btnInbound = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblMaterialName = new System.Windows.Forms.Label();
            this.lblSpecification = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtBarcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBarcode.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.txtBarcode.ForeColor = System.Drawing.Color.White;
            this.txtBarcode.Location = new System.Drawing.Point(45, 90);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(299, 50);
            this.txtBarcode.TabIndex = 0;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // btnManualInput
            // 
            this.btnManualInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnManualInput.FlatAppearance.BorderSize = 0;
            this.btnManualInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualInput.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnManualInput.ForeColor = System.Drawing.Color.White;
            this.btnManualInput.Location = new System.Drawing.Point(360, 90);
            this.btnManualInput.Margin = new System.Windows.Forms.Padding(4);
            this.btnManualInput.Name = "btnManualInput";
            this.btnManualInput.Size = new System.Drawing.Size(120, 54);
            this.btnManualInput.TabIndex = 1;
            this.btnManualInput.Tag = "Btn_ManualInput";
            this.btnManualInput.Text = "手動查詢";
            this.btnManualInput.UseVisualStyleBackColor = false;
            this.btnManualInput.Click += new System.EventHandler(this.btnManualInput_Click);
            // 
            // txtQty
            // 
            this.txtQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQty.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.txtQty.ForeColor = System.Drawing.Color.White;
            this.txtQty.Location = new System.Drawing.Point(45, 232);
            this.txtQty.Margin = new System.Windows.Forms.Padding(4);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(434, 50);
            this.txtQty.TabIndex = 2;
            this.txtQty.Text = "1";
            // 
            // cmbWarehouse
            // 
            this.cmbWarehouse.BackColor = System.Drawing.Color.White;
            this.cmbWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWarehouse.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmbWarehouse.ForeColor = System.Drawing.Color.Black;
            this.cmbWarehouse.FormattingEnabled = true;
            this.cmbWarehouse.Location = new System.Drawing.Point(45, 375);
            this.cmbWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.cmbWarehouse.Name = "cmbWarehouse";
            this.cmbWarehouse.Size = new System.Drawing.Size(433, 44);
            this.cmbWarehouse.TabIndex = 3;
            // 
            // btnPrintLabel
            // 
            this.btnPrintLabel.BackColor = System.Drawing.Color.DimGray;
            this.btnPrintLabel.Enabled = false;
            this.btnPrintLabel.FlatAppearance.BorderSize = 0;
            this.btnPrintLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintLabel.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPrintLabel.ForeColor = System.Drawing.Color.White;
            this.btnPrintLabel.Location = new System.Drawing.Point(45, 458);
            this.btnPrintLabel.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintLabel.Name = "btnPrintLabel";
            this.btnPrintLabel.Size = new System.Drawing.Size(435, 52);
            this.btnPrintLabel.TabIndex = 4;
            this.btnPrintLabel.Tag = "Btn_PrintLabel";
            this.btnPrintLabel.Text = "補印標籤 (Reprint)";
            this.btnPrintLabel.UseVisualStyleBackColor = false;
            this.btnPrintLabel.Click += new System.EventHandler(this.btnPrintLabel_Click);
            // 
            // btnInbound
            // 
            this.btnInbound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnInbound.FlatAppearance.BorderSize = 0;
            this.btnInbound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInbound.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnInbound.ForeColor = System.Drawing.Color.White;
            this.btnInbound.Location = new System.Drawing.Point(45, 532);
            this.btnInbound.Margin = new System.Windows.Forms.Padding(4);
            this.btnInbound.Name = "btnInbound";
            this.btnInbound.Size = new System.Drawing.Size(435, 75);
            this.btnInbound.TabIndex = 5;
            this.btnInbound.Tag = "Btn_Inbound";
            this.btnInbound.Text = "確認入庫 (INBOUND)";
            this.btnInbound.UseVisualStyleBackColor = false;
            this.btnInbound.Click += new System.EventHandler(this.btnInbound_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Lime;
            this.lblStatus.Location = new System.Drawing.Point(0, 600);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(30, 0, 0, 15);
            this.lblStatus.Size = new System.Drawing.Size(1225, 75);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Tag = "Status_Ready";
            this.lblStatus.Text = "Ready";
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaterialName.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Bold);
            this.lblMaterialName.ForeColor = System.Drawing.Color.White;
            this.lblMaterialName.Location = new System.Drawing.Point(45, 120);
            this.lblMaterialName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new System.Drawing.Size(1313, 60);
            this.lblMaterialName.TabIndex = 0;
            this.lblMaterialName.Text = "--";
            // 
            // lblSpecification
            // 
            this.lblSpecification.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpecification.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.lblSpecification.ForeColor = System.Drawing.Color.LightGray;
            this.lblSpecification.Location = new System.Drawing.Point(45, 240);
            this.lblSpecification.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpecification.Name = "lblSpecification";
            this.lblSpecification.Size = new System.Drawing.Size(1313, 300);
            this.lblSpecification.TabIndex = 1;
            this.lblSpecification.Text = "--";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(45, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 28);
            this.label1.TabIndex = 4;
            this.label1.Tag = "Label_ScanBarcode";
            this.label1.Text = "物料條碼 (Barcode)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(45, 195);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 28);
            this.label2.TabIndex = 5;
            this.label2.Tag = "Label_InboundQty";
            this.label2.Text = "入庫數量 (Quantity)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(45, 338);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(319, 28);
            this.label3.TabIndex = 6;
            this.label3.Tag = "Label_TargetWarehouse";
            this.label3.Text = "入庫倉庫 (Target Warehouse)";
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlLeft.Controls.Add(this.label1);
            this.pnlLeft.Controls.Add(this.txtBarcode);
            this.pnlLeft.Controls.Add(this.btnManualInput);
            this.pnlLeft.Controls.Add(this.label2);
            this.pnlLeft.Controls.Add(this.txtQty);
            this.pnlLeft.Controls.Add(this.label3);
            this.pnlLeft.Controls.Add(this.cmbWarehouse);
            this.pnlLeft.Controls.Add(this.btnPrintLabel);
            this.pnlLeft.Controls.Add(this.btnInbound);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(525, 675);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.pnlRight.Controls.Add(this.label4);
            this.pnlRight.Controls.Add(this.lblMaterialName);
            this.pnlRight.Controls.Add(this.label5);
            this.pnlRight.Controls.Add(this.lblSpecification);
            this.pnlRight.Controls.Add(this.lblStatus);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(525, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(4);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(1225, 675);
            this.pnlRight.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(45, 75);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(250, 30);
            this.label4.TabIndex = 3;
            this.label4.Tag = "Label_MaterialName";
            this.label4.Text = "品名 (Material Name)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(45, 195);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(228, 30);
            this.label5.TabIndex = 4;
            this.label5.Tag = "Label_Spec";
            this.label5.Text = "規格 (Specification)";
            // 
            // InboundForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1750, 675);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "InboundForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "InboundForm";
            this.Text = "零件管理系統 - 入庫作業";
            this.Load += new System.EventHandler(this.InboundForm_Load);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.Button btnInbound;
        private System.Windows.Forms.Button btnManualInput;
        private System.Windows.Forms.Button btnPrintLabel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblMaterialName;
        private System.Windows.Forms.Label lblSpecification;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
