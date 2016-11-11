namespace BPMN.View
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      System.Windows.Forms.Button buttonTable;
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.panelLeft = new System.Windows.Forms.Panel();
      this.buttonAbout = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.buttonZoomReset = new System.Windows.Forms.Button();
      this.buttonZoomOut = new System.Windows.Forms.Button();
      this.buttonZoomIn = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.buttonSaveAsEXIF = new System.Windows.Forms.Button();
      this.buttonSaveAsEMF = new System.Windows.Forms.Button();
      this.buttonSaveAsPNG = new System.Windows.Forms.Button();
      this.buttonSaveAsJPG = new System.Windows.Forms.Button();
      this.buttonSaveAsGIF = new System.Windows.Forms.Button();
      this.buttonSaveAsBMP = new System.Windows.Forms.Button();
      this.buttonPrint = new System.Windows.Forms.Button();
      this.buttonOpen = new System.Windows.Forms.Button();
      this.pictureLogo = new System.Windows.Forms.PictureBox();
      this.printDialog1 = new System.Windows.Forms.PrintDialog();
      this.pictureDiagram = new System.Windows.Forms.PictureBox();
      this.panelImage = new System.Windows.Forms.Panel();
      this.printDocument1 = new System.Drawing.Printing.PrintDocument();
      this.panelTop = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.comboDiagram = new System.Windows.Forms.ComboBox();
      buttonTable = new System.Windows.Forms.Button();
      this.panelLeft.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureDiagram)).BeginInit();
      this.panelImage.SuspendLayout();
      this.panelTop.SuspendLayout();
      this.SuspendLayout();
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      // 
      // statusStrip1
      // 
      this.statusStrip1.Location = new System.Drawing.Point(0, 533);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(889, 22);
      this.statusStrip1.TabIndex = 0;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // panelLeft
      // 
      this.panelLeft.BackColor = System.Drawing.SystemColors.Window;
      this.panelLeft.Controls.Add(this.buttonAbout);
      this.panelLeft.Controls.Add(this.groupBox2);
      this.panelLeft.Controls.Add(this.groupBox1);
      this.panelLeft.Controls.Add(this.buttonPrint);
      this.panelLeft.Controls.Add(this.buttonOpen);
      this.panelLeft.Controls.Add(this.pictureLogo);
      this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
      this.panelLeft.Location = new System.Drawing.Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Padding = new System.Windows.Forms.Padding(3);
      this.panelLeft.Size = new System.Drawing.Size(101, 533);
      this.panelLeft.TabIndex = 1;
      // 
      // buttonAbout
      // 
      this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonAbout.Location = new System.Drawing.Point(16, 490);
      this.buttonAbout.Name = "buttonAbout";
      this.buttonAbout.Size = new System.Drawing.Size(75, 23);
      this.buttonAbout.TabIndex = 5;
      this.buttonAbout.Text = "About";
      this.buttonAbout.UseVisualStyleBackColor = true;
      this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox2.Controls.Add(this.buttonZoomReset);
      this.groupBox2.Controls.Add(this.buttonZoomOut);
      this.groupBox2.Controls.Add(this.buttonZoomIn);
      this.groupBox2.Location = new System.Drawing.Point(16, 394);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(75, 79);
      this.groupBox2.TabIndex = 4;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Zoom";
      // 
      // buttonZoomReset
      // 
      this.buttonZoomReset.Location = new System.Drawing.Point(7, 48);
      this.buttonZoomReset.Name = "buttonZoomReset";
      this.buttonZoomReset.Size = new System.Drawing.Size(61, 23);
      this.buttonZoomReset.TabIndex = 8;
      this.buttonZoomReset.Text = "Reset";
      this.buttonZoomReset.UseVisualStyleBackColor = true;
      this.buttonZoomReset.Click += new System.EventHandler(this.buttonZoomReset_Click);
      // 
      // buttonZoomOut
      // 
      this.buttonZoomOut.Location = new System.Drawing.Point(42, 19);
      this.buttonZoomOut.Name = "buttonZoomOut";
      this.buttonZoomOut.Size = new System.Drawing.Size(27, 23);
      this.buttonZoomOut.TabIndex = 7;
      this.buttonZoomOut.Text = "-";
      this.buttonZoomOut.UseVisualStyleBackColor = true;
      this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
      // 
      // buttonZoomIn
      // 
      this.buttonZoomIn.Location = new System.Drawing.Point(6, 19);
      this.buttonZoomIn.Name = "buttonZoomIn";
      this.buttonZoomIn.Size = new System.Drawing.Size(28, 23);
      this.buttonZoomIn.TabIndex = 6;
      this.buttonZoomIn.Text = "+";
      this.buttonZoomIn.UseVisualStyleBackColor = true;
      this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.buttonSaveAsEXIF);
      this.groupBox1.Controls.Add(this.buttonSaveAsEMF);
      this.groupBox1.Controls.Add(this.buttonSaveAsPNG);
      this.groupBox1.Controls.Add(this.buttonSaveAsJPG);
      this.groupBox1.Controls.Add(this.buttonSaveAsGIF);
      this.groupBox1.Controls.Add(this.buttonSaveAsBMP);
      this.groupBox1.Location = new System.Drawing.Point(13, 147);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(75, 201);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Save As";
      // 
      // buttonSaveAsEXIF
      // 
      this.buttonSaveAsEXIF.Location = new System.Drawing.Point(7, 170);
      this.buttonSaveAsEXIF.Name = "buttonSaveAsEXIF";
      this.buttonSaveAsEXIF.Size = new System.Drawing.Size(62, 23);
      this.buttonSaveAsEXIF.TabIndex = 5;
      this.buttonSaveAsEXIF.Text = "EXIF";
      this.buttonSaveAsEXIF.UseVisualStyleBackColor = true;
      this.buttonSaveAsEXIF.Click += new System.EventHandler(this.buttonSaveAsEXIF_Click);
      // 
      // buttonSaveAsEMF
      // 
      this.buttonSaveAsEMF.Location = new System.Drawing.Point(7, 140);
      this.buttonSaveAsEMF.Name = "buttonSaveAsEMF";
      this.buttonSaveAsEMF.Size = new System.Drawing.Size(62, 23);
      this.buttonSaveAsEMF.TabIndex = 4;
      this.buttonSaveAsEMF.Text = "EMF";
      this.buttonSaveAsEMF.UseVisualStyleBackColor = true;
      this.buttonSaveAsEMF.Click += new System.EventHandler(this.buttonSaveAsEMF_Click);
      // 
      // buttonSaveAsPNG
      // 
      this.buttonSaveAsPNG.Location = new System.Drawing.Point(7, 110);
      this.buttonSaveAsPNG.Name = "buttonSaveAsPNG";
      this.buttonSaveAsPNG.Size = new System.Drawing.Size(62, 23);
      this.buttonSaveAsPNG.TabIndex = 3;
      this.buttonSaveAsPNG.Text = "PNG";
      this.buttonSaveAsPNG.UseVisualStyleBackColor = true;
      this.buttonSaveAsPNG.Click += new System.EventHandler(this.buttonSaveAsPNG_Click);
      // 
      // buttonSaveAsJPG
      // 
      this.buttonSaveAsJPG.Location = new System.Drawing.Point(7, 80);
      this.buttonSaveAsJPG.Name = "buttonSaveAsJPG";
      this.buttonSaveAsJPG.Size = new System.Drawing.Size(62, 23);
      this.buttonSaveAsJPG.TabIndex = 2;
      this.buttonSaveAsJPG.Text = "JPEG";
      this.buttonSaveAsJPG.UseVisualStyleBackColor = true;
      this.buttonSaveAsJPG.Click += new System.EventHandler(this.buttonSaveAsJPG_Click);
      // 
      // buttonSaveAsGIF
      // 
      this.buttonSaveAsGIF.Location = new System.Drawing.Point(7, 50);
      this.buttonSaveAsGIF.Name = "buttonSaveAsGIF";
      this.buttonSaveAsGIF.Size = new System.Drawing.Size(62, 23);
      this.buttonSaveAsGIF.TabIndex = 1;
      this.buttonSaveAsGIF.Text = "GIF";
      this.buttonSaveAsGIF.UseVisualStyleBackColor = true;
      this.buttonSaveAsGIF.Click += new System.EventHandler(this.buttonSaveAsGIF_Click);
      // 
      // buttonSaveAsBMP
      // 
      this.buttonSaveAsBMP.Location = new System.Drawing.Point(7, 20);
      this.buttonSaveAsBMP.Name = "buttonSaveAsBMP";
      this.buttonSaveAsBMP.Size = new System.Drawing.Size(62, 23);
      this.buttonSaveAsBMP.TabIndex = 0;
      this.buttonSaveAsBMP.Text = "BMP";
      this.buttonSaveAsBMP.UseVisualStyleBackColor = true;
      this.buttonSaveAsBMP.Click += new System.EventHandler(this.buttonSaveAsBMP_Click);
      // 
      // buttonPrint
      // 
      this.buttonPrint.Location = new System.Drawing.Point(13, 108);
      this.buttonPrint.Name = "buttonPrint";
      this.buttonPrint.Size = new System.Drawing.Size(75, 23);
      this.buttonPrint.TabIndex = 2;
      this.buttonPrint.Text = "Print";
      this.buttonPrint.UseVisualStyleBackColor = true;
      this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
      // 
      // buttonOpen
      // 
      this.buttonOpen.Location = new System.Drawing.Point(13, 69);
      this.buttonOpen.Name = "buttonOpen";
      this.buttonOpen.Size = new System.Drawing.Size(75, 23);
      this.buttonOpen.TabIndex = 1;
      this.buttonOpen.Text = "Open";
      this.buttonOpen.UseVisualStyleBackColor = true;
      this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
      // 
      // pictureLogo
      // 
      this.pictureLogo.BackColor = System.Drawing.SystemColors.Window;
      this.pictureLogo.Dock = System.Windows.Forms.DockStyle.Top;
      this.pictureLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureLogo.Image")));
      this.pictureLogo.Location = new System.Drawing.Point(3, 3);
      this.pictureLogo.Name = "pictureLogo";
      this.pictureLogo.Size = new System.Drawing.Size(95, 54);
      this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureLogo.TabIndex = 0;
      this.pictureLogo.TabStop = false;
      // 
      // printDialog1
      // 
      this.printDialog1.UseEXDialog = true;
      // 
      // pictureDiagram
      // 
      this.pictureDiagram.Location = new System.Drawing.Point(3, 3);
      this.pictureDiagram.Name = "pictureDiagram";
      this.pictureDiagram.Size = new System.Drawing.Size(388, 154);
      this.pictureDiagram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureDiagram.TabIndex = 2;
      this.pictureDiagram.TabStop = false;
      // 
      // panelImage
      // 
      this.panelImage.AutoScroll = true;
      this.panelImage.Controls.Add(this.pictureDiagram);
      this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelImage.Location = new System.Drawing.Point(101, 0);
      this.panelImage.Name = "panelImage";
      this.panelImage.Size = new System.Drawing.Size(788, 533);
      this.panelImage.TabIndex = 3;
      // 
      // printDocument1
      // 
      this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
      // 
      // panelTop
      // 
      this.panelTop.Controls.Add(buttonTable);
      this.panelTop.Controls.Add(this.comboDiagram);
      this.panelTop.Controls.Add(this.label1);
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelTop.Location = new System.Drawing.Point(101, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(788, 41);
      this.panelTop.TabIndex = 4;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 14);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(82, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select Diagram:";
      // 
      // comboDiagram
      // 
      this.comboDiagram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboDiagram.FormattingEnabled = true;
      this.comboDiagram.Location = new System.Drawing.Point(100, 11);
      this.comboDiagram.Name = "comboDiagram";
      this.comboDiagram.Size = new System.Drawing.Size(459, 21);
      this.comboDiagram.TabIndex = 1;
      this.comboDiagram.SelectedIndexChanged += new System.EventHandler(this.comboDiagram_SelectedIndexChanged);
      // 
      // buttonTable
      // 
      buttonTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      buttonTable.Location = new System.Drawing.Point(677, 9);
      buttonTable.Name = "buttonTable";
      buttonTable.Size = new System.Drawing.Size(99, 23);
      buttonTable.TabIndex = 3;
      buttonTable.Text = "View Elements..";
      buttonTable.UseVisualStyleBackColor = true;
      buttonTable.Click += new System.EventHandler(this.buttonTable_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.ClientSize = new System.Drawing.Size(889, 555);
      this.Controls.Add(this.panelTop);
      this.Controls.Add(this.panelImage);
      this.Controls.Add(this.panelLeft);
      this.Controls.Add(this.statusStrip1);
      this.Name = "MainForm";
      this.Text = "BPMN View";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.panelLeft.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureDiagram)).EndInit();
      this.panelImage.ResumeLayout(false);
      this.panelImage.PerformLayout();
      this.panelTop.ResumeLayout(false);
      this.panelTop.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.Panel panelLeft;
    private System.Windows.Forms.PictureBox pictureDiagram;
    private System.Windows.Forms.PictureBox pictureLogo;
    private System.Windows.Forms.PrintDialog printDialog1;
    private System.Windows.Forms.Button buttonOpen;
    private System.Windows.Forms.Button buttonPrint;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button buttonZoomIn;
    private System.Windows.Forms.Button buttonSaveAsEXIF;
    private System.Windows.Forms.Button buttonSaveAsEMF;
    private System.Windows.Forms.Button buttonSaveAsPNG;
    private System.Windows.Forms.Button buttonSaveAsJPG;
    private System.Windows.Forms.Button buttonSaveAsGIF;
    private System.Windows.Forms.Button buttonSaveAsBMP;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button buttonZoomReset;
    private System.Windows.Forms.Button buttonZoomOut;
    private System.Windows.Forms.Button buttonAbout;
    private System.Windows.Forms.Panel panelImage;
    private System.Drawing.Printing.PrintDocument printDocument1;
    private System.Windows.Forms.Panel panelTop;
    private System.Windows.Forms.ComboBox comboDiagram;
    private System.Windows.Forms.Label label1;
  }
}

