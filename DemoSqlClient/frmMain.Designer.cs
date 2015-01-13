namespace DemoSqlClient
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvBasicData = new System.Windows.Forms.DataGridView();
            this.btnGetSchema = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBasicData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBasicData
            // 
            this.dgvBasicData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBasicData.Location = new System.Drawing.Point(13, 13);
            this.dgvBasicData.Name = "dgvBasicData";
            this.dgvBasicData.Size = new System.Drawing.Size(603, 353);
            this.dgvBasicData.TabIndex = 0;
            // 
            // btnGetSchema
            // 
            this.btnGetSchema.Location = new System.Drawing.Point(13, 382);
            this.btnGetSchema.Name = "btnGetSchema";
            this.btnGetSchema.Size = new System.Drawing.Size(75, 23);
            this.btnGetSchema.TabIndex = 1;
            this.btnGetSchema.Text = "Get Schema";
            this.btnGetSchema.UseVisualStyleBackColor = true;
            this.btnGetSchema.Click += new System.EventHandler(this.btnGetSchema_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 417);
            this.Controls.Add(this.btnGetSchema);
            this.Controls.Add(this.dgvBasicData);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote-Data-Service";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBasicData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBasicData;
        private System.Windows.Forms.Button btnGetSchema;
    }
}

