using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

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
            _pdfIcon = LocalizationService.GetPdfIcon();

            this.Load += MaterialCreationForm_Load;
        }

        public MaterialCreationForm(int materialId) : this()
        {
            _materialId = materialId;
            this.Text = LocalizationService.GetString("MaterialEditForm");
            btnSave.Text = LocalizationService.GetString("Btn_Save");
        }

        private async void MaterialCreationForm_Load(object sender, EventArgs e)
        {
            await LoadWarehousesAsync();

            if (_materialId.HasValue)
            {
                // 編輯模式下禁用期初庫存設定
                numInitialStock.Enabled = false;
                cmbInitialWarehouse.Enabled = false;

                try
                {
                    var material = await _apiClient.GetMaterialAsync(_materialId.Value);
                    if (material != null)
                    {
                        txtPartNo.Text = material.PartNo;
                        txtName.Text = material.Name;
                        txtSpec.Text = material.Specification;
                        txtSupplier.Text = material.Supplier;
                        txtManufacturer.Text = material.Manufacturer;
                        txtSafeStock.Text = material.SafeStockQty.ToString();
                        txtLeadTime.Text = material.LeadTimeDays.ToString();
                        txtPartNo.Enabled = false;

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

        private async System.Threading.Tasks.Task LoadWarehousesAsync()
        {
            try
            {
                var warehouses = await _apiClient.GetWarehousesAsync();
                cmbInitialWarehouse.DisplayMember = "WarehouseName";
                cmbInitialWarehouse.ValueMember = "WarehouseID";
                cmbInitialWarehouse.DataSource = warehouses;

                // 設定預設倉庫
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
                    // 檢查檔案大小 (限制 2MB = 2,097,152 Bytes)
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

            // 顯示已存在的附件
            foreach (var att in _existingAttachments)
            {
                DisplayAttachment(att.FileName, true);
            }

            // 顯示待上傳的附件
            foreach (var path in _pendingFiles)
            {
                DisplayAttachment(Path.GetFileName(path), false, path);
            }

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
            
            // 設定懸停提示檔名
            toolTipAttachment.SetToolTip(pb, fileName);

            pb.Click += async (s, e) => {
                if (isExisting) await OpenRemoteFile(_materialId.Value, fileName);
                else {
                    try {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(localPath) { UseShellExecute = true });
                    } catch (Exception ex) { 
                        MessageBox.Show(LocalizationService.GetString("Msg_CannotOpenFile") + ex.Message, 
                            LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    }
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
                if (isExisting)
                {
                    _existingAttachments.RemoveAll(a => a.FileName == fileName);
                    _filesToDelete.Add(fileName);
                }
                else
                {
                    _pendingFiles.Remove(localPath);
                }
                RefreshAttachmentPanel();
            };

            itemPnl.Controls.Add(btnDel); // 先加按鈕確保在最上層
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
            catch (Exception ex) 
            { 
                MessageBox.Show(LocalizationService.GetString("Msg_CannotOpenFile") + ex.Message,
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPartNo.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_PartNoRequired"));
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_NameRequired"));
                return;
            }

            if (!int.TryParse(txtSafeStock.Text, out int safeStock)) safeStock = 0;
            if (!int.TryParse(txtLeadTime.Text, out int leadTime)) leadTime = 0;

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
                        Supplier = txtSupplier.Text.Trim(),
                        Manufacturer = txtManufacturer.Text.Trim(),
                        SafeStockQty = safeStock,
                        LeadTimeDays = leadTime
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
                        Supplier = txtSupplier.Text.Trim(),
                        Manufacturer = txtManufacturer.Text.Trim(),
                        SafeStockQty = safeStock,
                        LeadTimeDays = leadTime,
                        InitialStock = numInitialStock.Value,
                        WarehouseId = (int?)cmbInitialWarehouse.SelectedValue,
                        SourceType = 1
                    };
                    var result = await _apiClient.CreateMaterialAsync(dto);
                    finalMaterialId = result.MaterialID;
                }

                // 處理附件刪除
                foreach (var fileName in _filesToDelete)
                {
                    await _apiClient.DeleteAttachmentAsync(finalMaterialId, fileName);
                }

                // 處理附件上傳
                if (_pendingFiles.Count > 0)
                {
                    await _apiClient.UploadAttachmentsAsync(finalMaterialId, _pendingFiles);
                }

                MessageBox.Show(LocalizationService.GetString("Msg_SaveSuccess"),
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_SaveError") + ex.Message,
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
