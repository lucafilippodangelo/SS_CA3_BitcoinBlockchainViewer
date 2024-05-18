import React, { useState, useEffect } from 'react';
import BitcoinEvents from './components/BitcoinEvents';
import { Container, Tab, Tabs } from 'react-bootstrap';
import BlockDisplay from './components/BlockDisplay';
import TransactionDisplay from './components/TransactionDisplay';
import 'bootstrap/dist/css/bootstrap.min.css';

//LD This is a constant, will display 10 transactions per page
const itemsPerPage = 10;

function App() {
  //LD Using state to store the latest Bitcoin events
  const [latestBitcoinEvents, setLatestBitcoinEvents] = useState([]);
  //LD Using state to store the queue of blocks(at the momnent 3, but can be tweacked as preferred)
  const [blockQueue, setBlockQueue] = useState([]);
  //LD Using state to track the active tab. Reason is I splitted info in 2 tabs. Most important the "block one"
  const [activeTab, setActiveTab] = useState('blocks');
  //LD Using state to track total pages for each block's transactions
  const [totalPages, setTotalPages] = useState({});
  //LD separately tracking the pagination status per block
  const [paginationStatus, setPaginationStatus] = useState({});
  //LD I wanted to track and keep expanded transactions in memory when switching pages
  const [expandedTransactions, setExpandedTransactions] = useState([]);

  // Effect to maintain the block queue size(three to be sure UX interaction reactivity)
  useEffect(() => {
    if (blockQueue.length > 3) {
      setBlockQueue(prevQueue => prevQueue.slice(0, 3));
    }
  }, [blockQueue]);

  //LD Effect to update total pages for each block based on transactions
  useEffect(() => {
    const updatedTotalPages = {};
    blockQueue.forEach(block => {
      const totalTransactions = block.content.Transactions.length;
      updatedTotalPages[block.content.Hash] = Math.ceil(totalTransactions / itemsPerPage);
    });
    setTotalPages(updatedTotalPages);
  }, [blockQueue]);

  //LD Handling for TRANSACTIONS. So a function to handle new events from BitcoinEvents.js component
  const handleNewEvent = (eventType, eventData) => {
    if (eventType === 'Transaction') {
      //LD update latest Bitcoin events for transactions
      setLatestBitcoinEvents(prevEvents => [
        { eventType: 'Transaction', hash: eventData },
        ...prevEvents.slice(0, 9)
      ]);
    } else {
      //LD Handling for BLOCK EVENTS
      const parsedEventData = JSON.parse(eventData);
      setBlockQueue(prevQueue => [
        { content: parsedEventData },
        ...prevQueue
      ]);
    }
  };

  return (
    <div className="App">
      {/* just referencing the bitcoin events, I will handle new events coming from that component */}
      <BitcoinEvents onNewEvent={handleNewEvent} />

      <Container fluid>
        <Tabs
          id="tabs"
          activeKey={activeTab}
          onSelect={(key) => setActiveTab(key)}
          className="mb-3"
        >
          {/* LD this is the tab for the blocks, most of the magic happens in "BlockDisplay.js". Using props  */}
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
          {/* LD this is the tab for the transactions. Much simpler implementation than blocks */}
          <Tab eventKey="transactions" title="Transactions">
            <TransactionDisplay latestBitcoinEvents={latestBitcoinEvents} />
          </Tab>
        </Tabs>
      </Container>
    </div>
  );
}

export default App;



