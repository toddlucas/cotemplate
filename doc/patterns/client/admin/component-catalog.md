# Admin Component Catalog

This document catalogs all the admin interface components that have been updated to use Tailwind 4. Each component includes implementation details, usage examples, and design specifications.

## Table of Contents

- [ListSearch](#listsearch)
- [PageSizeSelector](#pagesizeselector)
- [Paginator](#paginator)
- [TableFooter](#tablefooter)
- [UserList](#userlist)
- [Component Relationships](#component-relationships)

---

## ListSearch

A search input component with integrated search button, designed for filtering lists and tables.

### Implementation
```tsx
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
    <div className={className}>
      <form onSubmit={handleSubmit} className="flex">
        <div className="relative flex-1 min-w-0">
          <input
            className="block w-full px-3 py-2 text-sm text-gray-900 bg-white border border-gray-300 rounded-l-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 focus:outline-none transition-colors duration-200"
            type="text"
            name="search"
            placeholder={placeholder}
            aria-label={placeholder}
            aria-describedby="search-submit"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
        <button
          className="inline-flex items-center px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-blue-600 rounded-r-md hover:bg-blue-700 hover:border-blue-700 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 focus:outline-none transition-colors duration-200"
          type="submit"
          id="search-submit"
        >
          <svg className="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
          Search
        </button>
      </form>
    </div>
  );
};
```

### Usage
```tsx
<ListSearch 
  onSearch={handleSearch} 
  placeholder="Search by user name" 
  className="flex-shrink-0"
/>
```

### Design Specifications
- **Input**: Rounded left corners, gray border, blue focus ring
- **Button**: Rounded right corners, blue background, search icon
- **Layout**: Flexbox with input taking available space
- **States**: Hover, focus, and transition effects
- **Accessibility**: Proper labels and ARIA attributes

---

## PageSizeSelector

A dropdown component for selecting the number of items to display per page in paginated lists.

### Implementation
```tsx
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
```

### Usage
```tsx
<PageSizeSelector
  pageSize={pageSize}
  onPageSizeChange={handlePageSizeChange}
  options={[10, 25, 50, 100]}
/>
```

### Design Specifications
- **Layout**: Horizontal flexbox with consistent spacing
- **Select**: Rounded corners, gray border, blue focus ring
- **Typography**: Small text with medium weight for labels
- **States**: Focus state with blue ring and border

---

## Paginator

A comprehensive pagination component with navigation buttons, page numbers, and page size controls.

### Implementation
```tsx
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
  adjacentPages?: number;
}

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
  // ... implementation details (see full component for complete code)
  
  return (
    <div className={className}>
      <nav aria-label="Page navigation">
        <ul className="flex items-center space-x-1">
          {/* Navigation buttons with SVG icons */}
          {/* Page numbers with active states */}
        </ul>
      </nav>
      
      {showPageInfo && (
        <div className="text-sm text-gray-500 mt-3">
          Page {currentPage + 1} of {totalPages}
        </div>
      )}
      
      {showPageSize && pageSize && onPageSizeChange && (
        <div className="flex items-center space-x-2 mt-3">
          {/* Page size controls */}
        </div>
      )}
    </div>
  );
};
```

### Usage
```tsx
<Paginator
  currentPage={currentPage}
  totalPages={totalPages}
  onPageChange={handlePageChange}
  showPageInfo={true}
  adjacentPages={2}
/>
```

### Design Specifications
- **Navigation**: SVG icons for first, previous, next, last buttons
- **Page Numbers**: Active state with blue background, hover effects
- **Layout**: Horizontal flexbox with consistent spacing
- **States**: Active, hover, disabled states with appropriate styling
- **Accessibility**: ARIA labels and proper navigation semantics

---

## TableFooter

A container component that combines pagination and page size controls for data tables.

### Implementation
```tsx
import Paginator from './Paginator';
import PageSizeSelector from './PageSizeSelector';

interface TableFooterProps {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  onPageChange: (page: number) => void;
  onPageSizeChange: (pageSize: number) => void;
  showPageInfo?: boolean;
  adjacentPages?: number;
  pageSizeOptions?: number[];
}

const TableFooter = ({
  currentPage,
  totalPages,
  pageSize,
  onPageChange,
  onPageSizeChange,
  showPageInfo = true,
  adjacentPages = 2,
  pageSizeOptions = [5, 10, 25, 50, 100]
}: TableFooterProps) => {
  return (
    <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
      <div className="flex-1">
        <Paginator
          currentPage={currentPage}
          totalPages={totalPages}
          onPageChange={onPageChange}
          showPageInfo={showPageInfo}
          showPageSize={false}
          adjacentPages={adjacentPages}
        />
      </div>

      <PageSizeSelector
        pageSize={pageSize}
        onPageSizeChange={onPageSizeChange}
        options={pageSizeOptions}
      />
    </div>
  );
};
```

### Usage
```tsx
<TableFooter
  currentPage={currentPage}
  totalPages={totalPages}
  pageSize={pageSize}
  onPageChange={handlePageChange}
  onPageSizeChange={handlePageSizeChange}
  showPageInfo={true}
  adjacentPages={2}
/>
```

### Design Specifications
- **Layout**: Responsive flexbox (stacks on mobile, side-by-side on desktop)
- **Spacing**: Consistent gaps between elements
- **Responsive**: Adapts layout based on screen size
- **Integration**: Combines Paginator and PageSizeSelector seamlessly

---

## UserList

A complete user management page that demonstrates the integration of all pagination and search components.

### Key Features
- **Modern Design**: Clean, professional appearance using Tailwind 4
- **Responsive Layout**: Mobile-first design that adapts to all screen sizes
- **Integrated Components**: Seamless integration of search, table, and pagination
- **State Management**: Proper loading, error, and empty states

### Design Patterns
- **Page Header**: White background with shadow and border
- **Data Table**: Clean table with alternating row colors and hover effects
- **Action Buttons**: SVG icons with hover states and semantic colors
- **Loading States**: Animated spinner with descriptive text
- **Error Handling**: Professional error cards with icons
- **Empty States**: Helpful guidance when no data is available

### Component Integration
```tsx
// Header with search
<header className="bg-white shadow-sm border-b border-gray-200">
  <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
    <div className="py-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Users</h1>
          <p className="mt-1 text-sm text-gray-500">Manage user accounts and permissions</p>
        </div>
        <div className="flex-shrink-0">
          <ListSearch onSearch={handleSearchChange} placeholder="Search by user name" />
        </div>
      </div>
    </div>
  </div>
</header>

// Table with pagination footer
<div className="bg-white shadow-sm rounded-lg border border-gray-200 overflow-hidden">
  {/* Table content */}
  <div className="bg-gray-50 px-6 py-3 border-t border-gray-200">
    <TableFooter
      currentPage={currentPage}
      totalPages={totalPages}
      pageSize={pageSize}
      onPageChange={handlePageChange}
      onPageSizeChange={handlePageSizeChangeWrapper}
    />
  </div>
</div>
```

---

## Component Relationships

### Data Flow
```
UserList (Page Component)
├── ListSearch (Search Input)
├── Data Table
│   ├── Table Headers (Sortable)
│   ├── Table Rows (with Action Buttons)
│   └── TableFooter
│       ├── Paginator (Page Navigation)
│       └── PageSizeSelector (Items per Page)
```

### State Management
- **Search State**: Managed by ListSearch component
- **Pagination State**: Managed by UserList and passed to TableFooter
- **Sorting State**: Managed by UserList and applied to table headers
- **Loading/Error States**: Managed by UserList for overall page state

### Responsive Behavior
- **Mobile**: Components stack vertically, full-width layouts
- **Tablet**: Side-by-side layouts, optimized spacing
- **Desktop**: Full horizontal layouts, maximum content width

### Accessibility Features
- **Keyboard Navigation**: All interactive elements are keyboard accessible
- **Screen Readers**: Proper ARIA labels and semantic HTML
- **Focus Management**: Clear focus indicators with blue rings
- **Color Contrast**: WCAG AA compliant color combinations

---

## Best Practices

### 1. **Component Composition**
- Build complex pages from simple, reusable components
- Maintain consistent spacing and sizing across components
- Use Tailwind's design tokens for consistency

### 2. **State Management**
- Keep component state as local as possible
- Pass down only necessary props to child components
- Use consistent naming conventions for handlers

### 3. **Performance**
- Leverage React.memo for components that don't need frequent updates
- Use useMemo for expensive calculations
- Minimize re-renders by optimizing prop changes

### 4. **Testing**
- Test component behavior in isolation
- Verify accessibility features work correctly
- Test responsive behavior across different screen sizes

---

*This catalog documents the current state of admin components. Update it as new components are added or existing ones are modified.*
