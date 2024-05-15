import React from 'react';
import { Container, Row, Col, Table } from 'react-bootstrap';
import CustomPagination from './CustomPagination';
import 'bootstrap/dist/css/bootstrap.min.css';

const itemsPerPage = 10;

const BlockQueueTable = ({ blockQueue, totalPages, paginationStatus, setPaginationStatus, expandedTransactions, setExpandedTransactions }) => {
  const handlePaginationClick = (hash, pageNumber) => {
    setPaginationStatus(prevStatus => ({
      ...prevStatus,
      [hash]: pageNumber
    }));
  };

  const toggleTransaction = (blockIndex, txIndex, currentPage) => {
    const expandedIndex = expandedTransactions.findIndex(item => item.blockIndex === blockIndex && item.txIndex === txIndex && item.page === currentPage);
    if (expandedIndex !== -1) {
      setExpandedTransactions(prevExpanded => prevExpanded.filter((_, index) => index !== expandedIndex));
    } else {
      setExpandedTransactions(prevExpanded => [...prevExpanded, { blockIndex, txIndex, page: currentPage }]);
    }
  };

  return (
    <Container fluid>
      {blockQueue.map((block, blockIndex) => (
        <div key={blockIndex}>
          <Row>
            <Col md={12}>
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
              <p>Nonce: {block.content.Nonce}(int)</p>
            </Col>
          </Row>
          <Row>
            <Col md={6}>
              <p>Difficulty: {block.content.Difficulty}(float, base 1)</p>
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
                  <React.Fragment key={txIndex}>
                    <tr onClick={() => toggleTransaction(blockIndex, txIndex, paginationStatus[block.content.Hash] || 1)} style={{ cursor: 'pointer' }}>
                      <td>{tx.TransactionId}</td>
                      <td>{tx.TotalValue}</td>
                    </tr>
                    {expandedTransactions.some(item => item.blockIndex === blockIndex && item.txIndex === txIndex && item.page === (paginationStatus[block.content.Hash] || 1)) && (
                      <tr>
                        <td colSpan="3">
                          <pre>{JSON.stringify(tx.TransactionRaw, null, 2)}</pre>
                        </td>
                      </tr>
                    )}
                  </React.Fragment>
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

const BlockDisplay = ({ blockQueue, totalPages, paginationStatus, setPaginationStatus, expandedTransactions, setExpandedTransactions }) => {
  return (
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
      <BlockQueueTable
        blockQueue={blockQueue}
        totalPages={totalPages}
        paginationStatus={paginationStatus}
        setPaginationStatus={setPaginationStatus}
        expandedTransactions={expandedTransactions}
        setExpandedTransactions={setExpandedTransactions}
      />
    </Container>
  );
};

export default BlockDisplay;
