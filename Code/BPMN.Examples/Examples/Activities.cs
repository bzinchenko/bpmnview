using System.Collections.Generic;
using System.Drawing;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Activities — AddActivity() covering ActivityType, ActivityMarker and TaskType.
    ///
    /// Produces two models:
    ///   A.3.0 — HR onboarding process showing all TaskType values in a realistic flow.
    ///   C.1.0 — Invoice handling process showing a Sub-Process with loop and
    ///           compensation, matching the MIWG C.1.0 reference pattern.
    ///
    /// MIWG reference A.3.0: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.3.0.png
    /// MIWG reference C.1.0: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/C.1.0.png
    /// BPMN spec: §10.3 Activities · §10.3.3 Tasks · §10.3.5 Sub-Processes
    ///            §10.3.6 Call Activity · §10.3.8 Loop Characteristics · §10.7 Compensation
    /// </summary>
    public static class Activities
    {
        public static void Run(string outputDir)
        {
            CreateOnboardingProcess(outputDir);
            CreateInvoiceHandlingProcess(outputDir);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // A.3.0 — HR Onboarding — all TaskType values in one realistic flow
        //
        // TaskType values (§10.3.3):
        //   None         — abstract task, no specific technology
        //   User         — performed by a person via a form/UI
        //   Manual       — performed by a person without system support
        //   Service      — automated via web service or system API
        //   Send         — sends a BPMN Message to a participant
        //   Receive      — waits for a BPMN Message from a participant
        //   BusinessRule — invokes a business rule engine
        //   Script       — executes a script
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateOnboardingProcess(string outputDir)
        {
            Editor editor = new Editor();
            editor.Create("A.3.0 – HR Onboarding", "BPMN.Sharp Examples");

            string start = editor.AddEvent(null, null, "New Hire Confirmed",
                EventType.Start, EventTrigger.Message, EventRole.None);

            // Service Task — automated call to HR system API
            string createProfile = editor.AddActivity(null, "Create HR Profile",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            // BusinessRule Task — determine entitlements via rules engine
            string calcEntitlements = editor.AddActivity(null, "Calculate Entitlements",
                ActivityType.Task, ActivityMarker.None, TaskType.BusinessRule, null);

            // Script Task — generate access credentials
            string generateCreds = editor.AddActivity(null, "Generate Credentials",
                ActivityType.Task, ActivityMarker.None, TaskType.Script, null);

            // Send Task — deliver welcome pack
            string sendWelcome = editor.AddActivity(null, "Send Welcome Pack",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            // Receive Task — wait for signed contract to arrive
            string receiveContract = editor.AddActivity(null, "Receive Signed Contract",
                ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);

            // User Task — manager reviews and confirms the hire
            string reviewHire = editor.AddActivity(null, "Review & Confirm Hire",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);

            // Manual Task — physical desk setup, no system involvement
            string setupDesk = editor.AddActivity(null, "Set Up Desk & Equipment",
                ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);

            // Sequential Multi-Instance Task (§10.3.8) — run orientation for each department
            // ActivityMarker.Sequential: one instance per list item, run one at a time
            string orientation = editor.AddActivity(null, "Department Orientation",
                ActivityType.Task, ActivityMarker.Sequential, TaskType.User, null);

            string end = editor.AddEvent(null, null, "Onboarding Complete",
                EventType.End, EventTrigger.None, EventRole.None);

            editor.AddFlow(null, null, start,           createProfile,    null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, createProfile,   calcEntitlements, null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, calcEntitlements,generateCreds,    null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, generateCreds,   sendWelcome,      null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, sendWelcome,     receiveContract,  null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, receiveContract, reviewHire,       null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, reviewHire,      setupDesk,        null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, setupDesk,       orientation,      null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, orientation,     end,              null, FlowType.Sequence, null, false, FlowDirection.None);

            string diag = editor.AddDiagram("Diagram 1", 96f);

            int x = 20, y = 40, gap = 20;
            S(editor, diag, start,           x, y + 27, 36, 36); x += 36 + gap;
            S(editor, diag, createProfile,   x, y, 120, 80);     x += 120 + gap;
            S(editor, diag, calcEntitlements,x, y, 120, 80);     x += 120 + gap;
            S(editor, diag, generateCreds,   x, y, 120, 80);     x += 120 + gap;
            S(editor, diag, sendWelcome,     x, y, 120, 80);     x += 120 + gap;
            S(editor, diag, receiveContract, x, y, 120, 80);     x += 120 + gap;
            S(editor, diag, reviewHire,      x, y, 120, 80);     x += 120 + gap;
            S(editor, diag, setupDesk,       x, y, 120, 80);     x += 120 + gap;
            S(editor, diag, orientation,     x, y, 120, 80);     x += 120 + gap;
            S(editor, diag, end,             x, y + 27, 36, 36);

            ExampleHelper.Save(editor, outputDir, "A.3.0");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // C.1.0 — Invoice Handling with Sub-Process and Compensation
        //
        // ActivityType values (§10.3):
        //   Task            — atomic unit of work
        //   SubProcess      — compound activity with its own internal flow
        //   Transaction     — sub-process with all-or-nothing transactional semantics
        //   CallActivity    — reusable reference to a global process or task
        //   AdHocSubProcess — activities performed in any order, any number of times
        //
        // ActivityMarker values (§10.3.8):
        //   Loop         — standard loop (while/until condition)
        //   Parallel     — parallel multi-instance
        //   Sequential   — sequential multi-instance
        //   Compensation — marks activity as a compensation handler (§10.7)
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateInvoiceHandlingProcess(string outputDir)
        {
            Editor editor = new Editor();
            editor.Create("C.1.0 – Invoice Handling", "BPMN.Sharp Examples");

            string start = editor.AddEvent(null, null, "Invoice Received",
                EventType.Start, EventTrigger.Message, EventRole.None);

            // Sub-Process — contains its own internal flow (§10.3.5).
            // Elements inside reference this ID as their processId.
            string reviewSubProc = editor.AddActivity(null, "Review Invoice",
                ActivityType.Activity, ActivityMarker.None, TaskType.None, null);

            // Internal elements of the sub-process
            string spStart = editor.AddEvent(reviewSubProc, null, "Start",
                EventType.Start, EventTrigger.None, EventRole.None);
            string assignApprover = editor.AddActivity(reviewSubProc, "Assign Approver",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);
            string approveInvoice = editor.AddActivity(reviewSubProc, "Approve Invoice",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);

            // Loop Task (§10.3.8) — retry clarification until resolved
            string clarify = editor.AddActivity(reviewSubProc, "Clarify Invoice",
                ActivityType.Task, ActivityMarker.Loop, TaskType.User, null);

            string spGw = editor.AddGateway(reviewSubProc, "OK?",
                GatewayType.Exclusive, 1, 2);
            string spEnd = editor.AddEvent(reviewSubProc, null, "End",
                EventType.End, EventTrigger.None, EventRole.None);

            editor.AddFlow(reviewSubProc, null, spStart,       assignApprover, null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(reviewSubProc, null, assignApprover,approveInvoice, null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(reviewSubProc, null, approveInvoice,spGw,           null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(reviewSubProc, null, spGw,          spEnd,          "Yes",FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(reviewSubProc, null, spGw,          clarify,        "No", FlowType.Sequence, null, true,  FlowDirection.None);
            editor.AddFlow(reviewSubProc, null, clarify,       approveInvoice, null, FlowType.Sequence, null, false, FlowDirection.None);

            // Compensation Task (§10.7) — invoked if the payment must be reversed
            string bookPayment = editor.AddActivity(null, "Book Payment",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            // ActivityMarker.Compensation marks this as a compensation handler
            string reversePayment = editor.AddActivity(null, "Reverse Payment",
                ActivityType.Task, ActivityMarker.Compensation, TaskType.Service, null);

            string end = editor.AddEvent(null, null, "Invoice Handled",
                EventType.End, EventTrigger.None, EventRole.None);

            editor.AddFlow(null, null, start,         reviewSubProc, null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, reviewSubProc, bookPayment,   null, FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, bookPayment,   end,           null, FlowType.Sequence, null, false, FlowDirection.None);

            // Compensation association (not a sequence flow — uses Association type)
            editor.AddFlow(null, null, bookPayment, reversePayment,
                null, FlowType.Association, null, false, FlowDirection.Directed);

            string diag = editor.AddDiagram("Diagram 1", 96f);

            S(editor, diag, start,          20, 162,  36,  36);
            S(editor, diag, reviewSubProc,  96,  20, 420, 280); // large expanded sub-process box
            S(editor, diag, bookPayment,   556, 152, 120,  80);
            S(editor, diag, reversePayment,556, 260, 120,  80);
            S(editor, diag, end,           726, 162,  36,  36);

            // Sub-process internals
            S(editor, diag, spStart,       116, 142,  36,  36);
            S(editor, diag, assignApprover,172, 120, 120,  80);
            S(editor, diag, approveInvoice,312, 120, 120,  80);
            S(editor, diag, spGw,          452, 140,  40,  40);
            S(editor, diag, clarify,       312, 220, 120,  80);
            S(editor, diag, spEnd,         460, 240,  36,  36);

            ExampleHelper.Save(editor, outputDir, "C.1.0");
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
