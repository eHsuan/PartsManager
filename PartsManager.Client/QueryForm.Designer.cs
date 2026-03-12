namespace PartsManager.Client
{
    partial class QueryForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchKeyword = new System.Windows.Forms.TextBox();
            this.lblKeyword = new System.Windows.Forms.Label();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.Col_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_PartNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Station = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_SafeStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_LeadTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.ctxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnShowAll);
            this.panelTop.Controls.Add(this.btnSearch);
            this.panelTop.Controls.Add(this.txtSearchKeyword);
            this.panelTop.Controls.Add(this.lblKeyword);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(984, 70);
            this.panelTop.TabIndex = 0;
            // 
            // btnShowAll
            // 
            this.btnShowAll.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.btnShowAll.Location = new System.Drawing.Point(460, 19);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(120, 32);
            this.btnShowAll.TabIndex = 3;
            this.btnShowAll.Tag = "Btn_ShowAll";
            this.btnShowAll.Text = "顯示全部";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(340, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 32);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Tag = "Btn_Search";
            this.btnSearch.Text = "🔍 查詢";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.txtSearchKeyword.Location = new System.Drawing.Point(135, 21);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(185, 29);
            this.txtSearchKeyword.TabIndex = 1;
            this.txtSearchKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchKeyword_KeyDown);
            // 
            // lblKeyword
            // 
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Font = new System.Drawing.Font("Microsoft JhengHei", 11F);
            this.lblKeyword.Location = new System.Drawing.Point(12, 25);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new System.Drawing.Size(102, 19);
            this.lblKeyword.TabIndex = 0;
            this.lblKeyword.Tag = "Label_Keyword";
            this.lblKeyword.Text = "關鍵字搜尋：";
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            this.dgvResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_Name,
            this.Col_Spec,
            this.Col_PartNo,
            this.Col_Station,
            this.Col_Warehouse,
            this.Col_Qty,
            this.Col_SafeStock,
            this.Col_LeadTime});
            this.dgvResults.ContextMenuStrip = this.ctxMenu;
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(0, 70);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowTemplate.Height = 28;
            this.dgvResults.Size = new System.Drawing.Size(984, 491);
            this.dgvResults.TabIndex = 1;
            this.dgvResults.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvResults_CellMouseDown);
            // 
            // ctxMenu
            // 
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuDelete});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(101, 48);
            this.ctxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenu_Opening);
            // 
            // menuEdit
            // 
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(100, 22);
            this.menuEdit.Tag = "Menu_Edit";
            this.menuEdit.Text = "編輯";
            this.menuEdit.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(100, 22);
            this.menuDelete.Tag = "Menu_Delete";
            this.menuDelete.Text = "刪除";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // Col_Name
            // 
            this.Col_Name.DataPropertyName = "Name";
            this.Col_Name.HeaderText = "物料名稱";
            this.Col_Name.Name = "Col_Name";
            this.Col_Name.ReadOnly = true;
            this.Col_Name.Width = 200;
            // 
            // Col_Spec
            // 
            this.Col_Spec.DataPropertyName = "Specification";
            this.Col_Spec.HeaderText = "規格/型號";
            this.Col_Spec.Name = "Col_Spec";
            this.Col_Spec.ReadOnly = true;
            this.Col_Spec.Width = 200;
            // 
            // Col_PartNo
            // 
            this.Col_PartNo.DataPropertyName = "PartNo";
            this.Col_PartNo.HeaderText = "料號";
            this.Col_PartNo.Name = "Col_PartNo";
            this.Col_PartNo.ReadOnly = true;
            this.Col_PartNo.Width = 150;
            // 
            // Col_Station
            // 
            this.Col_Station.DataPropertyName = "Station";
            this.Col_Station.HeaderText = "站別 (Station)";
            this.Col_Station.Name = "Col_Station";
            this.Col_Station.ReadOnly = true;
            this.Col_Station.Width = 120;
            // 
            // Col_Warehouse
            // 
            this.Col_Warehouse.DataPropertyName = "WarehouseName";
            this.Col_Warehouse.HeaderText = "倉庫";
            this.Col_Warehouse.Name = "Col_Warehouse";
            this.Col_Warehouse.ReadOnly = true;
            this.Col_Warehouse.Width = 150;
            // 
            // Col_Qty
            // 
            this.Col_Qty.DataPropertyName = "Quantity";
            this.Col_Qty.HeaderText = "在庫庫存";
            this.Col_Qty.Name = "Col_Qty";
            this.Col_Qty.ReadOnly = true;
            // 
            // Col_SafeStock
            // 
            this.Col_SafeStock.DataPropertyName = "SafeStockQty";
            this.Col_SafeStock.HeaderText = "安全庫存";
            this.Col_SafeStock.Name = "Col_SafeStock";
            this.Col_SafeStock.ReadOnly = true;
            // 
            // Col_LeadTime
            // 
            this.Col_LeadTime.DataPropertyName = "LeadTimeDays";
            this.Col_LeadTime.HeaderText = "交期(天)";
            this.Col_LeadTime.Name = "Col_LeadTime";
            this.Col_LeadTime.ReadOnly = true;
            // 
            // QueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.panelTop);
            this.Name = "QueryForm";
            this.Tag = "QueryForm";
            this.Text = "物料庫存查詢";
            this.Load += new System.EventHandler(this.QueryForm_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ctxMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.TextBox txtSearchKeyword;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_PartNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Station;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_SafeStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_LeadTime;
    }
}
