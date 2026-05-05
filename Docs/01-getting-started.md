# 01 – Getting Started

> **BPMN Spec Reference:** §8.2 Infrastructure · §8.3 Foundation · §15.3 XSD Exchange Format

---

## Overview

The `Editor` class is the central entry point for all programmatic creation and modification of BPMN 2.0 models. A BPMN model is managed as a recursive tree of `Element` objects covering two layers:

- **Semantic layer** (§8–§11) — events, activities, gateways, flows, pools, lanes and artifacts.
- **Diagram Interchange layer** (§12) — visual positions, sizes and waypoints.

Both layers are written to a single `.bpmn` XML file conforming to the BPMN 2.0 XSD schema.

---

## Installation

```bash
dotnet add package BPMN.Sharp
```

```powershell
# Visual Studio Package Manager Console
Install-Package BPMN.Sharp
```

Required namespace imports:

```csharp
using BPMN;
using System.Collections.Generic;
using System.Drawing;
```

---

## Load / Save Methods

### `Create(string name = null, string user = null)`

**§8.2.1 Definitions**

Initialises a new empty model. Creates the top-level `<definitions>` root element with an optional human-readable `name` and `user` (stored as the `exporter` attribute). Both parameters are optional.

```csharp
Editor editor = new Editor();
editor.Create("Order Fulfilment", "MyApp 1.0");
```

---

### `Load(string file)`

**§15.3 XSD Exchange Format**

Loads an existing BPMN 2.0 XML file from disk. After loading you can inspect, modify or extend the model using any `Add*`, `Update*` or `Remove*` method.

```csharp
Editor editor = new Editor();
editor.Load("existing-process.bpmn");
```

---

### `Parse(string xml)`

Loads a BPMN 2.0 model from an in-memory XML string rather than a file. Useful for receiving models from a database, REST API, or message queue.

```csharp
string bpmnXml = File.ReadAllText("process.bpmn");
Editor editor = new Editor();
editor.Parse(bpmnXml);
```

---

### `Save(string file)`

**§15.3 XSD Exchange Format**

Serialises the current model to a BPMN 2.0 XML file. Returns `true` on success. The output file can be opened by any BPMN 2.0-compliant tool.

```csharp
bool ok = editor.Save("output.bpmn");
```

---

### `Serialize()`

Serialises the current model to a BPMN 2.0 XML string rather than a file. The string can be stored in a database, sent over HTTP, or passed to `Parse()` in another `Editor` instance.

```csharp
string xml = editor.Serialize();
// store in database, send over HTTP, etc.
```

---

### `Refresh()`

Reloads the current document from disk, discarding any unsaved in-memory changes. Useful after external tools have modified the file.

```csharp
editor.Load("process.bpmn");
// ... some time later, file may have changed ...
editor.Refresh();
```

---

### `Normalize()`

Saves the loaded model in a normalised form, stripping all vendor-specific extensions (e.g. Camunda, Signavio, W4 attributes and elements) from the XML. Returns `true` on success.

This is particularly useful when importing models from third-party tools for archiving or interchange, and when you are a member of the BPMN MIWG and need clean portable files for conformance testing.

```csharp
Editor editor = new Editor();
editor.Load("camunda-process.bpmn");   // file contains vendor extensions
editor.Normalize();                     // strips extensions
editor.Save("clean-process.bpmn");     // portable BPMN 2.0
```

---

## Two-Phase Authoring Pattern

Every BPMN model built with `Editor` follows a consistent two-phase pattern. All `Add*` element methods return a `string` ID — the auto-generated identifier of the newly created element, used for cross-references throughout the model.

### Phase 1 — Semantic Layer

Add process elements. Pass `null` for `processId` to target the default process.

```csharp
string start = editor.AddEvent(null, null, "Start",
    EventType.Start, EventTrigger.None, EventRole.None);

string task = editor.AddActivity(null, "Process Order",
    ActivityType.Task, ActivityMarker.None, TaskType.User, null);

string end = editor.AddEvent(null, null, "End",
    EventType.End, EventTrigger.None, EventRole.None);

string f1 = editor.AddFlow(null, null, start, task,
    null, FlowType.Sequence, null, false, FlowDirection.None);
string f2 = editor.AddFlow(null, null, task, end,
    null, FlowType.Sequence, null, false, FlowDirection.None);
```

### Phase 2 — Diagram Layer

Create a diagram and add shapes (for nodes) and edges (for flows) referencing the IDs from Phase 1. Uses `RectangleF` for bounds and `PointF` for waypoints throughout.

```csharp
string diag = editor.AddDiagram("Diagram 1", 96f);

// AddShape returns the shape ID, needed for later position updates or label operations
string startShp = editor.AddShape(diag, new Shape {
    ElementRef = start,
    Bounds = new List<RectangleF> { new RectangleF(20, 42, 36, 36) }
});

string taskShp = editor.AddShape(diag, new Shape {
    ElementRef = task,
    Bounds = new List<RectangleF> { new RectangleF(96, 20, 120, 80) }
});

string endShp = editor.AddShape(diag, new Shape {
    ElementRef = end,
    Bounds = new List<RectangleF> { new RectangleF(266, 42, 36, 36) }
});

// AddEdge returns the edge ID, needed for later waypoint operations or label operations
string e1 = editor.AddEdge(diag, new Edge {
    ElementRef = f1,
    Points = new List<PointF> { new PointF(56, 60), new PointF(96, 60) }
});
string e2 = editor.AddEdge(diag, new Edge {
    ElementRef = f2,
    Points = new List<PointF> { new PointF(216, 60), new PointF(266, 60) }
});

editor.Save("output.bpmn");
```

---

## Complete Example — MIWG A.1.0

The simplest possible BPMN model: a plain start event, two tasks, and a plain end event. This corresponds to the BPMN MIWG reference model A.1.0.

**Reference:** https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.1.0.png

```csharp
public static void CreateA10(string outputPath)
{
    Editor editor = new Editor();
    editor.Create("A.1.0", "BPMN.Sharp");

    // Semantic layer
    string start = editor.AddEvent(null, null, "Start Event",
        EventType.Start, EventTrigger.None, EventRole.None);
    string task1 = editor.AddActivity(null, "Task 1",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string task2 = editor.AddActivity(null, "Task 2",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string end = editor.AddEvent(null, null, "End Event",
        EventType.End, EventTrigger.None, EventRole.None);

    string f1 = editor.AddFlow(null, null, start, task1,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    string f2 = editor.AddFlow(null, null, task1, task2,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    string f3 = editor.AddFlow(null, null, task2, end,
        null, FlowType.Sequence, null, false, FlowDirection.None);

    // Diagram layer
    string diag = editor.AddDiagram("Diagram 1", 96f);

    editor.AddShape(diag, new Shape { ElementRef = start,
        Bounds = new List<RectangleF> { new RectangleF(30, 62, 36, 36) } });
    editor.AddShape(diag, new Shape { ElementRef = task1,
        Bounds = new List<RectangleF> { new RectangleF(116, 40, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = task2,
        Bounds = new List<RectangleF> { new RectangleF(286, 40, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = end,
        Bounds = new List<RectangleF> { new RectangleF(456, 62, 36, 36) } });

    editor.AddEdge(diag, new Edge { ElementRef = f1,
        Points = new List<PointF> { new PointF(66, 80), new PointF(116, 80) } });
    editor.AddEdge(diag, new Edge { ElementRef = f2,
        Points = new List<PointF> { new PointF(236, 80), new PointF(286, 80) } });
    editor.AddEdge(diag, new Edge { ElementRef = f3,
        Points = new List<PointF> { new PointF(406, 80), new PointF(456, 80) } });

    editor.Save(Path.Combine(outputPath, "A.1.0.bpmn"));
}
```

---

## Standard Element Sizes

| Element | Width | Height |
|---------|-------|--------|
| Start / End Event | 36 | 36 |
| Intermediate Event | 36 | 36 |
| Task | 120 | 80 |
| Gateway | 44 | 44 |
| Pool (horizontal) | variable | 100+ |
| Lane (horizontal) | variable | 100 |
| Text Annotation | 100–200 | 30–60 |
| Data Object / Input / Output | 36 | 50 |
| Data Store | 50 | 60 |

---

## See Also

- [02 – Events](02-events.md)
- [08 – Diagram Layout](08-diagram-layout.md)
- [10 – Complete Examples](10-examples.md)
