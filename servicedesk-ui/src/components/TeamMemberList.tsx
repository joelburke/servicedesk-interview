import { useEffect, useState } from 'react';
import { getTeamMembers } from '../api/client';
import type { TeamMember } from '../api/client';

export function TeamMemberList() {
  const [members, setMembers] = useState<TeamMember[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    getTeamMembers().then(data => {
      setMembers(data);
      setLoading(false);
    });
  }, []);

  if (loading) return <p>Loading...</p>;

  return (
    <div>
      <h2>Team Members</h2>
      {members.map(member => {
        const activeTickets = member.tickets.filter(t => t.status !== 'Resolved' && t.status !== 'Closed');
        const pct = Math.min((activeTickets.length / member.maxCapacity) * 100, 100);
        const atCapacity = activeTickets.length >= member.maxCapacity;
        return (
          <div key={member.id} className="ticket-card">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 }}>
              <strong style={{ fontSize: 16 }}>{member.name}</strong>
              <span style={{ fontSize: 13, color: atCapacity ? '#dc2626' : '#16a34a' }}>
                {activeTickets.length} / {member.maxCapacity} active tickets
                {atCapacity && ' — AT CAPACITY'}
              </span>
            </div>
            <div style={{ background: '#e5e7eb', borderRadius: 4, height: 8, marginBottom: 12 }}>
              <div
                style={{
                  width: `${pct}%`,
                  height: '100%',
                  borderRadius: 4,
                  background: atCapacity ? '#dc2626' : pct > 60 ? '#d97706' : '#16a34a',
                  transition: 'width 0.3s',
                }}
              />
            </div>
            {member.tickets.length === 0 ? (
              <p style={{ fontSize: 13, color: '#888' }}>No tickets assigned</p>
            ) : (
              <ul style={{ margin: 0, paddingLeft: 20, fontSize: 13 }}>
                {member.tickets.map(t => (
                  <li key={t.id} style={{ marginBottom: 2 }}>
                    <span style={{ color: '#888' }}>#{t.id}</span> {t.title}
                    <span style={{ marginLeft: 6, fontSize: 11, color: '#666' }}>({t.status})</span>
                  </li>
                ))}
              </ul>
            )}
          </div>
        );
      })}
    </div>
  );
}
