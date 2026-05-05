# BPMN.Sharp Editor API Documentation

This folder contains the complete developer documentation for the `Editor` class in the [BPMN.Sharp](https://www.nuget.org/packages/BPMN.Sharp/) NuGet package.

`Editor` provides a full programmatic API for creating and modifying BPMN 2.0 models. Every method maps directly to elements and concepts defined in the [BPMN 2.0.2 specification](https://www.omg.org/spec/BPMN/2.0.2/PDF) published by the Object Management Group (OMG).

Examples in this documentation use models from the [BPMN Model Interchange Working Group (BPMN MIWG)](https://github.com/bpmn-miwg/bpmn-miwg-test-suite) reference test suite — the authoritative collection of BPMN 2.0 conformance models maintained by the OMG working group.

---

## Articles

| Article | Topics Covered | BPMN Spec Sections |
|---------|---------------|-------------------|
| [01 – Getting Started](01-getting-started.md) | `Create`, `Load`, `Parse`, `Save`, `Serialize`, `Refresh`, `Normalize` | §8.2, §8.3, §15.3 |
| [02 – Events](02-events.md) | `AddEvent` — all types, triggers and roles | §10.5 |
| [03 – Activities](03-activities.md) | `AddActivity` — tasks, sub-processes, markers | §10.3, §10.7 |
| [04 – Gateways](04-gateways.md) | `AddGateway` — all five gateway types | §10.6 |
| [05 – Flows](05-flows.md) | `AddFlow` — sequence, message, association, conditional, default | §8.4.13, §9.4, §8.4.1 |
| [06 – Pools and Lanes](06-pools-lanes.md) | `AddCollaboration`, `AddProcess`, `AddPool`, `AddLane`, `AddToLane` | §9.1, §9.3, §10.8 |
| [07 – Data and Artifacts](07-data-artifacts.md) | `AddAnnotation`, `AddDataObject`, `AddDataInput`, `AddDataOutput`, `AddDataStore`, `AddGroup`, `AddMessage` | §8.4.1, §10.4, §8.4.11 |
| [08 – Diagram Layout](08-diagram-layout.md) | `AddDiagram`, `AddShape`, `AddEdge`, `AddLabel`, `AddWayPoint` and all update/remove methods | §12 |
| [09 – Element Manipulation](09-element-manipulation.md) | `Element` class, `CountElements`, `UpdateElement`, `RemoveElement` | §8.3 |
| [10 – Complete Examples](10-examples.md) | Full MIWG-aligned examples: A.1.0 through C series | All |

---

## Complete API Reference

```csharp
public class Editor
{
    public Editor()

    // Load / Save
    public bool   Create(string name = null, string user = null)
    public bool   Load(string file)
    public bool   Parse(string xml)
    public bool   Save(string file)
    public string Serialize()
    public void   Refresh()
    public bool   Normalize()

    // Process Structure
    public string AddCollaboration(string name = "")
    public string AddProcess(string name = "")
    public string AddPool(string parentId, string processId, string name)
    public string AddLane(string parentId, string name)
    public bool   AddToLane(string processId, string laneId, string elementId)

    // Elements
    public string AddActivity(string processId, string name,
                              ActivityType activityType, ActivityMarker marker,
                              TaskType taskType, string calledElement)
    public string AddEvent(string processId, string attachedToRef, string name,
                           EventType eventType, EventTrigger trigger, EventRole eventRole)
    public string AddFlow(string parentId, string messageId,
                          string idFrom, string idTo, string name,
                          FlowType flowType, string condition,
                          bool isDefault, FlowDirection direction)
    public string AddGateway(string processId, string name,
                             GatewayType gatewayType,
                             int inputs = 0, int outputs = 0)
    public string AddAnnotation(string parentId, string text, bool rightAlign)
    public string AddDataObject(string processId, string text)
    public string AddDataInput(string processId, string text)
    public string AddDataOutput(string processId, string text)
    public string AddDataStore(string processId, string text)
    public string AddGroup(string parentId, string name)
    public string AddMessage(string itemId, string text)

    // Element Manipulation
    public int    CountElements(string elementId, string field)
    public bool   RemoveElement(string elementId)
    public bool   RemoveElement(string parentId, string field, int index)
    public bool   UpdateElement(string elementId, string field, string value)
    public bool   UpdateElement(string elementId, string field, int index, string value)

    // Diagram
    public string AddDiagram(string name, float resolution)
    public string AddShape(string diagramId, Shape shape)
    public string AddEdge(string diagramId, Edge edge)
    public string AddLabel(string parentId, RectangleF rectangle, string style = null)
    public bool   AddWayPoint(string edgeId, PointF point)
    public bool   UpdateShapePosition(string shapeId, RectangleF rectangle)
    public bool   UpdateLabelPosition(string parentId, RectangleF rectangle)
    public bool   UpdatePointPosition(string edgeId, int index, PointF point)
    public bool   RemoveShape(string shapeId)
    public bool   RemoveEdge(string edgeId)
    public bool   RemoveLabel(string parentId)
    public bool   RemoveWayPoint(string edgeId, int index)
}
```

---

## Quick Start

```csharp
using BPMN;
using System.Collections.Generic;
using System.Drawing;

Editor editor = new Editor();
editor.Create("My Process", "Author");

// Semantic layer — returns IDs used for cross-references
string start = editor.AddEvent(null, null, "Start",
    EventType.Start, EventTrigger.None, EventRole.None);
string task = editor.AddActivity(null, "Do Work",
    ActivityType.Task, ActivityMarker.None, TaskType.User, null);
string end = editor.AddEvent(null, null, "End",
    EventType.End, EventTrigger.None, EventRole.None);

editor.AddFlow(null, null, start, task, null,
    FlowType.Sequence, null, false, FlowDirection.None);
editor.AddFlow(null, null, task, end, null,
    FlowType.Sequence, null, false, FlowDirection.None);

// Diagram layer — positions and sizes
string diag = editor.AddDiagram("Diagram 1", 96f);

editor.AddShape(diag, new Shape { ElementRef = start,
    Bounds = new List<RectangleF> { new RectangleF(20, 42, 36, 36) } });
editor.AddShape(diag, new Shape { ElementRef = task,
    Bounds = new List<RectangleF> { new RectangleF(96, 20, 120, 80) } });
editor.AddShape(diag, new Shape { ElementRef = end,
    Bounds = new List<RectangleF> { new RectangleF(266, 42, 36, 36) } });

editor.Save("output.bpmn");
```

---

## Enum Reference

### EventType · §10.5.1
| Value | BPMN Element |
|-------|-------------|
| `Start` | Start Event |
| `End` | End Event |
| `Intermediate` | Intermediate Catch or Throw Event |

### EventTrigger · §10.5.5
| Value | BPMN Event Definition | Start | End | Intermediate |
|-------|-----------------------|:-----:|:---:|:------------:|
| `None` | Plain — no marker | ✓ | ✓ | ✓ |
| `Message` | MessageEventDefinition | ✓ | ✓ | ✓ |
| `Timer` | TimerEventDefinition | ✓ | – | ✓ |
| `Error` | ErrorEventDefinition | – | ✓ | ✓ |
| `Escalation` | EscalationEventDefinition | – | ✓ | ✓ |
| `Cancel` | CancelEventDefinition | – | ✓ | ✓ |
| `Compensation` | CompensateEventDefinition | – | ✓ | ✓ |
| `Conditional` | ConditionalEventDefinition | ✓ | – | ✓ |
| `Link` | LinkEventDefinition | – | – | ✓ |
| `Signal` | SignalEventDefinition | ✓ | ✓ | ✓ |
| `Terminate` | TerminateEventDefinition | – | ✓ | – |
| `Multiple` | Multiple triggers | ✓ | ✓ | ✓ |
| `ParallelMultiple` | Parallel Multiple | ✓ | – | ✓ |

### EventRole · §10.5.4
| Value | Description |
|-------|-------------|
| `None` | Catching (start/end/intermediate catch) |
| `Throwing` | Intermediate throwing event |
| `Catching` | Catching event — intermediate catch or interrupting boundary |
| `NonInterrupting` | Non-interrupting boundary event |

### ActivityType · §10.3
| Value | BPMN Element |
|-------|-------------|
| `Task` | Atomic Task |
| `Process` | Process |
| `Activity` | Sub-Process, Call Activity, or Ad-Hoc Sub-Process (`ActivityMarker.AdHoc`) |
| `Transaction` | Transaction Sub-Process |
| `Choreography` | Choreography |
| `EventSubProcess` | Event Sub-Process |

### ActivityMarker · §10.3.8
| Value | Description |
|-------|-------------|
| `None` | No marker |
| `Loop` | Standard loop |
| `Sequential` | Sequential multi-instance |
| `Parallel` | Parallel multi-instance |
| `Compensation` | Compensation handler marker (§10.7) |

### TaskType · §10.3.3
| Value | BPMN Task Type |
|-------|---------------|
| `None` | Abstract Task |
| `User` | User Task |
| `Manual` | Manual Task |
| `Service` | Service Task |
| `Send` | Send Task |
| `Receive` | Receive Task |
| `BusinessRule` | Business Rule Task |
| `Script` | Script Task |

### GatewayType · §10.6
| Value | BPMN Gateway |
|-------|-------------|
| `Exclusive` | Exclusive (XOR) Gateway |
| `Inclusive` | Inclusive (OR) Gateway |
| `Parallel` | Parallel (AND) Gateway |
| `Complex` | Complex Gateway |
| `EventBased` | Event-Based Gateway |

### FlowType
| Value | BPMN Element | Spec |
|-------|-------------|------|
| `Sequence` | Sequence Flow | §8.4.13 |
| `Message` | Message Flow | §9.4 |
| `Association` | Association | §8.4.1 |

### FlowDirection · §8.4.1
| Value | Description |
|-------|-------------|
| `None` | Undirected association |
| `Directed` | Directed — one arrowhead |
| `BiDirected` | Bidirectional — two arrowheads |

---

## Specification Reference

> **Business Process Model and Notation (BPMN), Version 2.0.2**
> OMG Document Number: formal/2013-12-09
> https://www.omg.org/spec/BPMN/2.0.2/PDF

## BPMN MIWG Test Suite

> Official BPMN 2.0 conformance models used as examples throughout this documentation.
> https://github.com/bpmn-miwg/bpmn-miwg-test-suite
