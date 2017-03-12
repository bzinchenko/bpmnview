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

namespace BPMN.Dump
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        System.Console.WriteLine("BPMN.Dump: Dump contenst of BPMN diagrams into text files");
        System.Console.WriteLine("Copyright © Boris Zinchenko, 2016");
        System.Console.WriteLine("Usege: BPMN.Dumpt [input folder] [output folder]");
        return;
      }

      try
      {
        string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        string inputPath = args.Length > 0 ? args[0] : exePath;
        if (!Directory.Exists(inputPath))
        {
          System.Console.WriteLine("Invalid input path!");
          return;
        }

        string outputPath = args.Length > 1 ? args[1] : exePath;
        if (!Directory.Exists(outputPath))
        {
          System.Console.WriteLine("Invalid output path!");
          return;
        }

        string[] files = Directory.GetFiles(inputPath, "*.bpmn");
        foreach (string file in files)
        {
          Model model = BPMN.Model.Read(file);

          string outputFile = outputPath +
            Path.GetFileNameWithoutExtension(file) + ".txt";

          // todo:
        }
      }
      catch (Exception ex)
      {
        System.Console.Write("Unhandled excepetion!");
        System.Console.Write(ex.Message);
      }
    }
  }
}
