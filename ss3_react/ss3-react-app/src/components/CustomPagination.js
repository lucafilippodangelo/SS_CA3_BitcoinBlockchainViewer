import React, { useState } from 'react';
import { Container, Row, Col, Form, Pagination } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

/**
 * CustomPagination component is handling the pagination functionality. Main goal is to allow users to navigate between pages
 * of block transactions. Using next and previous buttons, or directly selecting a page from a dropdown.
 * I did not manage for a question of time to make it work the go to the first or the last page. Maybe a future enhancement.
 * NOTE: usage of react state to manage the selected page and update the parent component by a callback when the page changes. 
 * Pretty modular and elegant approach I would say :)
 * 
 * Props:
 * - currentPage: the currently active page.
 * - totalPages: the total number of pages available.
 * - onPageChange: a callback function to handle the page change.
 */
const CustomPagination = ({ currentPage, totalPages, onPageChange }) => {
  //LD this state is for trackage of the selected page
  const [selectedPage, setSelectedPage] = useState(currentPage);

  //LD handling back button
  const handleGoBack = () => {
    if (currentPage > 1) {
      onPageChange(currentPage - 1);
      setSelectedPage(currentPage - 1);
    }
  };

  //LD handling next/up button
  const handleGoUp = () => {
    if (currentPage < totalPages) {
      onPageChange(currentPage + 1);
      setSelectedPage(currentPage + 1);
    }
  };

  //LD handling the page selection from the dropdown
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

