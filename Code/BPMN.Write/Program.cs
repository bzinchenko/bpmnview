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
using System.IO;
using System.Text;
using System.Drawing;
using BPMN;

namespace BPMN.Write
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        System.Console.WriteLine("BPMN.Write: Create simple BPMN diagram");
        System.Console.WriteLine("Copyright © Boris Zinchenko, 2018");
        System.Console.WriteLine("Usege: BPMN.Write [file name]");
        return;
      }

      try
      {
        string fileName = args[0];
        bool ok = Create(fileName);
        if (ok) System.Console.Write("File created successfully!");
        else System.Console.Write("Error creating file!");
      }
      catch (Exception ex)
      {
        System.Console.Write("Unhandled excepetion!");
        System.Console.Write(ex.Message);
      }
    }

    public static bool Create(string fileName)
    {
      Editor editor = new Editor();
      editor.Create("BPMN Model", "User");

      string id1 = editor.AddEvent(null, null, "Start Event", EventType.Start, EventTrigger.None, EventRole.None);
      string id2 = editor.AddActivity(null, "Task 1", ActivityType.Task, ActivityMarker.None, TaskType.User, null);
      string id3 = editor.AddActivity(null, "Task 2", ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);
      string id4 = editor.AddActivity(null, "Task 3", ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
      string id5 = editor.AddEvent(null, null, "End Event", EventType.End, EventTrigger.None, EventRole.None);
      //string id6 = editor.AddGateway(null, "Gate 1", GatewayType.Exclusive);
      string id7 = editor.AddFlow(null, null, id1, id2, null, FlowType.Sequence, null, false, FlowDirection.None);
      string id8 = editor.AddFlow(null, null, id2, id3, null, FlowType.Sequence, null, false, FlowDirection.None);
      string id9 = editor.AddFlow(null, null, id3, id4, null, FlowType.Sequence, null, false, FlowDirection.None);
      string id10 = editor.AddFlow(null, null, id4, id5, null, FlowType.Sequence, null, false, FlowDirection.None);

      string id = editor.AddDiagram("Test 1", 96);

      Shape shape = new Shape();
      Rectangle rect = new Rectangle(10, 10, 30, 30);
      shape.Bounds = new List<Rectangle>();
      shape.Bounds.Add(rect);
      shape.ElementRef = id1;
      editor.AddShape(id, shape);

      rect.Width = 70;
      rect.Offset(60, 0);
      shape.Bounds[0] = rect;
      shape.ElementRef = id2;
      editor.AddShape(id, shape);

      rect.Offset(100, 0);
      shape.Bounds[0] = rect;
      shape.ElementRef = id3;
      editor.AddShape(id, shape);

      rect.Offset(100, 0);
      shape.Bounds[0] = rect;
      shape.ElementRef = id4;
      editor.AddShape(id, shape);

      rect.Width = 30;
      rect.Offset(100, 0);
      shape.Bounds[0] = rect;
      shape.ElementRef = id5;
      editor.AddShape(id, shape);

      List<Point> points = new List<Point>();
      points.Add(new Point()); points.Add(new Point());
      points[0] = new Point(40, 25); points[1] = new Point(70, 25);
      Edge edge = new Edge() { ElementRef = id7, Points = points }; 
      editor.AddEdge(id, edge);

      points[0] = new Point(140, 25); points[1] = new Point(170, 25);
      edge = new Edge() { ElementRef = id8, Points = points }; 
      editor.AddEdge(id, edge);

      points[0] = new Point(240, 25); points[1] = new Point(270, 25);
      edge = new Edge() { ElementRef = id9, Points = points }; 
      editor.AddEdge(id, edge);

      points[0] = new Point(340, 25); points[1] = new Point(370, 25);
      edge = new Edge() { ElementRef = id10, Points = points }; 
      editor.AddEdge(id, edge);

      return editor.Save(fileName);
    }
  }
}
