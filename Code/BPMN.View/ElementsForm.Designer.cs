namespace BPMN.View
{
  partial class ElementsForm
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
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.splitRight = new System.Windows.Forms.SplitContainer();
      this.splitLeft = new System.Windows.Forms.SplitContainer();
      this.panel1 = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.panel2 = new System.Windows.Forms.Panel();
      this.label2 = new System.Windows.Forms.Label();
      this.panel3 = new System.Windows.Forms.Panel();
      this.label3 = new System.Windows.Forms.Label();
      this.panel4 = new System.Windows.Forms.Panel();
      this.label4 = new System.Windows.Forms.Label();
      this.gridElements = new System.Windows.Forms.DataGridView();
      this.gridSubElements = new System.Windows.Forms.DataGridView();
      this.gridAttributes = new System.Windows.Forms.DataGridView();
      this.gridProperties = new System.Windows.Forms.DataGridView();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.splitRight.Panel1.SuspendLayout();
      this.splitRight.Panel2.SuspendLayout();
      this.splitRight.SuspendLayout();
      this.splitLeft.Panel1.SuspendLayout();
      this.splitLeft.Panel2.SuspendLayout();
      this.splitLeft.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridElements)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridSubElements)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridProperties)).BeginInit();
      this.SuspendLayout();
      // 
      // splitMain
      // 
      this.splitMain.BackColor = System.Drawing.SystemColors.Control;
      this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitMain.Location = new System.Drawing.Point(0, 0);
      this.splitMain.Name = "splitMain";
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.splitLeft);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.splitRight);
      this.splitMain.Size = new System.Drawing.Size(973, 629);
      this.splitMain.SplitterDistance = 505;
      this.splitMain.TabIndex = 0;
      // 
      // splitRight
      // 
      this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitRight.Location = new System.Drawing.Point(0, 0);
      this.splitRight.Name = "splitRight";
      this.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitRight.Panel1
      // 
      this.splitRight.Panel1.Controls.Add(this.gridAttributes);
      this.splitRight.Panel1.Controls.Add(this.panel3);
      // 
      // splitRight.Panel2
      // 
      this.splitRight.Panel2.Controls.Add(this.gridProperties);
      this.splitRight.Panel2.Controls.Add(this.panel4);
      this.splitRight.Size = new System.Drawing.Size(464, 629);
      this.splitRight.SplitterDistance = 305;
      this.splitRight.TabIndex = 0;
      // 
      // splitLeft
      // 
      this.splitLeft.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitLeft.Location = new System.Drawing.Point(0, 0);
      this.splitLeft.Name = "splitLeft";
      this.splitLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitLeft.Panel1
      // 
      this.splitLeft.Panel1.Controls.Add(this.gridElements);
      this.splitLeft.Panel1.Controls.Add(this.panel1);
      // 
      // splitLeft.Panel2
      // 
      this.splitLeft.Panel2.Controls.Add(this.gridSubElements);
      this.splitLeft.Panel2.Controls.Add(this.panel2);
      this.splitLeft.Size = new System.Drawing.Size(505, 629);
      this.splitLeft.SplitterDistance = 307;
      this.splitLeft.TabIndex = 1;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.label1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(505, 34);
      this.panel1.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(82, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Model Elements";
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.label2);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(505, 34);
      this.panel2.TabIndex = 3;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(187, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Member Elements of selected Element";
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this.label3);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel3.Location = new System.Drawing.Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(464, 34);
      this.panel3.TabIndex = 4;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(147, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Attributes of selected Element";
      // 
      // panel4
      // 
      this.panel4.Controls.Add(this.label4);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel4.Location = new System.Drawing.Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(464, 34);
      this.panel4.TabIndex = 4;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 9);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(150, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Properties of selected Element";
      // 
      // gridElements
      // 
      this.gridElements.AllowUserToAddRows = false;
      this.gridElements.AllowUserToDeleteRows = false;
      this.gridElements.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.gridElements.BackgroundColor = System.Drawing.SystemColors.Window;
      this.gridElements.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.gridElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridElements.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridElements.GridColor = System.Drawing.SystemColors.Window;
      this.gridElements.Location = new System.Drawing.Point(0, 34);
      this.gridElements.Name = "gridElements";
      this.gridElements.ReadOnly = true;
      this.gridElements.RowHeadersVisible = false;
      this.gridElements.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.gridElements.Size = new System.Drawing.Size(505, 273);
      this.gridElements.TabIndex = 3;
      this.gridElements.SelectionChanged += new System.EventHandler(this.gridElements_SelectionChanged);
      // 
      // gridSubElements
      // 
      this.gridSubElements.AllowUserToAddRows = false;
      this.gridSubElements.AllowUserToDeleteRows = false;
      this.gridSubElements.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.gridSubElements.BackgroundColor = System.Drawing.SystemColors.Window;
      this.gridSubElements.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.gridSubElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridSubElements.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridSubElements.GridColor = System.Drawing.SystemColors.Window;
      this.gridSubElements.Location = new System.Drawing.Point(0, 34);
      this.gridSubElements.Name = "gridSubElements";
      this.gridSubElements.ReadOnly = true;
      this.gridSubElements.RowHeadersVisible = false;
      this.gridSubElements.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.gridSubElements.Size = new System.Drawing.Size(505, 284);
      this.gridSubElements.TabIndex = 4;
      this.gridSubElements.SelectionChanged += new System.EventHandler(this.gridSubElements_SelectionChanged);
      // 
      // gridAttributes
      // 
      this.gridAttributes.AllowUserToAddRows = false;
      this.gridAttributes.AllowUserToDeleteRows = false;
      this.gridAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.gridAttributes.BackgroundColor = System.Drawing.SystemColors.Window;
      this.gridAttributes.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.gridAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridAttributes.GridColor = System.Drawing.SystemColors.Window;
      this.gridAttributes.Location = new System.Drawing.Point(0, 34);
      this.gridAttributes.Name = "gridAttributes";
      this.gridAttributes.ReadOnly = true;
      this.gridAttributes.RowHeadersVisible = false;
      this.gridAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.gridAttributes.Size = new System.Drawing.Size(464, 271);
      this.gridAttributes.TabIndex = 5;
      // 
      // gridProperties
      // 
      this.gridProperties.AllowUserToAddRows = false;
      this.gridProperties.AllowUserToDeleteRows = false;
      this.gridProperties.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.gridProperties.BackgroundColor = System.Drawing.SystemColors.Window;
      this.gridProperties.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.gridProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridProperties.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridProperties.GridColor = System.Drawing.SystemColors.Window;
      this.gridProperties.Location = new System.Drawing.Point(0, 34);
      this.gridProperties.Name = "gridProperties";
      this.gridProperties.ReadOnly = true;
      this.gridProperties.RowHeadersVisible = false;
      this.gridProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.gridProperties.Size = new System.Drawing.Size(464, 286);
      this.gridProperties.TabIndex = 5;
      // 
      // ElementsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.ClientSize = new System.Drawing.Size(973, 629);
      this.Controls.Add(this.splitMain);
      this.Name = "ElementsForm";
      this.Text = "BPMN View - Model Elements";
      this.Load += new System.EventHandler(this.ElementsForm_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.ResumeLayout(false);
      this.splitRight.Panel1.ResumeLayout(false);
      this.splitRight.Panel2.ResumeLayout(false);
      this.splitRight.ResumeLayout(false);
      this.splitLeft.Panel1.ResumeLayout(false);
      this.splitLeft.Panel2.ResumeLayout(false);
      this.splitLeft.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridElements)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridSubElements)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridProperties)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.SplitContainer splitRight;
    private System.Windows.Forms.SplitContainer splitLeft;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.DataGridView gridElements;
    private System.Windows.Forms.DataGridView gridSubElements;
    private System.Windows.Forms.DataGridView gridAttributes;
    private System.Windows.Forms.DataGridView gridProperties;
  }
}