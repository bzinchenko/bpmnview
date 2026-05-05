using System.Drawing.Imaging;
using System.IO;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Shared helper used by all example classes.
    /// Saves both a .bpmn file and a rendered .png image to the output directory.
    /// </summary>
    internal static class ExampleHelper
    {
        /// <summary>
        /// Saves the model to a .bpmn file and renders diagram index 0 as a .png image.
        /// </summary>
        /// <param name="editor">The populated Editor instance.</param>
        /// <param name="outputDir">Directory to write files into.</param>
        /// <param name="baseName">File name without extension, e.g. "A.1.0".</param>
        /// <param name="scale">Rendering scale factor (default 1.5).</param>
        public static void Save(Editor editor, string outputDir, string baseName, float scale = 1.5f)
        {
            string bpmnPath = Path.Combine(outputDir, baseName + ".bpmn");
            string pngPath  = Path.Combine(outputDir, baseName + ".png");

            editor.Save(bpmnPath);

            // Render the first diagram to PNG using the Model API
            Model model = Model.Read(bpmnPath);
            if (model != null)
            {
                var image = model.GetImage(0, scale);
                if (image != null)
                    image.Save(pngPath, ImageFormat.Png);
            }
        }
    }
}
