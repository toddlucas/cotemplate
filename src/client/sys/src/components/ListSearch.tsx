import { useState } from 'react';

interface ListSearchProps {
  onSearch: (searchTerm: string) => void;
  placeholder?: string;
  className?: string;
}

const ListSearch = ({ onSearch, placeholder = "Search...", className = "" }: ListSearchProps) => {
  const [searchTerm, setSearchTerm] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSearch(searchTerm);
  };

  return (
    <div className={`paged-search ${className}`}>
      <form onSubmit={handleSubmit}>
        <div className="input-group mb-3">
          <input
            className="form-control"
            type="text"
            name="search"
            placeholder={placeholder}
            aria-label={placeholder}
            aria-describedby="search-submit"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          <input
            className="btn btn-outline-secondary"
            type="submit"
            id="search-submit"
            value="Search"
          />
        </div>
      </form>
    </div>
  );
};

export default ListSearch;
