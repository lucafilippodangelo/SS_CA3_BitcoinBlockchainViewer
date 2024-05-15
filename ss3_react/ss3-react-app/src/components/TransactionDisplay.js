import React from 'react';
import { Container, Row, Col, Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const TransactionDisplay = ({ latestBitcoinEvents }) => {
  return (
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
  );
};

export default TransactionDisplay;
