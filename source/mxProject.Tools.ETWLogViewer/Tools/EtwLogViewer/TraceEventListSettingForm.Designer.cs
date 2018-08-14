namespace mxProject.Tools.EtwLogViewer
{
    partial class TraceEventListSettingForm
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
            this.lstUnselected = new System.Windows.Forms.ListView();
            this.colUnseledtedField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstSelected = new System.Windows.Forms.ListView();
            this.colSelectedField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnDeselect = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMaxCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstUnselected
            // 
            this.lstUnselected.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUnseledtedField});
            this.lstUnselected.FullRowSelect = true;
            this.lstUnselected.GridLines = true;
            this.lstUnselected.HideSelection = false;
            this.lstUnselected.Location = new System.Drawing.Point(12, 12);
            this.lstUnselected.Name = "lstUnselected";
            this.lstUnselected.Size = new System.Drawing.Size(183, 378);
            this.lstUnselected.TabIndex = 0;
            this.lstUnselected.UseCompatibleStateImageBehavior = false;
            this.lstUnselected.View = System.Windows.Forms.View.Details;
            // 
            // colUnseledtedField
            // 
            this.colUnseledtedField.Text = "Field";
            this.colUnseledtedField.Width = 160;
            // 
            // lstSelected
            // 
            this.lstSelected.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSelectedField});
            this.lstSelected.FullRowSelect = true;
            this.lstSelected.GridLines = true;
            this.lstSelected.HideSelection = false;
            this.lstSelected.Location = new System.Drawing.Point(252, 12);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.Size = new System.Drawing.Size(183, 378);
            this.lstSelected.TabIndex = 1;
            this.lstSelected.UseCompatibleStateImageBehavior = false;
            this.lstSelected.View = System.Windows.Forms.View.Details;
            // 
            // colSelectedField
            // 
            this.colSelectedField.Text = "Field";
            this.colSelectedField.Width = 160;
            // 
            // btnSelect
            // 
            this.btnSelect.Image = global::mxProject.Properties.Resources.arrow_3_right;
            this.btnSelect.Location = new System.Drawing.Point(201, 149);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(45, 36);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // btnDeselect
            // 
            this.btnDeselect.Image = global::mxProject.Properties.Resources.arrow_3_left;
            this.btnDeselect.Location = new System.Drawing.Point(201, 191);
            this.btnDeselect.Name = "btnDeselect";
            this.btnDeselect.Size = new System.Drawing.Size(45, 36);
            this.btnDeselect.TabIndex = 3;
            this.btnDeselect.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(303, 426);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(84, 36);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(402, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 36);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtMaxCount
            // 
            this.txtMaxCount.Location = new System.Drawing.Point(98, 409);
            this.txtMaxCount.Name = "txtMaxCount";
            this.txtMaxCount.Size = new System.Drawing.Size(97, 19);
            this.txtMaxCount.TabIndex = 6;
            this.txtMaxCount.Text = "1000";
            this.txtMaxCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 412);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "Max row count";
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Image = global::mxProject.Properties.Resources.arrow_3_up;
            this.btnMoveUp.Location = new System.Drawing.Point(442, 149);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(45, 36);
            this.btnMoveUp.TabIndex = 4;
            this.btnMoveUp.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Image = global::mxProject.Properties.Resources.arrow_3_down;
            this.btnMoveDown.Location = new System.Drawing.Point(441, 191);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(45, 36);
            this.btnMoveDown.TabIndex = 5;
            this.btnMoveDown.UseVisualStyleBackColor = true;
            // 
            // TraceEventListSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 474);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMaxCount);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnDeselect);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lstSelected);
            this.Controls.Add(this.lstUnselected);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TraceEventListSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstUnselected;
        private System.Windows.Forms.ListView lstSelected;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDeselect;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader colUnseledtedField;
        private System.Windows.Forms.ColumnHeader colSelectedField;
        private System.Windows.Forms.TextBox txtMaxCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
    }
}