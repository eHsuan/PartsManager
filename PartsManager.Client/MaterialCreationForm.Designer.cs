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
            this.txtStation = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
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
            this.label6.Size = new System.Drawing.Size(102, 19);
            this.label6.TabIndex = 9;
            this.label6.Tag = "Label_Station";
            this.label6.Text = "站別 (Station)";
            // 
            // txtStation
            // 
            this.txtStation.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtStation.Location = new System.Drawing.Point(30, 282);
            this.txtStation.Name = "txtStation";
            this.txtStation.Size = new System.Drawing.Size(250, 29);
            this.txtStation.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(130, 340);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 40);
            this.btnSave.TabIndex = 6;
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
            this.btnCancel.Location = new System.Drawing.Point(290, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Tag = "Btn_Cancel";
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MaterialCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 410);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtStation);
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
        private System.Windows.Forms.TextBox txtStation;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
