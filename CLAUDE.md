# ServiceDesk Interview Project

A .NET 10 + React pair programming interview practice repo. Two interview problems are documented in `readme.md`.

## Running the App

Start backend first, then frontend:

```
# Terminal 1
cd ServiceDesk.Api && dotnet run
# Listens on http://localhost:5008

# Terminal 2
cd servicedesk-ui && npm run dev
# Listens on http://localhost:5173 — Vite proxies /api/* to the backend
```

## Running Tests

```
dotnet test ServiceDesk.Tests
```

## Project Structure

```
ServiceDesk.Api/          .NET 10 Web API
  Controllers/            TicketsController, TeamMembersController
  DTOs/                   ServiceDeskDtos.cs — all API response shapes
  Models/                 Ticket, TeamMember (EF Core entities)
  Services/               ITicketService / TicketService (auto-assign logic)
  Data/                   AppDbContext — in-memory DB, seeded with Alice/Bob/Carol

ServiceDesk.Tests/        xUnit tests
  TicketServiceTests.cs   Assignment logic tests
  TicketSerializationTests.cs  Verifies DTOs serialize without circular refs

servicedesk-ui/           React + Vite + TypeScript frontend
  src/api/client.ts       Typed fetch wrappers for all API endpoints
  src/components/         TicketList, CreateTicketForm, TeamMemberList
```

## Key Architectural Decisions

- **Controllers return DTOs, never EF entities.** Raw entities have circular navigation properties (`Ticket.AssignedTo.Tickets`) that cause JSON serialization cycles. All endpoints project into DTOs via EF `.Select()`, which also improves query performance by only fetching needed columns.
- **In-memory database.** State resets on every `dotnet run`. The DB is seeded with three team members in `AppDbContext.OnModelCreating`.
- **Vite proxy.** The frontend calls `/api/*` which Vite forwards to `http://localhost:5008`. No CORS config is needed in dev beyond what's already in `Program.cs`.

## Interview Problems

**Problem 1 (backend):** Implement `FindFirstAvailableAsigneeNotAtTicketCapacityAsync` in `TicketService.cs` — already solved, use as reference.

**Problem 2 (full-stack):** Wire up ticket status filtering end-to-end. Look for `// TODO (Interview Problem 2)` comments in:
- `servicedesk-ui/src/components/TicketList.tsx`
- `servicedesk-ui/src/api/client.ts`
- `ServiceDesk.Api/Controllers/TicketsController.cs` (no TODO comment — needs a `[FromQuery] TicketStatus? status` param added to `GetAll`)
