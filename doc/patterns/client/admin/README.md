# Admin UI Style Guide

This document outlines the design system and styling guidelines for the Corpos admin interface components. We use **Tailwind 4** as our primary styling framework to ensure consistency, maintainability, and a modern user experience.

## Table of Contents

- [Design Principles](#design-principles)
- [Color Palette](#color-palette)
- [Typography](#typography)
- [Spacing & Layout](#spacing--layout)
- [Component Patterns](#component-patterns)
- [Interactive States](#interactive-states)
- [Responsive Design](#responsive-design)
- [Accessibility](#accessibility)
- [Component Examples](#component-examples)

## Design Principles

### 1. **Consistency First**
- Use predefined Tailwind classes for spacing, colors, and typography
- Maintain consistent component behavior across the admin interface
- Follow established patterns for similar UI elements

### 2. **Accessibility by Default**
- Ensure proper contrast ratios (WCAG AA compliance)
- Provide clear focus states for keyboard navigation
- Use semantic HTML and ARIA labels appropriately

### 3. **Mobile-First Approach**
- Design for mobile devices first, then enhance for larger screens
- Use responsive utilities to adapt layouts across breakpoints
- Ensure touch-friendly interaction areas

### 4. **Performance & Maintainability**
- Leverage Tailwind's utility-first approach for rapid development
- Minimize custom CSS to reduce bundle size
- Use consistent class combinations for common patterns

## Color Palette

### Primary Colors
```css
/* Blue - Primary actions, links, focus states */
bg-blue-600          /* Primary buttons, active states */
text-blue-600        /* Links, primary text */
border-blue-500      /* Focus borders */
ring-blue-500        /* Focus rings */

/* Gray - Text, borders, backgrounds */
text-gray-900        /* Primary text */
text-gray-600        /* Secondary text */
text-gray-500        /* Muted text */
text-gray-400        /* Disabled text */
bg-gray-50           /* Light backgrounds */
bg-gray-100          /* Hover states */
border-gray-200      /* Light borders */
border-gray-300      /* Input borders */
```

### Semantic Colors
```css
/* Success */
bg-green-600         /* Success buttons */
text-green-700       /* Success text */

/* Warning */
bg-amber-600         /* Warning buttons */
text-amber-700       /* Warning text */

/* Error */
bg-red-600           /* Error buttons */
text-red-700         /* Error text */
bg-red-50            /* Error backgrounds */
border-red-200       /* Error borders */
```

## Typography

### Font Sizes & Weights
```css
/* Headings */
text-2xl font-bold   /* Page titles (h1) */
text-xl font-semibold /* Section titles (h2) */
text-lg font-medium  /* Subsection titles (h3) */

/* Body Text */
text-base            /* Default body text */
text-sm              /* Secondary text, labels */
text-xs              /* Captions, metadata */

/* Font Weights */
font-bold            /* Important text */
font-semibold        /* Section headers */
font-medium          /* Labels, emphasis */
font-normal          /* Default weight */
```

### Text Colors
```css
text-gray-900        /* Primary text */
text-gray-700        /* Secondary text */
text-gray-600        /* Tertiary text */
text-gray-500        /* Muted text */
text-gray-400        /* Disabled text */
```

## Spacing & Layout

### Container & Padding
```css
/* Page containers */
max-w-7xl mx-auto    /* Maximum width with centering */
px-4 sm:px-6 lg:px-8 /* Responsive horizontal padding */
py-6                 /* Vertical padding for headers */
py-8                 /* Vertical padding for main content */

/* Component spacing */
p-4                  /* Standard padding */
px-6 py-3            /* Table header padding */
px-6 py-4            /* Table cell padding */
```

### Margins & Gaps
```css
/* Vertical spacing */
mt-1                 /* Small margin top */
mt-2                 /* Medium margin top */
mt-3                 /* Large margin top */
mb-6                 /* Bottom margin for sections */

/* Horizontal spacing */
ml-3                 /* Left margin */
mr-2                 /* Right margin */

/* Gaps between elements */
gap-4                /* Standard gap */
space-x-2            /* Horizontal spacing */
space-y-3            /* Vertical spacing */
```

## Component Patterns

### 1. **Page Headers**
```tsx
<div className="bg-white shadow-sm border-b border-gray-200">
  <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
    <div className="py-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Page Title</h1>
          <p className="mt-1 text-sm text-gray-500">Page description</p>
        </div>
        <div className="flex-shrink-0">
          {/* Action buttons or search */}
        </div>
      </div>
    </div>
  </div>
</div>
```

### 2. **Data Tables**
```tsx
<div className="bg-white shadow-sm rounded-lg border border-gray-200 overflow-hidden">
  <div className="overflow-x-auto">
    <table className="min-w-full divide-y divide-gray-200">
      <thead className="bg-gray-50">
        <tr>
          <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
            Header
          </th>
        </tr>
      </thead>
      <tbody className="bg-white divide-y divide-gray-200">
        <tr className="hover:bg-gray-50 transition-colors duration-150">
          <td className="px-6 py-4 whitespace-nowrap">Content</td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

### 3. **Form Controls**
```tsx
<input
  className="block w-full px-3 py-2 text-sm text-gray-900 bg-white border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 focus:outline-none transition-colors duration-200"
  type="text"
  placeholder="Enter text..."
/>

<select className="px-3 py-2 text-sm text-gray-900 bg-white border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 focus:outline-none transition-colors duration-200">
  <option>Option 1</option>
</select>
```

### 4. **Buttons**
```tsx
{/* Primary Button */}
<button className="inline-flex items-center px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-blue-600 rounded-md hover:bg-blue-700 hover:border-blue-700 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 focus:outline-none transition-colors duration-200">
  Primary Action
</button>

{/* Secondary Button */}
<button className="inline-flex items-center px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 hover:text-gray-900 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 focus:outline-none transition-colors duration-200">
  Secondary Action
</button>

{/* Icon Button */}
<button className="inline-flex items-center justify-center w-8 h-8 text-gray-500 hover:text-blue-600 transition-colors duration-200 rounded-md hover:bg-blue-50">
  <svg className="w-4 h-4">...</svg>
</button>
```

## Interactive States

### Hover States
```css
hover:bg-gray-50      /* Light background on hover */
hover:text-gray-700   /* Darker text on hover */
hover:border-gray-400 /* Darker border on hover */
```

### Focus States
```css
focus:ring-2          /* Focus ring width */
focus:ring-blue-500   /* Focus ring color */
focus:ring-offset-2   /* Focus ring offset */
focus:outline-none    /* Remove default outline */
```

### Active States
```css
active:bg-blue-700    /* Darker background when pressed */
active:scale-95       /* Slight scale down effect */
```

### Disabled States
```css
disabled:opacity-50   /* Reduce opacity */
disabled:cursor-not-allowed /* Show disabled cursor */
```

## Responsive Design

### Breakpoint Strategy
```css
/* Mobile First - Default styles for mobile */
/* Small screens and up */
sm:px-6              /* 640px and up */
sm:flex-row          /* Stack horizontally on larger screens */

/* Medium screens and up */
md:px-8              /* 768px and up */

/* Large screens and up */
lg:px-8              /* 1024px and up */
lg:grid-cols-3       /* 3-column grid on large screens */

/* Extra large screens */
xl:max-w-7xl         /* 1280px and up */
```

### Responsive Patterns
```tsx
{/* Responsive layout */}
<div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
  {/* Content */}
</div>

{/* Responsive grid */}
<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
  {/* Grid items */}
</div>

{/* Responsive text */}
<h1 className="text-xl sm:text-2xl font-bold text-gray-900">
  Responsive Title
</h1>
```

## Accessibility

### Focus Management
- Always provide visible focus indicators using `focus:ring-2`
- Use `focus:outline-none` only when providing custom focus styles
- Ensure focus order follows logical document flow

### Color Contrast
- Use `text-gray-900` on `bg-white` for primary text (high contrast)
- Use `text-gray-600` on `bg-white` for secondary text (adequate contrast)
- Avoid using `text-gray-400` for important information

### Semantic HTML
- Use proper heading hierarchy (h1, h2, h3)
- Include `aria-label` for interactive elements without visible text
- Use `aria-current="page"` for current navigation items

## Component Examples

### Search Component
```tsx
const ListSearch = ({ onSearch, placeholder }: ListSearchProps) => {
  return (
    <form className="flex">
      <input
        className="block w-full px-3 py-2 text-sm text-gray-900 bg-white border border-gray-300 rounded-l-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 focus:outline-none transition-colors duration-200"
        type="text"
        placeholder={placeholder}
      />
      <button
        className="inline-flex items-center px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-blue-600 rounded-r-md hover:bg-blue-700 hover:border-blue-700 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 focus:outline-none transition-colors duration-200"
        type="submit"
      >
        Search
      </button>
    </form>
  );
};
```

### Pagination Component
```tsx
const Paginator = ({ currentPage, totalPages, onPageChange }: PaginatorProps) => {
  return (
    <nav aria-label="Page navigation">
      <ul className="flex items-center space-x-1">
        <li>
          <button
            className={`inline-flex items-center px-3 py-1 text-sm font-medium rounded-md transition-colors duration-200 ${
              currentPage === 0
                ? 'text-gray-400 bg-gray-100 cursor-not-allowed'
                : 'text-gray-500 bg-white border border-gray-300 hover:bg-gray-50 hover:text-gray-700'
            }`}
            onClick={() => onPageChange(currentPage - 1)}
            disabled={currentPage === 0}
          >
            Previous
          </button>
        </li>
        {/* Page numbers */}
      </ul>
    </nav>
  );
};
```

## Best Practices

### 1. **Class Organization**
- Group related classes logically (layout, spacing, typography, colors)
- Use consistent ordering: layout → spacing → typography → colors → states
- Example: `flex items-center px-4 py-2 text-sm font-medium text-white bg-blue-600`

### 2. **Component Composition**
- Build complex components from simple, reusable patterns
- Use consistent spacing and sizing across similar components
- Leverage Tailwind's design tokens for consistency

### 3. **Performance**
- Avoid creating custom CSS when Tailwind utilities suffice
- Use `@apply` sparingly and only for complex, repeated patterns
- Leverage Tailwind's JIT compilation for optimal bundle size

### 4. **Maintenance**
- Document any custom patterns or deviations from this guide
- Review and update the style guide as the design system evolves
- Use consistent naming conventions for custom classes

## Resources

- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [Tailwind CSS Cheat Sheet](https://nerdcave.com/tailwind-cheat-sheet)
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
- [Design System Best Practices](https://www.designsystems.com/)

---

*This style guide is a living document. Please update it as the design system evolves and new patterns emerge.*
