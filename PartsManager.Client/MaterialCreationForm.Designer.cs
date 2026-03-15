namespace PartsManager.Client
{
    partial class MaterialCreationForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPartNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSafeStock = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLeadTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSupplier = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtManufacturer = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numInitialStock = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbInitialWarehouse = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpAttachments = new System.Windows.Forms.GroupBox();
            this.pnlAttachments = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.toolTipAttachment = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numInitialStock)).BeginInit();
            this.grpAttachments.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label1.Location = new System.Drawing.Point(30, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 19);
            this.label1.TabIndex = 0;
            this.label1.Tag = "Label_PartNo";
            this.label1.Text = "料號 (Part No)";
            // 
            // txtPartNo
            // 
            this.txtPartNo.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtPartNo.Location = new System.Drawing.Point(30, 42);
            this.txtPartNo.Name = "txtPartNo";
            this.txtPartNo.Size = new System.Drawing.Size(250, 26);
            this.txtPartNo.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label2.Location = new System.Drawing.Point(30, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 19);
            this.label2.TabIndex = 2;
            this.label2.Tag = "Label_Name";
            this.label2.Text = "名稱 (Name)";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtName.Location = new System.Drawing.Point(30, 102);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(420, 29);
            this.txtName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label3.Location = new System.Drawing.Point(30, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 19);
            this.label3.TabIndex = 4;
            this.label3.Tag = "Label_Specification";
            this.label3.Text = "規格 (Specification)";
            // 
            // txtSpec
            // 
            this.txtSpec.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtSpec.Location = new System.Drawing.Point(30, 162);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(420, 29);
            this.txtSpec.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label4.Location = new System.Drawing.Point(30, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 19);
            this.label4.TabIndex = 6;
            this.label4.Tag = "Label_SafeStock";
            this.label4.Text = "安全庫存 (Safe Stock)";
            // 
            // txtSafeStock
            // 
            this.txtSafeStock.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtSafeStock.Location = new System.Drawing.Point(30, 222);
            this.txtSafeStock.Name = "txtSafeStock";
            this.txtSafeStock.Size = new System.Drawing.Size(150, 26);
            this.txtSafeStock.TabIndex = 3;
            this.txtSafeStock.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label5.Location = new System.Drawing.Point(210, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 19);
            this.label5.TabIndex = 8;
            this.label5.Tag = "Label_LeadTime";
            this.label5.Text = "交期天數 (Lead Days)";
            // 
            // txtLeadTime
            // 
            this.txtLeadTime.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtLeadTime.Location = new System.Drawing.Point(210, 222);
            this.txtLeadTime.Name = "txtLeadTime";
            this.txtLeadTime.Size = new System.Drawing.Size(150, 26);
            this.txtLeadTime.TabIndex = 4;
            this.txtLeadTime.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label6.Location = new System.Drawing.Point(30, 260);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 19);
            this.label6.TabIndex = 10;
            this.label6.Tag = "Label_Supplier";
            this.label6.Text = "供應商 (Supplier)";
            // 
            // txtSupplier
            // 
            this.txtSupplier.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtSupplier.Location = new System.Drawing.Point(30, 282);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(420, 29);
            this.txtSupplier.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label7.Location = new System.Drawing.Point(30, 320);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 19);
            this.label7.TabIndex = 12;
            this.label7.Tag = "Label_Manufacturer";
            this.label7.Text = "製造商 (Manufacturer)";
            // 
            // txtManufacturer
            // 
            this.txtManufacturer.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtManufacturer.Location = new System.Drawing.Point(30, 342);
            this.txtManufacturer.Name = "txtManufacturer";
            this.txtManufacturer.Size = new System.Drawing.Size(420, 29);
            this.txtManufacturer.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label8.Location = new System.Drawing.Point(30, 385);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 19);
            this.label8.TabIndex = 14;
            this.label8.Tag = "Label_InitialStock";
            this.label8.Text = "期初在庫數量";
            // 
            // numInitialStock
            // 
            this.numInitialStock.DecimalPlaces = 2;
            this.numInitialStock.Font = new System.Drawing.Font("Consolas", 12F);
            this.numInitialStock.Location = new System.Drawing.Point(30, 407);
            this.numInitialStock.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numInitialStock.Name = "numInitialStock";
            this.numInitialStock.Size = new System.Drawing.Size(150, 26);
            this.numInitialStock.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.label9.Location = new System.Drawing.Point(210, 385);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 19);
            this.label9.TabIndex = 16;
            this.label9.Tag = "Label_InitialWarehouse";
            this.label9.Text = "存放倉庫";
            // 
            // cmbInitialWarehouse
            // 
            this.cmbInitialWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInitialWarehouse.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.cmbInitialWarehouse.FormattingEnabled = true;
            this.cmbInitialWarehouse.Location = new System.Drawing.Point(210, 407);
            this.cmbInitialWarehouse.Name = "cmbInitialWarehouse";
            this.cmbInitialWarehouse.Size = new System.Drawing.Size(240, 27);
            this.cmbInitialWarehouse.TabIndex = 8;
            // 
            // grpAttachments
            // 
            this.grpAttachments.Controls.Add(this.pnlAttachments);
            this.grpAttachments.Controls.Add(this.btnUpload);
            this.grpAttachments.Font = new System.Drawing.Font("Microsoft JhengHei", 10F);
            this.grpAttachments.ForeColor = System.Drawing.Color.White;
            this.grpAttachments.Location = new System.Drawing.Point(30, 450);
            this.grpAttachments.Name = "grpAttachments";
            this.grpAttachments.Size = new System.Drawing.Size(420, 110);
            this.grpAttachments.TabIndex = 13;
            this.grpAttachments.TabStop = false;
            this.grpAttachments.Tag = "Label_Attachments";
            this.grpAttachments.Text = "附件 (Attachments - Max 2 PDF)";
            // 
            // pnlAttachments
            // 
            this.pnlAttachments.AutoScroll = true;
            this.pnlAttachments.Location = new System.Drawing.Point(110, 20);
            this.pnlAttachments.Name = "pnlAttachments";
            this.pnlAttachments.Size = new System.Drawing.Size(304, 84);
            this.pnlAttachments.TabIndex = 1;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(10, 30);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(90, 60);
            this.btnUpload.TabIndex = 0;
            this.btnUpload.Tag = "Btn_UploadPDF";
            this.btnUpload.Text = "Upload PDF";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(130, 580);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 40);
            this.btnSave.TabIndex = 9;
            this.btnSave.Tag = "Btn_Save";
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(290, 580);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Tag = "Btn_Cancel";
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MaterialCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 640);
            this.Controls.Add(this.cmbInitialWarehouse);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numInitialStock);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.grpAttachments);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtManufacturer);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLeadTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSafeStock);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSpec);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPartNo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialCreationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "MaterialCreationForm";
            this.Text = "建立物料資訊 (New Material)";
            this.grpAttachments.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPartNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSafeStock;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLeadTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSupplier;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtManufacturer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numInitialStock;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbInitialWarehouse;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpAttachments;
        private System.Windows.Forms.FlowLayoutPanel pnlAttachments;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.ToolTip toolTipAttachment;
    }
}
