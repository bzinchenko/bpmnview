using System.Collections.Generic;
using System.Drawing;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Gateways — AddGateway() covering all GatewayType values.
    ///
    /// Produces two models:
    ///   A.4.0 — Loan application showing Exclusive and Parallel gateways,
    ///           matching the MIWG A.4.0 reference pattern.
    ///   C.6.0 — Travel booking showing Inclusive and Event-Based gateways,
    ///           matching the MIWG C.6.0 reference pattern.
    ///
    /// MIWG reference A.4.0: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.4.0.png
    /// MIWG reference C.6.0: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/C.6.0.png
    /// BPMN spec: §10.6 Gateways · §10.6.2 Exclusive · §10.6.3 Inclusive
    ///            §10.6.4 Parallel · §10.6.5 Complex · §10.6.6 Event-Based
    /// </summary>
    public static class Gateways
    {
        public static void Run(string outputDir)
        {
            CreateLoanApplicationProcess(outputDir);
            CreateTravelBookingProcess(outputDir);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // A.4.0 — Loan Application: Parallel split + Exclusive decision
        //
        // AddGateway parameters:
        //   processId   — null = default process
        //   name        — label, typically a question for split gateways
        //   gatewayType — routing semantics
        //   inputs      — optional hint: expected number of incoming flows (default 0)
        //   outputs     — optional hint: expected number of outgoing flows (default 0)
        //
        // GatewayType.Parallel (§10.6.4)
        //   Split: all outgoing paths activated simultaneously (AND-split)
        //   Join:  waits for all incoming paths to complete (AND-join)
        //
        // GatewayType.Exclusive (§10.6.2)
        //   Split: exactly one outgoing path taken based on conditions (XOR-split)
        //   Join:  passes through when any one incoming token arrives (XOR-join)
        //   Use isDefault=true on AddFlow to mark the default path.
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateLoanApplicationProcess(string outputDir)
        {
            Editor editor = new Editor();
            editor.Create("A.4.0 – Loan Application", "BPMN.Sharp Examples");

            string start = editor.AddEvent(null, null, "Application Received",
                EventType.Start, EventTrigger.None, EventRole.None);

            // Parallel split — run credit and fraud checks simultaneously
            string parSplit = editor.AddGateway(null, null,
                GatewayType.Parallel, 1, 2);

            string creditCheck = editor.AddActivity(null, "Credit Check",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
            string fraudCheck = editor.AddActivity(null, "Fraud Check",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            // Parallel join — wait for both checks to complete before proceeding
            string parJoin = editor.AddGateway(null, null,
                GatewayType.Parallel, 2, 1);

            // Exclusive split — decide based on combined check results
            string xorDecide = editor.AddGateway(null, "Eligible?",
                GatewayType.Exclusive, 1, 2);

            string approve = editor.AddActivity(null, "Approve Loan",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);
            string reject = editor.AddActivity(null, "Reject Application",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            // Exclusive join — merge approved/rejected paths
            string xorJoin = editor.AddGateway(null, null,
                GatewayType.Exclusive, 2, 1);

            string notify = editor.AddActivity(null, "Notify Applicant",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            string end = editor.AddEvent(null, null, "Application Processed",
                EventType.End, EventTrigger.None, EventRole.None);

            editor.AddFlow(null, null, start,      parSplit,   null,       FlowType.Sequence, null,  false, FlowDirection.None);
            editor.AddFlow(null, null, parSplit,   creditCheck,null,       FlowType.Sequence, null,  false, FlowDirection.None);
            editor.AddFlow(null, null, parSplit,   fraudCheck, null,       FlowType.Sequence, null,  false, FlowDirection.None);
            editor.AddFlow(null, null, creditCheck,parJoin,    null,       FlowType.Sequence, null,  false, FlowDirection.None);
            editor.AddFlow(null, null, fraudCheck, parJoin,    null,       FlowType.Sequence, null,  false, FlowDirection.None);
            editor.AddFlow(null, null, parJoin,    xorDecide,  null,       FlowType.Sequence, null,  false, FlowDirection.None);

            // Conditional flows from exclusive split
            editor.AddFlow(null, null, xorDecide,  approve,    "Eligible", FlowType.Sequence, null,  false, FlowDirection.None);
            // isDefault=true marks the default path (taken when no condition matches)
            editor.AddFlow(null, null, xorDecide,  reject,     "Not Eligible", FlowType.Sequence, null, true, FlowDirection.None);

            editor.AddFlow(null, null, approve,    xorJoin,    null,       FlowType.Sequence, null,  false, FlowDirection.None);
            editor.AddFlow(null, null, reject,     xorJoin,    null,       FlowType.Sequence, null,  false, FlowDirection.None);
            editor.AddFlow(null, null, xorJoin,    notify,     null,       FlowType.Sequence, null,  false, FlowDirection.None);
            editor.AddFlow(null, null, notify,     end,        null,       FlowType.Sequence, null,  false, FlowDirection.None);

            string diag = editor.AddDiagram("Diagram 1", 96f);

            // Two rows: top (approved path), bottom (rejected path)
            S(editor, diag, start,       20,  112,  36,  36);
            S(editor, diag, parSplit,    96,  108,  44,  44);
            S(editor, diag, creditCheck, 180,  60, 120,  80);
            S(editor, diag, fraudCheck,  180, 160, 120,  80);
            S(editor, diag, parJoin,     340, 108,  44,  44);
            S(editor, diag, xorDecide,   424, 108,  44,  44);
            S(editor, diag, approve,     508,  60, 120,  80);
            S(editor, diag, reject,      508, 160, 120,  80);
            S(editor, diag, xorJoin,     668, 108,  44,  44);
            S(editor, diag, notify,      752,  90, 120,  80);
            S(editor, diag, end,         912, 112,  36,  36);

            ExampleHelper.Save(editor, outputDir, "A.4.0");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // C.6.0 — Travel Booking: Inclusive + Event-Based gateways
        //
        // GatewayType.Inclusive (§10.6.3)
        //   Split: one or more outgoing paths activated based on conditions (OR-split)
        //   Join:  waits for all currently active incoming paths to complete (OR-join)
        //
        // GatewayType.EventBased (§10.6.6)
        //   Routes to the first of several following catching events to fire.
        //   Each outgoing path must lead directly to an Intermediate Catching Event
        //   or a Receive Task.
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateTravelBookingProcess(string outputDir)
        {
            Editor editor = new Editor();
            editor.Create("C.6.0 – Travel Booking", "BPMN.Sharp Examples");

            string start = editor.AddEvent(null, null, "Trip Requested",
                EventType.Start, EventTrigger.None, EventRole.None);

            // Inclusive split — book one or more travel components
            string incSplit = editor.AddGateway(null, "Components Required?",
                GatewayType.Inclusive, 1, 3);

            string bookFlight = editor.AddActivity(null, "Book Flight",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
            string bookHotel = editor.AddActivity(null, "Book Hotel",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
            string bookCar = editor.AddActivity(null, "Book Car",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            // Inclusive join — waits for all active paths
            string incJoin = editor.AddGateway(null, null,
                GatewayType.Inclusive, 3, 1);

            string confirmTravel = editor.AddActivity(null, "Send Confirmation",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            // Event-Based gateway — wait for payment OR 7-day timeout
            string evtGw = editor.AddGateway(null, "Await Payment",
                GatewayType.EventBased, 1, 2);

            // Each outgoing path of an Event-Based gateway leads to a catching event
            string paymentReceived = editor.AddEvent(null, null, "Payment Received",
                EventType.Intermediate, EventTrigger.Message, EventRole.None);
            string paymentTimeout = editor.AddEvent(null, null, "7 Days No Payment",
                EventType.Intermediate, EventTrigger.Timer, EventRole.None);

            string processPayment = editor.AddActivity(null, "Post Payment",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
            string cancelBooking = editor.AddActivity(null, "Cancel Booking",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            string xorJoin = editor.AddGateway(null, null, GatewayType.Exclusive, 2, 1);
            string end = editor.AddEvent(null, null, "End",
                EventType.End, EventTrigger.None, EventRole.None);

            editor.AddFlow(null, null, start,          incSplit,       null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, incSplit,        bookFlight,     "Needs Flight",FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, incSplit,        bookHotel,      "Needs Hotel", FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, incSplit,        bookCar,        "Needs Car",   FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, bookFlight,      incJoin,        null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, bookHotel,       incJoin,        null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, bookCar,         incJoin,        null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, incJoin,         confirmTravel,  null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, confirmTravel,   evtGw,          null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, evtGw,           paymentReceived,null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, evtGw,           paymentTimeout, null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, paymentReceived, processPayment, null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, paymentTimeout,  cancelBooking,  null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, processPayment,  xorJoin,        null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, cancelBooking,   xorJoin,        null,          FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, xorJoin,         end,            null,          FlowType.Sequence, null, false, FlowDirection.None);

            string diag = editor.AddDiagram("Diagram 1", 96f);

            S(editor, diag, start,           20, 162,  36,  36);
            S(editor, diag, incSplit,         96, 158,  44,  44);
            S(editor, diag, bookFlight,      180,  60, 120,  80);
            S(editor, diag, bookHotel,       180, 160, 120,  80);
            S(editor, diag, bookCar,         180, 260, 120,  80);
            S(editor, diag, incJoin,         340, 158,  44,  44);
            S(editor, diag, confirmTravel,   424, 140, 120,  80);
            S(editor, diag, evtGw,           584, 158,  44,  44);
            S(editor, diag, paymentReceived, 668, 108,  36,  36);
            S(editor, diag, paymentTimeout,  668, 208,  36,  36);
            S(editor, diag, processPayment,  744,  88, 120,  80);
            S(editor, diag, cancelBooking,   744, 188, 120,  80);
            S(editor, diag, xorJoin,         904, 158,  44,  44);
            S(editor, diag, end,             988, 162,  36,  36);

            ExampleHelper.Save(editor, outputDir, "C.6.0");
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
