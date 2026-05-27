const BASE = '/api';

export type TicketStatus = 'Open' | 'InProgress' | 'Resolved' | 'Closed';

export interface TeamMemberSummary {
  id: number;
  name: string;
}

export interface TicketSummary {
  id: number;
  title: string;
  status: TicketStatus;
}

export interface Ticket {
  id: number;
  title: string;
  description: string;
  status: TicketStatus;
  assignedToId: number | null;
  assignedTo: TeamMemberSummary | null;
  createdAt: string;
}

export interface TeamMember {
  id: number;
  name: string;
  maxCapacity: number;
  tickets: TicketSummary[];
}

// TODO (Interview Problem 2): add an optional `status` parameter and include it as a query string when provided
export async function getTickets(): Promise<Ticket[]> {
  const res = await fetch(`${BASE}/tickets`);
  return res.json();
}

export async function createTicket(title: string, description: string): Promise<Ticket> {
  const res = await fetch(`${BASE}/tickets`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ title, description }),
  });
  return res.json();
}

export async function updateTicketStatus(id: number, status: TicketStatus): Promise<void> {
  await fetch(`${BASE}/tickets/${id}/status`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ status }),
  });
}

export async function deleteTicket(id: number): Promise<void> {
  await fetch(`${BASE}/tickets/${id}`, { method: 'DELETE' });
}

export async function getTeamMembers(): Promise<TeamMember[]> {
  const res = await fetch(`${BASE}/teamMembers`);
  return res.json();
}
