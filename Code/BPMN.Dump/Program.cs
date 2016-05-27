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
