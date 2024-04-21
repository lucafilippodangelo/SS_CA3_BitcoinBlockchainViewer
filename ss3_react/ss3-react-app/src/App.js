
import React, { useState, useEffect } from 'react';
import BitcoinEvents from './components/BitcoinEvents';
import { Table, Container, Row, Col, Form, Pagination } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const CustomPagination = ({ currentPage, totalPages, onPageChange }) => {
  const [selectedPage, setSelectedPage] = useState(currentPage);

  const handleGoBack = () => {
    if (currentPage > 1) {
      onPageChange(currentPage - 1);
      setSelectedPage(currentPage - 1);
    }
  };

  const handleGoUp = () => {
    if (currentPage < totalPages) {
      onPageChange(currentPage + 1);
      setSelectedPage(currentPage + 1);
    }
  };

  const handlePageSelect = (event) => {
    const selectedPage = parseInt(event.target.value);
    if (!isNaN(selectedPage) && selectedPage >= 1 && selectedPage <= totalPages) {
      onPageChange(selectedPage);
      setSelectedPage(selectedPage);
    }
  };

  return (
    <Container>
      <Row className="justify-content-center align-items-center">
        <Col>
          <Pagination>
            <Pagination.Item onClick={handleGoBack} disabled={currentPage === 1}>{'<'}</Pagination.Item>
            <Pagination.Item onClick={handleGoUp} disabled={currentPage === totalPages}>{'>'}</Pagination.Item>
          </Pagination>
        </Col>
        <Col>
          <Form.Control
            as="select"
            value={selectedPage}
            onChange={handlePageSelect}
          >
            {[...Array(totalPages)].map((_, index) => (
              <option key={index + 1} value={index + 1}>{index + 1}</option>
            ))}
          </Form.Control>
        </Col>
      </Row>
    </Container>
  );
};










const BlockQueueTable = ({ blockQueue }) => {
    const [paginationStatus, setPaginationStatus] = useState({});
    const [totalPages, setTotalPages] = useState({}); 
  
    useEffect(() => {
      const updatedTotalPages = {};
      blockQueue.forEach(block => {
        const totalTransactions = block.content.Transactions.length;
        updatedTotalPages[block.content.Hash] = Math.ceil(totalTransactions / 10); 
      });
      setTotalPages(updatedTotalPages);
    }, [blockQueue]);
    
    const handlePaginationClick = (hash, pageNumber) => {
      setPaginationStatus(prevStatus => ({
        ...prevStatus,
        [hash]: pageNumber
      }));
    };
  
    return (
      <Container>
        {blockQueue.map((block, index) => (
          <div key={index}>
            <h2>Block {block.content.Hash}</h2>
            <p>Timestamp: {block.content.Timestamp}</p>
            <p>Nonce: {block.content.Nonce}</p>
            <p>Difficulty: {block.content.Difficulty}</p>
            <p>Hash Verification: {block.content.HashVerification}</p>
  
            <Table striped bordered hover>
              <thead>
                <tr>
                  <th>Transaction ID</th>
                  <th>Total Value</th>
                </tr>
              </thead>
              <tbody>
                {block.content.Transactions.slice((paginationStatus[block.content.Hash] || 1) * 10 - 10, (paginationStatus[block.content.Hash] || 1) * 10).map((tx, txIndex) => (
                  <tr key={txIndex}>
                    <td>{tx.TransactionId}</td>
                    <td>{tx.TotalValue}</td>
                  </tr>
                ))}
              </tbody>
            </Table>
  
            <CustomPagination
              currentPage={paginationStatus[block.content.Hash] || 1}
              totalPages={totalPages[block.content.Hash] || 1} 
              onPageChange={(pageNumber) => handlePaginationClick(block.content.Hash, pageNumber)}
            />
          </div>
        ))}
      </Container>
    );
  };

  





  

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

  return (
    <div className="App">
      <BitcoinEvents onNewEvent={handleNewEvent} />

      <h1>Bitcoin Transactions</h1>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Event Type</th>
            <th>Hash</th>
          </tr>
        </thead>
        <tbody>
          {latestBitcoinEvents.map((event, index) => (
            <tr key={index}>
              <td>{event.eventType}</td>
              <td>{event.hash}</td>
            </tr>
          ))}
        </tbody>
      </Table>

      <h1>Block Events</h1>
      <BlockQueueTable blockQueue={blockQueue} />
    </div>
  );
}

export default App;
