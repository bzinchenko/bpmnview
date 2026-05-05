using System.Collections.Generic;
using System.Drawing;
using BPMN;

namespace BPMN.Examples
{
    /// <summary>
    /// Data and Artifacts — AddAnnotation(), AddDataObject(), AddDataInput(),
    /// AddDataOutput(), AddDataStore(), AddGroup(), AddMessage().
    ///
    /// Produces one model:
    ///   C.2.0 — Document approval process with data objects, a data store,
    ///           annotations, a group, and messages, inspired by MIWG C.2.0.
    ///
    /// MIWG reference C.2.0: https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/C.2.0.png
    /// BPMN spec: §8.4.1 Artifacts · §10.4.1 Data Modeling · §8.4.11 Message
    ///            §8.4.10 ItemDefinition
    /// </summary>
    public static class DataAndArtifacts
    {
        public static void Run(string outputDir)
        {
            CreateDocumentApprovalProcess(outputDir);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // C.2.0 — Document Approval with Data, Annotations and Groups
        //
        // AddAnnotation(parentId, text, rightAlign)
        //   parentId   — process ID; null = default process.
        //   text       — the annotation text content (§8.4.1.1).
        //   rightAlign — if true, the annotation bracket opens to the right;
        //                if false, it opens to the left (toward the linked element).
        //   Returns: annotation element ID. Connect to a flow element using
        //   AddFlow with FlowType.Association.
        //
        // AddDataObject(processId, text)
        //   Creates a Data Object (§10.4.1) — represents data used or produced
        //   by activities. Connect with FlowType.Association + FlowDirection.Directed
        //   to show data flowing into or out of a task.
        //
        // AddDataInput(processId, text)
        //   Creates a Data Input (§10.4.1) — formal input to an activity's
        //   InputOutputSpecification.
        //
        // AddDataOutput(processId, text)
        //   Creates a Data Output (§10.4.1) — formal output from an activity.
        //
        // AddDataStore(processId, text)
        //   Creates a Data Store Reference (§10.4.1) — a persistent data store
        //   (e.g. database) accessible across multiple activities. Rendered as
        //   a cylinder in BPMN diagrams.
        //
        // AddGroup(parentId, name)
        //   Creates a Group artifact (§8.4.1.2). Groups draw a dashed border
        //   around related elements for visual organisation. They have no
        //   effect on execution semantics and can span pool/lane boundaries.
        //   parentId — null = default process.
        //
        // AddMessage(itemId, text)
        //   Creates a BPMN Message element (§8.4.11). Messages carry data between
        //   participants. itemId references an ItemDefinition (§8.4.10) describing
        //   the message payload structure; pass null for an untyped message.
        //   The returned ID can be passed as messageId in AddFlow for Message flows.
        // ─────────────────────────────────────────────────────────────────────────
        static void CreateDocumentApprovalProcess(string outputDir)
        {
            Editor editor = new Editor();
            editor.Create("C.2.0 – Document Approval", "BPMN.Sharp Examples");

            // ── Messages (§8.4.11) ────────────────────────────────────────────────
            // itemId=null — untyped messages (no ItemDefinition)
            string submitMsg  = editor.AddMessage(null, "Document Submission");
            string decisionMsg= editor.AddMessage(null, "Approval Decision");

            // ── Process elements ──────────────────────────────────────────────────
            string start = editor.AddEvent(null, null, "Submission Received",
                EventType.Start, EventTrigger.Message, EventRole.None);

            string validate = editor.AddActivity(null, "Validate Document",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            string review = editor.AddActivity(null, "Review Document",
                ActivityType.Task, ActivityMarker.None, TaskType.User, null);

            string decide = editor.AddGateway(null, "Approved?",
                GatewayType.Exclusive, 1, 2);

            string notifyApproval = editor.AddActivity(null, "Send Approval Notice",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            string notifyRejection = editor.AddActivity(null, "Send Rejection Notice",
                ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

            string archiveGw = editor.AddGateway(null, null,
                GatewayType.Exclusive, 2, 1);

            string archive = editor.AddActivity(null, "Archive Document",
                ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

            string end = editor.AddEvent(null, null, "Review Complete",
                EventType.End, EventTrigger.None, EventRole.None);

            // ── Data Objects (§10.4.1) ────────────────────────────────────────────
            // Data Object — the submitted document itself
            string docObject = editor.AddDataObject(null, "Submitted Document");

            // Data Input — formal input to the Validate task
            string validationInput = editor.AddDataInput(null, "Document Content");

            // Data Output — result produced by the Review task
            string reviewOutput = editor.AddDataOutput(null, "Review Comments");

            // Data Store — persistent document archive (cylinder symbol)
            string docStore = editor.AddDataStore(null, "Document Archive");

            // ── Artifacts: Annotations and Groups ────────────────────────────────

            // Text Annotation (§8.4.1.1)
            // rightAlign=false: bracket opens to the left, pointing toward the task
            string slaNote = editor.AddAnnotation(null,
                "SLA: Complete within 2 business days", false);

            // Text Annotation for the archive
            string archiveNote = editor.AddAnnotation(null,
                "Retained for 7 years per compliance policy", true);

            // Group (§8.4.1.2) — visually groups the notification tasks
            // without affecting execution semantics
            string notifyGroup = editor.AddGroup(null, "Notification Tasks");

            // ── Associations (§8.4.1) ─────────────────────────────────────────────
            // Connect data objects to activities with directed associations
            // FlowDirection.Directed: arrow points from data object into task (input)
            editor.AddFlow(null, null, docObject, validate,
                null, FlowType.Association, null, false, FlowDirection.Directed);
            editor.AddFlow(null, null, docObject, review,
                null, FlowType.Association, null, false, FlowDirection.Directed);

            // FlowDirection.Directed: from task to data output (output)
            editor.AddFlow(null, null, review, reviewOutput,
                null, FlowType.Association, null, false, FlowDirection.Directed);

            // Data store association
            editor.AddFlow(null, null, archive, docStore,
                null, FlowType.Association, null, false, FlowDirection.Directed);

            // Undirected annotation associations
            editor.AddFlow(null, null, slaNote, review,
                null, FlowType.Association, null, false, FlowDirection.None);
            editor.AddFlow(null, null, archiveNote, archive,
                null, FlowType.Association, null, false, FlowDirection.None);

            // ── Sequence flows ────────────────────────────────────────────────────
            editor.AddFlow(null, null, start,          validate,       null,       FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, validate,       review,         null,       FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, review,         decide,         null,       FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, decide,         notifyApproval, "Approved", FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, decide,         notifyRejection,"Rejected", FlowType.Sequence, null, true,  FlowDirection.None);
            editor.AddFlow(null, null, notifyApproval, archiveGw,      null,       FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, notifyRejection,archiveGw,      null,       FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, archiveGw,      archive,        null,       FlowType.Sequence, null, false, FlowDirection.None);
            editor.AddFlow(null, null, archive,        end,            null,       FlowType.Sequence, null, false, FlowDirection.None);

            // ── Diagram ───────────────────────────────────────────────────────────
            string diag = editor.AddDiagram("Diagram 1", 96f);

            // Main flow (horizontal, y centre ≈ 200)
            S(editor, diag, start,           20, 182,  36,  36);
            S(editor, diag, validate,        96, 160, 120,  80);
            S(editor, diag, review,         256, 160, 120,  80);
            S(editor, diag, decide,         416, 178,  44,  44);
            S(editor, diag, notifyApproval, 500, 120, 140,  80);
            S(editor, diag, notifyRejection,500, 220, 140,  80);
            S(editor, diag, archiveGw,      680, 178,  44,  44);
            S(editor, diag, archive,        764, 160, 120,  80);
            S(editor, diag, end,            924, 182,  36,  36);

            // Data objects (above the flow)
            S(editor, diag, docObject,       130,  60,  36,  50);
            S(editor, diag, validationInput, 130,   0,  36,  50);
            S(editor, diag, reviewOutput,    310,   0,  36,  50);

            // Data store (below the archive task)
            S(editor, diag, docStore,        794,  260,  50,  60);

            // Annotations
            S(editor, diag, slaNote,          10,  60, 220,  40);
            S(editor, diag, archiveNote,     670,  300, 240,  40);

            // Group encompasses both notification tasks
            S(editor, diag, notifyGroup,     490, 108, 162, 204);

            ExampleHelper.Save(editor, outputDir, "C.2.0");
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
