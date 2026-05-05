# 03 – Activities

> **BPMN Spec Reference:** §10.3 Activities · §10.3.3 Tasks · §10.3.5 Sub-Processes · §10.3.6 Call Activity · §10.3.8 Loop Characteristics · §10.7 Compensation

---

## Overview

Activities represent work performed within a process — atomic tasks or compound sub-processes. All activity variants are created with a single method:

```csharp
string AddActivity(string processId, string name,
                   ActivityType activityType, ActivityMarker marker,
                   TaskType taskType, string calledElement)
```

---

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `processId` | `string` | ID of the parent process or sub-process. `null` = default process. |
| `name` | `string` | Human-readable label on the shape. |
| `activityType` | `ActivityType` | Task, Activity, Transaction, EventSubProcess, etc. See ActivityType table. |
| `marker` | `ActivityMarker` | Loop or multi-instance marker. |
| `taskType` | `TaskType` | Task specialisation (User, Service, etc.). Relevant only when `activityType` is `Task`. |
| `calledElement` | `string` | For Call Activity (`ActivityType.Activity`): the ID or name of the globally reusable element. `null` otherwise. |

**Returns:** `string` — the auto-generated element ID.

---

## ActivityType · §10.3

| Value | BPMN Element | Spec | Description |
|-------|-------------|------|-------------|
| `Task` | Task | §10.3.3 | Atomic unit of work, not decomposed further in the model. |
| `Activity` | Sub-Process / Call Activity | §10.3.5, §10.3.6 | Compound activity or call to a reusable element. Use `ActivityMarker.AdHoc` for ad-hoc behaviour. |
| `EventSubProcess` | Event Sub-Process | §10.3.5 | Sub-process triggered by a start event. |
| `Choreography` | Choreography | §11 | Choreography task. |
| `Transaction` | Transaction | §10.3.5 | Sub-process with all-or-nothing transactional semantics. |

---

## TaskType · §10.3.3

Applies only when `ActivityType.Task` is used.

| Value | BPMN Specialisation | Description |
|-------|---------------------|-------------|
| `None` | Abstract Task | General-purpose; no specific technology. |
| `User` | User Task | Performed by a person via a form or workflow UI. |
| `Manual` | Manual Task | Performed by a person without any system support. |
| `Service` | Service Task | Automated call to a web service or system API. |
| `Send` | Send Task | Sends a BPMN Message to a participant. |
| `Receive` | Receive Task | Waits to receive a BPMN Message from a participant. |
| `BusinessRule` | Business Rule Task | Invokes a business rules engine. |
| `Script` | Script Task | Executes a script (e.g. Groovy, JavaScript). |

---

## ActivityMarker · §10.3.8

| Value | Marker | Description |
|-------|--------|-------------|
| `None` | — | No loop or multi-instance behaviour. |
| `Loop` | Standard loop | Repeats while/until a condition is true. |
| `Sequential` | Sequential multi-instance | One instance per list item, run one at a time. |
| `Parallel` | Parallel multi-instance | One instance per list item, all run simultaneously. |
| `Compensation` | Compensation marker | Marks the activity as a compensation handler (§10.7). |

---

## Tasks · §10.3.3

```csharp
// Abstract task
string t1 = editor.AddActivity(null, "Review Document",
    ActivityType.Task, ActivityMarker.None, TaskType.None, null);

// User task — human via form
string t2 = editor.AddActivity(null, "Approve Purchase Order",
    ActivityType.Task, ActivityMarker.None, TaskType.User, null);

// Manual task — human, no system
string t3 = editor.AddActivity(null, "Sign Contract",
    ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);

// Service task — automated API call
string t4 = editor.AddActivity(null, "Validate Address",
    ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

// Send task — sends a message
string t5 = editor.AddActivity(null, "Send Invoice",
    ActivityType.Task, ActivityMarker.None, TaskType.Send, null);

// Receive task — waits for a message
string t6 = editor.AddActivity(null, "Wait for Payment",
    ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);

// Business rule task
string t7 = editor.AddActivity(null, "Determine Discount",
    ActivityType.Task, ActivityMarker.None, TaskType.BusinessRule, null);

// Script task
string t8 = editor.AddActivity(null, "Calculate Total",
    ActivityType.Task, ActivityMarker.None, TaskType.Script, null);
```

---

## Loop and Multi-Instance Markers · §10.3.8

```csharp
// Standard loop — repeats while a condition holds
string loopTask = editor.AddActivity(null, "Retry Payment",
    ActivityType.Task, ActivityMarker.Loop, TaskType.Service, null);

// Sequential multi-instance — one item at a time
string seqMI = editor.AddActivity(null, "Review Each Application",
    ActivityType.Task, ActivityMarker.Sequential, TaskType.User, null);

// Parallel multi-instance — all items simultaneously
string parMI = editor.AddActivity(null, "Notify All Approvers",
    ActivityType.Task, ActivityMarker.Parallel, TaskType.Send, null);
```

---

## Sub-Processes · §10.3.5

Child elements of a sub-process reference its ID as their `processId`.

```csharp
string subProc = editor.AddActivity(null, "Handle Shipment",
    ActivityType.Activity, ActivityMarker.None, TaskType.None, null);

// Child elements — pass subProc as their processId
string spStart = editor.AddEvent(subProc, null, "Start",
    EventType.Start, EventTrigger.None, EventRole.None);
string pack = editor.AddActivity(subProc, "Pack Items",
    ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);
string ship = editor.AddActivity(subProc, "Ship Package",
    ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
string spEnd = editor.AddEvent(subProc, null, "End",
    EventType.End, EventTrigger.None, EventRole.None);

// Flows inside the sub-process — parentId = subProc
editor.AddFlow(subProc, null, spStart, pack,
    null, FlowType.Sequence, null, false, FlowDirection.None);
editor.AddFlow(subProc, null, pack, ship,
    null, FlowType.Sequence, null, false, FlowDirection.None);
editor.AddFlow(subProc, null, ship, spEnd,
    null, FlowType.Sequence, null, false, FlowDirection.None);
```

---

## Transaction Sub-Process · §10.3.5

Transactions have all-or-nothing semantics. A Cancel End Event inside triggers compensation of all completed activities.

```csharp
string tx = editor.AddActivity(null, "Book Travel",
    ActivityType.Transaction, ActivityMarker.None, TaskType.None, null);

string flight = editor.AddActivity(tx, "Reserve Flight",
    ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
string hotel = editor.AddActivity(tx, "Reserve Hotel",
    ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

// Cancel boundary — fires when a Cancel End Event is reached inside the transaction
string cancelBound = editor.AddEvent(null, tx, "Booking Failed",
    EventType.Intermediate, EventTrigger.Cancel, EventRole.Catching);
```

---

## Call Activity · §10.3.6

Calls a globally reusable process or task. Pass the global element's ID or name in `calledElement`.

```csharp
string call = editor.AddActivity(null, "Perform KYC Check",
    ActivityType.Activity, ActivityMarker.None, TaskType.None,
    "GlobalKYCProcess");   // calledElement
```

---

## Compensation Handler · §10.7

Mark an activity as a compensation handler with `ActivityMarker.Compensation`. Connect it to the activity it compensates using `FlowType.Association`.

```csharp
string bookPayment = editor.AddActivity(null, "Book Payment",
    ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

// Compensation handler — runs if bookPayment must be undone
string refund = editor.AddActivity(null, "Refund Payment",
    ActivityType.Task, ActivityMarker.Compensation, TaskType.Service, null);

// Association links handler to the activity it compensates
editor.AddFlow(null, null, bookPayment, refund,
    null, FlowType.Association, null, false, FlowDirection.Directed);
```

---

## Example — MIWG C.1.0: Invoice Handling with Sub-Process

The MIWG C.1.0 reference model shows an invoice review sub-process with a loop task and a compensation handler.

**Reference:** https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/C.1.0.png

```csharp
public static void CreateC10(string outputPath)
{
    Editor editor = new Editor();
    editor.Create("C.1.0 – Invoice Handling", "BPMN.Sharp");

    string start = editor.AddEvent(null, null, "Invoice Received",
        EventType.Start, EventTrigger.Message, EventRole.None);

    // Sub-Process for review
    string review = editor.AddActivity(null, "Review Invoice",
        ActivityType.Activity, ActivityMarker.None, TaskType.None, null);

    // Internal elements
    string spStart = editor.AddEvent(review, null, "Start",
        EventType.Start, EventTrigger.None, EventRole.None);
    string assign = editor.AddActivity(review, "Assign Approver",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);
    string approve = editor.AddActivity(review, "Approve Invoice",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);
    // Loop task — repeats clarification until resolved
    string clarify = editor.AddActivity(review, "Clarify Invoice",
        ActivityType.Task, ActivityMarker.Loop, TaskType.User, null);
    string spGw = editor.AddGateway(review, "OK?",
        GatewayType.Exclusive, 1, 2);
    string spEnd = editor.AddEvent(review, null, "End",
        EventType.End, EventTrigger.None, EventRole.None);

    editor.AddFlow(review, null, spStart, assign,   null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(review, null, assign,  approve,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(review, null, approve, spGw,     null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(review, null, spGw,    spEnd,    "Yes",FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(review, null, spGw,    clarify,  "No", FlowType.Sequence, null, true,  FlowDirection.None);
    editor.AddFlow(review, null, clarify, approve,  null, FlowType.Sequence, null, false, FlowDirection.None);

    // Book payment with compensation handler
    string book = editor.AddActivity(null, "Book Payment",
        ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
    string reverse = editor.AddActivity(null, "Reverse Payment",
        ActivityType.Task, ActivityMarker.Compensation, TaskType.Service, null);

    string end = editor.AddEvent(null, null, "Invoice Handled",
        EventType.End, EventTrigger.None, EventRole.None);

    editor.AddFlow(null, null, start,  review, null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, review, book,   null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, book,   end,    null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, book,   reverse,null, FlowType.Association, null, false, FlowDirection.Directed);

    // Diagram
    string diag = editor.AddDiagram("Diagram 1", 96f);

    editor.AddShape(diag, new Shape { ElementRef = start,
        Bounds = new List<RectangleF> { new RectangleF(20, 180, 36, 36) } });
    editor.AddShape(diag, new Shape { ElementRef = review,
        Bounds = new List<RectangleF> { new RectangleF(96, 20, 440, 310) } });
    editor.AddShape(diag, new Shape { ElementRef = book,
        Bounds = new List<RectangleF> { new RectangleF(576, 170, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = reverse,
        Bounds = new List<RectangleF> { new RectangleF(576, 280, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = end,
        Bounds = new List<RectangleF> { new RectangleF(746, 180, 36, 36) } });

    // Sub-process internals
    editor.AddShape(diag, new Shape { ElementRef = spStart,
        Bounds = new List<RectangleF> { new RectangleF(116, 160, 36, 36) } });
    editor.AddShape(diag, new Shape { ElementRef = assign,
        Bounds = new List<RectangleF> { new RectangleF(192, 138, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = approve,
        Bounds = new List<RectangleF> { new RectangleF(352, 138, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = spGw,
        Bounds = new List<RectangleF> { new RectangleF(492, 156, 44, 44) } });
    editor.AddShape(diag, new Shape { ElementRef = clarify,
        Bounds = new List<RectangleF> { new RectangleF(352, 240, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = spEnd,
        Bounds = new List<RectangleF> { new RectangleF(476, 260, 36, 36) } });

    editor.Save(Path.Combine(outputPath, "C.1.0.bpmn"));
}
```

---

## See Also

- [02 – Events](02-events.md) — boundary events attach to activities
- [04 – Gateways](04-gateways.md) — routing flow after decisions
- [05 – Flows](05-flows.md) — connecting activities
- [06 – Pools and Lanes](06-pools-lanes.md) — assigning activities to lanes
