# 07 – Data and Artifacts

> **BPMN Spec Reference:** §8.4.1 Artifacts · §8.4.1.1 Text Annotation · §8.4.1.2 Group · §10.4 Data · §8.4.11 Message · §8.4.10 ItemDefinition

---

## Overview

BPMN.Sharp provides separate methods for each artifact and data element type. None of these elements affect process execution — they are informational extensions to the model connected to flow elements via `AddFlow` with `FlowType.Association`.

---

## `AddAnnotation(string parentId, string text, bool rightAlign)`

**§8.4.1.1 Text Annotation**

Adds a free-text note to the diagram. Connect it to any flow element using an Association flow.

| Parameter | Description |
|-----------|-------------|
| `parentId` | Parent process ID. `null` = default process. |
| `text` | The annotation text content. |
| `rightAlign` | `true` = bracket opens to the right; `false` = bracket opens to the left (toward the connected element). |

```csharp
string note = editor.AddAnnotation(null,
    "SLA: Must be completed within 24 hours", false);

// Connect to a task with an undirected association
editor.AddFlow(null, null, note, approveTask,
    null, FlowType.Association, null, false, FlowDirection.None);
```

---

## `AddDataObject(string processId, string text)`

**§10.4.1 Data Object**

Represents data used or produced by activities within the process. Rendered as a folded-corner document symbol.

```csharp
string orderForm = editor.AddDataObject(null, "Order Form");

// Arrow into task — data flows in
editor.AddFlow(null, null, orderForm, processTask,
    null, FlowType.Association, null, false, FlowDirection.Directed);

string receipt = editor.AddDataObject(null, "Receipt");

// Arrow out of task — data flows out
editor.AddFlow(null, null, processTask, receipt,
    null, FlowType.Association, null, false, FlowDirection.Directed);
```

---

## `AddDataInput(string processId, string text)`

**§10.4.1 Data Input**

Represents a formal data input to a process or activity's InputOutputSpecification. Rendered with a filled arrowhead in the top-left corner.

```csharp
string inputData = editor.AddDataInput(null, "Customer Record");

editor.AddFlow(null, null, inputData, validateTask,
    null, FlowType.Association, null, false, FlowDirection.Directed);
```

---

## `AddDataOutput(string processId, string text)`

**§10.4.1 Data Output**

Represents a formal data output from a process or activity. Rendered with an open arrowhead in the top-left corner.

```csharp
string outputData = editor.AddDataOutput(null, "Validation Result");

editor.AddFlow(null, null, validateTask, outputData,
    null, FlowType.Association, null, false, FlowDirection.Directed);
```

---

## `AddDataStore(string processId, string text)`

**§10.4.1 Data Store Reference**

Represents a persistent data store (database, file system, etc.) accessible across multiple activities and potentially across process instances. Rendered as a cylinder.

```csharp
string archive = editor.AddDataStore(null, "Document Archive");

// Task reads from and writes to the store
editor.AddFlow(null, null, archive, processTask,
    null, FlowType.Association, null, false, FlowDirection.Directed);
editor.AddFlow(null, null, processTask, archive,
    null, FlowType.Association, null, false, FlowDirection.Directed);

// Or use bidirectional
editor.AddFlow(null, null, processTask, archive,
    null, FlowType.Association, null, false, FlowDirection.BiDirected);
```

---

## `AddGroup(string parentId, string name)`

**§8.4.1.2 Group**

Draws a dashed rounded-rectangle border around related elements for visual organisation. Groups have no effect on execution and can span pool and lane boundaries (unlike sub-processes).

| Parameter | Description |
|-----------|-------------|
| `parentId` | Parent process or collaboration ID. `null` = default process. |
| `name` | Optional label displayed on the group border. |

```csharp
string group = editor.AddGroup(null, "Automated Checks");

// In the diagram, size the group to encompass the shapes it contains
editor.AddShape(diag, new Shape {
    ElementRef = group,
    Bounds = new List<RectangleF> { new RectangleF(80, 10, 380, 140) }
});
```

> Groups do not contain elements semantically — they group them visually. Any element whose shape falls within the group's bounding rectangle appears grouped.

---

## `AddMessage(string itemId, string text)`

**§8.4.11 Message · §8.4.10 ItemDefinition**

Creates a BPMN Message element. Messages carry data between participants. According to the BPMN spec (§8.4.11), a message may have an associated `ItemDefinition` (§8.4.10) that describes its payload structure.

| Parameter | Description |
|-----------|-------------|
| `itemId` | ID of an `ItemDefinition` describing the message payload. `null` for an untyped message. |
| `text` | Human-readable message name (e.g. `"Purchase Order"`). |

The returned ID can be passed as `messageId` in `AddFlow` to create a typed Message Flow.

```csharp
// Untyped message — no payload structure
string orderMsg = editor.AddMessage(null, "Purchase Order");

// Typed message — references an ItemDefinition
string invoiceItemDef = /* ID of an ItemDefinition element */;
string invoiceMsg = editor.AddMessage(invoiceItemDef, "Invoice");

// Use in a Message flow
editor.AddFlow(null, orderMsg, sendPO, suppStart,
    "Purchase Order", FlowType.Message, null, false, FlowDirection.None);
```

Messages can also be referenced by Message Start Events and Message End Events — the message name is displayed as the event's label.

---

## Example — Document Approval with Data and Annotations

```csharp
public static void CreateDocumentApproval(string outputPath)
{
    Editor editor = new Editor();
    editor.Create("Document Approval", "BPMN.Sharp");

    // Define messages
    string submitMsg   = editor.AddMessage(null, "Document Submission");
    string decisionMsg = editor.AddMessage(null, "Approval Decision");

    // Process elements
    string start = editor.AddEvent(null, null, "Submission Received",
        EventType.Start, EventTrigger.Message, EventRole.None);
    string validate = editor.AddActivity(null, "Validate Document",
        ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
    string review = editor.AddActivity(null, "Review Document",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);
    string decide = editor.AddGateway(null, "Approved?",
        GatewayType.Exclusive, 1, 2);
    string notifyApprove = editor.AddActivity(null, "Send Approval",
        ActivityType.Task, ActivityMarker.None, TaskType.Send, null);
    string notifyReject  = editor.AddActivity(null, "Send Rejection",
        ActivityType.Task, ActivityMarker.None, TaskType.Send, null);
    string merge = editor.AddGateway(null, null,
        GatewayType.Exclusive, 2, 1);
    string archive = editor.AddActivity(null, "Archive Document",
        ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
    string end = editor.AddEvent(null, null, "Complete",
        EventType.End, EventTrigger.None, EventRole.None);

    // Data elements
    string docObj   = editor.AddDataObject(null, "Document");
    string reviewOut= editor.AddDataOutput(null, "Review Comments");
    string docStore = editor.AddDataStore(null, "Document Archive");

    // Annotations
    string slaNote   = editor.AddAnnotation(null, "SLA: 2 business days", false);
    string retNote   = editor.AddAnnotation(null, "Retained 7 years", true);

    // Group — visually groups the notification tasks
    string notifyGrp = editor.AddGroup(null, "Notifications");

    // Sequence flows
    editor.AddFlow(null, null, start,        validate,     null,       FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, validate,     review,       null,       FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, review,       decide,       null,       FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, decide,       notifyApprove,"Approved", FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, decide,       notifyReject, "Rejected", FlowType.Sequence, null, true,  FlowDirection.None);
    editor.AddFlow(null, null, notifyApprove,merge,        null,       FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, notifyReject, merge,        null,       FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, merge,        archive,      null,       FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, archive,      end,          null,       FlowType.Sequence, null, false, FlowDirection.None);

    // Associations
    editor.AddFlow(null, null, docObj,   validate,  null, FlowType.Association, null, false, FlowDirection.Directed);
    editor.AddFlow(null, null, docObj,   review,    null, FlowType.Association, null, false, FlowDirection.Directed);
    editor.AddFlow(null, null, review,   reviewOut, null, FlowType.Association, null, false, FlowDirection.Directed);
    editor.AddFlow(null, null, archive,  docStore,  null, FlowType.Association, null, false, FlowDirection.Directed);
    editor.AddFlow(null, null, slaNote,  review,    null, FlowType.Association, null, false, FlowDirection.None);
    editor.AddFlow(null, null, retNote,  archive,   null, FlowType.Association, null, false, FlowDirection.None);

    // Diagram
    string diag = editor.AddDiagram("Diagram 1", 96f);

    void S(string id, float x, float y, float w, float h) =>
        editor.AddShape(diag, new Shape { ElementRef = id,
            Bounds = new List<RectangleF> { new RectangleF(x, y, w, h) } });

    S(start,        20, 192, 36,  36);
    S(validate,     96, 170,120,  80);
    S(review,      256, 170,120,  80);
    S(decide,      416, 188, 44,  44);
    S(notifyApprove,500,130,140,  80);
    S(notifyReject, 500,230,140,  80);
    S(merge,        680,188, 44,  44);
    S(archive,      764,170,120,  80);
    S(end,          924,192, 36,  36);

    S(docObj,       130,  70, 36,  50);
    S(reviewOut,    286,  70, 36,  50);
    S(docStore,     794,  280, 50, 60);
    S(slaNote,       10,  70,220,  40);
    S(retNote,      680,  290,240, 40);
    S(notifyGrp,    488,  118,164, 204);

    editor.Save(Path.Combine(outputPath, "document-approval.bpmn"));
}
```

---

## See Also

- [05 – Flows](05-flows.md) — associations connect artifacts to flow elements
- [06 – Pools and Lanes](06-pools-lanes.md) — groups can span pool boundaries
- [08 – Diagram Layout](08-diagram-layout.md) — positioning artifact shapes
