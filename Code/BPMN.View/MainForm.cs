// 
// The MIT License (MIT)
//
// Copyright (c) 2017 Boris Zinchenko
// mail: info@caseagile.com
// web: http://www.caseagile.com
// code: https://github.com/bzinchenko/bpmnview
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace BPMN.View
{
  public partial class MainForm : Form
  {
    private Model model;
    private Image diagramImage;
    private double zoomRatio;
    private float scale = 2.0f;

    public MainForm()
    {
      InitializeComponent();
      splitMain.Panel2Collapsed = true;
      ctlElement.Visible = false;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      string[] args = Environment.GetCommandLineArgs();
      if (args.Length > 1) OpenFile(args[1]);
    }

    private void OpenFile(string file)
    {
      try
      {
        splitMain.Panel2Collapsed = true;

        model = BPMN.Model.Read(file);
        TreeNode node = ModelTree.AddElementToTree(treeModel, null, model.Root);
        if (node != null) node.Expand();
        ctlElement.Init(model);
        splitMain.Panel2Collapsed = false;

        foreach (Diagram dia in model.Diagrams)
          comboDiagram.Items.Add(dia.Name);
        if (comboDiagram.Items.Count > 0)
          comboDiagram.SelectedIndex = 0;
        
        this.Text = "BPMN View - " + file;
      }
      catch (Exception ex)
      {
        this.Text = "BPMN View";
        MessageBox.Show("Error opening file!");
      }
    }

    private void buttonOpen_Click(object sender, EventArgs e)
    {
      openFileDialog1.FileName = "";
      openFileDialog1.CheckPathExists = true;
      openFileDialog1.Filter = "BPMN Files|*.bpmn|All files (*.*)|*.*";
      openFileDialog1.Title = "Select a BPMN File"; 
      DialogResult result = openFileDialog1.ShowDialog();
      if (result == DialogResult.OK)
      {
        comboDiagram.Items.Clear();
        OpenFile(openFileDialog1.FileName);
      }
    }

    private void comboDiagram_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (model != null)
      {
        int idx = comboDiagram.SelectedIndex;
        if(idx >= 0) diagramImage = model.GetImage(idx, scale);
        ZoomReset();
      }
    }

    private void buttonPrint_Click(object sender, EventArgs e)
    {
      printDocument1.DocumentName = this.Text;
      printDialog1.Document = printDocument1;
      DialogResult result = printDialog1.ShowDialog();
      if (result == DialogResult.OK)
      {
        printDocument1.Print();
      }
    }

    private void SaveAs(System.Drawing.Imaging.ImageFormat format)
    {
      if (diagramImage == null)
      { 
        MessageBox.Show("No model loaded!");
        return;
      }

      string extension = format.ToString().ToLower();
      saveFileDialog1.CheckPathExists = true;
      saveFileDialog1.OverwritePrompt = true;
      saveFileDialog1.DefaultExt = extension;
      saveFileDialog1.Filter = "Image files (*." + extension + ")|*." + extension;
      saveFileDialog1.Title = "Enter image file name.";

      DialogResult result = saveFileDialog1.ShowDialog();
      if (result == DialogResult.OK)
      {
        string file = saveFileDialog1.FileName;
        try
        {
          diagramImage.Save(file, format);
        }
        catch (Exception ex)
        {
          MessageBox.Show("Error saving file!");
        }
      }
    }

    private void buttonSaveAsBMP_Click(object sender, EventArgs e)
    {
      SaveAs(System.Drawing.Imaging.ImageFormat.Bmp);
    }

    private void buttonSaveAsGIF_Click(object sender, EventArgs e)
    {
      SaveAs(System.Drawing.Imaging.ImageFormat.Gif);
    }

    private void buttonSaveAsJPG_Click(object sender, EventArgs e)
    {
      SaveAs(System.Drawing.Imaging.ImageFormat.Jpeg);
    }

    private void buttonSaveAsPNG_Click(object sender, EventArgs e)
    {
      SaveAs(System.Drawing.Imaging.ImageFormat.Png);
    }

    private void buttonSaveAsEMF_Click(object sender, EventArgs e)
    {
      SaveAs(System.Drawing.Imaging.ImageFormat.Emf);
    }

    private void buttonSaveAsEXIF_Click(object sender, EventArgs e)
    {
      SaveAs(System.Drawing.Imaging.ImageFormat.Exif);
    }

    private void buttonZoomIn_Click(object sender, EventArgs e)
    {
      double ratio = 1.1 * zoomRatio;
      Zoom(ratio);
    }

    private void buttonZoomOut_Click(object sender, EventArgs e)
    {
      double ratio = 0.9 * zoomRatio;
      Zoom(ratio);
    }

    private void buttonZoomReset_Click(object sender, EventArgs e)
    {
      ZoomReset();
    }

    private void buttonAbout_Click(object sender, EventArgs e)
    {
      AboutBox about = new AboutBox();
      about.ShowDialog(this);
    }

    private void ZoomReset()
    {
      if (diagramImage == null) return;

      double zoomH = (double)(panelImage.Width - 20) / diagramImage.Width;
      double zoomV = (double)(panelImage.Height - 20) / diagramImage.Height;

      double ratio = Math.Min(zoomH, zoomV);
      Zoom(ratio);
    }
    
    private void Zoom(double ratio)
    {
      if (diagramImage == null) return;

      int targetWidth = (int)(ratio * diagramImage.Width);
      int targetHeight = (int)(ratio * diagramImage.Height);

      Bitmap bmp = ResizeImage(diagramImage, targetWidth, targetHeight); 
      pictureDiagram.Image = bmp;

      zoomRatio = ratio;
    }
    
    private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
      if (diagramImage == null) return;

      double zoomH = (double)(e.MarginBounds.Width - 20) / diagramImage.Width;
      double zoomV = (double)(e.MarginBounds.Height - 20) / diagramImage.Height;
      double zoomPrint = Math.Min(zoomH, zoomV);
      
      int targetWidth = (int)(zoomPrint * diagramImage.Width);
      int targetHeight = (int)(zoomPrint * diagramImage.Height);

      e.Graphics.DrawImage(diagramImage, 0, 0);
    }
    
    public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
    {
      var destRect = new Rectangle(0, 0, width, height);
      var destImage = new Bitmap(width, height);

      destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

      using (var graphics = Graphics.FromImage(destImage))
      {
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
        {
          wrapMode.SetWrapMode(WrapMode.TileFlipXY);
          graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        }
      }

      return destImage;
    }

    private void buttonTable_Click(object sender, EventArgs e)
    {
      //ElementsForm form = new ElementsForm(model);
      ModelForm form = new ModelForm(model);
      form.ShowDialog();
    }

    private void pictureDiagram_Click(object sender, EventArgs e)
    {
      MouseEventArgs me = (MouseEventArgs)e;
      Point pt = me.Location;
      pt.X = (int)(pt.X / zoomRatio / scale);
      pt.Y = (int)(pt.Y / zoomRatio / scale);

      if (model != null)
      {
        int idx = comboDiagram.SelectedIndex;
        if (idx >= 0)
        {
          string id = null;
          Diagram dia = model.Diagrams[idx];
          
          Shape shape = BPMN.Geometry.ShapeAtPoint(dia, pt);
          if (shape != null)
          {
            id = shape.ElementRef;
          }
          else
          {
            List<Edge> edges = BPMN.Geometry.EdgesAtPoint(dia, pt, 1);
            if (edges != null && edges.Count > 0)
              id = edges[0].ElementRef;
          }

          Element element = ModelTree.ElementByID(model, id);
          if (element != null)
          {
            ctlElement.ViewElement(element);
            ctlElement.Visible = true;

            TreeNode node = ModelTree.NodeForElement(treeModel.Nodes[0], element);
            if (node != null) treeModel.SelectedNode = node;
          }
          else ctlElement.Visible = false;
        }
      }
    }

    private void treeModel_AfterSelect(object sender, TreeViewEventArgs e)
    {
      ctlElement.Cleanup();
      TreeNode node = treeModel.SelectedNode;
      if (node != null && node.Tag != null)
      {
        Element elm = node.Tag as Element;
        ctlElement.ViewElement(elm);
        ctlElement.Visible = true;
      }
      else ctlElement.Visible = false;
    }

  }
}
