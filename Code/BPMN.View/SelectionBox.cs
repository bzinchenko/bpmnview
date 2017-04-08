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
using System.Windows.Forms;
using System.Drawing;

namespace BPMN.View
{
  [System.ComponentModel.DesignerCategory("Code")]
  public class SelectionBox : Panel
  {
    protected override void OnPaint(PaintEventArgs e)
    {
      const int penWidth = 2;
      int offset = penWidth - 1;
      using (Pen pen = new Pen(Color.Red, 2))
        e.Graphics.DrawRectangle(pen, offset, offset, 
          ClientSize.Width - offset - 1, ClientSize.Height - offset - 1);
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams cp = base.CreateParams;
        cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
        return cp;
      }
    }
  }
}
