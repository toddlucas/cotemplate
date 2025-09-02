import React from 'react';

interface PaginatorProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  className?: string;
  showPageInfo?: boolean;
  showPageSize?: boolean;
  pageSize?: number;
  onPageSizeChange?: (pageSize: number) => void;
  pageSizeOptions?: number[];
  adjacentPages?: number; // Number of adjacent pages to show on each side
}

/**
 * Shows a paginator with adjacent pages.
 * [«] [‹] [1] [...] [8] [9] [10] [11] [12] [...] [99] [100] [›] [»]
 */
const Paginator: React.FC<PaginatorProps> = ({
  currentPage,
  totalPages,
  onPageChange,
  className = '',
  showPageInfo = true,
  showPageSize = false,
  pageSize,
  onPageSizeChange,
  pageSizeOptions = [5, 10, 20, 50],
  adjacentPages = 2
}) => {
  const handlePageSizeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    if (onPageSizeChange) {
      onPageSizeChange(Number(event.target.value));
    }
  };

  // Generate page numbers to display
  const getPageNumbers = () => {
    const pages: (number | string)[] = [];
    
    if (totalPages <= 1) {
      return pages;
    }

    const startPage = Math.max(1, currentPage - adjacentPages);
    const endPage = Math.min(totalPages, currentPage + adjacentPages);

    // Always show first page
    if (startPage > 1) {
      pages.push(1);
      if (startPage > 2) {
        pages.push('...');
      }
    }

    // Show pages around current page
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    // Always show last page
    if (endPage < totalPages) {
      if (endPage < totalPages - 1) {
        pages.push('...');
      }
      pages.push(totalPages);
    }

    return pages;
  };

  const pageNumbers = getPageNumbers();

  if (totalPages <= 1 && !showPageSize) {
    return null;
  }

  return (
    <div className={`paginator ${className}`}>
      <nav aria-label="Page navigation">
        <ul className="pagination pagination-sm mb-0">
          {/* First Page */}
          <li className={`page-item ${currentPage === 0 ? 'disabled' : ''}`}>
            <button
              onClick={() => onPageChange(0)}
              disabled={currentPage === 0}
              className="page-link"
              aria-label="First page"
              title="First page"
              tabIndex={currentPage === 0 ? -1 : undefined}
              aria-disabled={currentPage === 0}
            >
              <span aria-hidden="true">&laquo;</span>
            </button>
          </li>

          {/* Previous Page */}
          <li className={`page-item ${currentPage === 0 ? 'disabled' : ''}`}>
            <button
              onClick={() => onPageChange(currentPage - 1)}
              disabled={currentPage === 0}
              className="page-link"
              aria-label="Previous page"
              title="Previous page"
              tabIndex={currentPage === 0 ? -1 : undefined}
              aria-disabled={currentPage === 0}
            >
              <span aria-hidden="true">&lsaquo;</span>
            </button>
          </li>

          {/* Page Numbers */}
          {pageNumbers.map((page, index) => (
            <React.Fragment key={index}>
              {typeof page === 'number' ? (
                <li className={`page-item ${page - 1 === currentPage ? 'active' : ''}`} aria-current={page - 1 === currentPage ? 'page' : undefined}>
                  <button
                    onClick={() => onPageChange(page - 1)}
                    className="page-link"
                    aria-label={`Page ${page}`}
                  >
                    {page}
                  </button>
                </li>
              ) : (
                <li className="page-item disabled">
                  <span className="page-link" aria-hidden="true">
                    {page}
                  </span>
                </li>
              )}
            </React.Fragment>
          ))}

          {/* Next Page */}
          <li className={`page-item ${currentPage >= totalPages - 1 ? 'disabled' : ''}`}>
            <button
              onClick={() => onPageChange(currentPage + 1)}
              disabled={currentPage >= totalPages - 1}
              className="page-link"
              aria-label="Next page"
              title="Next page"
              tabIndex={currentPage >= totalPages - 1 ? -1 : undefined}
              aria-disabled={currentPage >= totalPages - 1}
            >
              <span aria-hidden="true">&rsaquo;</span>
            </button>
          </li>

          {/* Last Page */}
          <li className={`page-item ${currentPage >= totalPages - 1 ? 'disabled' : ''}`}>
            <button
              onClick={() => onPageChange(totalPages - 1)}
              disabled={currentPage >= totalPages - 1}
              className="page-link"
              aria-label="Last page"
              title="Last page"
              tabIndex={currentPage >= totalPages - 1 ? -1 : undefined}
              aria-disabled={currentPage >= totalPages - 1}
            >
              <span aria-hidden="true">&raquo;</span>
            </button>
          </li>
        </ul>
      </nav>

      {/* Page Info */}
      {showPageInfo && (
        <div className="page-info small text-muted mt-2">
          Page {currentPage + 1} of {totalPages}
        </div>
      )}

      {/* Page Size Selector */}
      {showPageSize && pageSize && onPageSizeChange && (
        <div className="page-size-controls d-flex align-items-center gap-2 mt-2">
          <label htmlFor="page-size" className="form-label small mb-0">
            Show:
          </label>
          <select
            id="page-size"
            value={pageSize}
            onChange={handlePageSizeChange}
            className="form-select form-select-sm"
            style={{ width: 'auto' }}
          >
            {pageSizeOptions.map(size => (
              <option key={size} value={size}>
                {size}
              </option>
            ))}
          </select>
          <span className="page-size-text small text-muted">per page</span>
        </div>
      )}
    </div>
  );
};

export default Paginator;
