import React, { useState } from 'react';
import BitcoinEvents from './components/BitcoinEvents'; 

function App() {
    const [latestEvent, setLatestEvent] = useState(null);

    //LD state is updated at each new(received)event
    const handleNewEvent = (eventType, hash) => {
        setLatestEvent({ eventType, hash });
    };

    return (
        <div className="App">
            <BitcoinEvents onNewEvent={handleNewEvent} /> {}
            <h1>Bitcoin Transactions</h1>
            <ul>
                {latestEvent && ( //LD at the moment rendering only latest event
                    <li>
                        <strong>Event Type:</strong> {latestEvent.eventType}, <strong>Hash:</strong> {latestEvent.hash}
                    </li>
                )}
            </ul>
        </div>
    );
}

export default App;