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
    const [latestEvents, setLatestEvents] = useState([]);

    const handleNewEvent = (eventType, hash) => {
        setLatestEvents(prevEvents => [
            { eventType, hash },
            ...prevEvents.slice(0, 9) 
        ]);
    };

    return (
        <div className="App">
            <BitcoinEvents onNewEvent={handleNewEvent} />
            <h1>Bitcoin Transactions</h1>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Event Type</TableCell>
                            <TableCell>Hash</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {latestEvents.map((event, index) => (
                            <TableRow key={index}>
                                <TableCell>{event.eventType}</TableCell>
                                <TableCell>{event.hash}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </div>
    );
}

export default App;