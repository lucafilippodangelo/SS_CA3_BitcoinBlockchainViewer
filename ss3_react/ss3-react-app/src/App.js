import React, { useState, useEffect } from 'react';
import BitcoinEvents from './components/BitcoinEvents';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Pagination } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';







function BlockQueueTable({ blockQueue }) {

    const [paginationStatus, setPaginationStatus] = useState({});
    

    const handlePaginationClick = (Hash, pageNumber) => {
      setPaginationStatus(prevStatus => ({
        ...prevStatus,
        [Hash]: pageNumber
      }));
    };
  
    return (
      <div>
        {blockQueue.map((block, index) => (
          <div key={index}>
            <h2>Block {block.content.Hash}</h2>
            <p>Timestamp: {block.content.Timestamp}</p>
            <p>Nonce: {block.content.Nonce}</p>
            <p>Difficulty: {block.content.Difficulty}</p>
            <p>Hash Verification: {block.content.HashVerification}</p>
  
            <TableContainer component={Paper}>
              <Table sx={{ minWidth: 650 }} size="small" aria-label="transactions table">
                <TableBody>
                  {block.content.Transactions.slice((paginationStatus[block.content.Hash] || 1) * 10 - 10, (paginationStatus[block.content.Hash] || 1) * 10).map((tx, txIndex) => (
                    <TableRow key={txIndex}>
                      <TableCell>{tx.TransactionId}</TableCell>
                      <TableCell>{tx.TotalValue}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>

            <Pagination>
              {Array.from({ length: Math.ceil(block.content.Transactions.length / 10) }).map((_, pageIndex) => (
                <Pagination.Item
                  key={pageIndex}
                  active={(pageIndex + 1) === (paginationStatus[block.content.Hash] || 1)}
                  onClick={() => handlePaginationClick(block.content.Hash, pageIndex + 1)}
                >
                  {pageIndex + 1}
                </Pagination.Item>
              ))}
            </Pagination>
          </div>
        ))}
      </div>
    );
  }
  


  
  
  
  



  
  

function App() {
  const [latestBitcoinEvents, setLatestBitcoinEvents] = useState([]);
  const [blockQueue, setBlockQueue] = useState([]);
  const [blockPage, setBlockPage] = useState({});

  useEffect(() => {
    // Keep the most recent three blocks
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
      <BlockQueueTable blockQueue={blockQueue} />
    </div>
  );
}

export default App;
