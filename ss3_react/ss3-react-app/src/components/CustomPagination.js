import React, { useState } from 'react';
import { Container, Row, Col, Form, Pagination } from 'react-bootstrap';
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

export default CustomPagination;

