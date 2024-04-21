import React, { useState, useEffect } from 'react';
import BitcoinEvents from './components/BitcoinEvents';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';

function App() {
    const [latestBitcoinEvents, setLatestBitcoinEvents] = useState([]);
    const [blockQueue, setBlockQueue] = useState([]);
    const [blockPage, setBlockPage] = useState({});

    useEffect(() => {
        //LD keep the most recent three
        setBlockQueue(prevQueue => prevQueue.slice(0, 3));
    }, [blockQueue]);

    const handleNewEvent = (eventType, eventData) => {
        if (eventType === 'Transaction') {
            setLatestBitcoinEvents(prevEvents => [
                { eventType: 'BitcoinEvent', hash: eventData },
                ...prevEvents.slice(0, 9)
            ]);
        } else {
            const parsedEventData = JSON.parse(eventData);
            setBlockQueue(prevQueue => [
                { content: parsedEventData },
                ...prevQueue
            ]);
            setBlockPage(prevPageState => ({
                ...prevPageState,
                [parsedEventData.BlockId]: 0
            }));
        }
    };

    const handleChangeBlockPage = (event, newPage, blockId) => {
        setBlockPage(prevPageState => ({
            ...prevPageState,
            [blockId]: newPage
        }));
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
                        {blockQueue.map((block, index) => (
                            <TableRow key={index}>
                                <TableCell>{block.content.Timestamp}</TableCell>
                                <TableCell>
                                    <div style={{ maxHeight: '200px', overflowY: 'scroll' }}>
                                        <TableContainer component={Paper}>
                                            <Table size="small" aria-label="transactions table">
                                                <TableBody>
                                                    {block.content.Transactions.map((tx, txIndex) => (
                                                        <TableRow key={txIndex}>
                                                            <TableCell>{tx.TransactionId}</TableCell>
                                                            <TableCell>{tx.TotalValue}</TableCell>
                                                        </TableRow>
                                                    ))}
                                                </TableBody>
                                            </Table>
                                        </TableContainer>
                                    </div>
                                </TableCell>
                                <TableCell>{block.content.Nonce}</TableCell>
                                <TableCell>{block.content.Difficulty}</TableCell>
                                <TableCell>{block.content.HashVerification}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </div>
    );
}

export default App;