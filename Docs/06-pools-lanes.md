# 06 – Pools and Lanes

> **BPMN Spec Reference:** §9.1 Collaboration · §9.3 Pool and Participant · §9.3.2 Lanes · §10.8 Lanes

---

## Overview

Pools and lanes are the swimlane constructs in BPMN. A **Pool** represents a participant — an independent organisation, system or role with its own process. Multiple pools form a **Collaboration** (§9.1). A **Lane** subdivides a pool by role or department without affecting execution semantics (§10.8).

---

## `AddCollaboration(string name = "")`

**§9.1 Collaboration**

Creates a named Collaboration element — the container for all Participants and Message Flows in a multi-pool model. If not called explicitly, a default collaboration is created automatically when the first pool is added.

```csharp
string collabId = editor.AddCollaboration("Procurement Collaboration");
```

---

## `AddProcess(string name = "")`

**§10.1 Process**

Creates a named Process element. Normally one process is created automatically per pool. Call explicitly when you need the process ID — for example, to pass it to `AddLane` or `AddToLane`, or to scope elements to a specific named process.

```csharp
string custProcess = editor.AddProcess("Customer Process");
string suppProcess = editor.AddProcess("Supplier Process");
```

---

## `AddPool(string parentId, string processId, string name)`

**§9.3.1 Participant**

Creates a pool (Participant element) within a collaboration.

| Parameter | Description |
|-----------|-------------|
| `parentId` | ID of the collaboration. `null` = default collaboration. |
| `processId` | ID of an existing process to associate. `null` = new process created automatically. |
| `name` | Display name of the pool — the participant's name. |

**Returns:** `string` — pool ID, used in `AddShape`.

```csharp
string custPool = editor.AddPool(collabId, custProcess, "Customer");
string suppPool = editor.AddPool(collabId, suppProcess, "Supplier");

// null parentId — adds to default collaboration
string pool = editor.AddPool(null, null, "My Process");
```

---

## `AddLane(string parentId, string name)`

**§9.3.2 / §10.8 Lane**

Creates a lane within a pool. For nested lanes, pass a lane ID as `parentId`.

| Parameter | Description |
|-----------|-------------|
| `parentId` | ID of the parent pool (or parent lane for nesting). `null` = default process. |
| `name` | Display name of the lane. |

**Returns:** `string` — lane ID, used in `AddShape` and `AddToLane`.

```csharp
string salesLane   = editor.AddLane(custPool, "Sales");
string financeLane = editor.AddLane(custPool, "Finance");

// Nested lanes
string itDept  = editor.AddLane(pool, "IT Department");
string devLane = editor.AddLane(itDept, "Development");   // child of itDept
string opsLane = editor.AddLane(itDept, "Operations");
```

---

## `AddToLane(string processId, string laneId, string elementId)`

**§10.8 Lane**

Assigns an existing element to a lane. Lane assignment is for visual organisation only — it has no effect on execution.

| Parameter | Description |
|-----------|-------------|
| `processId` | ID of the process containing the element. `null` = default process. |
| `laneId` | ID of the target lane. |
| `elementId` | ID of the element to assign. |

```csharp
editor.AddToLane(custProcess, salesLane,   createPO);
editor.AddToLane(custProcess, salesLane,   sendPO);
editor.AddToLane(custProcess, financeLane, payInvoice);
```

---

## Example — MIWG B.2.0: Collaboration with Lanes

The MIWG B.2.0 reference model shows a two-pool collaboration where the customer pool has Sales and Finance lanes.

**Reference:** https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/B.2.0.png

```csharp
public static void CreateB20(string outputPath)
{
    Editor editor = new Editor();
    editor.Create("B.2.0 – Collaboration with Lanes", "BPMN.Sharp");

    // Explicit collaboration and processes
    string collab = editor.AddCollaboration("Procurement");
    string custProc = editor.AddProcess("Customer Process");
    string suppProc = editor.AddProcess("Supplier Process");

    string custPool = editor.AddPool(collab, custProc, "Customer");
    string suppPool = editor.AddPool(collab, suppProc, "Supplier");

    // Customer lanes
    string salesLane = editor.AddLane(custPool, "Sales");
    string finLane   = editor.AddLane(custPool, "Finance");

    // Supplier lane
    string fulfLane  = editor.AddLane(suppPool, "Fulfilment");

    // Customer elements
    string cStart   = editor.AddEvent(custProc, null, "Need Goods",
        EventType.Start, EventTrigger.None, EventRole.None);
    string createPO = editor.AddActivity(custProc, "Create PO",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);
    string sendPO   = editor.AddActivity(custProc, "Send PO",
        ActivityType.Task, ActivityMarker.None, TaskType.Send, null);
    string recvGds  = editor.AddActivity(custProc, "Receive Goods",
        ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);
    string recvInv  = editor.AddActivity(custProc, "Receive Invoice",
        ActivityType.Task, ActivityMarker.None, TaskType.Receive, null);
    string pay      = editor.AddActivity(custProc, "Pay Invoice",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);
    string cEnd     = editor.AddEvent(custProc, null, "Purchase Complete",
        EventType.End, EventTrigger.None, EventRole.None);

    // Assign to lanes
    editor.AddToLane(custProc, salesLane, cStart);
    editor.AddToLane(custProc, salesLane, createPO);
    editor.AddToLane(custProc, salesLane, sendPO);
    editor.AddToLane(custProc, salesLane, recvGds);
    editor.AddToLane(custProc, salesLane, cEnd);
    editor.AddToLane(custProc, finLane,   recvInv);
    editor.AddToLane(custProc, finLane,   pay);

    // Supplier elements
    string sStart   = editor.AddEvent(suppProc, null, "PO Received",
        EventType.Start, EventTrigger.Message, EventRole.None);
    string process  = editor.AddActivity(suppProc, "Process Order",
        ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
    string ship     = editor.AddActivity(suppProc, "Ship Goods",
        ActivityType.Task, ActivityMarker.None, TaskType.Manual, null);
    string invoice  = editor.AddActivity(suppProc, "Send Invoice",
        ActivityType.Task, ActivityMarker.None, TaskType.Send, null);
    string sEnd     = editor.AddEvent(suppProc, null, "Order Fulfilled",
        EventType.End, EventTrigger.None, EventRole.None);

    editor.AddToLane(suppProc, fulfLane, sStart);
    editor.AddToLane(suppProc, fulfLane, process);
    editor.AddToLane(suppProc, fulfLane, ship);
    editor.AddToLane(suppProc, fulfLane, invoice);
    editor.AddToLane(suppProc, fulfLane, sEnd);

    // Sequence flows
    editor.AddFlow(custProc, null, cStart,  createPO, null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(custProc, null, createPO,sendPO,   null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(custProc, null, sendPO,  recvGds,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(custProc, null, recvGds, recvInv,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(custProc, null, recvInv, pay,      null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(custProc, null, pay,     cEnd,     null, FlowType.Sequence, null, false, FlowDirection.None);

    editor.AddFlow(suppProc, null, sStart,  process,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(suppProc, null, process, ship,     null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(suppProc, null, ship,    invoice,  null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(suppProc, null, invoice, sEnd,     null, FlowType.Sequence, null, false, FlowDirection.None);

    // Message flows
    editor.AddFlow(null, null, sendPO,  sStart,  "PO",      FlowType.Message, null, false, FlowDirection.None);
    editor.AddFlow(null, null, ship,    recvGds, "Goods",   FlowType.Message, null, false, FlowDirection.None);
    editor.AddFlow(null, null, invoice, recvInv, "Invoice", FlowType.Message, null, false, FlowDirection.None);

    // Diagram
    string diag = editor.AddDiagram("Diagram 1", 96f);

    void S(string id, float x, float y, float w, float h) =>
        editor.AddShape(diag, new Shape { ElementRef = id,
            Bounds = new List<RectangleF> { new RectangleF(x, y, w, h) } });

    // Customer pool: 900w × 200h (2 lanes × 100h)
    S(custPool,  10,  10, 900, 200);
    S(salesLane, 10,  10, 900, 100);
    S(finLane,   10, 110, 900, 100);

    // Supplier pool: 900w × 100h
    S(suppPool,  10, 230, 900, 100);
    S(fulfLane,  10, 230, 900, 100);

    // Customer Sales lane (y centre ≈ 60)
    S(cStart,   50,  42, 36,  36);
    S(createPO,126,  20,120,  80);
    S(sendPO,  286,  20,120,  80);
    S(recvGds, 606,  20,120,  80);
    S(cEnd,    826,  42, 36,  36);

    // Customer Finance lane (y centre ≈ 160)
    S(recvInv, 446, 120,120, 80);
    S(pay,     606, 120,120, 80);

    // Supplier lane (y centre ≈ 280)
    S(sStart,  286, 262, 36,  36);
    S(process, 362, 240,120,  80);
    S(ship,    522, 240,120,  80);
    S(invoice, 682, 240,120,  80);
    S(sEnd,    842, 262, 36,  36);

    editor.Save(Path.Combine(outputPath, "B.2.0.bpmn"));
}
```

---

## Black-Box Pool · §9.3.1

A black-box pool represents an external participant whose internal process is not modelled. Add the pool with no internal elements — connect to it only via message flows.

```csharp
string externalPool = editor.AddPool(null, null, "Payment Provider");

// Only message flows connect to it — no internal sequence flows
editor.AddFlow(null, null, payTask, externalPool,
    "Payment Request", FlowType.Message, null, false, FlowDirection.None);
```

---

## See Also

- [05 – Flows](05-flows.md) — message flows between pools
- [07 – Data and Artifacts](07-data-artifacts.md) — annotations within pools
- [08 – Diagram Layout](08-diagram-layout.md) — sizing pools and lanes
