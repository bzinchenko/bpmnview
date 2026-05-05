using System;
using System.IO;

namespace BPMN.Examples
{
    /// <summary>
    /// BPMN.Sharp Editor API — Code Examples
    ///
    /// Each example class corresponds to an article in the documentation.
    /// Running this program generates .bpmn and .png files in the Output folder.
    ///
    /// Reference models are taken from the BPMN Model Interchange Working Group (BPMN MIWG)
    /// official test suite: https://github.com/bpmn-miwg/bpmn-miwg-test-suite
    ///
    /// BPMN 2.0 Specification: https://www.omg.org/spec/BPMN/2.0.2/PDF
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Resolve output directory relative to the executable
            string outputDir = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Output");

            Directory.CreateDirectory(outputDir);

            Console.WriteLine("BPMN.Sharp Examples");
            Console.WriteLine("===================");
            Console.WriteLine($"Output directory: {outputDir}");
            Console.WriteLine();

            Run("01 – Getting Started",        () => GettingStarted.Run(outputDir));
            Run("02 – Events",                 () => Events.Run(outputDir));
            Run("03 – Activities",             () => Activities.Run(outputDir));
            Run("04 – Gateways",               () => Gateways.Run(outputDir));
            Run("05 – Flows",                  () => Flows.Run(outputDir));
            Run("06 – Pools and Lanes",        () => PoolsAndLanes.Run(outputDir));
            Run("07 – Data and Artifacts",     () => DataAndArtifacts.Run(outputDir));
            Run("08 – Diagram Layout",         () => DiagramLayout.Run(outputDir));
            Run("09 – Element Manipulation",   () => ElementManipulation.Run(outputDir));

            Console.WriteLine();
            Console.WriteLine("Done. Open the Output folder to view the generated files.");
        }

        static void Run(string name, Action example)
        {
            Console.Write($"  {name} ... ");
            try
            {
                example();
                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FAILED: {ex.Message}");
            }
        }
    }
}
