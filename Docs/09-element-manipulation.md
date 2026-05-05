# 09 – Element Manipulation

> **BPMN Spec Reference:** §8.3.1 Base Element · §8.3 Foundation

---

## Overview

The `Element` class is the base type on which the entire BPMN model is constructed. The model is a **recursive tree of Elements** — every event, task, gateway, flow, pool, lane, annotation, data object, and diagram node is an Element. The `Editor` class provides direct manipulation of elements without requiring manual tree traversal.

Four methods cover inspection and modification:

```csharp
int    CountElements(string elementId, string field)
bool   UpdateElement(string elementId, string field, string value)
bool   UpdateElement(string elementId, string field, int index, string value)
bool   RemoveElement(string elementId)
bool   RemoveElement(string parentId, string field, int index)
```

---

## `CountElements(string elementId, string field)`

Returns the number of child items in the named field of an element. The `field` name mirrors the BPMN XSD element or attribute name.

| Parameter | Description |
|-----------|-------------|
| `elementId` | ID of the parent element to inspect. |
| `field` | Name of the field/child collection to count (e.g. `"flowElement"`, `"sequenceFlow"`, `"incoming"`, `"outgoing"`). |

**Returns:** `int` — count of items in the field.

```csharp
// Count all flow elements (events, tasks, gateways, flows) in a process
int total = editor.CountElements(processId, "flowElement");

// Count only sequence flows
int flows = editor.CountElements(processId, "sequenceFlow");

// Count incoming flows of a specific task
int incoming = editor.CountElements(taskId, "incoming");
```

---

## `UpdateElement(string elementId, string field, string value)`

Sets a named field (attribute or child text content) on an element to a new string value.

| Parameter | Description |
|-----------|-------------|
| `elementId` | ID of the element to update. |
| `field` | Field or attribute name (e.g. `"name"`, `"documentation"`, `"conditionExpression"`). |
| `value` | New string value. |

**Returns:** `bool` — `true` on success.

```csharp
// Rename a task
editor.UpdateElement(taskId, "name", "Validate Customer Data");

// Update a gateway's name
editor.UpdateElement(gatewayId, "name", "Credit Score Sufficient?");

// Set a condition expression on a sequence flow
editor.UpdateElement(flowId, "conditionExpression", "${score >= 700}");
```

---

## `UpdateElement(string elementId, string field, int index, string value)`

Sets the value of the item at the specified zero-based index within a list-valued field.

| Parameter | Description |
|-----------|-------------|
| `elementId` | ID of the element to update. |
| `field` | Name of the list field. |
| `index` | Zero-based index of the item to update. |
| `value` | New string value. |

**Returns:** `bool` — `true` on success.

```csharp
// Update the first documentation entry on an event
editor.UpdateElement(startId, "documentation", 0,
    "Process begins when a new customer request arrives.");

// Update the second resource role reference
editor.UpdateElement(taskId, "resourceRole", 1, "SeniorApprover");
```

---

## `RemoveElement(string elementId)`

Removes the element with the given ID from the semantic model entirely. If the element has a corresponding shape or edge in the diagram, remove those separately with `RemoveShape` / `RemoveEdge`.

```csharp
// Remove a task from the model
editor.RemoveElement(taskId);

// Remove the task's flows too
editor.RemoveElement(incomingFlowId);
editor.RemoveElement(outgoingFlowId);

// Remove its diagram shape
editor.RemoveShape(taskShapeId);
```

---

## `RemoveElement(string parentId, string field, int index)`

Removes the child element at the given zero-based index from a named list field of the parent element. Useful for cleaning up list-valued fields such as `incoming`/`outgoing` flow references, documentation entries, or resource roles.

| Parameter | Description |
|-----------|-------------|
| `parentId` | ID of the parent element. |
| `field` | Name of the list field. |
| `index` | Zero-based index of the child to remove. |

```csharp
// Remove the first incoming flow reference from a task
editor.RemoveElement(taskId, "incoming", 0);

// Remove the second documentation entry from a gateway
editor.RemoveElement(gatewayId, "documentation", 1);
```

---

## Example — Load, Inspect, Modify and Save

```csharp
public static void ModifyProcess(string sourcePath, string outputPath)
{
    // ── Load ─────────────────────────────────────────────────────────────
    Editor editor = new Editor();
    editor.Load(sourcePath);

    // ── Inspect ──────────────────────────────────────────────────────────
    // Build a small model to demonstrate — in production you would
    // use IDs obtained from the loaded model.
    string processId = editor.AddProcess("Inspection Demo");

    string start = editor.AddEvent(processId, null, "Start",
        EventType.Start, EventTrigger.None, EventRole.None);
    string task1 = editor.AddActivity(processId, "Step A",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);
    string task2 = editor.AddActivity(processId, "Step B",
        ActivityType.Task, ActivityMarker.None, TaskType.Service, null);
    string task3 = editor.AddActivity(processId, "Step C — placeholder",
        ActivityType.Task, ActivityMarker.None, TaskType.Script, null);
    string end   = editor.AddEvent(processId, null, "End",
        EventType.End, EventTrigger.None, EventRole.None);

    string f1 = editor.AddFlow(processId, null, start, task1,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    string f2 = editor.AddFlow(processId, null, task1, task2,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    string f3 = editor.AddFlow(processId, null, task2, task3,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    string f4 = editor.AddFlow(processId, null, task3, end,
        null, FlowType.Sequence, null, false, FlowDirection.None);

    string diag = editor.AddDiagram("Diagram 1", 96f);
    string startShp = editor.AddShape(diag, new Shape { ElementRef = start,
        Bounds = new List<RectangleF> { new RectangleF(20, 42, 36, 36) } });
    string t1Shp = editor.AddShape(diag, new Shape { ElementRef = task1,
        Bounds = new List<RectangleF> { new RectangleF(96, 20, 120, 80) } });
    string t2Shp = editor.AddShape(diag, new Shape { ElementRef = task2,
        Bounds = new List<RectangleF> { new RectangleF(256, 20, 120, 80) } });
    string t3Shp = editor.AddShape(diag, new Shape { ElementRef = task3,
        Bounds = new List<RectangleF> { new RectangleF(416, 20, 120, 80) } });
    string endShp = editor.AddShape(diag, new Shape { ElementRef = end,
        Bounds = new List<RectangleF> { new RectangleF(576, 42, 36, 36) } });

    // CountElements — how many flow elements in the process?
    int elemCount = editor.CountElements(processId, "flowElement");
    // elemCount reflects all events + tasks + flows

    // CountElements — how many incoming flows does task2 have?
    int incomingCount = editor.CountElements(task2, "incoming");

    // ── UpdateElement — rename tasks ─────────────────────────────────────
    editor.UpdateElement(task1, "name", "Validate Input");
    editor.UpdateElement(task2, "name", "Call Validation API");

    // UpdateElement (indexed) — add documentation to the start event
    editor.UpdateElement(start, "documentation", 0,
        "Triggered when a new request arrives in the queue.");

    // UpdateElement — name the flow between task1 and task2
    editor.UpdateElement(f2, "name", "Valid");

    // ── RemoveElement — remove task3 and its flows ────────────────────────
    editor.RemoveElement(f3);       // flow task2 → task3
    editor.RemoveElement(f4);       // flow task3 → end
    editor.RemoveElement(task3);    // the task itself
    editor.RemoveShape(t3Shp);      // its diagram shape

    // Wire task2 directly to end
    string f5 = editor.AddFlow(processId, null, task2, end,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddEdge(diag, new Edge { ElementRef = f5,
        Points = new List<PointF> { new PointF(376, 60), new PointF(576, 60) } });

    // Move end shape left to close the gap
    editor.UpdateShapePosition(endShp, new RectangleF(416, 42, 36, 36));

    // ── Save ─────────────────────────────────────────────────────────────
    editor.Save(Path.Combine(outputPath, "modified.bpmn"));
}
```

---

## See Also

- [01 – Getting Started](01-getting-started.md) — `Load`, `Save`, `Normalize`
- [08 – Diagram Layout](08-diagram-layout.md) — `RemoveShape`, `RemoveEdge`, `UpdateShapePosition`
- [10 – Complete Examples](10-examples.md)
