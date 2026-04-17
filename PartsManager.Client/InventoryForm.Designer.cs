namespace PartsManager.Client
{
    partial class InventoryForm
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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblCountInfo = new System.Windows.Forms.Label();
            this.btnCompare = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbWarehouse = new System.Windows.Forms.ComboBox();
            this.lblSpec = new System.Windows.Forms.Label();
            this.lblMaterialName = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.lblBarcodeHint = new System.Windows.Forms.Label();
            this.dgvScanned = new System.Windows.Forms.DataGridView();
            this.colPartNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScanned)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.pnlLeft.Controls.Add(this.lblCountInfo);
            this.pnlLeft.Controls.Add(this.btnCompare);
            this.pnlLeft.Controls.Add(this.label1);
            this.pnlLeft.Controls.Add(this.cmbWarehouse);
            this.pnlLeft.Controls.Add(this.lblSpec);
            this.pnlLeft.Controls.Add(this.lblMaterialName);
            this.pnlLeft.Controls.Add(this.txtBarcode);
            this.pnlLeft.Controls.Add(this.lblBarcodeHint);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(350, 600);
            this.pnlLeft.TabIndex = 0;
            // 
            // lblCountInfo
            // 
            this.lblCountInfo.AutoSize = true;
            this.lblCountInfo.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblCountInfo.ForeColor = System.Drawing.Color.Lime;
            this.lblCountInfo.Location = new System.Drawing.Point(20, 400);
            this.lblCountInfo.Name = "lblCountInfo";
            this.lblCountInfo.Size = new System.Drawing.Size(135, 25);
            this.lblCountInfo.TabIndex = 7;
            this.lblCountInfo.Text = "已掃描: 0 項目";
            // 
            // btnCompare
            // 
            this.btnCompare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompare.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.btnCompare.ForeColor = System.Drawing.Color.White;
            this.btnCompare.Location = new System.Drawing.Point(20, 480);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(310, 80);
            this.btnCompare.TabIndex = 6;
            this.btnCompare.Tag = "Btn_GenerateReport";
            this.btnCompare.Text = "產出差異報告";
            this.btnCompare.UseVisualStyleBackColor = false;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(20, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 22);
            this.label1.TabIndex = 5;
            this.label1.Tag = "Label_TargetWarehouse";
            this.label1.Text = "盤點倉庫 (Warehouse)";
            // 
            // cmbWarehouse
            // 
            this.cmbWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWarehouse.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.cmbWarehouse.FormattingEnabled = true;
            this.cmbWarehouse.Location = new System.Drawing.Point(20, 275);
            this.cmbWarehouse.Name = "cmbWarehouse";
            this.cmbWarehouse.Size = new System.Drawing.Size(310, 33);
            this.cmbWarehouse.TabIndex = 4;
            // 
            // lblSpec
            // 
            this.lblSpec.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.lblSpec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblSpec.Location = new System.Drawing.Point(20, 180);
            this.lblSpec.Name = "lblSpec";
            this.lblSpec.Size = new System.Drawing.Size(310, 50);
            this.lblSpec.TabIndex = 3;
            this.lblSpec.Text = "---";
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblMaterialName.ForeColor = System.Drawing.Color.Yellow;
            this.lblMaterialName.Location = new System.Drawing.Point(20, 120);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new System.Drawing.Size(310, 60);
            this.lblMaterialName.TabIndex = 2;
            this.lblMaterialName.Text = "---";
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txtBarcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBarcode.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.txtBarcode.ForeColor = System.Drawing.Color.White;
            this.txtBarcode.Location = new System.Drawing.Point(20, 60);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(310, 43);
            this.txtBarcode.TabIndex = 1;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // lblBarcodeHint
            // 
            this.lblBarcodeHint.AutoSize = true;
            this.lblBarcodeHint.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lblBarcodeHint.ForeColor = System.Drawing.Color.White;
            this.lblBarcodeHint.Location = new System.Drawing.Point(20, 30);
            this.lblBarcodeHint.Name = "lblBarcodeHint";
            this.lblBarcodeHint.Size = new System.Drawing.Size(236, 25);
            this.lblBarcodeHint.TabIndex = 0;
            this.lblBarcodeHint.Tag = "Label_ScanBarcode";
            this.lblBarcodeHint.Text = "掃描條碼 (Scan Barcode)";
            // 
            // dgvScanned
            // 
            this.dgvScanned.AllowUserToAddRows = false;
            this.dgvScanned.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvScanned.BackgroundColor = System.Drawing.Color.White;
            this.dgvScanned.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvScanned.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScanned.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPartNo,
            this.colName,
            this.colQty});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvScanned.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvScanned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvScanned.Location = new System.Drawing.Point(350, 0);
            this.dgvScanned.Name = "dgvScanned";
            this.dgvScanned.ReadOnly = true;
            this.dgvScanned.RowHeadersVisible = false;
            this.dgvScanned.RowTemplate.Height = 35;
            this.dgvScanned.Size = new System.Drawing.Size(650, 600);
            this.dgvScanned.TabIndex = 1;
            // 
            // colPartNo
            // 
            this.colPartNo.FillWeight = 150F;
            this.colPartNo.HeaderText = "零件編號";
            this.colPartNo.Name = "colPartNo";
            this.colPartNo.ReadOnly = true;
            this.colPartNo.Tag = "Col_PartNo";
            // 
            // colName
            // 
            this.colName.FillWeight = 200F;
            this.colName.HeaderText = "品名";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Tag = "Col_Name";
            // 
            // colQty
            // 
            this.colQty.FillWeight = 80F;
            this.colQty.HeaderText = "數量";
            this.colQty.Name = "colQty";
            this.colQty.ReadOnly = true;
            this.colQty.Tag = "Col_Qty";
            // 
            // InventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.dgvScanned);
            this.Controls.Add(this.pnlLeft);
            this.Name = "InventoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "InventoryForm";
            this.Text = "盤點功能";
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScanned)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label lblBarcodeHint;
        private System.Windows.Forms.Label lblSpec;
        private System.Windows.Forms.Label lblMaterialName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.DataGridView dgvScanned;
        private System.Windows.Forms.Label lblCountInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPartNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
    }
}
