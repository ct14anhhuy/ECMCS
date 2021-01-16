
namespace ECMCS.App
{
    partial class frmSyncToECM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSyncToECM));
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.btnClose = new MetroFramework.Controls.MetroButton();
            this.btnConfirm = new MetroFramework.Controls.MetroButton();
            this.txtFileName = new MetroFramework.Controls.MetroTextBox();
            this.txtOwner = new MetroFramework.Controls.MetroTextBox();
            this.treeECMFolder = new System.Windows.Forms.TreeView();
            this.txtPath = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.txtTag = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(243, 85);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(66, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "File name";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(261, 124);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(48, 19);
            this.metroLabel2.TabIndex = 2;
            this.metroLabel2.Text = "Owner";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(487, 239);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseSelectable = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(396, 239);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 8;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseSelectable = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtFileName
            // 
            // 
            // 
            // 
            this.txtFileName.CustomButton.Image = null;
            this.txtFileName.CustomButton.Location = new System.Drawing.Point(278, 1);
            this.txtFileName.CustomButton.Name = "";
            this.txtFileName.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtFileName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFileName.CustomButton.TabIndex = 1;
            this.txtFileName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFileName.CustomButton.UseSelectable = true;
            this.txtFileName.CustomButton.Visible = false;
            this.txtFileName.Lines = new string[0];
            this.txtFileName.Location = new System.Drawing.Point(329, 85);
            this.txtFileName.MaxLength = 32767;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.PasswordChar = '\0';
            this.txtFileName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFileName.SelectedText = "";
            this.txtFileName.SelectionLength = 0;
            this.txtFileName.SelectionStart = 0;
            this.txtFileName.ShortcutsEnabled = true;
            this.txtFileName.Size = new System.Drawing.Size(300, 23);
            this.txtFileName.TabIndex = 10;
            this.txtFileName.UseSelectable = true;
            this.txtFileName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFileName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtOwner
            // 
            // 
            // 
            // 
            this.txtOwner.CustomButton.Image = null;
            this.txtOwner.CustomButton.Location = new System.Drawing.Point(278, 1);
            this.txtOwner.CustomButton.Name = "";
            this.txtOwner.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtOwner.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtOwner.CustomButton.TabIndex = 1;
            this.txtOwner.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtOwner.CustomButton.UseSelectable = true;
            this.txtOwner.CustomButton.Visible = false;
            this.txtOwner.Lines = new string[0];
            this.txtOwner.Location = new System.Drawing.Point(329, 124);
            this.txtOwner.MaxLength = 32767;
            this.txtOwner.Name = "txtOwner";
            this.txtOwner.PasswordChar = '\0';
            this.txtOwner.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtOwner.SelectedText = "";
            this.txtOwner.SelectionLength = 0;
            this.txtOwner.SelectionStart = 0;
            this.txtOwner.ShortcutsEnabled = true;
            this.txtOwner.Size = new System.Drawing.Size(300, 23);
            this.txtOwner.TabIndex = 11;
            this.txtOwner.UseSelectable = true;
            this.txtOwner.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtOwner.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // treeECMFolder
            // 
            this.treeECMFolder.FullRowSelect = true;
            this.treeECMFolder.Location = new System.Drawing.Point(23, 63);
            this.treeECMFolder.Name = "treeECMFolder";
            this.treeECMFolder.Size = new System.Drawing.Size(194, 290);
            this.treeECMFolder.TabIndex = 12;
            this.treeECMFolder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeECMFolder_AfterSelect);
            // 
            // txtPath
            // 
            // 
            // 
            // 
            this.txtPath.CustomButton.Image = null;
            this.txtPath.CustomButton.Location = new System.Drawing.Point(278, 1);
            this.txtPath.CustomButton.Name = "";
            this.txtPath.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtPath.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPath.CustomButton.TabIndex = 1;
            this.txtPath.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPath.CustomButton.UseSelectable = true;
            this.txtPath.CustomButton.Visible = false;
            this.txtPath.Lines = new string[0];
            this.txtPath.Location = new System.Drawing.Point(329, 160);
            this.txtPath.MaxLength = 32767;
            this.txtPath.Name = "txtPath";
            this.txtPath.PasswordChar = '\0';
            this.txtPath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPath.SelectedText = "";
            this.txtPath.SelectionLength = 0;
            this.txtPath.SelectionStart = 0;
            this.txtPath.ShortcutsEnabled = true;
            this.txtPath.Size = new System.Drawing.Size(300, 23);
            this.txtPath.TabIndex = 14;
            this.txtPath.UseSelectable = true;
            this.txtPath.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPath.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(275, 160);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(34, 19);
            this.metroLabel3.TabIndex = 13;
            this.metroLabel3.Text = "Path";
            // 
            // txtTag
            // 
            // 
            // 
            // 
            this.txtTag.CustomButton.Image = null;
            this.txtTag.CustomButton.Location = new System.Drawing.Point(278, 1);
            this.txtTag.CustomButton.Name = "";
            this.txtTag.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtTag.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtTag.CustomButton.TabIndex = 1;
            this.txtTag.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtTag.CustomButton.UseSelectable = true;
            this.txtTag.CustomButton.Visible = false;
            this.txtTag.Lines = new string[] {
        "#"};
            this.txtTag.Location = new System.Drawing.Point(329, 199);
            this.txtTag.MaxLength = 32767;
            this.txtTag.Name = "txtTag";
            this.txtTag.PasswordChar = '\0';
            this.txtTag.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtTag.SelectedText = "";
            this.txtTag.SelectionLength = 0;
            this.txtTag.SelectionStart = 0;
            this.txtTag.ShortcutsEnabled = true;
            this.txtTag.Size = new System.Drawing.Size(300, 23);
            this.txtTag.TabIndex = 16;
            this.txtTag.Text = "#";
            this.txtTag.UseSelectable = true;
            this.txtTag.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtTag.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(280, 199);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(29, 19);
            this.metroLabel4.TabIndex = 15;
            this.metroLabel4.Text = "Tag";
            // 
            // frmSyncToECM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 376);
            this.Controls.Add(this.txtTag);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.treeECMFolder);
            this.Controls.Add(this.txtOwner);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSyncToECM";
            this.Resizable = false;
            this.Text = "[ECM] Sync";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSyncToECM_FormClosed);
            this.Load += new System.EventHandler(this.frmSyncToECM_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton btnClose;
        private MetroFramework.Controls.MetroButton btnConfirm;
        private MetroFramework.Controls.MetroTextBox txtFileName;
        private MetroFramework.Controls.MetroTextBox txtOwner;
        private System.Windows.Forms.TreeView treeECMFolder;
        private MetroFramework.Controls.MetroTextBox txtPath;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroTextBox txtTag;
        private MetroFramework.Controls.MetroLabel metroLabel4;
    }
}