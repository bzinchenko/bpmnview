namespace BPMN.View
{
  partial class ModelForm
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
      this.treeElements = new System.Windows.Forms.TreeView();
      this.panel1 = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.ctlElement = new BPMN.View.ElementControl();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitMain
      // 
      this.splitMain.BackColor = System.Drawing.SystemColors.Control;
      this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitMain.Location = new System.Drawing.Point(0, 0);
      this.splitMain.Name = "splitMain";
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.treeElements);
      this.splitMain.Panel1.Controls.Add(this.panel1);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.ctlElement);
      this.splitMain.Size = new System.Drawing.Size(810, 629);
      this.splitMain.SplitterDistance = 332;
      this.splitMain.TabIndex = 0;
      // 
      // treeElements
      // 
      this.treeElements.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.treeElements.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeElements.Location = new System.Drawing.Point(0, 34);
      this.treeElements.Name = "treeElements";
      this.treeElements.Size = new System.Drawing.Size(332, 595);
      this.treeElements.TabIndex = 4;
      this.treeElements.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeElements_AfterSelect);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.label1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(332, 34);
      this.panel1.TabIndex = 3;
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
      // ctlElement
      // 
      this.ctlElement.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlElement.Location = new System.Drawing.Point(0, 0);
      this.ctlElement.Name = "ctlElement";
      this.ctlElement.Size = new System.Drawing.Size(474, 629);
      this.ctlElement.TabIndex = 0;
      // 
      // ModelForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.ClientSize = new System.Drawing.Size(810, 629);
      this.Controls.Add(this.splitMain);
      this.Name = "ModelForm";
      this.Text = "BPMN View - Model Elements";
      this.Load += new System.EventHandler(this.ElementsForm_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TreeView treeElements;
    private ElementControl ctlElement;
  }
}