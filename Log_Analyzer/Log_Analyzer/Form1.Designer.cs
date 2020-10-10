namespace Log_Analyzer
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnixTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcessID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnknownNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProxyStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DateTime,
            this.UnixTime,
            this.ProcessID,
            this.UserAddress,
            this.Action,
            this.UnknownNum,
            this.Status,
            this.URL,
            this.UserID,
            this.ProxyStatus,
            this.DocumentType});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 72;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.Size = new System.Drawing.Size(2087, 980);
            this.dataGridView1.TabIndex = 0;
            // 
            // DateTime
            // 
            this.DateTime.HeaderText = "DateTime";
            this.DateTime.MinimumWidth = 9;
            this.DateTime.Name = "DateTime";
            this.DateTime.Width = 175;
            // 
            // UnixTime
            // 
            this.UnixTime.HeaderText = "UnixTime";
            this.UnixTime.MinimumWidth = 9;
            this.UnixTime.Name = "UnixTime";
            this.UnixTime.Width = 175;
            // 
            // ProcessID
            // 
            this.ProcessID.HeaderText = "ProcessID";
            this.ProcessID.MinimumWidth = 9;
            this.ProcessID.Name = "ProcessID";
            this.ProcessID.Width = 175;
            // 
            // UserAddress
            // 
            this.UserAddress.HeaderText = "UserAddress";
            this.UserAddress.MinimumWidth = 9;
            this.UserAddress.Name = "UserAddress";
            this.UserAddress.Width = 175;
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.MinimumWidth = 9;
            this.Action.Name = "Action";
            this.Action.Width = 175;
            // 
            // UnknownNum
            // 
            this.UnknownNum.HeaderText = "UnknownNum";
            this.UnknownNum.MinimumWidth = 9;
            this.UnknownNum.Name = "UnknownNum";
            this.UnknownNum.Width = 175;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 9;
            this.Status.Name = "Status";
            this.Status.Width = 175;
            // 
            // URL
            // 
            this.URL.HeaderText = "URL";
            this.URL.MinimumWidth = 9;
            this.URL.Name = "URL";
            this.URL.Width = 175;
            // 
            // UserID
            // 
            this.UserID.HeaderText = "UserID";
            this.UserID.MinimumWidth = 9;
            this.UserID.Name = "UserID";
            this.UserID.Width = 175;
            // 
            // ProxyStatus
            // 
            this.ProxyStatus.HeaderText = "ProxyStatus";
            this.ProxyStatus.MinimumWidth = 9;
            this.ProxyStatus.Name = "ProxyStatus";
            this.ProxyStatus.Width = 175;
            // 
            // DocumentType
            // 
            this.DocumentType.HeaderText = "DocumentType";
            this.DocumentType.MinimumWidth = 9;
            this.DocumentType.Name = "DocumentType";
            this.DocumentType.Width = 175;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2111, 1004);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnixTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcessID;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnknownNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn URL;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProxyStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentType;
    }
}

