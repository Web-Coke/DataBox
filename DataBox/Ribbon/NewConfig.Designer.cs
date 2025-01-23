namespace DataBox.Ribbon
{
    partial class NewConfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewConfig));
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.SheetNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataAddrs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileOperation = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.SaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SelectSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.SelectFolder = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            this.TableLayoutPanel.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGridView
            // 
            this.DataGridView.AllowUserToAddRows = false;
            this.DataGridView.AllowUserToResizeRows = false;
            this.DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.DataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridView.ColumnHeadersHeight = 50;
            this.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SheetNames,
            this.DataAddrs,
            this.DataNames});
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 60);
            this.DataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.RowHeadersWidth = 102;
            this.DataGridView.RowTemplate.Height = 44;
            this.DataGridView.Size = new System.Drawing.Size(951, 590);
            this.DataGridView.TabIndex = 0;
            this.DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellClick);
            this.DataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DataGridView_RowsAdded);
            this.DataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.DataGridView_RowsRemoved);
            // 
            // SheetNames
            // 
            this.SheetNames.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SheetNames.DefaultCellStyle = dataGridViewCellStyle2;
            this.SheetNames.MinimumWidth = 30;
            this.SheetNames.Name = "SheetNames";
            this.SheetNames.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DataAddrs
            // 
            this.DataAddrs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataAddrs.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataAddrs.MinimumWidth = 30;
            this.DataAddrs.Name = "DataAddrs";
            this.DataAddrs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DataNames
            // 
            this.DataNames.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataNames.DefaultCellStyle = dataGridViewCellStyle4;
            this.DataNames.MinimumWidth = 30;
            this.DataNames.Name = "DataNames";
            this.DataNames.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.AutoSize = true;
            this.TableLayoutPanel.ColumnCount = 1;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel.Controls.Add(this.DataGridView, 0, 1);
            this.TableLayoutPanel.Controls.Add(this.MenuStrip, 0, 0);
            this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.RowCount = 2;
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel.Size = new System.Drawing.Size(951, 650);
            this.TableLayoutPanel.TabIndex = 1;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileOperation});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(951, 60);
            this.MenuStrip.TabIndex = 1;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // FileOperation
            // 
            this.FileOperation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFile,
            this.OpenFile,
            this.ToolStripSeparator,
            this.SaveFile,
            this.SaveAs});
            this.FileOperation.Name = "FileOperation";
            this.FileOperation.Size = new System.Drawing.Size(137, 56);
            // 
            // NewFile
            // 
            this.NewFile.Image = ((System.Drawing.Image)(resources.GetObject("NewFile.Image")));
            this.NewFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewFile.Name = "NewFile";
            this.NewFile.Size = new System.Drawing.Size(313, 54);
            this.NewFile.Click += new System.EventHandler(this.NewFile_Click);
            // 
            // OpenFile
            // 
            this.OpenFile.Image = ((System.Drawing.Image)(resources.GetObject("OpenFile.Image")));
            this.OpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(313, 54);
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // ToolStripSeparator
            // 
            this.ToolStripSeparator.Name = "ToolStripSeparator";
            this.ToolStripSeparator.Size = new System.Drawing.Size(310, 6);
            // 
            // SaveFile
            // 
            this.SaveFile.Image = ((System.Drawing.Image)(resources.GetObject("SaveFile.Image")));
            this.SaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveFile.Name = "SaveFile";
            this.SaveFile.Size = new System.Drawing.Size(313, 54);
            this.SaveFile.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // SaveAs
            // 
            this.SaveAs.Name = "SaveAs";
            this.SaveAs.Size = new System.Drawing.Size(313, 54);
            this.SaveAs.Click += new System.EventHandler(this.SaveAs_Click);
            // 
            // SelectOpenFile
            // 
            this.SelectOpenFile.DefaultExt = "json";
            // 
            // SelectSaveFile
            // 
            this.SelectSaveFile.CreatePrompt = true;
            this.SelectSaveFile.DefaultExt = "json";
            // 
            // SelectFolder
            // 
            // 
            // NewConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(951, 650);
            this.Controls.Add(this.TableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "NewConfig";
            this.ShowIcon = false;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            this.TableLayoutPanel.ResumeLayout(false);
            this.TableLayoutPanel.PerformLayout();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView DataGridView;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileOperation;
        private System.Windows.Forms.ToolStripMenuItem NewFile;
        private System.Windows.Forms.ToolStripMenuItem OpenFile;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem SaveFile;
        private System.Windows.Forms.ToolStripMenuItem SaveAs;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheetNames;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataAddrs;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataNames;
        public System.Windows.Forms.OpenFileDialog SelectOpenFile;
        public System.Windows.Forms.SaveFileDialog SelectSaveFile;
        public System.Windows.Forms.FolderBrowserDialog SelectFolder;
    }
}