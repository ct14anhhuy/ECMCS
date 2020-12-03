
namespace ECMCS.App
{
    partial class frmUpdateVersion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdateVersion));
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.txtCurrentVersion = new MetroFramework.Controls.MetroTextBox();
            this.txtNextVersion = new MetroFramework.Controls.MetroTextBox();
            this.rdUpdateNextVersion = new MetroFramework.Controls.MetroRadioButton();
            this.rdOverrideVersion = new MetroFramework.Controls.MetroRadioButton();
            this.btnConfirm = new MetroFramework.Controls.MetroButton();
            this.btnClose = new MetroFramework.Controls.MetroButton();
            this.txtFileName = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 131);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(98, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Current version";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(40, 172);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(81, 19);
            this.metroLabel2.TabIndex = 1;
            this.metroLabel2.Text = "Next version";
            // 
            // txtCurrentVersion
            // 
            // 
            // 
            // 
            this.txtCurrentVersion.CustomButton.Image = null;
            this.txtCurrentVersion.CustomButton.Location = new System.Drawing.Point(274, 1);
            this.txtCurrentVersion.CustomButton.Name = "";
            this.txtCurrentVersion.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtCurrentVersion.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCurrentVersion.CustomButton.TabIndex = 1;
            this.txtCurrentVersion.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCurrentVersion.CustomButton.UseSelectable = true;
            this.txtCurrentVersion.CustomButton.Visible = false;
            this.txtCurrentVersion.Lines = new string[0];
            this.txtCurrentVersion.Location = new System.Drawing.Point(127, 131);
            this.txtCurrentVersion.MaxLength = 32767;
            this.txtCurrentVersion.Name = "txtCurrentVersion";
            this.txtCurrentVersion.PasswordChar = '\0';
            this.txtCurrentVersion.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCurrentVersion.SelectedText = "";
            this.txtCurrentVersion.SelectionLength = 0;
            this.txtCurrentVersion.SelectionStart = 0;
            this.txtCurrentVersion.ShortcutsEnabled = true;
            this.txtCurrentVersion.Size = new System.Drawing.Size(296, 23);
            this.txtCurrentVersion.TabIndex = 2;
            this.txtCurrentVersion.UseSelectable = true;
            this.txtCurrentVersion.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCurrentVersion.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtNextVersion
            // 
            // 
            // 
            // 
            this.txtNextVersion.CustomButton.Image = null;
            this.txtNextVersion.CustomButton.Location = new System.Drawing.Point(274, 1);
            this.txtNextVersion.CustomButton.Name = "";
            this.txtNextVersion.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtNextVersion.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtNextVersion.CustomButton.TabIndex = 1;
            this.txtNextVersion.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtNextVersion.CustomButton.UseSelectable = true;
            this.txtNextVersion.CustomButton.Visible = false;
            this.txtNextVersion.Lines = new string[0];
            this.txtNextVersion.Location = new System.Drawing.Point(127, 172);
            this.txtNextVersion.MaxLength = 32767;
            this.txtNextVersion.Name = "txtNextVersion";
            this.txtNextVersion.PasswordChar = '\0';
            this.txtNextVersion.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNextVersion.SelectedText = "";
            this.txtNextVersion.SelectionLength = 0;
            this.txtNextVersion.SelectionStart = 0;
            this.txtNextVersion.ShortcutsEnabled = true;
            this.txtNextVersion.Size = new System.Drawing.Size(296, 23);
            this.txtNextVersion.TabIndex = 3;
            this.txtNextVersion.UseSelectable = true;
            this.txtNextVersion.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtNextVersion.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // rdUpdateNextVersion
            // 
            this.rdUpdateNextVersion.AutoSize = true;
            this.rdUpdateNextVersion.Location = new System.Drawing.Point(127, 219);
            this.rdUpdateNextVersion.Name = "rdUpdateNextVersion";
            this.rdUpdateNextVersion.Size = new System.Drawing.Size(142, 15);
            this.rdUpdateNextVersion.TabIndex = 4;
            this.rdUpdateNextVersion.Text = "Update to next version";
            this.rdUpdateNextVersion.UseSelectable = true;
            // 
            // rdOverrideVersion
            // 
            this.rdOverrideVersion.AutoSize = true;
            this.rdOverrideVersion.Location = new System.Drawing.Point(127, 240);
            this.rdOverrideVersion.Name = "rdOverrideVersion";
            this.rdOverrideVersion.Size = new System.Drawing.Size(150, 15);
            this.rdOverrideVersion.TabIndex = 5;
            this.rdOverrideVersion.Text = "Override current version";
            this.rdOverrideVersion.UseSelectable = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(127, 279);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 6;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseSelectable = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(218, 279);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseSelectable = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtFileName
            // 
            // 
            // 
            // 
            this.txtFileName.CustomButton.Image = null;
            this.txtFileName.CustomButton.Location = new System.Drawing.Point(274, 1);
            this.txtFileName.CustomButton.Name = "";
            this.txtFileName.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtFileName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFileName.CustomButton.TabIndex = 1;
            this.txtFileName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFileName.CustomButton.UseSelectable = true;
            this.txtFileName.CustomButton.Visible = false;
            this.txtFileName.Lines = new string[0];
            this.txtFileName.Location = new System.Drawing.Point(127, 90);
            this.txtFileName.MaxLength = 32767;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.PasswordChar = '\0';
            this.txtFileName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFileName.SelectedText = "";
            this.txtFileName.SelectionLength = 0;
            this.txtFileName.SelectionStart = 0;
            this.txtFileName.ShortcutsEnabled = true;
            this.txtFileName.Size = new System.Drawing.Size(296, 23);
            this.txtFileName.TabIndex = 8;
            this.txtFileName.UseSelectable = true;
            this.txtFileName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFileName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(55, 90);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(66, 19);
            this.metroLabel3.TabIndex = 9;
            this.metroLabel3.Text = "File name";
            // 
            // frmUpdateVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 328);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.rdOverrideVersion);
            this.Controls.Add(this.rdUpdateNextVersion);
            this.Controls.Add(this.txtNextVersion);
            this.Controls.Add(this.txtCurrentVersion);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateVersion";
            this.Resizable = false;
            this.Text = "[ECM] Update Version";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpdateVersion_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUpdateVersion_FormClosed);
            this.Load += new System.EventHandler(this.frmUpdateVersion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox txtCurrentVersion;
        private MetroFramework.Controls.MetroTextBox txtNextVersion;
        private MetroFramework.Controls.MetroRadioButton rdUpdateNextVersion;
        private MetroFramework.Controls.MetroRadioButton rdOverrideVersion;
        private MetroFramework.Controls.MetroButton btnConfirm;
        private MetroFramework.Controls.MetroButton btnClose;
        private MetroFramework.Controls.MetroTextBox txtFileName;
        private MetroFramework.Controls.MetroLabel metroLabel3;
    }
}