using System.Collections.Generic;
using System.Drawing;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Diagram Layout — AddDiagram(), AddShape(), AddEdge(), AddLabel(),
    /// AddWayPoint(), UpdatePointPosition(), UpdateShapePosition(),
    /// UpdateLabelPosition(), RemoveWayPoint(), RemoveLabel(),
    /// RemoveShape(), RemoveEdge().
    ///
    /// Produces one model:
    ///   C.3.0 — Equipment repair process built programmatically then refined
    ///           using all diagram manipulation methods.
    ///
    /// MIWG reference C.3.0: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/C.3.0.png
    /// BPMN spec: §12 BPMN Notation and Diagrams · §12.1 BPMN DI
    ///            §12.2 DI Meta-model · §12.3.2 BPMNShape · §12.3.3 BPMNEdge
    /// </summary>
    public static class DiagramLayout
    {
        public static void Run(string outputDir)
        {
            CreateAndRefineLayout(outputDir);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // C.3.0 — Equipment Repair: build then refine diagram layout
        //
        // Two-phase authoring pattern (§12):
        //   Phase 1 — define semantic elements (all Add* element methods)
        //   Phase 2 — define visual layout (AddDiagram, AddShape, AddEdge)
        //
        // AddDiagram(name, resolution)
        //   Creates a BPMNDiagram + BPMNPlane (§12.2). resolution is in DPI.
        //   Returns the diagram ID used by all subsequent shape/edge calls.
        //
        // AddShape(diagramId, shape)
        //   shape.ElementRef — ID of the semantic element.
        //   shape.Bounds     — List<Rectangle> with position and size in diagram units.
        //   Returns the shape ID for subsequent label/position updates.
        //
        // AddEdge(diagramId, edge)
        //   edge.ElementRef — ID of the semantic flow element.
        //   edge.Points     — List<Point> waypoints (minimum 2: source, target).
        //   Returns the edge ID for subsequent waypoint operations.
        //
        // AddLabel(parentId, rectangle, style)
        //   Adds a label to a shape or edge at the specified RectangleF position.
        //   parentId — shape or edge ID returned by AddShape/AddEdge.
        //   style    — optional style string defined in the BPMN file; null for default.
        //
        // AddWayPoint(edgeId, point)
        //   Appends a PointF waypoint to an existing edge.
        //
        // UpdatePointPosition(edgeId, index, point)
        //   Moves the waypoint at the given index to a new PointF position.
        //   index 0 = source point, index (n-1) = target point.
        //
        // UpdateShapePosition(shapeId, rectangle)
        //   Moves and/or resizes an existing shape.
        //
        // UpdateLabelPosition(parentId, rectangle)
        //   Repositions the label attached to a shape or edge.
        //
        // RemoveWayPoint(edgeId, index)
        //   Removes the waypoint at the given index from an edge.
        //
        // RemoveLabel(parentId)
        //   Removes the label from a shape or edge.
        //
        // RemoveShape(shapeId)
        //   Removes a shape from the diagram (does not remove the semantic element).
        //
        // RemoveEdge(edgeId)
        //   Removes an edge from the diagram (does not remove the semantic flow).
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateAndRefineLayout(string outputDir)
        {
            Editor editor = new Editor();
            editor.Create("C.3.0 – Equipment Repair", "BPMN.Sharp Examples");

            // ── Semantic layer ────────────────────────────────────────────────────
            string start = editor.AddEvent(null, null, "Malfunction Reported",
                EventType.Start, EventTrigger.None, EventRole.None);

            string diagnose = editor.AddActivity(null, "Diagnose Fault",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);

            string assessGw = editor.AddGateway(null, "Repairable?",
                GatewayType.Exclusive, 1, 2);

            string repairTask = editor.AddActivity(null, "Repair Equipment",
                ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);

            string replaceTask = editor.AddActivity(null, "Replace Equipment",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            string testTask = editor.AddActivity(null, "Test Equipment",
                ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);

            string testGw = editor.AddGateway(null, "Test Passed?",
                GatewayType.Exclusive, 1, 2);

            string joinGw = editor.AddGateway(null, null,
                GatewayType.Exclusive, 2, 1);

            string closeTicket = editor.AddActivity(null, "Close Ticket",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            string end = editor.AddEvent(null, null, "Equipment Operational",
                EventType.End, EventTrigger.None, EventRole.None);

            // Timer boundary — escalate if repair takes > 4 hours
            string timerBound = editor.AddEvent(null, repairTask, "4hr Escalation",
                EventType.Intermediate, EventTrigger.Timer, EventRole.NonInterrupting);
            string escalate = editor.AddActivity(null, "Escalate to Vendor",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            string f1  = editor.AddFlow(null, null, start,      diagnose,    null,      FlowType.Sequence, null, false, FlowDirection.None);
            string f2  = editor.AddFlow(null, null, diagnose,   assessGw,    null,      FlowType.Sequence, null, false, FlowDirection.None);
            string f3  = editor.AddFlow(null, null, assessGw,   repairTask,  "Yes",     FlowType.Sequence, null, false, FlowDirection.None);
            string f4  = editor.AddFlow(null, null, assessGw,   replaceTask, "No",      FlowType.Sequence, null, true,  FlowDirection.None);
            string f5  = editor.AddFlow(null, null, repairTask, testTask,    null,      FlowType.Sequence, null, false, FlowDirection.None);
            string f6  = editor.AddFlow(null, null, replaceTask,joinGw,      null,      FlowType.Sequence, null, false, FlowDirection.None);
            string f7  = editor.AddFlow(null, null, testTask,   testGw,      null,      FlowType.Sequence, null, false, FlowDirection.None);
            string f8  = editor.AddFlow(null, null, testGw,     joinGw,      "Pass",    FlowType.Sequence, null, false, FlowDirection.None);
            string f9  = editor.AddFlow(null, null, testGw,     repairTask,  "Fail",    FlowType.Sequence, null, false, FlowDirection.None);
            string f10 = editor.AddFlow(null, null, joinGw,     closeTicket, null,      FlowType.Sequence, null, false, FlowDirection.None);
            string f11 = editor.AddFlow(null, null, closeTicket,end,         null,      FlowType.Sequence, null, false, FlowDirection.None);
            string fEsc= editor.AddFlow(null, null, timerBound, escalate,    null,      FlowType.Sequence, null, false, FlowDirection.None);

            // ── Diagram layer — initial layout ─────────────────────────────────────
            string diag = editor.AddDiagram("Diagram 1", 96f);

            // AddShape returns the shape ID needed for label and position operations
            string startShp    = editor.AddShape(diag, new Shape { ElementRef = start,
                Bounds = new List<Rectangle> { new Rectangle(20, 112, 36, 36) } });
            string diagnoseShp = editor.AddShape(diag, new Shape { ElementRef = diagnose,
                Bounds = new List<Rectangle> { new Rectangle(96, 90, 120, 80) } });
            string assessShp   = editor.AddShape(diag, new Shape { ElementRef = assessGw,
                Bounds = new List<Rectangle> { new Rectangle(256, 108, 44, 44) } });
            string repairShp   = editor.AddShape(diag, new Shape { ElementRef = repairTask,
                Bounds = new List<Rectangle> { new Rectangle(340, 90, 120, 80) } });
            string replaceShp  = editor.AddShape(diag, new Shape { ElementRef = replaceTask,
                Bounds = new List<Rectangle> { new Rectangle(340, 200, 120, 80) } });
            string testShp     = editor.AddShape(diag, new Shape { ElementRef = testTask,
                Bounds = new List<Rectangle> { new Rectangle(500, 90, 120, 80) } });
            string testGwShp   = editor.AddShape(diag, new Shape { ElementRef = testGw,
                Bounds = new List<Rectangle> { new Rectangle(660, 108, 44, 44) } });
            string joinShp     = editor.AddShape(diag, new Shape { ElementRef = joinGw,
                Bounds = new List<Rectangle> { new Rectangle(660, 218, 44, 44) } });
            string closeShp    = editor.AddShape(diag, new Shape { ElementRef = closeTicket,
                Bounds = new List<Rectangle> { new Rectangle(744, 100, 120, 80) } });
            string endShp      = editor.AddShape(diag, new Shape { ElementRef = end,
                Bounds = new List<Rectangle> { new Rectangle(904, 122, 36, 36) } });

            // Boundary + escalation
            string timerShp    = editor.AddShape(diag, new Shape { ElementRef = timerBound,
                Bounds = new List<Rectangle> { new Rectangle(382, 152, 36, 36) } });
            string escalateShp = editor.AddShape(diag, new Shape { ElementRef = escalate,
                Bounds = new List<Rectangle> { new Rectangle(340, 230, 120, 80) } });

            // AddEdge returns the edge ID needed for waypoint operations
            string e1  = editor.AddEdge(diag, new Edge { ElementRef = f1,
                Points = new List<Point> { new Point(56, 130), new Point(96, 130) } });
            string e3  = editor.AddEdge(diag, new Edge { ElementRef = f3,
                Points = new List<Point> { new Point(300, 130), new Point(340, 130) } });
            string e4  = editor.AddEdge(diag, new Edge { ElementRef = f4,
                Points = new List<Point> { new Point(278, 152), new Point(278, 240), new Point(340, 240) } });
            string e9  = editor.AddEdge(diag, new Edge { ElementRef = f9,
                Points = new List<Point> { new Point(682, 152), new Point(682, 180), new Point(400, 180), new Point(400, 170) } });

            // ── AddLabel — add explicit labels to selected edges ──────────────────
            // Labels can be placed anywhere relative to their parent edge.
            // style=null uses the diagram's default label style.
            string e3Label = editor.AddLabel(e3,
                new RectangleF(310, 108, 40, 20));    // "Yes" label above the flow

            string e4Label = editor.AddLabel(e4,
                new RectangleF(260, 190, 30, 20));    // "No" label beside the bend

            // ── Refinements using update/remove methods ───────────────────────────

            // UpdateShapePosition — move the escalate task slightly right
            // Editor.UpdateShapePosition takes RectangleF (float)
            editor.UpdateShapePosition(escalateShp,
                new RectangleF(360, 240, 120, 80));

            // UpdateLabelPosition — nudge the "Yes" label
            // Editor.UpdateLabelPosition takes RectangleF (float)
            editor.UpdateLabelPosition(e3Label,
                new RectangleF(312, 110, 40, 20));

            // AddWayPoint — add an intermediate waypoint to the fail-loop edge
            // Editor.AddWayPoint takes PointF (float)
            editor.AddWayPoint(e9, new PointF(682, 290));

            // UpdatePointPosition — adjust the existing bend point index 1
            // Editor.UpdatePointPosition takes PointF (float)
            editor.UpdatePointPosition(e9, 1,
                new PointF(682, 195));

            // RemoveWayPoint — remove the redundant intermediate point we just added
            editor.RemoveWayPoint(e9, 3);

            // RemoveLabel — remove label from e4 (will rely on gateway name instead)
            editor.RemoveLabel(e4Label);

            // Save the refined model
            ExampleHelper.Save(editor, outputDir, "C.3.0");
        }
    }
}
