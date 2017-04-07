namespace BPMN.View
{
  partial class ElementControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.splitRightAll = new System.Windows.Forms.SplitContainer();
      this.gridSubElements = new System.Windows.Forms.DataGridView();
      this.panel2 = new System.Windows.Forms.Panel();
      this.label2 = new System.Windows.Forms.Label();
      this.splitRightNested = new System.Windows.Forms.SplitContainer();
      this.gridAttributes = new System.Windows.Forms.DataGridView();
      this.panel3 = new System.Windows.Forms.Panel();
      this.label3 = new System.Windows.Forms.Label();
      this.gridProperties = new System.Windows.Forms.DataGridView();
      this.panel4 = new System.Windows.Forms.Panel();
      this.label4 = new System.Windows.Forms.Label();
      this.splitRightAll.Panel1.SuspendLayout();
      this.splitRightAll.Panel2.SuspendLayout();
      this.splitRightAll.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridSubElements)).BeginInit();
      this.panel2.SuspendLayout();
      this.splitRightNested.Panel1.SuspendLayout();
      this.splitRightNested.Panel2.SuspendLayout();
      this.splitRightNested.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).BeginInit();
      this.panel3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridProperties)).BeginInit();
      this.panel4.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitRightAll
      // 
      this.splitRightAll.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitRightAll.Location = new System.Drawing.Point(0, 0);
      this.splitRightAll.Name = "splitRightAll";
      this.splitRightAll.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitRightAll.Panel1
      // 
      this.splitRightAll.Panel1.Controls.Add(this.gridSubElements);
      this.splitRightAll.Panel1.Controls.Add(this.panel2);
      // 
      // splitRightAll.Panel2
      // 
      this.splitRightAll.Panel2.Controls.Add(this.splitRightNested);
      this.splitRightAll.Size = new System.Drawing.Size(350, 518);
      this.splitRightAll.SplitterDistance = 298;
      this.splitRightAll.TabIndex = 2;
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
      this.gridSubElements.Size = new System.Drawing.Size(350, 264);
      this.gridSubElements.TabIndex = 4;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.label2);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(350, 34);
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
      // splitRightNested
      // 
      this.splitRightNested.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitRightNested.Location = new System.Drawing.Point(0, 0);
      this.splitRightNested.Name = "splitRightNested";
      this.splitRightNested.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitRightNested.Panel1
      // 
      this.splitRightNested.Panel1.Controls.Add(this.gridAttributes);
      this.splitRightNested.Panel1.Controls.Add(this.panel3);
      // 
      // splitRightNested.Panel2
      // 
      this.splitRightNested.Panel2.Controls.Add(this.gridProperties);
      this.splitRightNested.Panel2.Controls.Add(this.panel4);
      this.splitRightNested.Size = new System.Drawing.Size(350, 216);
      this.splitRightNested.SplitterDistance = 136;
      this.splitRightNested.TabIndex = 0;
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
      this.gridAttributes.Size = new System.Drawing.Size(350, 102);
      this.gridAttributes.TabIndex = 5;
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this.label3);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel3.Location = new System.Drawing.Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(350, 34);
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
      this.gridProperties.Size = new System.Drawing.Size(350, 42);
      this.gridProperties.TabIndex = 5;
      // 
      // panel4
      // 
      this.panel4.Controls.Add(this.label4);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel4.Location = new System.Drawing.Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(350, 34);
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
      // ElementControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitRightAll);
      this.Name = "ElementControl";
      this.Size = new System.Drawing.Size(350, 518);
      this.splitRightAll.Panel1.ResumeLayout(false);
      this.splitRightAll.Panel2.ResumeLayout(false);
      this.splitRightAll.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.gridSubElements)).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.splitRightNested.Panel1.ResumeLayout(false);
      this.splitRightNested.Panel2.ResumeLayout(false);
      this.splitRightNested.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.gridAttributes)).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridProperties)).EndInit();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitRightAll;
    private System.Windows.Forms.DataGridView gridSubElements;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.SplitContainer splitRightNested;
    private System.Windows.Forms.DataGridView gridAttributes;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.DataGridView gridProperties;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Label label4;
  }
}
