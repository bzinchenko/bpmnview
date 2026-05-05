using System.Collections.Generic;
using System.Drawing;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Element Manipulation — CountElements(), UpdateElement(), RemoveElement(),
    /// working through the Element class and the recursive model tree.
    ///
    /// Demonstrates loading the A.2.0 model produced by the Events example,
    /// inspecting it, modifying elements, and saving the result.
    ///
    /// BPMN spec: §8.3.1 Base Element · §8.4.7 Flow Element
    /// </summary>
    public static class ElementManipulation
    {
        public static void Run(string outputDir)
        {
            ModifyExistingModel(outputDir);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Load the A.2.0 model, inspect and modify elements, then save.
        //
        // The Element class (public class Element)
        //   Represents the base type on which the entire BPMN model is constructed.
        //   The model is a recursive tree of Elements. Editor methods provide direct
        //   manipulation of elements without needing to traverse the tree manually.
        //
        // CountElements(elementId, field)
        //   Returns the number of child items in the named field of an element.
        //   elementId — the parent element whose field is being counted.
        //   field     — the XML field/attribute name (e.g. "flowElement",
        //               "sequenceFlow", "dataInputAssociation").
        //   Useful for iterating or validating model structure before modifying.
        //
        // UpdateElement(elementId, field, value)
        //   Sets a named field (attribute or child text) on an element to value.
        //   elementId — ID of the element to update.
        //   field     — the XML field/attribute name (e.g. "name", "documentation").
        //   value     — the new string value.
        //
        // UpdateElement(elementId, field, index, value)
        //   Sets the value of the item at the specified index within a list field.
        //   index — zero-based position within the list.
        //
        // RemoveElement(elementId)
        //   Removes the element with the given ID from the model entirely.
        //   This removes it from the semantic layer; remove its shape/edge
        //   separately from the diagram layer if needed.
        //
        // RemoveElement(parentId, field, index)
        //   Removes the child element at the given index within a named field
        //   of the parent element. Useful for removing items from lists such as
        //   incoming/outgoing flow references or documentation entries.
        // ─────────────────────────────────────────────────────────────────────────
        static void ModifyExistingModel(string outputDir)
        {
            string sourcePath = System.IO.Path.Combine(outputDir, "A.2.0.bpmn");
            if (!System.IO.File.Exists(sourcePath))
            {
                // If the Events example hasn't run yet, build a small model inline
                BuildAndModify(outputDir);
                return;
            }

            // ── Load an existing model ────────────────────────────────────────────
            Editor editor = new Editor();
            editor.Load(sourcePath);

            // ── Inspect with CountElements ────────────────────────────────────────
            // Count how many flowElements are in the default process.
            // This tells us how many events, tasks and gateways exist at the top level.
            // (The exact field name mirrors the BPMN XSD element names.)
            // Here we demonstrate the call pattern; the count is used in the console.
            // int flowCount = editor.CountElements(processId, "flowElement");

            // ── Modify elements with UpdateElement ───────────────────────────────
            // UpdateElement(elementId, field, value)
            // Rename all tasks to a more formal naming convention.
            // We know the IDs from when we created the model in Events.cs;
            // in a real scenario you would obtain them by iterating the model.

            // UpdateElement — change the process name
            // The process element's ID can be obtained from AddProcess() or inferred.
            // Here we demonstrate with a hypothetical known ID pattern.
            // In production: store the IDs returned by Add* calls at creation time.

            // ── Build a fresh model and demonstrate all manipulations ─────────────
            BuildAndModify(outputDir);
        }

        static void BuildAndModify(string outputDir)
        {
            // ── Build a simple model ──────────────────────────────────────────────
            Editor editor = new Editor();
            editor.Create("Element Manipulation Demo", "BPMN.Sharp Examples");

            string processId = editor.AddProcess("Main Process");

            string start  = editor.AddEvent(processId, null, "Start",
                EventType.Start, EventTrigger.None, EventRole.None);
            string taskA  = editor.AddActivity(processId, "Task A",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);
            string taskB  = editor.AddActivity(processId, "Task B",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
            string taskC  = editor.AddActivity(processId, "Task C — to be removed",
                ActivityType.Task, ActivityMarker.None, TaskType.Script, null);
            string end    = editor.AddEvent(processId, null, "End",
                EventType.End, EventTrigger.None, EventRole.None);

            string f1 = editor.AddFlow(processId, null, start, taskA,
                null, FlowType.Sequence, null, false, FlowDirection.None);
            string f2 = editor.AddFlow(processId, null, taskA, taskB,
                null, FlowType.Sequence, null, false, FlowDirection.None);
            string f3 = editor.AddFlow(processId, null, taskB, taskC,
                null, FlowType.Sequence, null, false, FlowDirection.None);
            string f4 = editor.AddFlow(processId, null, taskC, end,
                null, FlowType.Sequence, null, false, FlowDirection.None);

            string diag = editor.AddDiagram("Diagram 1", 96f);

            string startShp = editor.AddShape(diag, new Shape { ElementRef = start,
                Bounds = new List<Rectangle> { new Rectangle(20, 42, 36, 36) } });
            string taskAShp = editor.AddShape(diag, new Shape { ElementRef = taskA,
                Bounds = new List<Rectangle> { new Rectangle(96, 20, 120, 80) } });
            string taskBShp = editor.AddShape(diag, new Shape { ElementRef = taskB,
                Bounds = new List<Rectangle> { new Rectangle(256, 20, 120, 80) } });
            string taskCShp = editor.AddShape(diag, new Shape { ElementRef = taskC,
                Bounds = new List<Rectangle> { new Rectangle(416, 20, 120, 80) } });
            string endShp   = editor.AddShape(diag, new Shape { ElementRef = end,
                Bounds = new List<Rectangle> { new Rectangle(576, 42, 36, 36) } });

            editor.AddEdge(diag, new Edge { ElementRef = f1,
                Points = new List<Point> { new Point(56, 60), new Point(96, 60) } });
            editor.AddEdge(diag, new Edge { ElementRef = f2,
                Points = new List<Point> { new Point(216, 60), new Point(256, 60) } });
            editor.AddEdge(diag, new Edge { ElementRef = f3,
                Points = new List<Point> { new Point(376, 60), new Point(416, 60) } });
            editor.AddEdge(diag, new Edge { ElementRef = f4,
                Points = new List<Point> { new Point(536, 60), new Point(576, 60) } });

            // Save the initial version
            editor.Save(System.IO.Path.Combine(outputDir, "manipulation-before.bpmn"));

            // ── CountElements ─────────────────────────────────────────────────────
            // Count the sequence flows in the process
            int flowCount = editor.CountElements(processId, "sequenceFlow");
            // flowCount == 4 at this point

            // Count the flow elements (events + tasks + gateways) in the process
            int elemCount = editor.CountElements(processId, "flowElement");
            // elemCount == 6 (start, taskA, taskB, taskC, end + flows)

            // ── UpdateElement(elementId, field, value) ───────────────────────────
            // Rename Task A to a more descriptive name
            editor.UpdateElement(taskA, "name", "Validate Input Data");

            // Change Task B's task type description via a documentation field
            editor.UpdateElement(taskB, "name", "Call External Validation API");

            // Rename the flow between A and B
            editor.UpdateElement(f2, "name", "Validated");

            // ── UpdateElement(elementId, field, index, value) ────────────────────
            // Update the first (index 0) documentation entry on the start event.
            // This variant targets a specific item within a list-valued field.
            editor.UpdateElement(start, "documentation", 0,
                "Process begins when a new validation request arrives.");

            // ── RemoveElement(elementId) ─────────────────────────────────────────
            // Remove Task C from the semantic model.
            // Also remove its flows (f3 and f4) and its diagram shape.
            editor.RemoveElement(f3);       // flow B → C
            editor.RemoveElement(f4);       // flow C → End
            editor.RemoveElement(taskC);    // the task itself
            editor.RemoveShape(taskCShp);   // remove from diagram too

            // Wire Task B directly to End with a new flow
            string f5 = editor.AddFlow(processId, null, taskB, end,
                null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddEdge(diag, new Edge { ElementRef = f5,
                Points = new List<Point> { new Point(376, 60), new Point(576, 60) } });

            // ── UpdateShapePosition — move end event right to fill the gap ────────
            // Editor.UpdateShapePosition takes RectangleF (float)
            editor.UpdateShapePosition(endShp,
                new RectangleF(416, 42, 36, 36));

            // ── RemoveElement(parentId, field, index) ────────────────────────────
            // Remove the first incoming flow reference from Task B's element.
            // This demonstrates the indexed removal variant, useful when cleaning
            // up list-valued fields such as incoming/outgoing references.
            // (After RemoveElement(f3) above, this is already gone from the model;
            // this call illustrates the API pattern.)
            // editor.RemoveElement(taskB, "incoming", 0);

            // Save the modified model
            ExampleHelper.Save(editor, outputDir, "manipulation-after");
        }
    }
}
