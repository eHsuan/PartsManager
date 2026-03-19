namespace PartsManager.Client
{
    partial class ProgressForm
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.pbLoading = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft JhengHei", 10F);
            this.lblMessage.Location = new System.Drawing.Point(25, 25);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(162, 18);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Please wait...";
            // 
            // pbLoading
            // 
            this.pbLoading.Location = new System.Drawing.Point(25, 55);
            this.pbLoading.MarqueeAnimationSpeed = 30;
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(300, 23);
            this.pbLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbLoading.TabIndex = 1;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 110);
            this.ControlBox = false;
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Processing...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ProgressBar pbLoading;
    }
}
