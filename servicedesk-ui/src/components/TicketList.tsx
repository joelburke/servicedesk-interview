import { useEffect, useState } from 'react';
import { getTickets, deleteTicket, updateTicketStatus } from '../api/client';
import type { Ticket, TicketStatus } from '../api/client';

type FilterOption = TicketStatus | 'All';
const FILTERS: FilterOption[] = ['All', 'Open', 'InProgress', 'Resolved', 'Closed'];

const STATUS_COLORS: Record<FilterOption, string> = {
  All: '#555',
  Open: '#2563eb',
  InProgress: '#d97706',
  Resolved: '#16a34a',
  Closed: '#6b7280',
};

interface Props {
  onCreateClick: () => void;
}

export function TicketList({ onCreateClick }: Props) {
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [activeFilter, setActiveFilter] = useState<FilterOption>('All');
  const [loading, setLoading] = useState(false);

  async function fetchTickets() {
    setLoading(true);
    // TODO (Interview Problem 2): pass activeFilter to getTickets so only matching
    // tickets are returned from the API instead of filtering on the client.
    const data = await getTickets();
    setTickets(data);
    setLoading(false);
  }

  // TODO (Interview Problem 2): add activeFilter as a dependency here so the list
  // re-fetches when the filter changes.
  useEffect(() => {
    fetchTickets();
  }, []);

  async function handleDelete(id: number) {
    await deleteTicket(id);
    fetchTickets();
  }

  async function handleStatusChange(id: number, status: TicketStatus) {
    await updateTicketStatus(id, status);
    fetchTickets();
  }

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
        <h2 style={{ margin: 0 }}>Tickets</h2>
        <button className="btn-primary" onClick={onCreateClick}>+ New Ticket</button>
      </div>

      <div style={{ display: 'flex', gap: 8, marginBottom: 20 }}>
        {FILTERS.map(f => (
          <button
            key={f}
            className={`filter-btn ${activeFilter === f ? 'active' : ''}`}
            style={{ borderColor: activeFilter === f ? STATUS_COLORS[f] : '#ccc', color: activeFilter === f ? STATUS_COLORS[f] : '#555' }}
            onClick={() => setActiveFilter(f)}
          >
            {f}
          </button>
        ))}
      </div>

      {loading ? (
        <p>Loading...</p>
      ) : tickets.length === 0 ? (
        <p style={{ color: '#888' }}>No tickets found.</p>
      ) : (
        tickets.map(ticket => (
          <div key={ticket.id} className="ticket-card">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'baseline' }}>
              <strong>#{ticket.id} {ticket.title}</strong>
              <span
                className="status-badge"
                style={{ background: STATUS_COLORS[ticket.status as FilterOption] }}
              >
                {ticket.status}
              </span>
            </div>
            <p style={{ margin: '6px 0', fontSize: 14, color: '#444' }}>{ticket.description}</p>
            <div style={{ fontSize: 12, color: '#888', marginBottom: 10 }}>
              Assigned to: <strong>{ticket.assignedTo?.name ?? 'Unassigned'}</strong>
              &nbsp;&middot;&nbsp;
              Created: {new Date(ticket.createdAt).toLocaleDateString()}
            </div>
            <div style={{ display: 'flex', gap: 8, alignItems: 'center' }}>
              <select
                value={ticket.status}
                onChange={e => handleStatusChange(ticket.id, e.target.value as TicketStatus)}
              >
                <option value="Open">Open</option>
                <option value="InProgress">InProgress</option>
                <option value="Resolved">Resolved</option>
                <option value="Closed">Closed</option>
              </select>
              <button className="btn-danger" onClick={() => handleDelete(ticket.id)}>Delete</button>
            </div>
          </div>
        ))
      )}
    </div>
  );
}
