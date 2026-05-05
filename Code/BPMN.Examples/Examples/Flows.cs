using System.Collections.Generic;
using System.Drawing;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Flows — AddFlow() covering FlowType (Sequence, Message, Association),
    /// conditional flows, default flows, and FlowDirection for associations.
    ///
    /// Produces one model:
    ///   B.1.0 — B2B procurement collaboration showing sequence flows within pools
    ///           and message flows between pools, matching the MIWG B.1.0 reference.
    ///
    /// MIWG reference B.1.0: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/B.1.0.png
    /// BPMN spec: §8.4.13 Sequence Flow · §9.4 Message Flow · §8.4.1 Association
    ///            §7.6 Flow Object Connection Rules
    /// </summary>
    public static class Flows
    {
        public static void Run(string outputDir)
        {
            CreateB2BProcurementProcess(outputDir);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // B.1.0 — B2B Procurement
        //
        // AddFlow parameters:
        //   parentId           — process/sub-process ID; null = default process.
        //                        For Message flows, null is also valid.
        //   messageId          — ID of a BPMN Message element associated with this
        //                        flow; null if no specific message is modelled.
        //   idFrom / idTo      — source and target element IDs.
        //   name               — optional label (used for condition descriptions).
        //   flowType           — Sequence, Message, or Association.
        //   condition          — formal condition expression for conditional flows;
        //                        null if unconditional.
        //   isDefault          — true marks this as the default flow from an
        //                        Exclusive or Inclusive gateway (§8.4.13).
        //   direction          — for Association flows: None, One, or Both (§8.4.1).
        //
        // FlowType.Sequence (§8.4.13) — connects elements within the same pool.
        // FlowType.Message  (§9.4)    — connects elements in different pools.
        // FlowType.Association (§8.4.1) — links artifacts to flow elements.
        //
        // Connection rules (§7.6):
        //   Sequence flows may NOT cross pool boundaries.
        //   Message flows may ONLY cross pool boundaries.
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateB2BProcurementProcess(string outputDir)
        {
            Editor editor = new Editor();
            editor.Create("B.1.0 – B2B Procurement", "BPMN.Sharp Examples");

            // Two pools are required for message flows (§9.1 Collaboration)
            string custPool = editor.AddPool(null, null, "Customer");
            string suppPool = editor.AddPool(null, null, "Supplier");

            // ── Customer process ──────────────────────────────────────────────────
            string custStart = editor.AddEvent(null, null, "Need Goods",
                EventType.Start, EventTrigger.None, EventRole.None);

            string createPO = editor.AddActivity(null, "Create Purchase Order",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);

            // Exclusive gateway — approve or revise the PO
            string reviewGw = editor.AddGateway(null, "PO Approved?",
                GatewayType.Exclusive, 1, 2);

            string revisePO = editor.AddActivity(null, "Revise PO",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);

            // Send Task — sends the PO as a message to the supplier
            string sendPO = editor.AddActivity(null, "Send PO",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            // Receive Task — waits for goods delivery confirmation
            string receiveGoods = editor.AddActivity(null, "Receive Goods",
                ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);

            // Receive Task — waits for the invoice
            string receiveInvoice = editor.AddActivity(null, "Receive Invoice",
                ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);

            string payInvoice = editor.AddActivity(null, "Pay Invoice",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);

            string custEnd = editor.AddEvent(null, null, "Purchase Complete",
                EventType.End, EventTrigger.None, EventRole.None);

            // ── Supplier process ──────────────────────────────────────────────────
            string suppStart = editor.AddEvent(null, null, "PO Received",
                EventType.Start, EventTrigger.Message, EventRole.None);

            string processPO = editor.AddActivity(null, "Process Order",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            string shipGoods = editor.AddActivity(null, "Ship Goods",
                ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);

            string sendInvoice = editor.AddActivity(null, "Send Invoice",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            string receivePayment = editor.AddActivity(null, "Receive Payment",
                ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);

            string suppEnd = editor.AddEvent(null, null, "Order Fulfilled",
                EventType.End, EventTrigger.None, EventRole.None);

            // ── Sequence flows (within each pool) — FlowType.Sequence ────────────
            SF(editor, custStart,    createPO);
            SF(editor, createPO,     reviewGw);
            SF(editor, reviewGw,     sendPO,      "Approved");
            SF(editor, reviewGw,     revisePO,    "Needs Revision", isDefault: true);
            SF(editor, revisePO,     reviewGw);
            SF(editor, sendPO,       receiveGoods);
            SF(editor, receiveGoods, receiveInvoice);
            SF(editor, receiveInvoice,payInvoice);
            SF(editor, payInvoice,   custEnd);

            SF(editor, suppStart,    processPO);
            SF(editor, processPO,    shipGoods);
            SF(editor, shipGoods,    sendInvoice);
            SF(editor, sendInvoice,  receivePayment);
            SF(editor, receivePayment,suppEnd);

            // ── Message flows (between pools) — FlowType.Message ─────────────────
            // messageId = null here (no explicit BPMN Message element defined).
            // A message element can be created with AddMessage() and its ID passed
            // as messageId when more precise modelling is required.
            MF(editor, sendPO,       suppStart,      "Purchase Order");
            MF(editor, shipGoods,    receiveGoods,   "Goods");
            MF(editor, sendInvoice,  receiveInvoice, "Invoice");
            MF(editor, payInvoice,   receivePayment, "Payment");

            // ── Text annotation with Association — FlowType.Association ───────────
            // AddAnnotation(parentId, text, rightAlign)
            // parentId = null adds to the default process.
            // rightAlign controls which side the bracket opens toward.
            string note = editor.AddAnnotation(null,
                "All purchase orders require manager sign-off", false);

            // Undirected association (§8.4.1) — FlowDirection.None
            editor.AddFlow(null, null, note, reviewGw,
                null, FlowType.Association, null, false, FlowDirection.None);

            // ── Diagram ───────────────────────────────────────────────────────────
            string diag = editor.AddDiagram("Diagram 1", 96f);

            // Pool shapes — pools are large container rectangles
            editor.AddShape(diag, new Shape { ElementRef = custPool,
                Bounds = new List<Rectangle> { new Rectangle(10, 10, 1060, 130) } });
            editor.AddShape(diag, new Shape { ElementRef = suppPool,
                Bounds = new List<Rectangle> { new Rectangle(10, 160, 1060, 130) } });

            // Customer pool elements (y centres ≈ 75)
            S(editor, diag, custStart,     30,  57,  36,  36);
            S(editor, diag, createPO,      96,  35, 120,  80);
            S(editor, diag, reviewGw,     256,  53,  44,  44);
            S(editor, diag, revisePO,     256, 110, 120,  80); // below gateway
            S(editor, diag, sendPO,       340,  35, 120,  80);
            S(editor, diag, receiveGoods, 500,  35, 120,  80);
            S(editor, diag, receiveInvoice,660,  35, 120,  80);
            S(editor, diag, payInvoice,   820,  35, 120,  80);
            S(editor, diag, custEnd,      980,  57,  36,  36);

            // Supplier pool elements (y centres ≈ 225)
            S(editor, diag, suppStart,    340, 207,  36,  36);
            S(editor, diag, processPO,    416, 185, 120,  80);
            S(editor, diag, shipGoods,    576, 185, 120,  80);
            S(editor, diag, sendInvoice,  736, 185, 120,  80);
            S(editor, diag, receivePayment,896, 185, 120,  80);
            S(editor, diag, suppEnd,     1056, 207,  36,  36);

            // Annotation
            S(editor, diag, note, 160, 0, 200, 50);

            ExampleHelper.Save(editor, outputDir, "B.1.0");
        }

        // ── Shorthand helpers ─────────────────────────────────────────────────────

        static void SF(Editor editor, string from, string to,
            string name = null, bool isDefault = false)
        {
            editor.AddFlow(null, null, from, to, name,
                FlowType.Sequence, null, isDefault, FlowDirection.None);
        }

        static void MF(Editor editor, string from, string to, string name)
        {
            editor.AddFlow(null, null, from, to, name,
                FlowType.Message, null, false, FlowDirection.None);
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
