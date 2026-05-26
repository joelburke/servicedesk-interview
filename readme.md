# Interview Prep for Engineering Manager
The purpose of this project is to use claude code to setup a pair programming interview practice session in .net
## Installables/Requirements
1. Install Git and GitGUI for windows, create account, login
1. Install Claude code, get pro subscription
1. Install VS Code

## Claude Prompt

### Prompt
Create a pair programming interview that I can practice for this job in .net and lets run through it so I can practice.  I want to be the driver and you be the navigator. Here is the job description and the transcript from the first interview for context. I'd like a new git repository that I can commit to, a codebase with some interview ask that we can pair program through together, and I might need to install .net because I don't have it installed locally.  I do have vs code, git for windows installed already and I'd like to use those.  

### Context
1. Recording transcript of first interview
1. Job description

## Running the App

### Backend (.NET API)
```
cd ServiceDesk.Api
dotnet run
# Runs on http://localhost:5008
```

### Frontend (React + Vite)
```
cd servicedesk-ui
npm run dev
# Runs on http://localhost:5173
```
Start the backend first so the proxy can reach it.

---

## Interview Problem 1 — Auto-Assign Tickets (Backend)

Your task as driver: Implement `FindAvailableAssigneeAsync` in `TicketService.cs` so that it returns the team member who currently has the fewest open tickets, as long as they haven't hit their `MaxCapacity`. Then wire it into the `Create` action in the controller.

This is exactly the kind of problem Mitch described — not super complex, but it reveals how you think: do you ask clarifying questions, do you reach out for input, do you consider edge cases?

So — before you write any code, what questions do you have for me about the requirements?

---

## Interview Problem 2 — Ticket Status Filter (Full-Stack)

### The Setup
The React frontend is running. Users can see all tickets on the dashboard and create new ones. They've now asked for a way to **filter tickets by status** — they want to click a button (All / Open / InProgress / Resolved / Closed) and see only tickets with that status.

### What to Implement
You'll trace this feature end-to-end, from click to database query and back:

**Frontend** — `servicedesk-ui/src/components/TicketList.tsx`
- The filter buttons already render and `activeFilter` state already exists
- But clicking a filter button does NOT yet trigger a new API call
- Wire `activeFilter` into the fetch so the list re-loads whenever the filter changes

**Frontend** — `servicedesk-ui/src/api/client.ts`
- `getTickets()` currently calls `GET /api/tickets` with no parameters
- Update it to accept an optional `status` argument and append `?status=X` to the URL when provided

**Backend** — `ServiceDesk.Api/Controllers/TicketsController.cs`
- `GetAll()` currently ignores query parameters entirely
- Add an optional `[FromQuery] TicketStatus? status` parameter and filter the EF Core query when it's present

### The Full-Stack Trace (talk through this before coding)
```
User clicks "Open"
  → setActiveFilter("Open")            [React state]
  → useEffect re-runs (activeFilter dep)
  → getTickets("Open")                 [api/client.ts]
  → fetch GET /api/tickets?status=Open [HTTP]
  → Vite proxy → http://localhost:5008
  → TicketsController.GetAll(status: Open)  [.NET]
  → _context.Tickets.Where(t => t.Status == status)  [EF Core]
  → returns filtered JSON
  → setTickets(data)                   [React state]
  → list re-renders with only Open tickets
```

### Questions to Work Through Before Writing Code
1. What React hook controls "run this code when state changes"? What goes in its dependency array?
2. How do you build a URL with an optional query parameter in TypeScript — what does the URL look like when `status` is `null` vs `"Open"`?
3. In ASP.NET Core, what attribute binds a query string value to a controller parameter?
4. What does the EF Core `.Where()` call look like when filtering by a nullable enum parameter?
5. What should the API return when `?status=InvalidValue` is passed — and how does `[FromQuery]` handle enum parsing for you?
6. How would you write a test for the new filtered behavior?

### Starting Points
Look for `// TODO (Interview Problem 2)` comments in:
- `servicedesk-ui/src/components/TicketList.tsx` — two TODOs
- `servicedesk-ui/src/api/client.ts` — one TODO
- `ServiceDesk.Api/Controllers/TicketsController.cs` — needs new query param + filter