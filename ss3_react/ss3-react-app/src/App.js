import React, { useState } from 'react';
import BitcoinEvents from './components/BitcoinEvents'; 

import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';

function App() {
    const [latestBitcoinEvents, setLatestTransactionEvents] = useState([]);
    const [latestBlockEvents, setLatestBlockEvents] = useState([]);

    const handleNewEvent = (eventType, eventData) => {
        if (eventType === 'Transaction') {
            console.log(`***************** LD TRANSACTION ${eventType}`);
            setLatestTransactionEvents(prevEvents => [
                { eventType: 'BitcoinEvent', hash: eventData },
                ...prevEvents.slice(0, 9)
            ]);
        } else if (eventType === 'ReceiveBlockEvent') {
            console.log(`***************** LD BLOCK ${eventType}`);
            setLatestBlockEvents(prevEvents => [
                eventData,
                ...prevEvents.slice(0, 9)
            ]);
        }
    };

    return (
        <div className="App">
            <BitcoinEvents onNewEvent={handleNewEvent} />
            <h1>Bitcoin Transactions</h1>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} size="small" aria-label="transaction events table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Event Type</TableCell>
                            <TableCell>Hash</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {latestBitcoinEvents.map((event, index) => (
                            <TableRow key={index}>
                                <TableCell>{event.eventType}</TableCell>
                                <TableCell>{event.hash}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <h1>Block Events</h1>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} size="small" aria-label="block events table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Timestamp</TableCell>
                            <TableCell>Transactions</TableCell>
                            <TableCell>Nonce</TableCell>
                            <TableCell>Difficulty</TableCell>
                            <TableCell>Hash Verification</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {latestBlockEvents.map((event, index) => (
                            <TableRow key={index}>
                                <TableCell>{event.Timestamp}</TableCell>
                                <TableCell>
                                    <ul>
                                        {event.Transactions.map((tx, i) => (
                                            <li key={i}>
                                                <strong>Transaction ID:</strong> {tx.TransactionId}, 
                                                <strong>Total Value:</strong> {tx.TotalValue}
                                            </li>
                                        ))}
                                    </ul>
                                </TableCell>
                                <TableCell>{event.Nonce}</TableCell>
                                <TableCell>{event.Difficulty}</TableCell>
                                <TableCell>{event.HashVerification}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </div>
    );
}

export default App;
