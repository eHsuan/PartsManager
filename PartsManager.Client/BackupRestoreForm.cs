using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public class BackupRestoreForm : Form
    {
        private ApiClient _apiClient;
        private DataGridView _dgvBackups;
        private Label _lblStatus;
        private Button _btnRestore;
        private Button _btnBackup;
        private ProgressBar _progressBar;
        private Label _lblProgress;
        private System.Windows.Forms.Timer _progressTimer;

        public BackupRestoreForm()
        {
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
            InitializeComponents();
            UIStyle.Apply(this);
            LoadBackups();
        }

        private void InitializeComponents()
        {
            this.Text = LocalizationService.GetString("Menu_BackupRestore") ?? "備份與還原";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            _lblStatus = new Label
            {
                Text = LocalizationService.GetString("Lbl_LoadingBackups") ?? "載入中...",
                Location = new Point(20, 20),
                Size = new Size(540, 25),
                Font = new Font("Microsoft JhengHei", 12F)
            };
            this.Controls.Add(_lblStatus);

            _dgvBackups = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(540, 200),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.Black,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(_dgvBackups);

            _btnRestore = new Button
            {
                Text = LocalizationService.GetString("Btn_Restore") ?? "還原",
                Location = new Point(20, 270),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei", 12F)
            };
            _btnRestore.Click += BtnRestore_Click;
            this.Controls.Add(_btnRestore);

            _btnBackup = new Button
            {
                Text = LocalizationService.GetString("Btn_Backup") ?? "手動備份",
                Location = new Point(130, 270),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei", 12F)
            };
            _btnBackup.Click += BtnBackup_Click;
            this.Controls.Add(_btnBackup);

            _progressBar = new ProgressBar
            {
                Location = new Point(240, 275),
                Size = new Size(250, 25),
                Visible = false,
                Style = ProgressBarStyle.Continuous
            };
            this.Controls.Add(_progressBar);

            _lblProgress = new Label
            {
                Text = "0%",
                Location = new Point(500, 278),
                AutoSize = true,
                Visible = false,
                Font = new Font("Microsoft JhengHei", 10F)
            };
            this.Controls.Add(_lblProgress);

            _progressTimer = new System.Windows.Forms.Timer
            {
                Interval = 500
            };
            _progressTimer.Tick += ProgressTimer_Tick;
        }

        private async void LoadBackups()
        {
            try
            {
                var backups = await _apiClient.GetBackupsAsync();
                _dgvBackups.DataSource = backups.Select(b => new
                {
                    b.FolderId,
                    Name = b.FolderName,
                    Time = b.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToList();

                _dgvBackups.Columns["FolderId"].Visible = false;

                if (backups.Any())
                {
                    string lblLast = LocalizationService.GetString("Lbl_LastBackupTime") ?? "最後備份時間:";
                    _lblStatus.Text = $"{lblLast} {backups.First().CreatedTime:yyyy-MM-dd HH:mm:ss}";
                }
                else
                {
                    _lblStatus.Text = LocalizationService.GetString("Lbl_NoBackupFiles") ?? "雲端上沒有備份檔案。";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((LocalizationService.GetString("Msg_LoadBackupFailed") ?? "載入備份失敗: ") + ex.Message, LocalizationService.GetString("Common_Error") ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _lblStatus.Text = LocalizationService.GetString("Lbl_LoadFailed") ?? "載入失敗";
            }
        }

        private async void BtnRestore_Click(object sender, EventArgs e)
        {
            if (_dgvBackups.SelectedRows.Count == 0) return;

            string folderId = _dgvBackups.SelectedRows[0].Cells["FolderId"].Value.ToString();
            string msg = LocalizationService.GetString("Msg_AskBackupBeforeRestore") ?? "還原將會覆蓋現有資料，是否要在還原前先執行一次手動備份？(建議選是)";
            
            var result = MessageBox.Show(msg, LocalizationService.GetString("Common_Confirm") ?? "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return;

            _btnRestore.Enabled = false;
            _btnBackup.Enabled = false;
            
            if (result == DialogResult.Yes)
            {
                string oldStatus = _lblStatus.Text;
                _lblStatus.Text = "備份中，請稍候...";
                try
                {
                    await _apiClient.RunBackupAsync();
                    MessageBox.Show(LocalizationService.GetString("Msg_BackupSuccess") ?? "備份成功！即將開始還原", LocalizationService.GetString("Common_Success") ?? "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBackups();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{(LocalizationService.GetString("Msg_BackupFailed") ?? "備份失敗：")}{ex.Message}\n\n還原程序已中止。", LocalizationService.GetString("Common_Error") ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _lblStatus.Text = oldStatus;
                    _btnRestore.Enabled = true;
                    _btnBackup.Enabled = true;
                    return;
                }
                _lblStatus.Text = oldStatus;
            }

            _progressBar.Visible = true;
            _progressBar.Value = 0;
            _lblProgress.Visible = true;
            _lblProgress.Text = "0%";

            try
            {
                await _apiClient.RestoreBackupAsync(folderId);
                _progressTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show((LocalizationService.GetString("Msg_TriggerRestoreFailed") ?? "啟動還原失敗: ") + ex.Message, LocalizationService.GetString("Common_Error") ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _btnRestore.Enabled = true;
                _btnBackup.Enabled = true;
                _progressBar.Visible = false;
                _lblProgress.Visible = false;
            }
        }

        private async void BtnBackup_Click(object sender, EventArgs e)
        {
            string msg = LocalizationService.GetString("Msg_BackupConfirm") ?? "確定要執行手動備份嗎？";
            if (MessageBox.Show(msg, LocalizationService.GetString("Common_Confirm") ?? "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                _btnRestore.Enabled = false;
                _btnBackup.Enabled = false;
                string oldStatus = _lblStatus.Text;
                _lblStatus.Text = "備份中，請稍候...";

                try
                {
                    await _apiClient.RunBackupAsync();
                    MessageBox.Show(LocalizationService.GetString("Msg_BackupSuccess") ?? "備份成功！", LocalizationService.GetString("Common_Success") ?? "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBackups();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{(LocalizationService.GetString("Msg_BackupFailed") ?? "備份失敗：")}{ex.Message}", LocalizationService.GetString("Common_Error") ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _lblStatus.Text = oldStatus;
                }
                finally
                {
                    _btnRestore.Enabled = true;
                    _btnBackup.Enabled = true;
                }
            }
        }

        private async void ProgressTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var progress = await _apiClient.GetRestoreProgressAsync();
                
                _progressBar.Value = progress.Progress;
                _lblProgress.Text = $"{progress.Progress}% - {progress.Status}";

                if (progress.Status == "Completed")
                {
                    _progressTimer.Stop();
                    string successMsg = LocalizationService.GetString("Msg_RestoreSuccess") ?? "還原成功，請重新啟動系統";
                    MessageBox.Show(successMsg, LocalizationService.GetString("Common_Success") ?? "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                else if (progress.Status.StartsWith("Failed"))
                {
                    _progressTimer.Stop();
                    string failMsg = LocalizationService.GetString("Msg_RestoreFailed") ?? "還原失敗：";
                    MessageBox.Show($"{failMsg}{progress.Status}", LocalizationService.GetString("Common_Error") ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _btnRestore.Enabled = true;
                    _btnBackup.Enabled = true;
                }
            }
            catch
            {
                // Ignore transient errors
            }
        }
    }
}
