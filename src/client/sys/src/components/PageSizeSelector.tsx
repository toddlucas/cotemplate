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
    <div className="flex items-center space-x-2 text-sm">
      <span className="text-gray-600 font-medium">Show:</span>
      <select
        id="pageSize"
        className="px-3 py-1.5 text-sm text-gray-900 bg-white border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 focus:outline-none transition-colors duration-200"
        style={{ width: 'auto' }}
        value={pageSize}
        onChange={(e) => onPageSizeChange(Number(e.target.value))}
      >
        {options.map(option => (
          <option key={option} value={option}>{option}</option>
        ))}
      </select>
      <span className="text-gray-500">per page</span>
    </div>
  );
};

export default PageSizeSelector;
