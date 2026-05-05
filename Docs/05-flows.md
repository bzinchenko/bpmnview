# 05 – Flows

> **BPMN Spec Reference:** §8.4.13 Sequence Flow · §9.4 Message Flow · §8.4.1 Association · §7.6 Connection Rules

---

## Overview

Flows connect elements. BPMN defines three types, all created with a single method:

```csharp
string AddFlow(string parentId, string messageId,
               string idFrom, string idTo, string name,
               FlowType flowType, string condition,
               bool isDefault, FlowDirection direction)
```

| Flow Type | Purpose | Spec |
|-----------|---------|------|
| **Sequence** | Orders execution within a pool or sub-process | §8.4.13 |
| **Message** | Communicates between separate pools | §9.4 |
| **Association** | Links artifacts to flow elements (informational only) | §8.4.1 |

---

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `parentId` | `string` | For Sequence flows: ID of the parent process or sub-process. `null` = default process. For Message/Association flows: `null`. |
| `messageId` | `string` | For Message flows: ID of a BPMN Message element created with `AddMessage()`. `null` for an untyped message flow or for Sequence/Association flows. |
| `idFrom` | `string` | ID of the source element. |
| `idTo` | `string` | ID of the target element. |
| `name` | `string` | Optional label. Used as condition description on gateway outflows. |
| `flowType` | `FlowType` | `Sequence`, `Message`, or `Association`. |
| `condition` | `string` | Formal condition expression (e.g. `"${approved == true}"`). `null` if not conditional. |
| `isDefault` | `bool` | `true` marks this as the default flow from an Exclusive or Inclusive gateway (§8.4.13). |
| `direction` | `FlowDirection` | For `Association` flows only: `None`, `Directed`, or `BiDirected`. |

**Returns:** `string` — the auto-generated flow ID, used in `AddEdge`.

---

## Sequence Flows · §8.4.13

```csharp
// Plain — unconditional
string f = editor.AddFlow(null, null, taskA, taskB,
    null, FlowType.Sequence, null, false, FlowDirection.None);

// Named — visible label on diagram (useful for gateway branches)
string fNamed = editor.AddFlow(null, null, gateway, approveTask,
    "Approved", FlowType.Sequence, null, false, FlowDirection.None);

// Conditional — carries a formal expression evaluated at runtime
string fCond = editor.AddFlow(null, null, gateway, approveTask,
    "Approved",
    FlowType.Sequence,
    "${data.approved == true}",   // condition expression
    false,
    FlowDirection.None);

// Default — taken when no other condition matches (§10.6.1)
// Only one default flow per gateway; Exclusive and Inclusive only.
string fDefault = editor.AddFlow(null, null, gateway, rejectTask,
    "Otherwise",
    FlowType.Sequence,
    null,
    true,    // isDefault = true
    FlowDirection.None);

// Sub-process — pass the sub-process ID as parentId
string fInner = editor.AddFlow(subProcId, null, innerStart, innerTask,
    null, FlowType.Sequence, null, false, FlowDirection.None);
```

---

## Message Flows · §9.4

Message flows connect elements in **different** pools. They cannot cross within the same pool (§7.6.2).

```csharp
// messageId = null — untyped message flow
editor.AddFlow(null, null, sendTask, receiveTask,
    "Order", FlowType.Message, null, false, FlowDirection.None);

// messageId = ID returned by AddMessage() — typed message flow
string msgId = editor.AddMessage(null, "Purchase Order");
editor.AddFlow(null, msgId, sendPO, suppStart,
    "Purchase Order", FlowType.Message, null, false, FlowDirection.None);
```

---

## Associations · §8.4.1

Associations link artifacts to flow elements. They carry no execution semantics.

```csharp
string note = editor.AddAnnotation(null,
    "SLA: complete within 24h", false);

// Undirected — no arrowheads
editor.AddFlow(null, null, note, reviewTask,
    null, FlowType.Association, null, false, FlowDirection.None);

// Directed — arrow from data object into task (data input)
string dataObj = editor.AddDataObject(null, "Order Form");
editor.AddFlow(null, null, dataObj, processTask,
    null, FlowType.Association, null, false, FlowDirection.Directed);

// Directed — arrow from task to data object (data output)
string receipt = editor.AddDataObject(null, "Receipt");
editor.AddFlow(null, null, processTask, receipt,
    null, FlowType.Association, null, false, FlowDirection.Directed);

// Bidirectional
editor.AddFlow(null, null, dataStore, task,
    null, FlowType.Association, null, false, FlowDirection.BiDirected);
```

---

## Connection Rules · §7.6

| Source → Target | Sequence | Message |
|-----------------|:--------:|:-------:|
| Element → Element (same pool) | ✓ | – |
| Element → Element (different pools) | – | ✓ |
| Pool boundary → Pool boundary | – | ✓ |
| Flow element → Artifact | – (use Association) | – |

---

## Example — MIWG B.1.0: B2B Procurement with Message Flows

The MIWG B.1.0 reference model shows two pools exchanging message flows for a procurement scenario.

**Reference:** https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/B.1.0.png

```csharp
public static void CreateB10(string outputPath)
{
    Editor editor = new Editor();
    editor.Create("B.1.0 – Procurement", "BPMN.Sharp");

    string custPool = editor.AddPool(null, null, "Customer");
    string suppPool = editor.AddPool(null, null, "Supplier");

    // Customer elements
    string cStart  = editor.AddEvent(null, null, "Start",
        EventType.Start, EventTrigger.None, EventRole.None);
    string createPO= editor.AddActivity(null, "Create PO",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);
    string sendPO  = editor.AddActivity(null, "Send PO",
        ActivityType.Task, ActivityMarker.None, TaskType.Send, null);
    string recvGds = editor.AddActivity(null, "Receive Goods",
        ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);
    string recvInv = editor.AddActivity(null, "Receive Invoice",
        ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);
    string pay     = editor.AddActivity(null, "Pay Invoice",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);
    string cEnd    = editor.AddEvent(null, null, "End",
        EventType.End, EventTrigger.None, EventRole.None);

    // Supplier elements
    string sStart  = editor.AddEvent(null, null, "PO Received",
        EventType.Start, EventTrigger.Message, EventRole.None);
    string process = editor.AddActivity(null, "Process Order",
        ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
    string ship    = editor.AddActivity(null, "Ship Goods",
        ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);
    string invoice = editor.AddActivity(null, "Send Invoice",
        ActivityType.Task, ActivityMarker.None, TaskType.Send, null);
    string sEnd    = editor.AddEvent(null, null, "End",
        EventType.End, EventTrigger.None, EventRole.None);

    // Sequence flows — parentId = null (default process for each pool)
    editor.AddFlow(null, null, cStart,  createPO, null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, createPO,sendPO,   null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, sendPO,  recvGds,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, recvGds, recvInv,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, recvInv, pay,      null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, pay,     cEnd,     null, FlowType.Sequence, null, false, FlowDirection.None);

    editor.AddFlow(null, null, sStart,  process,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, process, ship,     null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, ship,    invoice,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, invoice, sEnd,     null, FlowType.Sequence, null, false, FlowDirection.None);

    // Message flows — messageId = null (untyped)
    editor.AddFlow(null, null, sendPO,  sStart,  "Purchase Order", FlowType.Message, null, false, FlowDirection.None);
    editor.AddFlow(null, null, ship,    recvGds, "Goods",          FlowType.Message, null, false, FlowDirection.None);
    editor.AddFlow(null, null, invoice, recvInv, "Invoice",        FlowType.Message, null, false, FlowDirection.None);
    editor.AddFlow(null, null, pay,     sEnd,    "Payment",        FlowType.Message, null, false, FlowDirection.None);

    // Text annotation with association
    string note = editor.AddAnnotation(null, "Standard terms apply", false);
    editor.AddFlow(null, null, note, sendPO,
        null, FlowType.Association, null, false, FlowDirection.None);

    // Diagram
    string diag = editor.AddDiagram("Diagram 1", 96f);

    editor.AddShape(diag, new Shape { ElementRef = custPool,
        Bounds = new List<RectangleF> { new RectangleF(10, 10, 920, 130) } });
    editor.AddShape(diag, new Shape { ElementRef = suppPool,
        Bounds = new List<RectangleF> { new RectangleF(10, 160, 920, 130) } });

    void S(string id, float x, float y, float w, float h) =>
        editor.AddShape(diag, new Shape { ElementRef = id,
            Bounds = new List<RectangleF> { new RectangleF(x, y, w, h) } });

    // Customer pool (y ≈ 10–140, centre ≈ 75)
    S(cStart,   40,  57, 36, 36);
    S(createPO,116,  35,120, 80);
    S(sendPO,  276,  35,120, 80);
    S(recvGds, 516,  35,120, 80);
    S(recvInv, 676,  35,120, 80);
    S(pay,     836,  35,120, 80);
    S(cEnd,    896,  57, 36, 36);

    // Supplier pool (y ≈ 160–290, centre ≈ 225)
    S(sStart,  276, 207, 36, 36);
    S(process, 352, 185,120, 80);
    S(ship,    512, 185,120, 80);
    S(invoice, 672, 185,120, 80);
    S(sEnd,    832, 207, 36, 36);

    // Annotation
    S(note,     160,   0,180, 40);

    editor.Save(Path.Combine(outputPath, "B.1.0.bpmn"));
}
```

---

## See Also

- [04 – Gateways](04-gateways.md) — conditional and default flows
- [06 – Pools and Lanes](06-pools-lanes.md) — message flows require multiple pools
- [07 – Data and Artifacts](07-data-artifacts.md) — associations link artifacts to flow elements
