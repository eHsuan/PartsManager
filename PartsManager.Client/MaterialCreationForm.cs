using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;
using PartsManager.Client.Services;

namespace PartsManager.Client
{
    public partial class MaterialCreationForm : Form
    {
        private readonly ApiClient _apiClient;
        private int? _materialId = null;
        private List<string> _pendingFiles = new List<string>(); // 待上傳的本地路徑
        private List<AttachmentDto> _existingAttachments = new List<AttachmentDto>(); // 已存在於伺服器的附件
        private List<string> _filesToDelete = new List<string>(); // 待從伺服器刪除的檔名
        private Image _pdfIcon;

        public MaterialCreationForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);

            // 從資源檔載入嵌入的 PDF 圖示
            _pdfIcon = ClientResources.PdfIcon;

            this.Load += MaterialCreationForm_Load;
        }

        public MaterialCreationForm(int materialId) : this()
        {
            _materialId = materialId;
            this.Tag = "MaterialEditForm"; // 修改 Tag 以觸發編輯模式標題翻譯
            I18nHelper.Apply(this); // 再次套用，切換標題
        }

        private async void MaterialCreationForm_Load(object sender, EventArgs e)
        {
            await LoadWarehousesAsync();
            await LoadMachinesAsync();

            if (_materialId.HasValue)
            {
                // 編輯模式：允許校正庫存
                numInitialStock.Enabled = true;
                cmbInitialWarehouse.Enabled = true;

                try
                {
                    var material = await _apiClient.GetMaterialAsync(_materialId.Value);
                    if (material != null)
                    {
                        txtPartNo.Text = material.PartNo;
                        txtName.Text = material.Name;
                        txtSpec.Text = material.Specification;
                        txtStorageLocation.Text = material.StorageLocation;
                        numSafeStock.Value = material.SafeStockQty;
                        numLeadTime.Value = material.LeadTimeDays;
                        numPrice.Value = material.Price;
                        txtPartNo.Enabled = false;
                        btnGenTempPartNo.Enabled = false;

                        // 顯示庫存與倉庫
                        numInitialStock.Value = material.CurrentStock;
                        if (material.WarehouseId.HasValue)
                        {
                            cmbInitialWarehouse.SelectedValue = material.WarehouseId.Value;
                        }

                        // 回顯機台
                        cmbMachine.SelectedValue = material.MachineID;

                        // 載入附件清單
                        _existingAttachments = await _apiClient.GetAttachmentsAsync(_materialId.Value);
                        RefreshAttachmentPanel();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_LoadMaterialError") + ex.Message,
                        LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private void btnGenTempPartNo_Click(object sender, EventArgs e)
        {
            // 優先從選中的機台獲取前綴，否則使用 TMP
            string prefix = "TMP";
            if (cmbMachine.SelectedItem is MachineDto selectedMachine && selectedMachine.MachineID > 0)
            {
                prefix = selectedMachine.MachineCode.Trim();
            }

            // 總長度 13 位。日期部分長度 = 13 - 前綴長度
            int datePartLength = 13 - prefix.Length;
            string datePart = "";
            if (datePartLength > 0)
            {
                string fullDateStr = DateTime.Now.ToString("yyMMddHHmm");
                datePart = fullDateStr.Substring(0, Math.Min(datePartLength, fullDateStr.Length));
            }

            txtPartNo.Text = prefix + datePart;
            
            MessageBox.Show(LocalizationService.GetString("Msg_TempPartNoGenerated"), 
                LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async System.Threading.Tasks.Task LoadMachinesAsync()
        {
            try
            {
                var machines = await _apiClient.GetMachinesAsync();
                var machineList = new List<MachineDto>();
                machineList.Add(new MachineDto { MachineID = 0, MachineCode = "---", MachineName = "--- None ---" });
                machineList.AddRange(machines);

                cmbMachine.DisplayMember = "MachineName";
                cmbMachine.ValueMember = "MachineID";
                cmbMachine.DataSource = machineList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Machines Error: " + ex.Message);
            }
        }

        private async System.Threading.Tasks.Task LoadWarehousesAsync()
        {
            try
            {
                var warehouses = await _apiClient.GetWarehousesAsync();
                cmbInitialWarehouse.DisplayMember = "WarehouseName";
                cmbInitialWarehouse.ValueMember = "WarehouseID";
                cmbInitialWarehouse.DataSource = warehouses;

                int defaultWhId = GlobalSettings.DefaultWarehouseId;
                cmbInitialWarehouse.SelectedValue = defaultWhId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_LoadWarehouseError") + ex.Message);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            int currentTotal = _existingAttachments.Count + _pendingFiles.Count;
            if (currentTotal >= 2)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_MaxAttachments"), 
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files (*.pdf)|*.pdf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    if (fi.Length > 2 * 1024 * 1024)
                    {
                        MessageBox.Show(LocalizationService.GetString("Msg_FileSizeExceeded"),
                            LocalizationService.GetString("Common_Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _pendingFiles.Add(ofd.FileName);
                    RefreshAttachmentPanel();
                }
            }
        }

        private void RefreshAttachmentPanel()
        {
            pnlAttachments.Controls.Clear();
            foreach (var att in _existingAttachments) DisplayAttachment(att.FileName, true);
            foreach (var path in _pendingFiles) DisplayAttachment(Path.GetFileName(path), false, path);
            btnUpload.Enabled = (_existingAttachments.Count + _pendingFiles.Count) < 2;
        }

        private void DisplayAttachment(string fileName, bool isExisting, string localPath = null)
        {
            Panel itemPnl = new Panel { Size = new Size(60, 60), Padding = new Padding(2) };
            PictureBox pb = new PictureBox
            {
                Image = _pdfIcon,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(50, 50),
                Location = new Point(5, 5),
                Cursor = Cursors.Hand
            };
            toolTipAttachment.SetToolTip(pb, fileName);

            pb.Click += async (s, e) => {
                if (isExisting) await OpenRemoteFile(_materialId.Value, fileName);
                else {
                    try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(localPath) { UseShellExecute = true }); }
                    catch (Exception ex) { MessageBox.Show(LocalizationService.GetString("Msg_CannotOpenFile") + ex.Message); }
                }
            };

            Button btnDel = new Button
            {
                Text = "×",
                Size = new Size(18, 18),
                Location = new Point(40, 2),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            btnDel.FlatAppearance.BorderSize = 0;
            btnDel.Click += (s, e) => {
                if (isExisting) {
                    _existingAttachments.RemoveAll(a => a.FileName == fileName);
                    _filesToDelete.Add(fileName);
                } else _pendingFiles.Remove(localPath);
                RefreshAttachmentPanel();
            };

            itemPnl.Controls.Add(btnDel);
            itemPnl.Controls.Add(pb);
            btnDel.BringToFront();
            pnlAttachments.Controls.Add(itemPnl);
        }

        private async System.Threading.Tasks.Task OpenRemoteFile(int materialId, string fileName)
        {
            try
            {
                byte[] data = await _apiClient.DownloadAttachmentAsync(materialId, fileName);
                if (data != null)
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), fileName);
                    File.WriteAllBytes(tempPath, data);
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath) { UseShellExecute = true });
                }
            }
            catch (Exception ex) { MessageBox.Show(LocalizationService.GetString("Msg_CannotOpenFile") + ex.Message); }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPartNo.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_PartNoRequired") + " / " + LocalizationService.GetString("Msg_NameRequired"));
                return;
            }

            int selectedMachineId = (int)(cmbMachine.SelectedValue ?? 0);
            if (selectedMachineId == 0)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_MachineRequired"));
                return;
            }

            btnSave.Enabled = false;

            try
            {
                int finalMaterialId;
                if (_materialId.HasValue)
                {
                    finalMaterialId = _materialId.Value;
                    var dto = new UpdateMaterialDto
                    {
                        PartNo = txtPartNo.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Specification = txtSpec.Text.Trim(),
                        StorageLocation = txtStorageLocation.Text.Trim(),
                        SafeStockQty = (int)numSafeStock.Value,
                        LeadTimeDays = (int)numLeadTime.Value,
                        Price = numPrice.Value,
                        MachineID = selectedMachineId,
                        CurrentStock = numInitialStock.Value,
                        WarehouseId = (int?)cmbInitialWarehouse.SelectedValue,
                        OperatorID = UserSession.Username ?? "SYSTEM"
                    };
                    await _apiClient.UpdateMaterialAsync(finalMaterialId, dto);
                }
                else
                {
                    var dto = new CreateMaterialDto
                    {
                        PartNo = txtPartNo.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Specification = txtSpec.Text.Trim(),
                        StorageLocation = txtStorageLocation.Text.Trim(),
                        SafeStockQty = (int)numSafeStock.Value,
                        LeadTimeDays = (int)numLeadTime.Value,
                        Price = numPrice.Value,
                        InitialStock = numInitialStock.Value,
                        WarehouseId = (int?)cmbInitialWarehouse.SelectedValue,
                        SourceType = 1,
                        MachineID = selectedMachineId,
                        OperatorID = UserSession.Username ?? "SYSTEM"
                    };
                    var result = await _apiClient.CreateMaterialAsync(dto);
                    finalMaterialId = result.MaterialID;
                }

                foreach (var fileName in _filesToDelete) await _apiClient.DeleteAttachmentAsync(finalMaterialId, fileName);
                if (_pendingFiles.Count > 0) await _apiClient.UploadAttachmentsAsync(finalMaterialId, _pendingFiles);

                MessageBox.Show(LocalizationService.GetString("Msg_SaveSuccess"));

                if (GlobalSettings.EnableLabelPrinting)
                {
                    if (MessageBox.Show(LocalizationService.GetString("Dialog_AskPrintLabel"), "Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        LabelPrinterService.PrintLabel(txtPartNo.Text.Trim(), txtName.Text.Trim());
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_SaveError") + ex.Message);
            }
            finally { btnSave.Enabled = true; }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}
