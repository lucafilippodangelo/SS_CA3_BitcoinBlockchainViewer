import React, { useState, useEffect } from 'react';
import BitcoinEvents from './components/BitcoinEvents';
import { Container, Tab, Tabs } from 'react-bootstrap';
import BlockDisplay from './components/BlockDisplay';
import TransactionDisplay from './components/TransactionDisplay';  
import 'bootstrap/dist/css/bootstrap.min.css';

const itemsPerPage = 10;

function App() {
  const [latestBitcoinEvents, setLatestBitcoinEvents] = useState([]);
  const [blockQueue, setBlockQueue] = useState([]);
  const [activeTab, setActiveTab] = useState('blocks');
  const [totalPages, setTotalPages] = useState({});
  const [paginationStatus, setPaginationStatus] = useState({});
  const [expandedTransactions, setExpandedTransactions] = useState([]);

  useEffect(() => {
    if (blockQueue.length > 3) {
      setBlockQueue(prevQueue => prevQueue.slice(0, 3));
    }
  }, [blockQueue]);

  useEffect(() => {
    const updatedTotalPages = {};
    blockQueue.forEach(block => {
      const totalTransactions = block.content.Transactions.length;
      updatedTotalPages[block.content.Hash] = Math.ceil(totalTransactions / itemsPerPage);
    });
    setTotalPages(updatedTotalPages);
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
            <BlockDisplay
              blockQueue={blockQueue}
              totalPages={totalPages}
              paginationStatus={paginationStatus}
              setPaginationStatus={setPaginationStatus}
              expandedTransactions={expandedTransactions}
              setExpandedTransactions={setExpandedTransactions}
            />
          </Tab>
          <Tab eventKey="transactions" title="Transactions">
            <TransactionDisplay latestBitcoinEvents={latestBitcoinEvents} />
          </Tab>
        </Tabs>
      </Container>
    </div>
  );
}

export default App;


