using System.Collections.Generic;
using System.Drawing;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Pools and Lanes — AddPool(), AddLane(), AddToLane(), AddCollaboration(), AddProcess().
    ///
    /// Produces one model:
    ///   B.2.0 — Procurement collaboration with lanes, matching the MIWG B.2.0 reference.
    ///           Customer pool has Sales and Finance lanes; Supplier pool has one lane.
    ///
    /// MIWG reference B.2.0: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/B.2.0.png
    /// BPMN spec: §9.3 Pool and Participant · §9.3.1 Participants · §9.3.2 Lanes · §10.8 Lanes
    /// </summary>
    public static class PoolsAndLanes
    {
        public static void Run(string outputDir)
        {
            CreateProcurementCollaboration(outputDir);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // B.2.0 — Procurement Collaboration with Lanes
        //
        // AddCollaboration(name)
        //   Creates a named Collaboration element (§9.1). In a multi-pool model,
        //   a Collaboration is the container for all Participants and Message Flows.
        //   If not called explicitly, a default collaboration is created automatically
        //   when the first pool is added.
        //
        // AddProcess(name)
        //   Creates a named Process element. Normally one process is created
        //   automatically per pool. Call this explicitly when you need to reference
        //   the process ID, e.g. to pass it to AddLane or AddToLane.
        //
        // AddPool(parentId, processId, name)
        //   parentId  — ID of the Collaboration; null = default collaboration.
        //   processId — ID of an existing Process to associate; null = new process.
        //   name      — display name of the pool (the Participant name).
        //
        // AddLane(parentId, name)
        //   parentId — ID of the parent pool (or parent lane for nested lanes).
        //              null = add to default process.
        //
        // AddToLane(processId, laneId, elementId)
        //   Assigns an existing element to a lane.
        //   processId — null = default process.
        //   Elements are positioned within the lane's bounding rectangle in the diagram.
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateProcurementCollaboration(string outputDir)
        {
            Editor editor = new Editor();
            editor.Create("B.2.0 – Procurement with Lanes", "BPMN.Sharp Examples");

            // Explicitly create the collaboration container
            string collabId = editor.AddCollaboration("Procurement Collaboration");

            // Explicitly create processes so we can reference their IDs for lanes
            string custProcessId = editor.AddProcess("Customer Process");
            string suppProcessId = editor.AddProcess("Supplier Process");

            // AddPool: parentId=collabId links pool to our named collaboration
            string custPool = editor.AddPool(collabId, custProcessId, "Customer");
            string suppPool = editor.AddPool(collabId, suppProcessId, "Supplier");

            // Customer pool: two lanes — Sales and Finance
            string salesLane   = editor.AddLane(custPool, "Sales");
            string financeLane = editor.AddLane(custPool, "Finance");

            // Supplier pool: one lane (demonstrates single-lane pool)
            string suppLane = editor.AddLane(suppPool, "Fulfilment");

            // ── Customer elements ─────────────────────────────────────────────────
            string custStart = editor.AddEvent(custProcessId, null, "Need Goods",
                EventType.Start, EventTrigger.None, EventRole.None);
            string createPO = editor.AddActivity(custProcessId, "Create PO",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);
            string sendPO = editor.AddActivity(custProcessId, "Send PO",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);
            string receiveGoods = editor.AddActivity(custProcessId, "Receive Goods",
                ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);
            string receiveInvoice = editor.AddActivity(custProcessId, "Receive Invoice",
                ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);
            string payInvoice = editor.AddActivity(custProcessId, "Pay Invoice",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);
            string custEnd = editor.AddEvent(custProcessId, null, "Purchase Complete",
                EventType.End, EventTrigger.None, EventRole.None);

            // ── Assign customer elements to lanes ─────────────────────────────────
            // AddToLane assigns the element to a lane for visual organisation.
            // Lane assignment does not affect execution semantics.
            editor.AddToLane(custProcessId, salesLane,   custStart);
            editor.AddToLane(custProcessId, salesLane,   createPO);
            editor.AddToLane(custProcessId, salesLane,   sendPO);
            editor.AddToLane(custProcessId, salesLane,   receiveGoods);
            editor.AddToLane(custProcessId, salesLane,   custEnd);
            editor.AddToLane(custProcessId, financeLane, receiveInvoice);
            editor.AddToLane(custProcessId, financeLane, payInvoice);

            // ── Supplier elements ─────────────────────────────────────────────────
            string suppStart = editor.AddEvent(suppProcessId, null, "PO Received",
                EventType.Start, EventTrigger.Message, EventRole.None);
            string processPO = editor.AddActivity(suppProcessId, "Process Order",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
            string shipGoods = editor.AddActivity(suppProcessId, "Ship Goods",
                ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);
            string sendInvoice = editor.AddActivity(suppProcessId, "Send Invoice",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);
            string suppEnd = editor.AddEvent(suppProcessId, null, "Order Fulfilled",
                EventType.End, EventTrigger.None, EventRole.None);

            editor.AddToLane(suppProcessId, suppLane, suppStart);
            editor.AddToLane(suppProcessId, suppLane, processPO);
            editor.AddToLane(suppProcessId, suppLane, shipGoods);
            editor.AddToLane(suppProcessId, suppLane, sendInvoice);
            editor.AddToLane(suppProcessId, suppLane, suppEnd);

            // ── Sequence flows (within each pool) ─────────────────────────────────
            SF(editor, custProcessId, custStart,    createPO);
            SF(editor, custProcessId, createPO,     sendPO);
            SF(editor, custProcessId, sendPO,       receiveGoods);
            SF(editor, custProcessId, receiveGoods, receiveInvoice);
            SF(editor, custProcessId, receiveInvoice,payInvoice);
            SF(editor, custProcessId, payInvoice,   custEnd);

            SF(editor, suppProcessId, suppStart,  processPO);
            SF(editor, suppProcessId, processPO,  shipGoods);
            SF(editor, suppProcessId, shipGoods,  sendInvoice);
            SF(editor, suppProcessId, sendInvoice,suppEnd);

            // ── Message flows (between pools) ─────────────────────────────────────
            editor.AddFlow(null, null, sendPO,      suppStart,    "PO",      FlowType.Message, null, false, FlowDirection.None);
            editor.AddFlow(null, null, shipGoods,   receiveGoods, "Goods",   FlowType.Message, null, false, FlowDirection.None);
            editor.AddFlow(null, null, sendInvoice, receiveInvoice,"Invoice",FlowType.Message, null, false, FlowDirection.None);

            // ── Diagram ───────────────────────────────────────────────────────────
            string diag = editor.AddDiagram("Diagram 1", 96f);

            // Customer pool: 980w × 200h (two lanes × 100h each)
            editor.AddShape(diag, new Shape { ElementRef = custPool,
                Bounds = new List<Rectangle> { new Rectangle(10, 10, 980, 200) } });
            editor.AddShape(diag, new Shape { ElementRef = salesLane,
                Bounds = new List<Rectangle> { new Rectangle(10, 10, 980, 100) } });
            editor.AddShape(diag, new Shape { ElementRef = financeLane,
                Bounds = new List<Rectangle> { new Rectangle(10, 110, 980, 100) } });

            // Supplier pool: 980w × 100h (single lane)
            editor.AddShape(diag, new Shape { ElementRef = suppPool,
                Bounds = new List<Rectangle> { new Rectangle(10, 230, 980, 100) } });
            editor.AddShape(diag, new Shape { ElementRef = suppLane,
                Bounds = new List<Rectangle> { new Rectangle(10, 230, 980, 100) } });

            // Customer Sales lane (y ≈ 10–110, centre ≈ 60)
            S(editor, diag, custStart,    60,  42,  36,  36);
            S(editor, diag, createPO,    136,  20, 120,  80);
            S(editor, diag, sendPO,      296,  20, 120,  80);
            S(editor, diag, receiveGoods,616,  20, 120,  80);
            S(editor, diag, custEnd,     856,  42,  36,  36);

            // Customer Finance lane (y ≈ 110–210, centre ≈ 160)
            S(editor, diag, receiveInvoice,456, 120, 120,  80);
            S(editor, diag, payInvoice,    616, 120, 120,  80);

            // Supplier lane (y ≈ 230–330, centre ≈ 280)
            S(editor, diag, suppStart,    296, 262,  36,  36);
            S(editor, diag, processPO,    376, 240, 120,  80);
            S(editor, diag, shipGoods,    536, 240, 120,  80);
            S(editor, diag, sendInvoice,  696, 240, 120,  80);
            S(editor, diag, suppEnd,      856, 262,  36,  36);

            ExampleHelper.Save(editor, outputDir, "B.2.0");
        }

        static void SF(Editor editor, string processId, string from, string to)
        {
            editor.AddFlow(processId, null, from, to,
                null, FlowType.Sequence, null, false, FlowDirection.None);
        }

        static void S(Editor editor, string diagId, string elemId,
            int x, int y, int w, int h)
        {
            editor.AddShape(diagId, new Shape
            {
                ElementRef = elemId,
                Bounds = new List<Rectangle> { new Rectangle(x, y, w, h) }
            });
        }
    }
}
