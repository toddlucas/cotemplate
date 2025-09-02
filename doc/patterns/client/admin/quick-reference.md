# Admin UI Quick Reference

A quick reference guide for developers working with the Corpos admin interface using Tailwind 4.

## 🎨 **Common Color Classes**

### Text Colors
```css
text-gray-900        /* Primary text */
text-gray-700        /* Secondary text */
text-gray-600        /* Tertiary text */
text-gray-500        /* Muted text */
text-gray-400        /* Disabled text */
text-blue-600        /* Links, primary actions */
text-white           /* Text on colored backgrounds */
```

### Background Colors
```css
bg-white             /* Primary background */
bg-gray-50           /* Light background */
bg-gray-100          /* Hover states */
bg-blue-600          /* Primary buttons */
bg-red-50            /* Error backgrounds */
bg-green-50          /* Success backgrounds */
```

### Border Colors
```css
border-gray-200      /* Light borders */
border-gray-300      /* Input borders */
border-blue-500      /* Focus borders */
border-red-200       /* Error borders */
```

## 📏 **Spacing & Layout**

### Container Classes
```css
max-w-7xl mx-auto    /* Maximum width, centered */
px-4 sm:px-6 lg:px-8 /* Responsive horizontal padding */
py-6                 /* Header padding */
py-8                 /* Main content padding */
```

### Common Spacing
```css
p-4                  /* All sides padding */
px-6 py-3            /* Table header padding */
px-6 py-4            /* Table cell padding */
mt-1, mt-2, mt-3    /* Top margins */
mb-6                 /* Bottom margin */
gap-4                /* Flexbox gap */
space-x-2            /* Horizontal spacing */
space-y-3            /* Vertical spacing */
```

## 🔤 **Typography**

### Font Sizes
```css
text-2xl             /* Page titles */
text-xl              /* Section titles */
text-lg              /* Subsection titles */
text-base            /* Body text */
text-sm              /* Secondary text */
text-xs              /* Captions, metadata */
```

### Font Weights
```css
font-bold            /* Important text */
font-semibold        /* Section headers */
font-medium          /* Labels, emphasis */
font-normal          /* Default weight */
```

### Text Colors
```css
text-gray-900        /* Primary text */
text-gray-600        /* Secondary text */
text-gray-500        /* Muted text */
```

## 🎯 **Interactive States**

### Hover Effects
```css
hover:bg-gray-50     /* Light background */
hover:text-gray-700  /* Darker text */
hover:bg-blue-50     /* Blue tint background */
```

### Focus States
```css
focus:ring-2         /* Focus ring width */
focus:ring-blue-500  /* Focus ring color */
focus:ring-offset-2  /* Focus ring offset */
focus:outline-none   /* Remove default outline */
focus:border-blue-500 /* Focus border color */
```

### Transitions
```css
transition-colors duration-200    /* Color transitions */
transition-all duration-150       /* All property transitions */
```

## 📱 **Responsive Design**

### Breakpoint Prefixes
```css
sm:                  /* 640px and up */
md:                  /* 768px and up */
lg:                  /* 1024px and up */
xl:                  /* 1280px and up */
```

### Common Responsive Patterns
```css
flex-col sm:flex-row              /* Stack on mobile, row on desktop */
text-xl sm:text-2xl              /* Smaller on mobile */
px-4 sm:px-6 lg:px-8             /* Progressive padding */
grid-cols-1 md:grid-cols-2 lg:grid-cols-3 /* Responsive grid */
```

## 🧩 **Component Patterns**

### Page Header
```tsx
<div className="bg-white shadow-sm border-b border-gray-200">
  <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
    <div className="py-6">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Title</h1>
          <p className="mt-1 text-sm text-gray-500">Description</p>
        </div>
        <div className="flex-shrink-0">
          {/* Actions */}
        </div>
      </div>
    </div>
  </div>
</div>
```

### Data Table
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

### Form Inputs
```tsx
<input
  className="block w-full px-3 py-2 text-sm text-gray-900 bg-white border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 focus:outline-none transition-colors duration-200"
  type="text"
  placeholder="Enter text..."
/>

<select className="px-3 py-2 text-sm text-gray-900 bg-white border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 focus:outline-none transition-colors duration-200">
  <option>Option</option>
</select>
```

### Buttons
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

## 🚨 **State Indicators**

### Loading State
```tsx
<div className="flex items-center justify-center py-12">
  <div className="flex items-center space-x-3">
    <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-blue-600"></div>
    <span className="text-gray-600">Loading...</span>
  </div>
</div>
```

### Error State
```tsx
<div className="bg-red-50 border border-red-200 rounded-lg p-4">
  <div className="flex">
    <div className="flex-shrink-0">
      <svg className="h-5 w-5 text-red-400" viewBox="0 0 20 20" fill="currentColor">
        <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
      </svg>
    </div>
    <div className="ml-3">
      <h3 className="text-sm font-medium text-red-800">Error Title</h3>
      <div className="mt-2 text-sm text-red-700">Error message</div>
    </div>
  </div>
</div>
```

### Empty State
```tsx
<div className="text-center py-12">
  <svg className="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197m13.5-9a2.5 2.5 0 11-5 0 2.5 2.5 0 015 0z" />
  </svg>
  <h3 className="mt-2 text-sm font-medium text-gray-900">No items found</h3>
  <p className="mt-1 text-sm text-gray-500">Get started by creating a new item.</p>
</div>
```

## 🔧 **Utility Classes**

### Flexbox
```css
flex                 /* Display flex */
flex-col             /* Column direction */
flex-row             /* Row direction */
items-center         /* Align items center */
justify-between      /* Justify content space-between */
justify-center       /* Justify content center */
flex-1               /* Flex grow 1 */
flex-shrink-0        /* Don't shrink */
```

### Grid
```css
grid                 /* Display grid */
grid-cols-1          /* 1 column */
grid-cols-2          /* 2 columns */
grid-cols-3          /* 3 columns */
gap-6                /* Grid gap */
```

### Positioning
```css
relative             /* Position relative */
absolute             /* Position absolute */
overflow-hidden      /* Hide overflow */
overflow-x-auto      /* Horizontal scroll */
```

### Sizing
```css
w-full               /* Full width */
w-auto               /* Auto width */
min-w-0              /* Minimum width 0 */
h-8                  /* Height 32px */
h-12                 /* Height 48px */
```

## 📋 **Class Organization Pattern**

### Recommended Order
```css
/* Layout → Spacing → Typography → Colors → States */
flex items-center px-4 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 transition-colors duration-200
```

### Example Breakdown
```css
flex                 /* Layout */
items-center         /* Layout */
px-4 py-2           /* Spacing */
text-sm              /* Typography */
font-medium          /* Typography */
text-white           /* Colors */
bg-blue-600          /* Colors */
hover:bg-blue-700    /* States */
transition-colors duration-200 /* States */
```

## 🎯 **Common Use Cases**

### Card Layout
```tsx
<div className="bg-white shadow-sm rounded-lg border border-gray-200 p-6">
  <h3 className="text-lg font-medium text-gray-900 mb-4">Card Title</h3>
  <p className="text-gray-600">Card content goes here.</p>
</div>
```

### Section Divider
```tsx
<div className="border-t border-gray-200 my-6"></div>
```

### Responsive Container
```tsx
<div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
  {/* Content */}
</div>
```

### Icon with Text
```tsx
<div className="flex items-center space-x-2">
  <svg className="w-4 h-4 text-gray-500">...</svg>
  <span className="text-sm text-gray-600">Label</span>
</div>
```

---

## 📚 **Additional Resources**

- **Full Style Guide**: [Admin UI Style Guide](./README.md)
- **Component Catalog**: [Component Catalog](./component-catalog.md)
- **Tailwind Docs**: [https://tailwindcss.com/docs](https://tailwindcss.com/docs)
- **Tailwind Cheat Sheet**: [https://nerdcave.com/tailwind-cheat-sheet](https://nerdcave.com/tailwind-cheat-sheet)

---

*Keep this reference handy for quick access to common patterns and classes while developing admin components.*
