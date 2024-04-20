import React, { useState } from 'react';
import BitcoinEvents from './components/BitcoinEvents'; 
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import TablePagination from '@mui/material/TablePagination';

function App() {
    const [latestBitcoinEvents, setLatestBitcoinEvents] = useState([]);
    const [latestBlockEvents, setLatestBlockEvents] = useState([]);
    const [page, setPage] = useState(0);
    const rowsPerPage = 3; // Maximum number of blocks displayed at a time
    const transactionsPerPage = 3; // Maximum number of transactions per page

    const handleNewEvent = (eventType, eventData) => {
        if (eventType === 'Transaction') {
            setLatestBitcoinEvents(prevEvents => [
                { eventType: 'BitcoinEvent', hash: eventData },
                ...prevEvents.slice(0, 9)
            ]);
        } else {
            const parsedEventData = JSON.parse(eventData); // Parse JSON string to object
            setLatestBlockEvents(prevEvents => [
                { content: parsedEventData },
                ...prevEvents.slice(0, 9)
            ]);
        }
    };

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
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
                            <TableRow key={index} style={{ backgroundColor: index === 0 ? '#d9ead3' : 'inherit' }}>
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
                        {latestBlockEvents.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((block, index) => {
                            //console.log("********** CHECK " + block); // Log the block object to the console
                            return (
                                <TableRow key={index}>
                                    <TableCell>{block.content.Timestamp}</TableCell>
                                    <TableCell>
                                        <TableContainer component={Paper}>
                                            <Table size="small" aria-label="transactions table">
                                                <TableHead>
                                                    <TableRow>
                                                        <TableCell>Transaction ID</TableCell>
                                                        <TableCell>Total Value</TableCell>
                                                    </TableRow>
                                                </TableHead>
                                                <TableBody>
                                                {block.content.Transactions.map((tx, txIndex) => (
                                                    txIndex >= page * transactionsPerPage && txIndex < (page + 1) * transactionsPerPage && (
                                                        <TableRow key={txIndex}>
                                                            <TableCell>{tx.TransactionId}</TableCell>
                                                            <TableCell>{tx.TotalValue}</TableCell>
                                                        </TableRow>
                                                    )
                                                ))}
                                            </TableBody>
                                            </Table>
                                        </TableContainer>
                                    </TableCell>
                                    <TableCell>{block.content.Nonce}</TableCell>
                                    <TableCell>{block.content.Difficulty}</TableCell>
                                    <TableCell>{block.content.HashVerification}</TableCell>
                                </TableRow>
                            );
                        })}
                    </TableBody>

                    {/* <TableBody>
                        {latestBlockEvents.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((block, index) => (
                            
                            <TableRow key={index}>
                                <TableCell>{block.Timestamp}</TableCell>
                                <TableCell>
                                    <TableContainer component={Paper}>
                                        <Table size="small" aria-label="transactions table">
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell>Transaction ID</TableCell>
                                                    <TableCell>Total Value</TableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {block.Transactions.map((tx, txIndex) => (
                                                    txIndex >= page * transactionsPerPage && txIndex < (page + 1) * transactionsPerPage && (
                                                        <TableRow key={txIndex}>
                                                            <TableCell>{tx.TransactionId}</TableCell>
                                                            <TableCell>{tx.TotalValue}</TableCell>
                                                        </TableRow>
                                                    )
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                </TableCell>
                                <TableCell>{block.Nonce}</TableCell>
                                <TableCell>{block.Difficulty}</TableCell>
                                <TableCell>{block.HashVerification}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody> */}

                </Table>
            </TableContainer>

            <TablePagination
                rowsPerPageOptions={[]} // Disable rows per page selector
                component="div"
                count={latestBlockEvents.length}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={handleChangePage}
            />
        </div>
    );
}

export default App;
