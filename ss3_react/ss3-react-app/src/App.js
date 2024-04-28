import React, { useState, useEffect } from 'react';
import BitcoinEvents from './components/BitcoinEvents';
import { Table, Container, Row, Col, Form, Pagination, Tab, Tabs } from 'react-bootstrap';
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
  const itemsPerPage = 10;

  useEffect(() => {
    const updatedTotalPages = {};
    blockQueue.forEach(block => {
      const totalTransactions = block.content.Transactions.length;
      updatedTotalPages[block.content.Hash] = Math.ceil(totalTransactions / itemsPerPage);
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
    <Container fluid>
      {blockQueue.map((block, index) => (
        <div key={index}>
          <Row>
            <Col md={12}>
              {/* LD some space */}
              <div style={{ height: '50px' }}></div>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <h6>Block: {block.content.Hash}</h6>
            </Col>
          </Row>
          <Row>
            <Col md={6}>
              <p>Timestamp: {block.content.Timestamp}</p>
            </Col>
            <Col md={6}>
              <p>Nonce: {block.content.Nonce}</p>
            </Col>
          </Row>
          <Row>
            <Col md={6}>
              <p>Difficulty: {block.content.Difficulty}</p>
            </Col>
            <Col md={6}>
              <p>Hash Verification: {block.content.HashVerification}</p>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <Table striped bordered hover>
                <thead>
                  <tr>
                    <th>Transaction ID</th>
                    <th>Total Value</th>
                  </tr>
                </thead>
                <tbody>
                  {block.content.Transactions.slice((paginationStatus[block.content.Hash] || 1) * itemsPerPage - itemsPerPage, (paginationStatus[block.content.Hash] || 1) * itemsPerPage).map((tx, txIndex) => (
                    <tr key={txIndex}>
                      <td>{tx.TransactionId}</td>
                      <td>{tx.TotalValue}</td>
                    </tr>
                  ))}
                </tbody>
              </Table>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <CustomPagination
                currentPage={paginationStatus[block.content.Hash] || 1}
                totalPages={totalPages[block.content.Hash] || 1}
                onPageChange={(pageNumber) => handlePaginationClick(block.content.Hash, pageNumber)}
              />
            </Col>
          </Row>
        </div>
      ))}
    </Container>
  );
};

function App() {
  const [latestBitcoinEvents, setLatestBitcoinEvents] = useState([]);
  const [blockQueue, setBlockQueue] = useState([]);
  const [activeTab, setActiveTab] = useState('blocks');

  useEffect(() => {
    //LD Keep the most recent three blocks
    setBlockQueue(prevQueue => prevQueue.slice(0, 3));
  }, [blockQueue]);

  const handleNewEvent = (eventType, eventData) => {
    if (eventType === 'Transaction') {
      setLatestBitcoinEvents(prevEvents => [
        { eventType: 'Transaction', hash: eventData },
        ...prevEvents.slice(0, 9)
      ]);
    } else {
      const parsedEventData = JSON.parse(eventData);
      setBlockQueue(prevQueue => [
        { content: parsedEventData },
        ...prevQueue
      ]);
    }
  };

  return (
    <div className="App">
      <BitcoinEvents onNewEvent={handleNewEvent} />

      <Container fluid>
        <Tabs
          id="tabs"
          activeKey={activeTab}
          onSelect={(key) => setActiveTab(key)}
          className="mb-3"
        >
          <Tab eventKey="blocks" title="Blocks">
            <Container>
              <Row>
                <Col md={12}>
                  <h1 style={{ textAlign: 'center' }}>Bitcoin Blocks</h1>
                </Col>
              </Row>
              <Row>
                <Col md={12}>
                  <h6 style={{ textAlign: 'center' }}>Last 3 blocks, most recent at the top</h6>
                </Col>
              </Row>
              <BlockQueueTable blockQueue={blockQueue} />
            </Container>
          </Tab>
          <Tab eventKey="transactions" title="Transactions">
            <Container>
              <Row>
                <Col md={12}>
                  <h1 style={{ textAlign: 'center' }}>Bitcoin Transactions</h1>
                </Col>
              </Row>
              <Row>
                <Col md={12}>
                  <h6 style={{ textAlign: 'center' }}>Last 10 Transactions, most recent at the top</h6>
                </Col>
              </Row>
              <Row>
                <Col md={12}>
                  <Table striped bordered hover style={{ width: '90%', margin: 'auto' }}>
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
                </Col>
              </Row>
            </Container>
          </Tab>
        </Tabs>
      </Container>
    </div>
  );
}

export default App;

