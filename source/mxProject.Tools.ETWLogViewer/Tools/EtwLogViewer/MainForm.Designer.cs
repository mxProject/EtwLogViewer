namespace mxProject.Tools.EtwLogViewer
{
    partial class MainForm
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
            this.lstProvider = new System.Windows.Forms.ListView();
            this.colProviderFriendlyName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colProviderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colProviderID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grdLog = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuListSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.grdLog)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstProvider
            // 
            this.lstProvider.CheckBoxes = true;
            this.lstProvider.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colProviderFriendlyName,
            this.colProviderName,
            this.colProviderID});
            this.lstProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstProvider.FullRowSelect = true;
            this.lstProvider.GridLines = true;
            this.lstProvider.Location = new System.Drawing.Point(0, 0);
            this.lstProvider.MultiSelect = false;
            this.lstProvider.Name = "lstProvider";
            this.lstProvider.Size = new System.Drawing.Size(676, 125);
            this.lstProvider.TabIndex = 6;
            this.lstProvider.UseCompatibleStateImageBehavior = false;
            this.lstProvider.View = System.Windows.Forms.View.Details;
            // 
            // colProviderFriendlyName
            // 
            this.colProviderFriendlyName.Text = "FriendlyName";
            this.colProviderFriendlyName.Width = 150;
            // 
            // colProviderName
            // 
            this.colProviderName.Text = "ProviderName";
            this.colProviderName.Width = 250;
            // 
            // colProviderID
            // 
            this.colProviderID.Text = "ProviderID";
            this.colProviderID.Width = 240;
            // 
            // grdLog
            // 
            this.grdLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdLog.Location = new System.Drawing.Point(0, 0);
            this.grdLog.Name = "grdLog";
            this.grdLog.RowTemplate.Height = 21;
            this.grdLog.Size = new System.Drawing.Size(676, 300);
            this.grdLog.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSettings});
            this.menuStrip1.Location = new System.Drawing.Point(4, 4);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(676, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuSettings
            // 
            this.mnuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuListSetting});
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(78, 20);
            this.mnuSettings.Text = "Settings (&S)";
            // 
            // mnuListSetting
            // 
            this.mnuListSetting.Name = "mnuListSetting";
            this.mnuListSetting.Size = new System.Drawing.Size(149, 22);
            this.mnuListSetting.Text = "List Setting (&L)";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(4, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstProvider);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdLog);
            this.splitContainer1.Size = new System.Drawing.Size(676, 429);
            this.splitContainer1.SplitterDistance = 125;
            this.splitContainer1.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ETW LogViewer";
            ((System.ComponentModel.ISupportInitialize)(this.grdLog)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView grdLog;
        private System.Windows.Forms.ListView lstProvider;
        private System.Windows.Forms.ColumnHeader colProviderName;
        private System.Windows.Forms.ColumnHeader colProviderID;
        private System.Windows.Forms.ColumnHeader colProviderFriendlyName;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuListSetting;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}