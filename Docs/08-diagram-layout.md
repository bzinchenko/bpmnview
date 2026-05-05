# 08 – Diagram Layout (BPMN DI)

> **BPMN Spec Reference:** §12 BPMN Notation and Diagrams · §12.2 DI Meta-model · §12.3.2 BPMNShape · §12.3.3 BPMNEdge

---

## Overview

Every `.bpmn` file contains two separate layers. BPMN.Sharp manages both:

- **Semantic layer** (§8–§11) — process logic created by `Add*` element methods.
- **Diagram Interchange (DI) layer** (§12) — visual presentation: positions, sizes, waypoints and labels.

All diagram coordinates use `RectangleF(x, y, width, height)` and `PointF(x, y)` with the origin at the top-left corner of the diagram canvas.

---

## `AddDiagram(string name, float resolution)`

**§12.2 BPMNDiagram**

Creates a diagram. A model may contain multiple diagrams — for example one overview and one expanded sub-process view.

| Parameter | Description |
|-----------|-------------|
| `name` | Display name shown in modelling tool diagram tabs. |
| `resolution` | Diagram resolution in DPI. Standard value is `96f`. |

**Returns:** `string` — diagram ID passed to every subsequent `AddShape`/`AddEdge` call.

```csharp
string diag = editor.AddDiagram("Main Diagram", 96f);
```

---

## `AddShape(string diagramId, Shape shape)`

**§12.3.2 BPMNShape**

Adds a visual node to the diagram. Every semantic element that should appear on screen needs a corresponding Shape.

**Shape properties:**

| Property | Type | Description |
|----------|------|-------------|
| `ElementRef` | `string` | ID of the semantic element this shape represents. |
| `Bounds` | `List<RectangleF>` | `RectangleF(x, y, width, height)` — position and size. |

**Returns:** `string` — the shape ID, needed for `UpdateShapePosition`, `AddLabel`, `RemoveShape`.

```csharp
string taskShp = editor.AddShape(diag, new Shape {
    ElementRef = taskId,
    Bounds = new List<RectangleF> { new RectangleF(96f, 20f, 120f, 80f) }
});
```

---

## `AddEdge(string diagramId, Edge edge)`

**§12.3.3 BPMNEdge**

Adds a visual connector to the diagram. Every flow (sequence, message, association) that should appear on screen needs a corresponding Edge.

**Edge properties:**

| Property | Type | Description |
|----------|------|-------------|
| `ElementRef` | `string` | ID of the semantic flow element. |
| `Points` | `List<PointF>` | Waypoints — minimum two (source, target). Additional points create bends. |

**Returns:** `string` — the edge ID, needed for `AddWayPoint`, `UpdatePointPosition`, `AddLabel`, `RemoveEdge`.

```csharp
string flowEdge = editor.AddEdge(diag, new Edge {
    ElementRef = flowId,
    Points = new List<PointF> {
        new PointF(216f, 60f),   // source point
        new PointF(266f, 60f)    // target point
    }
});
```

---

## `AddLabel(string parentId, RectangleF rectangle, string style = null)`

Adds a label to a shape or edge at the given position. Labels allow you to control exactly where text appears in the diagram.

| Parameter | Description |
|-----------|-------------|
| `parentId` | Shape ID or edge ID returned by `AddShape`/`AddEdge`. |
| `rectangle` | `RectangleF` position and size of the label. |
| `style` | Optional style string defined in the BPMN file. `null` = default style. |

**Returns:** `string` — label ID, needed for `UpdateLabelPosition`/`RemoveLabel`.

```csharp
string lbl = editor.AddLabel(flowEdge,
    new RectangleF(230f, 40f, 60f, 20f));

// With a named style
string lbl2 = editor.AddLabel(taskShp,
    new RectangleF(96f, 100f, 120f, 20f), "BoldLabel");
```

---

## `AddWayPoint(string edgeId, PointF point)`

Appends a new waypoint to the end of an existing edge's point list.

```csharp
// Add a third point to create an L-shaped connector
editor.AddWayPoint(flowEdge, new PointF(350f, 120f));
```

---

## `UpdateShapePosition(string shapeId, RectangleF rectangle)`

Moves and/or resizes a shape.

```csharp
editor.UpdateShapePosition(taskShp,
    new RectangleF(150f, 30f, 120f, 80f));
```

---

## `UpdateLabelPosition(string parentId, RectangleF rectangle)`

Repositions an existing label.

```csharp
editor.UpdateLabelPosition(lbl,
    new RectangleF(240f, 42f, 60f, 20f));
```

---

## `UpdatePointPosition(string edgeId, int index, PointF point)`

Moves the waypoint at the given index. Index 0 is the source point; index (n−1) is the target point.

```csharp
// Move the second waypoint (index 1)
editor.UpdatePointPosition(flowEdge, 1,
    new PointF(280f, 130f));
```

---

## `RemoveShape(string shapeId)`

Removes a shape from the diagram. Does not remove the underlying semantic element.

```csharp
editor.RemoveShape(taskShp);
```

---

## `RemoveEdge(string edgeId)`

Removes an edge from the diagram. Does not remove the underlying semantic flow.

```csharp
editor.RemoveEdge(flowEdge);
```

---

## `RemoveLabel(string parentId)`

Removes the label from a shape or edge.

```csharp
editor.RemoveLabel(lbl);
```

---

## `RemoveWayPoint(string edgeId, int index)`

Removes the waypoint at the given index from an edge.

```csharp
editor.RemoveWayPoint(flowEdge, 2);  // remove the third point
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

## Layout Tips

- **Every visible element needs a shape.** Semantic elements with no shape are invisible in all BPMN tools.
- **Waypoints should meet shape boundaries.** Connect from the right/bottom/left/top edge of the source shape to the left/top/right/bottom edge of the target shape.
- **Lane shapes must be inside pool shapes.** The pool's bounding `RectangleF` must fully contain all its lane rectangles.
- **Gateway connection point is its centre.** Gateway x + width/2, y + height/2.
- **Boundary event shapes overlap the host activity's border.** Place them so their centre sits on the task edge.

---

## Routing a Bend

```csharp
// Boundary event at bottom of task → error end event below and right
editor.AddEdge(diag, new Edge {
    ElementRef = boundaryFlowId,
    Points = new List<PointF> {
        new PointF(136f, 148f),  // boundary event centre
        new PointF(136f, 200f),  // drop straight down
        new PointF(266f, 200f)   // go right to error end
    }
});
```

---

## Multiple Diagrams

```csharp
// Top-level overview
string overview = editor.AddDiagram("Overview", 96f);
editor.AddShape(overview, /* collapsed sub-process shape */);

// Expanded detail diagram for the sub-process
string detail = editor.AddDiagram("Fulfil Order – Detail", 96f);
editor.AddShape(detail, /* inner start event */);
editor.AddShape(detail, /* inner tasks */);
```

---

## See Also

- [01 – Getting Started](01-getting-started.md) — the two-phase authoring pattern
- [06 – Pools and Lanes](06-pools-lanes.md) — sizing pools and lanes
- [09 – Element Manipulation](09-element-manipulation.md) — modifying existing diagrams
