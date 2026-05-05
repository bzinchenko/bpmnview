# 04 – Gateways

> **BPMN Spec Reference:** §10.6 Gateways · §10.6.2 Exclusive · §10.6.3 Inclusive · §10.6.4 Parallel · §10.6.5 Complex · §10.6.6 Event-Based

---

## Overview

Gateways control how sequence flow diverges (splits) and converges (merges). BPMN 2.0 defines five gateway types:

```csharp
string AddGateway(string processId, string name,
                  GatewayType gatewayType,
                  int inputs = 0, int outputs = 0)
```

---

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `processId` | `string` | ID of the parent process or sub-process. `null` = default process. |
| `name` | `string` | Human-readable label. Use a question for split gateways, e.g. `"Approved?"`. |
| `gatewayType` | `GatewayType` | Routing semantics. |
| `inputs` | `int` | Optional hint: expected number of incoming flows (default 0 = unspecified). |
| `outputs` | `int` | Optional hint: expected number of outgoing flows (default 0 = unspecified). |

**Returns:** `string` — the auto-generated element ID.

---

## GatewayType · §10.6

| Value | BPMN Name | Spec | Split Behaviour | Join Behaviour |
|-------|-----------|------|-----------------|----------------|
| `Exclusive` | Exclusive (XOR) Gateway | §10.6.2 | Exactly one path taken | Pass when any one token arrives |
| `Inclusive` | Inclusive (OR) Gateway | §10.6.3 | One or more paths based on conditions | Wait for all active incoming paths |
| `Parallel` | Parallel (AND) Gateway | §10.6.4 | All paths activated simultaneously | Wait for all incoming paths |
| `Complex` | Complex Gateway | §10.6.5 | Custom activation condition | Custom merge condition |
| `EventBased` | Event-Based Gateway | §10.6.6 | Route to first following event to fire | N/A (split only) |

---

## Exclusive Gateway · §10.6.2

Exactly one outgoing path is taken. The default path (marked with `isDefault=true` on `AddFlow`) is taken when no condition matches.

```csharp
string decide = editor.AddGateway(null, "Approved?",
    GatewayType.Exclusive, 1, 2);

editor.AddFlow(null, null, decide, approveTask,
    "Yes", FlowType.Sequence, null, false, FlowDirection.None);

// Default flow — taken when no condition matches
editor.AddFlow(null, null, decide, rejectTask,
    "No", FlowType.Sequence, null, true, FlowDirection.None);

// Exclusive join — pass through on first arriving token
string join = editor.AddGateway(null, null,
    GatewayType.Exclusive, 2, 1);
```

---

## Inclusive Gateway · §10.6.3

One or more outgoing paths can be active simultaneously. The join waits for all currently active paths.

```csharp
string incSplit = editor.AddGateway(null, "Which Services?",
    GatewayType.Inclusive, 1, 3);

editor.AddFlow(null, null, incSplit, bookFlight,
    "Needs Flight", FlowType.Sequence, null, false, FlowDirection.None);
editor.AddFlow(null, null, incSplit, bookHotel,
    "Needs Hotel", FlowType.Sequence, null, false, FlowDirection.None);
editor.AddFlow(null, null, incSplit, bookCar,
    "Needs Car", FlowType.Sequence, null, false, FlowDirection.None);

string incJoin = editor.AddGateway(null, null,
    GatewayType.Inclusive, 3, 1);
```

---

## Parallel Gateway · §10.6.4

All outgoing paths are activated at once. The join waits for every incoming path to complete.

```csharp
string parSplit = editor.AddGateway(null, null,
    GatewayType.Parallel, 1, 3);

// All three run simultaneously
editor.AddFlow(null, null, parSplit, creditCheck,
    null, FlowType.Sequence, null, false, FlowDirection.None);
editor.AddFlow(null, null, parSplit, fraudCheck,
    null, FlowType.Sequence, null, false, FlowDirection.None);
editor.AddFlow(null, null, parSplit, idCheck,
    null, FlowType.Sequence, null, false, FlowDirection.None);

// Join waits for all three
string parJoin = editor.AddGateway(null, null,
    GatewayType.Parallel, 3, 1);
```

---

## Complex Gateway · §10.6.5

For routing rules that cannot be expressed with the other types — for example, "continue when at least 2 of 3 paths complete".

```csharp
string complexGw = editor.AddGateway(null, "2 of 3 Required",
    GatewayType.Complex, 3, 1);
```

---

## Event-Based Gateway · §10.6.6

Routes to whichever of its following catching events fires first. Every outgoing path must lead directly to an Intermediate Catching Event or a Receive Task.

```csharp
string evtGw = editor.AddGateway(null, "Await Response",
    GatewayType.EventBased, 1, 2);

string msgEvt = editor.AddEvent(null, null, "Payment Received",
    EventType.Intermediate, EventTrigger.Message, EventRole.None);
string timerEvt = editor.AddEvent(null, null, "7 Days Elapsed",
    EventType.Intermediate, EventTrigger.Timer, EventRole.None);

editor.AddFlow(null, null, evtGw, msgEvt,
    null, FlowType.Sequence, null, false, FlowDirection.None);
editor.AddFlow(null, null, evtGw, timerEvt,
    null, FlowType.Sequence, null, false, FlowDirection.None);
```

---

## Example — MIWG A.4.0: Exclusive and Parallel Gateways

The MIWG A.4.0 reference model combines a parallel split/join for simultaneous checks with an exclusive decision gateway.

**Reference:** https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.4.0.png

```csharp
public static void CreateA40(string outputPath)
{
    Editor editor = new Editor();
    editor.Create("A.4.0 – Gateways", "BPMN.Sharp");

    string start = editor.AddEvent(null, null, "Start",
        EventType.Start, EventTrigger.None, EventRole.None);

    // Parallel split — run two checks simultaneously
    string parSplit = editor.AddGateway(null, null,
        GatewayType.Parallel, 1, 2);

    string task1 = editor.AddActivity(null, "Task 1",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string task2 = editor.AddActivity(null, "Task 2",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);

    // Parallel join
    string parJoin = editor.AddGateway(null, null,
        GatewayType.Parallel, 2, 1);

    // Exclusive decision
    string xorSplit = editor.AddGateway(null, "Condition?",
        GatewayType.Exclusive, 1, 2);

    string task3 = editor.AddActivity(null, "Task 3",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string task4 = editor.AddActivity(null, "Task 4",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);

    // Exclusive join
    string xorJoin = editor.AddGateway(null, null,
        GatewayType.Exclusive, 2, 1);

    string end = editor.AddEvent(null, null, "End",
        EventType.End, EventTrigger.None, EventRole.None);

    editor.AddFlow(null, null, start,    parSplit, null,   FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, parSplit, task1,    null,   FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, parSplit, task2,    null,   FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task1,    parJoin,  null,   FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task2,    parJoin,  null,   FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, parJoin,  xorSplit, null,   FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, xorSplit, task3,    "Yes",  FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, xorSplit, task4,    "No",   FlowType.Sequence, null, true,  FlowDirection.None);
    editor.AddFlow(null, null, task3,    xorJoin,  null,   FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task4,    xorJoin,  null,   FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, xorJoin,  end,      null,   FlowType.Sequence, null, false, FlowDirection.None);

    string diag = editor.AddDiagram("Diagram 1", 96f);

    editor.AddShape(diag, new Shape { ElementRef = start,
        Bounds = new List<RectangleF> { new RectangleF(20, 118, 36, 36) } });
    editor.AddShape(diag, new Shape { ElementRef = parSplit,
        Bounds = new List<RectangleF> { new RectangleF(96, 114, 44, 44) } });
    editor.AddShape(diag, new Shape { ElementRef = task1,
        Bounds = new List<RectangleF> { new RectangleF(180, 60, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = task2,
        Bounds = new List<RectangleF> { new RectangleF(180, 160, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = parJoin,
        Bounds = new List<RectangleF> { new RectangleF(340, 114, 44, 44) } });
    editor.AddShape(diag, new Shape { ElementRef = xorSplit,
        Bounds = new List<RectangleF> { new RectangleF(424, 114, 44, 44) } });
    editor.AddShape(diag, new Shape { ElementRef = task3,
        Bounds = new List<RectangleF> { new RectangleF(508, 60, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = task4,
        Bounds = new List<RectangleF> { new RectangleF(508, 160, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = xorJoin,
        Bounds = new List<RectangleF> { new RectangleF(668, 114, 44, 44) } });
    editor.AddShape(diag, new Shape { ElementRef = end,
        Bounds = new List<RectangleF> { new RectangleF(752, 118, 36, 36) } });

    editor.Save(Path.Combine(outputPath, "A.4.0.bpmn"));
}
```

---

## See Also

- [02 – Events](02-events.md) — Event-Based Gateway targets
- [05 – Flows](05-flows.md) — conditional and default flows from gateways
