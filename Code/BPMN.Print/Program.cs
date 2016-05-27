using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace BPMN.Print
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        System.Console.WriteLine("BPMN.Print: Converts your BPMN diagrams into images");
        System.Console.WriteLine("Copyright © Boris Zinchenko, 2016");
        System.Console.WriteLine("Usege: BPMN.Print [input folder] [output folder] [format]");
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

        var format = System.Drawing.Imaging.ImageFormat.Png;
        if (args.Length > 2)
        {
          string extension = args[2].ToLower();
          foreach (var fmt in Enum.GetValues(typeof(System.Drawing.Imaging.ImageFormat)))
          {
            var currFormat = (System.Drawing.Imaging.ImageFormat)fmt;
            if (currFormat.ToString().ToLower() == extension)
            {
              format = currFormat;
              break;
            }
          }
        }

        string[] files = Directory.GetFiles(inputPath, "*.bpmn");
        foreach (string file in files)
        {
          Model model = BPMN.Model.Read(file);
          Image img = model.GetImage(0, 2.0f);

          string outputFile = outputPath + 
            Path.GetFileNameWithoutExtension(file) + 
            "." + format.ToString().ToLower();
          
          img.Save(outputFile, format);
        }
      }
      catch(Exception ex) 
      {
        System.Console.Write("Unhandled excepetion!");
        System.Console.Write(ex.Message);
      }
    }
  }
}
