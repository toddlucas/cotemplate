interface PageSizeSelectorProps {
  pageSize: number;
  onPageSizeChange: (pageSize: number) => void;
  options?: number[];
}

const PageSizeSelector = ({ 
  pageSize, 
  onPageSizeChange, 
  options = [5, 10, 25, 50, 100] 
}: PageSizeSelectorProps) => {
  return (
    <div className="page-size-selector d-flex align-items-start">
      <span className="me-2 text-muted">Show:</span>
      <select
        id="pageSize"
        className="form-select form-select-sm me-2"
        style={{ width: 'auto' }}
        value={pageSize}
        onChange={(e) => onPageSizeChange(Number(e.target.value))}
      >
        {options.map(option => (
          <option key={option} value={option}>{option}</option>
        ))}
      </select>
      <span className="text-muted">per page</span>
    </div>
  );
};

export default PageSizeSelector; 