using System.Collections.Generic;
using System.Drawing;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Getting Started — Editor lifecycle: Create, Save, Load, Normalize, Serialize/Parse.
    ///
    /// Produces a model equivalent to MIWG reference A.1.0:
    /// a simple sequential process with a none start event, two tasks, and a none end event.
    ///
    /// MIWG reference: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.1.0.png
    /// BPMN spec: §8.2 Infrastructure · §8.3 Foundation · §10.2 Basic Process Concepts
    /// </summary>
    public static class GettingStarted
    {
        public static void Run(string outputDir)
        {
            // ── Example 1: Create a new model and save it ─────────────────────────
            CreateAndSave(outputDir);

            // ── Example 2: Load an existing model and normalize it ────────────────
            LoadAndNormalize(outputDir);

            // ── Example 3: Round-trip via XML string (Parse / Serialize) ──────────
            SerializeRoundtrip(outputDir);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Example 1
        // Create a new BPMN model from scratch and save it as a file.
        //
        // editor.Create()  — §8.2.1 Definitions: initialises the top-level
        //                    <definitions> root element.
        // editor.Save()    — §15.3 XSD: serialises to a BPMN 2.0 XML file.
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateAndSave(string outputDir)
        {
            Editor editor = new Editor();

            // Create() initialises a new definitions root with an optional name and
            // author (stored as the 'exporter' attribute in the XML).
            editor.Create("A.1.0 – Getting Started", "BPMN.Sharp Examples");

            // ── Semantic layer ────────────────────────────────────────────────────
            // Add elements to the default process (processId = null).
            // Every Add* method returns the auto-generated ID of the new element.

            string startId = editor.AddEvent(
                null,                   // processId  — null = default process
                null,                   // attachedToRef — null = not a boundary event
                "Start Event",
                EventType.Start,
                EventTrigger.None,      // plain, no trigger marker
                EventRole.None);

            string task1Id = editor.AddActivity(
                null,                   // processId
                "Task 1",
                ActivityType.Task,
                ActivityMarker.None,
                TaskType.None,          // abstract task
                null);                  // calledElement — only for CallActivity

            string task2Id = editor.AddActivity(
                null, "Task 2",
                ActivityType.Task, ActivityMarker.None, TaskType.None, null);

            string endId = editor.AddEvent(
                null, null, "End Event",
                EventType.End, EventTrigger.None, EventRole.None);

            // AddFlow: parentId=null uses the default process.
            // messageId=null (only needed for Message flows).
            // isDefault=false, direction=None (only for Associations).
            string f1 = editor.AddFlow(null, null, startId, task1Id,
                null, FlowType.Sequence, null, false, FlowDirection.None);
            string f2 = editor.AddFlow(null, null, task1Id, task2Id,
                null, FlowType.Sequence, null, false, FlowDirection.None);
            string f3 = editor.AddFlow(null, null, task2Id, endId,
                null, FlowType.Sequence, null, false, FlowDirection.None);

            // ── Diagram layer (BPMN DI — §12) ────────────────────────────────────
            // AddDiagram creates a BPMNDiagram + BPMNPlane in the DI layer.
            // Resolution is in DPI; 96 is the standard screen resolution.
            string diagId = editor.AddDiagram("Diagram 1", 96f);

            // AddShape links a visual BPMNShape to a semantic element via ElementRef.
            // Shape.Bounds uses List<Rectangle> (integer coordinates).
            editor.AddShape(diagId, new Shape
            {
                ElementRef = startId,
                Bounds = new List<Rectangle> { new Rectangle(30, 62, 36, 36) }
            });
            editor.AddShape(diagId, new Shape
            {
                ElementRef = task1Id,
                Bounds = new List<Rectangle> { new Rectangle(120, 45, 100, 80) }
            });
            editor.AddShape(diagId, new Shape
            {
                ElementRef = task2Id,
                Bounds = new List<Rectangle> { new Rectangle(270, 45, 100, 80) }
            });
            editor.AddShape(diagId, new Shape
            {
                ElementRef = endId,
                Bounds = new List<Rectangle> { new Rectangle(420, 62, 36, 36) }
            });

            // AddEdge links a visual BPMNEdge to a semantic flow via ElementRef.
            // Points are waypoints: minimum two (source, target), more for bends.
            editor.AddEdge(diagId, new Edge
            {
                ElementRef = f1,
                Points = new List<Point>
                {
                    new Point(66, 80),
                    new Point(120, 80)
                }
            });
            editor.AddEdge(diagId, new Edge
            {
                ElementRef = f2,
                Points = new List<Point>
                {
                    new Point(220, 80),
                    new Point(270, 80)
                }
            });
            editor.AddEdge(diagId, new Edge
            {
                ElementRef = f3,
                Points = new List<Point>
                {
                    new Point(370, 80),
                    new Point(420, 80)
                }
            });

            ExampleHelper.Save(editor, outputDir, "A.1.0");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Example 2
        // Load an existing .bpmn file and save a normalized copy.
        //
        // editor.Load()      — reads a BPMN 2.0 XML file from disk.
        // editor.Normalize() — strips vendor-specific extensions (e.g. Camunda,
        //                      Signavio, W4 attributes) and re-saves in clean
        //                      standard BPMN 2.0 form. Useful when importing
        //                      models from third-party tools.
        // editor.Refresh()   — reloads the current document from disk, discarding
        //                      any unsaved in-memory changes.
        // ─────────────────────────────────────────────────────────────────────────
        static void LoadAndNormalize(string outputDir)
        {
            string sourcePath = System.IO.Path.Combine(outputDir, "A.1.0.bpmn");
            if (!System.IO.File.Exists(sourcePath))
                return; // created by CreateAndSave above

            Editor editor = new Editor();
            editor.Load(sourcePath);

            // Refresh() reloads the document from its last saved state.
            editor.Refresh();
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Example 3
        // Parse a BPMN model from an XML string and serialize it back.
        //
        // editor.Parse()     — loads a BPMN 2.0 model from an in-memory XML string
        //                      instead of a file. Useful for web services, databases,
        //                      or any scenario where the BPMN content is not on disk.
        // editor.Serialize() — serialises the current model to an XML string
        //                      instead of a file.
        // ─────────────────────────────────────────────────────────────────────────
        static void SerializeRoundtrip(string outputDir)
        {
            // Build a minimal model and serialize it to a string
            Editor editor = new Editor();
            editor.Create("Serialize Example", "BPMN.Sharp Examples");

            string s = editor.AddEvent(null, null, "Start",
                EventType.Start, EventTrigger.None, EventRole.None);
            string t = editor.AddActivity(null, "Do Work",
                ActivityType.Task, ActivityMarker.None, TaskType.None, null);
            string e = editor.AddEvent(null, null, "End",
                EventType.End, EventTrigger.None, EventRole.None);

            editor.AddFlow(null, null, s, t, null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, t, e, null, FlowType.Sequence, null, false, FlowDirection.None);

            string diagId = editor.AddDiagram("D", 96f);
            editor.AddShape(diagId, new Shape { ElementRef = s, Bounds = new List<Rectangle> { new Rectangle(20, 42, 36, 36) } });
            editor.AddShape(diagId, new Shape { ElementRef = t, Bounds = new List<Rectangle> { new Rectangle(100, 20, 100, 80) } });
            editor.AddShape(diagId, new Shape { ElementRef = e, Bounds = new List<Rectangle> { new Rectangle(250, 42, 36, 36) } });

            // Serialize() returns the full BPMN 2.0 XML as a string.
            // This is ideal for storing BPMN in a database or sending over HTTP.
            string xml = editor.Serialize();

            // Parse() loads a model from that XML string.
            // The resulting model is identical to loading from a file.
            Editor editor2 = new Editor();
            editor2.Parse(xml);

            // Verify round-trip by saving
            editor2.Save(System.IO.Path.Combine(outputDir, "A.1.0-roundtrip.bpmn"));
        }
    }
}
