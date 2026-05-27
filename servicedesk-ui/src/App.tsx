import { useState } from 'react';
import { TicketList } from './components/TicketList';
import { CreateTicketForm } from './components/CreateTicketForm';
import { TeamMemberList } from './components/TeamMemberList';

type Tab = 'tickets' | 'team';

export default function App() {
  const [tab, setTab] = useState<Tab>('tickets');
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [ticketListKey, setTicketListKey] = useState(0);

  function handleTicketCreated() {
    setShowCreateForm(false);
    setTicketListKey(k => k + 1);
  }

  return (
    <div className="app">
      <header className="app-header">
        <h1>ServiceDesk</h1>
        <nav>
          <button className={`tab-btn ${tab === 'tickets' ? 'active' : ''}`} onClick={() => setTab('tickets')}>
            Tickets
          </button>
          <button className={`tab-btn ${tab === 'team' ? 'active' : ''}`} onClick={() => setTab('team')}>
            Team Members
          </button>
        </nav>
      </header>

      <main className="app-main">
        {tab === 'tickets' && (
          <TicketList key={ticketListKey} onCreateClick={() => setShowCreateForm(true)} />
        )}
        {tab === 'team' && <TeamMemberList />}
      </main>

      {showCreateForm && (
        <CreateTicketForm
          onCreated={handleTicketCreated}
          onCancel={() => setShowCreateForm(false)}
        />
      )}
    </div>
  );
}
