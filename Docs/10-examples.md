# 10 – Complete Examples

Each example in this article is a complete, self-contained BPMN model aligned with a reference model from the [BPMN MIWG test suite](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/tree/master/Reference) — the authoritative set of BPMN 2.0 conformance models maintained by the OMG working group.

---

## A.1.0 — Simple Sequential Process

The simplest possible BPMN model: a plain start event, two tasks, and a plain end event.

**MIWG reference:** [A.1.0.png](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.1.0.png)

```csharp
public static void A10_SimpleSequential(string outputDir)
{
    Editor editor = new Editor();
    editor.Create("A.1.0", "BPMN.Sharp");

    string start = editor.AddEvent(null, null, "Start Event",
        EventType.Start, EventTrigger.None, EventRole.None);
    string task1 = editor.AddActivity(null, "Task 1",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string task2 = editor.AddActivity(null, "Task 2",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string end = editor.AddEvent(null, null, "End Event",
        EventType.End, EventTrigger.None, EventRole.None);

    string f1 = editor.AddFlow(null, null, start, task1, null, FlowType.Sequence, null, false, FlowDirection.None);
    string f2 = editor.AddFlow(null, null, task1, task2, null, FlowType.Sequence, null, false, FlowDirection.None);
    string f3 = editor.AddFlow(null, null, task2, end,   null, FlowType.Sequence, null, false, FlowDirection.None);

    string diag = editor.AddDiagram("Diagram 1", 96f);

    editor.AddShape(diag, new Shape { ElementRef = start,
        Bounds = new List<RectangleF> { new RectangleF(30f, 62f, 36f, 36f) } });
    editor.AddShape(diag, new Shape { ElementRef = task1,
        Bounds = new List<RectangleF> { new RectangleF(116f, 40f, 120f, 80f) } });
    editor.AddShape(diag, new Shape { ElementRef = task2,
        Bounds = new List<RectangleF> { new RectangleF(286f, 40f, 120f, 80f) } });
    editor.AddShape(diag, new Shape { ElementRef = end,
        Bounds = new List<RectangleF> { new RectangleF(456f, 62f, 36f, 36f) } });

    editor.AddEdge(diag, new Edge { ElementRef = f1,
        Points = new List<PointF> { new PointF(66f, 80f), new PointF(116f, 80f) } });
    editor.AddEdge(diag, new Edge { ElementRef = f2,
        Points = new List<PointF> { new PointF(236f, 80f), new PointF(286f, 80f) } });
    editor.AddEdge(diag, new Edge { ElementRef = f3,
        Points = new List<PointF> { new PointF(406f, 80f), new PointF(456f, 80f) } });

    editor.Save(Path.Combine(outputDir, "A.1.0.bpmn"));
}
```

---

## A.2.0 — Exclusive Gateway (Split and Join)

A decision gateway routing to two different task paths that merge at a joining gateway.

**MIWG reference:** [A.2.0.png](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.2.0.png)

```csharp
public static void A20_ExclusiveGateway(string outputDir)
{
    Editor editor = new Editor();
    editor.Create("A.2.0", "BPMN.Sharp");

    string start  = editor.AddEvent(null, null, "Start Event",
        EventType.Start, EventTrigger.None, EventRole.None);
    string task1  = editor.AddActivity(null, "Task 1",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string split  = editor.AddGateway(null, "Gateway\n(Split)",
        GatewayType.Exclusive, 1, 2);
    string task2  = editor.AddActivity(null, "Task 2",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string task3  = editor.AddActivity(null, "Task 3",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string join   = editor.AddGateway(null, "Gateway\n(Join)",
        GatewayType.Exclusive, 2, 1);
    string task4  = editor.AddActivity(null, "Task 4",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string end    = editor.AddEvent(null, null, "End Event",
        EventType.End, EventTrigger.None, EventRole.None);

    editor.AddFlow(null, null, start, task1,  null,          FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task1, split,  null,          FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, split, task2,  "Condition 1", FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, split, task3,  "Condition 2", FlowType.Sequence, null, true,  FlowDirection.None);
    editor.AddFlow(null, null, task2, join,   null,          FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task3, join,   null,          FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, join,  task4,  null,          FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task4, end,    null,          FlowType.Sequence, null, false, FlowDirection.None);

    string diag = editor.AddDiagram("Diagram 1", 96f);

    void S(string id, float x, float y, float w, float h) =>
        editor.AddShape(diag, new Shape { ElementRef = id,
            Bounds = new List<RectangleF> { new RectangleF(x, y, w, h) } });

    S(start, 30f,  118f,  36f,  36f);
    S(task1, 116f,  96f, 120f,  80f);
    S(split, 276f, 114f,  44f,  44f);
    S(task2, 360f,  60f, 120f,  80f);
    S(task3, 360f, 156f, 120f,  80f);
    S(join,  520f, 114f,  44f,  44f);
    S(task4, 604f,  96f, 120f,  80f);
    S(end,   764f, 118f,  36f,  36f);

    editor.Save(Path.Combine(outputDir, "A.2.0.bpmn"));
}
```

---

## A.3.0 — Lane and Intermediate Events

A single-pool process with a lane and intermediate events.

**MIWG reference:** [A.3.0.png](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.3.0.png)

```csharp
public static void A30_LaneAndEvents(string outputDir)
{
    Editor editor = new Editor();
    editor.Create("A.3.0", "BPMN.Sharp");

    string pool = editor.AddPool(null, null, "Process Engine");
    string lane = editor.AddLane(pool, "Clerk");

    string start    = editor.AddEvent(null, null, "Start Event",
        EventType.Start, EventTrigger.None, EventRole.None);
    string task1    = editor.AddActivity(null, "Task 1",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string intEvt   = editor.AddEvent(null, null, "Intermediate Event",
        EventType.Intermediate, EventTrigger.None, EventRole.Throwing);
    string task2    = editor.AddActivity(null, "Task 2",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);
    string end      = editor.AddEvent(null, null, "End Event",
        EventType.End, EventTrigger.None, EventRole.None);

    editor.AddToLane(null, lane, start);
    editor.AddToLane(null, lane, task1);
    editor.AddToLane(null, lane, intEvt);
    editor.AddToLane(null, lane, task2);
    editor.AddToLane(null, lane, end);

    string f1 = editor.AddFlow(null, null, start,  task1,  null, FlowType.Sequence, null, false, FlowDirection.None);
    string f2 = editor.AddFlow(null, null, task1,  intEvt, null, FlowType.Sequence, null, false, FlowDirection.None);
    string f3 = editor.AddFlow(null, null, intEvt, task2,  null, FlowType.Sequence, null, false, FlowDirection.None);
    string f4 = editor.AddFlow(null, null, task2,  end,    null, FlowType.Sequence, null, false, FlowDirection.None);

    string diag = editor.AddDiagram("Diagram 1", 96f);

    editor.AddShape(diag, new Shape { ElementRef = pool,
        Bounds = new List<RectangleF> { new RectangleF(10f, 10f, 620f, 130f) } });
    editor.AddShape(diag, new Shape { ElementRef = lane,
        Bounds = new List<RectangleF> { new RectangleF(10f, 10f, 620f, 130f) } });

    void S(string id, float x, float y, float w, float h) =>
        editor.AddShape(diag, new Shape { ElementRef = id,
            Bounds = new List<RectangleF> { new RectangleF(x, y, w, h) } });

    S(start,  60f,  57f,  36f, 36f);
    S(task1, 146f,  35f, 120f, 80f);
    S(intEvt,316f,  57f,  36f, 36f);
    S(task2, 402f,  35f, 120f, 80f);
    S(end,   572f,  57f,  36f, 36f);

    editor.Save(Path.Combine(outputDir, "A.3.0.bpmn"));
}
```

---

## A.4.0 — Parallel and Exclusive Gateways Combined

**MIWG reference:** [A.4.0.png](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.4.0.png)

See [04 – Gateways](04-gateways.md) for the full code listing of this example.

---

## B.1.0 — Two-Pool Collaboration with Message Flows

**MIWG reference:** [B.1.0.png](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/B.1.0.png)

See [05 – Flows](05-flows.md) for the full code listing of this example.

---

## B.2.0 — Collaboration with Lanes

**MIWG reference:** [B.2.0.png](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/B.2.0.png)

See [06 – Pools and Lanes](06-pools-lanes.md) for the full code listing of this example.

---

## C.1.0 — Sub-Process with Loop and Compensation

**MIWG reference:** [C.1.0.png](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/C.1.0.png)

See [03 – Activities](03-activities.md) for the full code listing of this example.

---

## C.3.0 — Boundary Events and Error Handling

**MIWG reference:** [C.3.0.png](https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/C.3.0.png)

```csharp
public static void C30_BoundaryEventsAndErrorHandling(string outputDir)
{
    Editor editor = new Editor();
    editor.Create("C.3.0", "BPMN.Sharp");

    string start = editor.AddEvent(null, null, "Start",
        EventType.Start, EventTrigger.None, EventRole.None);

    string task1 = editor.AddActivity(null, "Task 1",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);

    // Interrupting error boundary on task1
    string errBound = editor.AddEvent(null, task1, "Error",
        EventType.Intermediate, EventTrigger.Error, EventRole.Catching);

    string task2 = editor.AddActivity(null, "Task 2",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);

    // Non-interrupting timer boundary on task1
    string timerBound = editor.AddEvent(null, task1, "Timer",
        EventType.Intermediate, EventTrigger.Timer,
        EventRole.NonInterrupting);

    string task3 = editor.AddActivity(null, "Task 3",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);

    string xorGw = editor.AddGateway(null, "Condition?",
        GatewayType.Exclusive, 1, 2);

    string task4 = editor.AddActivity(null, "Task 4",
        ActivityType.Task, ActivityMarker.None, TaskType.User, null);

    string xorJoin = editor.AddGateway(null, null,
        GatewayType.Exclusive, 2, 1);

    string end = editor.AddEvent(null, null, "End",
        EventType.End, EventTrigger.None, EventRole.None);

    string endError = editor.AddEvent(null, null, "End",
        EventType.End, EventTrigger.Error, EventRole.None);

    // Main flow
    editor.AddFlow(null, null, start,     task1,   null,  FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task1,     xorGw,   null,  FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, xorGw,     task3,   "Yes", FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, xorGw,     task4,   "No",  FlowType.Sequence, null, true,  FlowDirection.None);
    editor.AddFlow(null, null, task3,     xorJoin, null,  FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task4,     xorJoin, null,  FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, xorJoin,   end,     null,  FlowType.Sequence, null, false, FlowDirection.None);

    // Error boundary path
    editor.AddFlow(null, null, errBound,  task2,   null,  FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task2,     endError,null,  FlowType.Sequence, null, false, FlowDirection.None);

    // Timer boundary path (non-interrupting — no end event, merges back)
    editor.AddFlow(null, null, timerBound,task3,   null,  FlowType.Sequence, null, false, FlowDirection.None);

    string diag = editor.AddDiagram("Diagram 1", 96f);

    void S(string id, float x, float y, float w, float h) =>
        editor.AddShape(diag, new Shape { ElementRef = id,
            Bounds = new List<RectangleF> { new RectangleF(x, y, w, h) } });

    S(start,     20f, 118f,  36f,  36f);
    S(task1,     96f,  96f, 120f,  80f);
    S(xorGw,    256f, 114f,  44f,  44f);
    S(task3,    340f,  60f, 120f,  80f);
    S(task4,    340f, 156f, 120f,  80f);
    S(xorJoin,  500f, 114f,  44f,  44f);
    S(end,      584f, 118f,  36f,  36f);

    // Boundary events on bottom of task1
    S(errBound,  118f, 158f,  36f,  36f);
    S(timerBound,162f, 158f,  36f,  36f);

    S(task2,     96f, 240f, 120f,  80f);
    S(endError,  256f, 262f,  36f,  36f);

    editor.Save(Path.Combine(outputDir, "C.3.0.bpmn"));
}
```

---

## Running All Examples

```csharp
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string output = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Output");
        Directory.CreateDirectory(output);

        A10_SimpleSequential(output);
        A20_ExclusiveGateway(output);
        A30_LaneAndEvents(output);
        // A.4.0 — see 04-gateways.md
        // B.1.0 — see 05-flows.md
        // B.2.0 — see 06-pools-lanes.md
        // C.1.0 — see 03-activities.md
        C30_BoundaryEventsAndErrorHandling(output);

        Console.WriteLine("Done. Files written to: " + output);
    }
}
```

---

## See Also

- [01 – Getting Started](01-getting-started.md)
- [02 – Events](02-events.md)
- [03 – Activities](03-activities.md)
- [04 – Gateways](04-gateways.md)
- [05 – Flows](05-flows.md)
- [06 – Pools and Lanes](06-pools-lanes.md)
- [07 – Data and Artifacts](07-data-artifacts.md)
- [08 – Diagram Layout](08-diagram-layout.md)
- [09 – Element Manipulation](09-element-manipulation.md)
