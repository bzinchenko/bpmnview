# 02 – Events

> **BPMN Spec Reference:** §10.5 Events · §10.5.2 Start Event · §10.5.3 End Event · §10.5.4 Intermediate Event · §10.5.5 Event Definitions

---

## Overview

Events represent things that *happen* during a process — what starts it, what ends it, or what occurs in between. The BPMN spec (§10.5.1) classifies events by position (Start, End, Intermediate) and by effect (Catching or Throwing).

All events are created with a single method:

```csharp
string AddEvent(string processId, string attachedToRef, string name,
                EventType eventType, EventTrigger trigger, EventRole eventRole)
```

---

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `processId` | `string` | ID of the parent process or sub-process. `null` = default process. |
| `attachedToRef` | `string` | For boundary events: ID of the host activity. `null` for all non-boundary events. |
| `name` | `string` | Human-readable label shown on the diagram. |
| `eventType` | `EventType` | Position: `Start`, `End`, or `Intermediate`. |
| `trigger` | `EventTrigger` | The event definition — what causes it or is produced by it. |
| `eventRole` | `EventRole` | Catching / throwing behaviour and boundary placement. |

**Returns:** `string` — the auto-generated element ID.

---

## EventType · §10.5.1

| Value | BPMN Element | Spec |
|-------|-------------|------|
| `Start` | Start Event | §10.5.2 |
| `End` | End Event | §10.5.3 |
| `Intermediate` | Intermediate Catch or Throw Event | §10.5.4 |

---

## EventTrigger · §10.5.5

| Value | BPMN Event Definition | Spec | Start | End | Intermediate |
|-------|-----------------------|------|:-----:|:---:|:------------:|
| `None` | Plain — no marker | §10.5.1 | ✓ | ✓ | ✓ |
| `Message` | MessageEventDefinition | §10.5.5.2 | ✓ | ✓ | ✓ |
| `Timer` | TimerEventDefinition | §10.5.5.3 | ✓ | – | ✓ |
| `Error` | ErrorEventDefinition | §10.5.5.4 | – | ✓ | ✓ |
| `Escalation` | EscalationEventDefinition | §10.5.5.5 | – | ✓ | ✓ |
| `Cancel` | CancelEventDefinition | §10.5.5.6 | – | ✓ | ✓ |
| `Compensation` | CompensateEventDefinition | §10.5.5.7 | – | ✓ | ✓ |
| `Conditional` | ConditionalEventDefinition | §10.5.5.8 | ✓ | – | ✓ |
| `Link` | LinkEventDefinition | §10.5.5.9 | – | – | ✓ |
| `Signal` | SignalEventDefinition | §10.5.5.10 | ✓ | ✓ | ✓ |
| `Terminate` | TerminateEventDefinition | §10.5.5.11 | – | ✓ | – |
| `Multiple` | Multiple triggers | §10.5.1 | ✓ | ✓ | ✓ |
| `ParallelMultiple` | Parallel Multiple | §10.5.1 | ✓ | – | ✓ |

---

## EventRole · §10.5.4

| Value | Description |
|-------|-------------|
| `None` | Catching event — plain start/end, or intermediate catch |
| `Throwing` | Intermediate throwing event |
| `Catching` | Catching event — intermediate catch or interrupting boundary (`attachedToRef` distinguishes boundary) |
| `NonInterrupting` | Non-interrupting boundary event (`attachedToRef` required) |

---

## Start Events · §10.5.2

```csharp
// Plain — no trigger; process starts manually
string start = editor.AddEvent(null, null, "Start",
    EventType.Start, EventTrigger.None, EventRole.None);

// Message — process triggered by an incoming message
string startMsg = editor.AddEvent(null, null, "Order Received",
    EventType.Start, EventTrigger.Message, EventRole.None);

// Timer — process starts on a schedule
string startTimer = editor.AddEvent(null, null, "Daily at 9am",
    EventType.Start, EventTrigger.Timer, EventRole.None);

// Signal — process triggered by a broadcast signal
string startSignal = editor.AddEvent(null, null, "Alert Received",
    EventType.Start, EventTrigger.Signal, EventRole.None);

// Conditional — starts when a condition becomes true
string startCond = editor.AddEvent(null, null, "Stock Below Threshold",
    EventType.Start, EventTrigger.Conditional, EventRole.None);
```

---

## End Events · §10.5.3

```csharp
// Plain — process ends normally
string end = editor.AddEvent(null, null, "End",
    EventType.End, EventTrigger.None, EventRole.None);

// Terminate — immediately cancels all active tokens in the process (§10.5.5.11)
string endTerminate = editor.AddEvent(null, null, "Process Terminated",
    EventType.End, EventTrigger.Terminate, EventRole.None);

// Error — throws an error, to be caught by an Error Boundary upstream
string endError = editor.AddEvent(null, null, "Payment Failed",
    EventType.End, EventTrigger.Error, EventRole.None);

// Message — sends a message as the process ends
string endMsg = editor.AddEvent(null, null, "Confirmation Sent",
    EventType.End, EventTrigger.Message, EventRole.None);

// Escalation — escalates to a parent process or boundary
string endEsc = editor.AddEvent(null, null, "Escalate to Manager",
    EventType.End, EventTrigger.Escalation, EventRole.None);

// Cancel — used inside Transaction sub-processes to trigger compensation
string endCancel = editor.AddEvent(null, null, "Transaction Cancelled",
    EventType.End, EventTrigger.Cancel, EventRole.None);

// Compensation — triggers registered compensation handlers
string endComp = editor.AddEvent(null, null, "Trigger Compensation",
    EventType.End, EventTrigger.Compensation, EventRole.None);

// Signal — broadcasts a signal when the process ends
string endSignal = editor.AddEvent(null, null, "Alert Raised",
    EventType.End, EventTrigger.Signal, EventRole.None);
```

---

## Intermediate Catching Events · §10.5.4

Intermediate catching events pause the flow and wait for a trigger to arrive.

```csharp
// Message Catch — pause until a message arrives
string catchMsg = editor.AddEvent(null, null, "Await Approval",
    EventType.Intermediate, EventTrigger.Message, EventRole.None);

// Timer Catch — pause for a duration or until a date
string catchTimer = editor.AddEvent(null, null, "Wait 3 Days",
    EventType.Intermediate, EventTrigger.Timer, EventRole.None);

// Link Catch — off-page connector (incoming side)
string catchLink = editor.AddEvent(null, null, "From Page 1",
    EventType.Intermediate, EventTrigger.Link, EventRole.None);

// Signal Catch — wait for a broadcast signal
string catchSignal = editor.AddEvent(null, null, "Await Signal",
    EventType.Intermediate, EventTrigger.Signal, EventRole.None);

// Conditional Catch — wait until a condition becomes true
string catchCond = editor.AddEvent(null, null, "Price Drops",
    EventType.Intermediate, EventTrigger.Conditional, EventRole.None);
```

---

## Intermediate Throwing Events · §10.5.4

Intermediate throwing events produce a result and continue immediately.

```csharp
// Message Throw — send a message mid-process
string throwMsg = editor.AddEvent(null, null, "Send Notification",
    EventType.Intermediate, EventTrigger.Message, EventRole.Throwing);

// Link Throw — off-page connector (outgoing side)
string throwLink = editor.AddEvent(null, null, "To Page 2",
    EventType.Intermediate, EventTrigger.Link, EventRole.Throwing);

// Signal Throw — broadcast a signal
string throwSignal = editor.AddEvent(null, null, "Broadcast Signal",
    EventType.Intermediate, EventTrigger.Signal, EventRole.Throwing);

// Escalation Throw — escalate mid-process
string throwEsc = editor.AddEvent(null, null, "Escalate",
    EventType.Intermediate, EventTrigger.Escalation, EventRole.Throwing);

// Compensation Throw — trigger compensation handlers
string throwComp = editor.AddEvent(null, null, "Compensate",
    EventType.Intermediate, EventTrigger.Compensation, EventRole.Throwing);
```

---

## Boundary Events · §10.5.4

Boundary events are attached to an activity. Pass the **host activity's ID** in `attachedToRef`. The `processId` should be the process that *contains* the host activity.

```csharp
string task = editor.AddActivity(null, "Process Payment",
    ActivityType.Task, ActivityMarker.None, TaskType.Service, null);

// Interrupting Error Boundary — cancels the task and redirects flow
string errBound = editor.AddEvent(
    null,              // processId — same process as the task
    task,              // attachedToRef — the host activity
    "Payment Failed",
    EventType.Intermediate, EventTrigger.Error, EventRole.Catching);

// Interrupting Timer Boundary — task is cancelled after timeout
string timerBound = editor.AddEvent(null, task, "Timeout 30min",
    EventType.Intermediate, EventTrigger.Timer, EventRole.Catching);

// Non-Interrupting Timer Boundary — spawns a parallel path; task continues
string timerNI = editor.AddEvent(null, task, "Send Reminder",
    EventType.Intermediate, EventTrigger.Timer,
    EventRole.NonInterrupting);

// Non-Interrupting Message Boundary
string msgNI = editor.AddEvent(null, task, "Priority Change",
    EventType.Intermediate, EventTrigger.Message,
    EventRole.NonInterrupting);
```

---

## Example — MIWG A.4.1: Boundary Events on a Task

The MIWG A.4.1 reference model shows both interrupting and non-interrupting boundary events attached to a single task.

**Reference:** https://github.com/bpmn-miwg/bpmn-miwg-test-suite/blob/master/Reference/A.4.1.png

```csharp
public static void CreateA41(string outputPath)
{
    Editor editor = new Editor();
    editor.Create("A.4.1 – Boundary Events", "BPMN.Sharp");

    string start = editor.AddEvent(null, null, "Start",
        EventType.Start, EventTrigger.None, EventRole.None);

    string task = editor.AddActivity(null, "Task 1",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);

    string end = editor.AddEvent(null, null, "End",
        EventType.End, EventTrigger.None, EventRole.None);

    // Interrupting error boundary on the task
    string errBound = editor.AddEvent(null, task, "Error",
        EventType.Intermediate, EventTrigger.Error, EventRole.Catching);

    string handleErr = editor.AddActivity(null, "Handle Error",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);

    string endErr = editor.AddEvent(null, null, "End",
        EventType.End, EventTrigger.Error, EventRole.None);

    // Non-interrupting timer boundary on the same task
    string timerNI = editor.AddEvent(null, task, "Timer",
        EventType.Intermediate, EventTrigger.Timer,
        EventRole.NonInterrupting);

    string escalate = editor.AddActivity(null, "Escalate",
        ActivityType.Task, ActivityMarker.None, TaskType.None, null);

    // Sequence flows
    editor.AddFlow(null, null, start, task,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, task, end,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, errBound, handleErr,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, handleErr, endErr,
        null, FlowType.Sequence, null, false, FlowDirection.None);
    editor.AddFlow(null, null, timerNI, escalate,
        null, FlowType.Sequence, null, false, FlowDirection.None);

    // Diagram
    string diag = editor.AddDiagram("Diagram 1", 96f);

    editor.AddShape(diag, new Shape { ElementRef = start,
        Bounds = new List<RectangleF> { new RectangleF(20, 72, 36, 36) } });
    editor.AddShape(diag, new Shape { ElementRef = task,
        Bounds = new List<RectangleF> { new RectangleF(96, 50, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = end,
        Bounds = new List<RectangleF> { new RectangleF(266, 72, 36, 36) } });

    // Boundary events on bottom edge of task
    editor.AddShape(diag, new Shape { ElementRef = errBound,
        Bounds = new List<RectangleF> { new RectangleF(118, 112, 36, 36) } });
    editor.AddShape(diag, new Shape { ElementRef = timerNI,
        Bounds = new List<RectangleF> { new RectangleF(162, 112, 36, 36) } });

    // Error path (goes down and right)
    editor.AddShape(diag, new Shape { ElementRef = handleErr,
        Bounds = new List<RectangleF> { new RectangleF(96, 200, 120, 80) } });
    editor.AddShape(diag, new Shape { ElementRef = endErr,
        Bounds = new List<RectangleF> { new RectangleF(266, 222, 36, 36) } });

    // Escalation path
    editor.AddShape(diag, new Shape { ElementRef = escalate,
        Bounds = new List<RectangleF> { new RectangleF(196, 200, 120, 80) } });

    editor.Save(Path.Combine(outputPath, "A.4.1.bpmn"));
}
```

---

## See Also

- [03 – Activities](03-activities.md) — boundary events attach to activities
- [04 – Gateways](04-gateways.md) — Event-Based Gateway routes to catching events
- [05 – Flows](05-flows.md) — connecting events with sequence and message flows
