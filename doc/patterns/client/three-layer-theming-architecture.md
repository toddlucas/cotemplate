# Three-Layer Theming Architecture

## Overview

The app uses a three-layer theming architecture that provides clear separation of concerns, maintainable code, and flexible theming capabilities. This document explains the architecture, implementation patterns, and usage guidelines.

## Architecture Layers

### Layer 1: Semantic Colors (Theme Foundation)
**Purpose**: Core brand identity and semantic meaning  
**Scope**: Small, focused set of semantic colors  
**Usage**: High-level semantic decisions  
**Implementation**: Tailwind utilities via `@theme` block  

### Layer 2: Structural Colors (UI Patterns)
**Purpose**: Specific UI patterns and structural elements  
**Scope**: Surface colors, border hierarchy, text hierarchy  
**Usage**: Structural decisions and reusable patterns  
**Implementation**: CSS custom properties and reusable classes  

### Layer 3: Utility Classes (Implementation Details)
**Purpose**: Spacing, layout, typography, and other utility classes  
**Scope**: Built-in Tailwind classes  
**Usage**: Implementation details and rapid development  
**Implementation**: Standard Tailwind utilities  

## Layer 1: Semantic Colors

### Core Color Roles
```css
@theme static {
  /* Core color roles (theme anchors) */
  --color-primary: #284b63ff;    /* Main brand accent */
  --color-secondary: #3c6e71ff;  /* Supporting accent */
  --color-accent: #708A58;       /* Highlight color */
  --color-neutral: #d9d9d9ff;    /* UI surfaces and text */

  /* "On" color qualifiers */
  --color-on-primary: #fff;      /* Text/icons on primary */
  --color-on-secondary: #fff;    /* Text/icons on secondary */
  --color-on-neutral: #000;      /* Text/icons on neutral */
  --color-on-accent: #fff;       /* Text/icons on accent */

  /* Semantic state colors */
  --color-success: #548c2fff;    /* Success messages */
  --color-warning: #ffd449ff;    /* Warning messages */
  --color-error: #f9a620ff;      /* Error messages */
  --color-info: #a8d5e2ff;       /* Info messages */

  /* "On" semantic colors */
  --color-on-success: #fff;
  --color-on-warning: #000;
  --color-on-error: #000;
  --color-on-info: #000;

  /* Interactive States - Primary */
  --color-primary-hover: #3e7397ff;    /* Primary button hover */
  --color-primary-active: #203c4eff;    /* Primary button active */
  --color-primary-pressed: #182d3bff;   /* Primary button pressed */
  --color-primary-focus: #6099beff;     /* Primary button focus */

  /* Interactive States - Secondary */
  --color-secondary-hover: #539a9eff;   /* Secondary button hover */
  --color-secondary-active: #30595bff;   /* Secondary button active */
  --color-secondary-pressed: #244344ff;  /* Secondary button pressed */
  --color-secondary-focus: #7bb6b9ff;    /* Secondary button focus */

  /* Interactive States - Semantic Colors */
  --color-success-hover: #72be3fff;     /* Success hover */
  --color-success-active: #447126ff;     /* Success active */
  --color-success-pressed: #33551cff;    /* Success pressed */
  --color-success-focus: #95cf6eff;      /* Success focus */

  --color-error-hover: #fab84cff;       /* Error hover */
  --color-error-active: #db8906ff;       /* Error active */
  --color-error-pressed: #a46704ff;      /* Error pressed */
  --color-error-focus: #fcc979ff;        /* Error focus */

  --color-warning-hover: #ffdd6cff;     /* Warning hover */
  --color-warning-active: #ffc506ff;     /* Warning active */
  --color-warning-pressed: #c49600ff;    /* Warning pressed */
  --color-warning-focus: #ffe591ff;      /* Warning focus */

  --color-info-hover: #b9dde8ff;        /* Info hover */
  --color-info-active: #6cb9ceff;        /* Info active */
  --color-info-pressed: #3b97b1ff;       /* Info pressed */
  --color-info-focus: #cae6edff;         /* Info focus */

  /* Disabled States */
  --color-disabled: #ccccccff;           /* Disabled elements */
  --color-on-disabled: #666666ff;        /* Text on disabled */

  /* Interactive Feedback */
  --color-drag: #3b82f6ff;              /* Drag and drop state */
  --color-on-drag: #ffffff;              /* Text on drag state */
  --color-selected: #3b82f6ff;           /* Selection state */
  --color-on-selected: #ffffff;          /* Text on selection */
}
```

### Usage Guidelines
- **Primary**: Main CTAs, brand elements, key actions
- **Secondary**: Supporting actions, secondary buttons
- **Accent**: Highlights, special features, attention-grabbing elements
- **Neutral**: Backgrounds, borders, subtle UI elements
- **Semantic**: Use for state feedback (success, warning, error, info)
- **Interactive States**: Use for hover, active, pressed, focus states
- **Disabled States**: Use for inactive form elements and controls
- **Feedback States**: Use for drag and drop, selection states

### Examples
```html
<!-- Primary action -->
<button class="bg-primary text-on-primary">Primary Action</button>

<!-- Success message -->
<div class="bg-success text-on-success">Operation completed successfully</div>

<!-- Error state -->
<div class="bg-error text-on-error">An error occurred</div>
```

## Layer 2: Structural Colors

### Surface Hierarchy
```css
:root {
  /* Surface Hierarchy */
  --color-panel: #f8f9fa;        /* Panel backgrounds */
  --color-header: #f1f3f4;       /* Header/toolbar backgrounds */
  --color-card: #ffffff;          /* Card/module backgrounds */

  --color-on-panel: #000;
  --color-on-header: #000;
  --color-on-card: #000;
}
```

### Border Hierarchy
```css
:root {
  /* Border Hierarchy */
  --color-border: #e0e0e0;       /* Standard borders */
  --color-border-light: #f0f0f0; /* Light borders */
  --color-border-emphasis: #007acc; /* Focus/active borders */
}
```

### Text Hierarchy
```css
:root {
  /* Text Hierarchy */
  --color-text-primary: #333;     /* Primary text */
  --color-text-secondary: #666;   /* Secondary text */
  --color-text-tertiary: #999;    /* Tertiary text */
  --color-text-muted: #6b7280;   /* Muted text */
}
```

### Utility Classes (Recommended Approach)
Instead of the verbose `color:var(--color-panel)` syntax, we now provide clean utility classes:

```css
/* Background Utilities */
.bg-surface { background-color: var(--color-surface); }
.bg-panel { background-color: var(--color-panel); }
.bg-header { background-color: var(--color-header); }
.bg-card { background-color: var(--color-card); }

/* Text Color Utilities */
.text-surface { color: var(--color-on-surface); }
.text-panel { color: var(--color-on-panel); }
.text-header { color: var(--color-on-header); }
.text-card { color: var(--color-on-card); }
.text-muted { color: var(--color-text-muted); }
.text-secondary { color: var(--color-text-secondary); }
.text-tertiary { color: var(--color-text-tertiary); }

/* Border Utilities */
.border-standard { border-color: var(--color-border); }
.border-light { border-color: var(--color-border-light); }
.border-emphasis { border-color: var(--color-border-emphasis); }

/* Interactive State Utilities */
.hover-panel:hover { background-color: var(--color-panel-hover); }
.hover-card:hover { background-color: var(--color-card-hover); }
.hover-header:hover { background-color: var(--color-header-hover); }
.hover-surface:hover { background-color: var(--color-surface-hover); }

.active-panel:active { background-color: var(--color-panel-active); }
.active-card:active { background-color: var(--color-card-active); }
.active-header:active { background-color: var(--color-header-active); }
.active-surface:active { background-color: var(--color-surface-active); }

/* Common Pattern Utilities */
.surface-elevated {
  background-color: var(--color-card);
  border: 1px solid var(--color-border);
  border-radius: 6px;
}

.surface-flat {
  background-color: var(--color-panel);
  border: 1px solid var(--color-border-light);
}

.surface-inset {
  background-color: var(--color-header);
  border: 1px solid var(--color-border);
  border-radius: 4px;
}

.text-label {
  color: var(--color-text-secondary);
  font-size: 0.875rem;
  font-weight: 500;
}

.text-caption {
  color: var(--color-text-muted);
  font-size: 0.75rem;
}

/* Focus and Selection Utilities */
.focus-emphasis:focus {
  border-color: var(--color-border-emphasis);
  outline: none;
}

.focus-ring:focus {
  outline: 2px solid var(--color-border-emphasis);
  outline-offset: 2px;
}

.selected {
  background-color: var(--color-selected);
  color: var(--color-on-selected);
}

.drag {
  background-color: var(--color-drag);
  color: var(--color-on-drag);
}
```

### Component Patterns
```css
/* Panel Container */
.panel-container {
  background-color: var(--color-panel);
  border: 1px solid var(--color-border);
  border-radius: 4px;
  overflow: hidden;
}

.panel-header {
  background-color: var(--color-header);
  color: var(--color-on-header);
  padding: 12px 16px;
  border-bottom: 1px solid var(--color-border);
  font-weight: 600;
  font-size: 14px;
}

.panel-content {
  padding: 16px;
  color: var(--color-on-panel);
}

/* Button Patterns */
.button-primary {
  background-color: var(--color-primary);
  color: var(--color-on-primary);
  border: 1px solid var(--color-primary);
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.button-secondary {
  background-color: var(--color-secondary);
  color: var(--color-on-secondary);
  border: 1px solid var(--color-secondary);
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.button-surface {
  background-color: var(--color-card);
  color: var(--color-on-card);
  border: 1px solid var(--color-border);
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

/* Input Patterns */
.input-field {
  background-color: var(--color-card);
  color: var(--color-on-card);
  border: 1px solid var(--color-border);
  padding: 8px 12px;
  border-radius: 4px;
  outline: none;
  transition: border-color 0.2s ease;
}

.input-field:focus {
  border-color: var(--color-border-emphasis);
}

.input-field::placeholder {
  color: var(--color-text-muted);
}
```

### Usage Examples
```html
<!-- Before: Verbose syntax -->
<div class="bg-[color:var(--color-panel)] text-[color:var(--color-text-secondary)] border-[color:var(--color-border)]">

<!-- After: Clean utility classes -->
<div class="bg-panel text-secondary border-standard">

<!-- Surface patterns -->
<div class="surface-elevated p-4">
  <h4 class="text-label">Card Title</h4>
  <p class="text-caption">Card description</p>
</div>

<!-- Interactive elements -->
<div class="bg-card hover:bg-card-hover active:bg-card-active p-4 rounded">
  Interactive card with hover and active states
</div>

<!-- Focus states -->
<button class="bg-primary text-on-primary focus:focus-ring">
  Button with focus ring
</button>

<!-- Legacy classes (still available) -->
<div class="panel-container">
  <div class="panel-header">Panel Header</div>
  <div class="panel-content">Panel content goes here</div>
</div>

<!-- Component patterns -->
<button class="button-primary">Primary Action</button>
<button class="button-secondary">Secondary Action</button>
<button class="button-surface">Surface Action</button>

<!-- Input field -->
<input type="text" placeholder="Enter text..." class="input-field" />
```

### Inline CSS Variable Usage
Layer 2 also supports direct CSS variable references for one-off styling needs:

```html
<!-- Direct CSS variable usage for custom styling -->
<div class="border border-[color:var(--color-border)] bg-[color:var(--color-panel)]">
  Custom panel with direct variable references
</div>

<!-- Mixed approach: reusable class + direct variables -->
<div class="panel-container" style="border-color: var(--color-border-emphasis);">
  Panel with custom border emphasis
</div>

<!-- Arbitrary value syntax for Tailwind -->
<div class="border border-[color:var(--color-border-light)] bg-[color:var(--color-header)]">
  Header-style container with light border
</div>
```

**When to use direct CSS variables:**
- One-off styling that doesn't warrant a new reusable class
- Custom combinations of existing variables
- Dynamic styling that changes based on component state
- Prototyping new patterns before creating reusable classes

## Layer 3: Utility Classes

### Standard Tailwind Utilities
```html
<!-- Layout utilities -->
<div class="flex items-center justify-between p-4 space-x-2">
  <div class="flex-1">Content</div>
  <div class="flex-shrink-0">Actions</div>
</div>

<!-- Spacing utilities -->
<div class="p-4 m-2 space-y-3">
  <div class="mb-2">Item 1</div>
  <div class="mb-2">Item 2</div>
</div>

<!-- Typography utilities -->
<h1 class="text-3xl font-bold">Heading</h1>
<p class="text-sm leading-relaxed">Body text</p>

<!-- Responsive utilities -->
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
  <!-- Responsive grid -->
</div>
```

## Dark Mode Support

### Layer 1 Dark Mode
```css
.dark {
  /* Layer 1 overrides */
  --color-ambient: #2f3e46;
  --color-on-ambient: #fff;
  --color-default: #fff;
  --color-surface: #2f3e46;
  --color-on-surface: #fff;
}
```

### Layer 2 Dark Mode
```css
.dark {
  /* Layer 2 overrides */
  --color-panel: #374151;
  --color-header: #4b5563;
  --color-card: #1f2937;

  --color-on-panel: #fff;
  --color-on-header: #fff;
  --color-on-card: #fff;

  /* Border Hierarchy - Dark Mode */
  --color-border: #374151;
  --color-border-light: #4b5563;
  --color-border-emphasis: #3b82f6;

  /* Text Hierarchy - Dark Mode */
  --color-text-primary: #f9fafb;
  --color-text-secondary: #d1d5db;
  --color-text-tertiary: #9ca3af;
  --color-text-muted: #6b7280;
}
```

## File Organization

### theme.css
- **Layer 1**: `@theme` block with semantic colors for Tailwind utilities
- **Layer 2**: Custom properties section with structural colors for CSS classes
- **Dark mode**: Comprehensive overrides for both layers

### utilities.css
- **Background utilities**: `.bg-surface`, `.bg-panel`, `.bg-header`, `.bg-card`
- **Text utilities**: `.text-surface`, `.text-panel`, `.text-header`, `.text-card`, `.text-muted`, `.text-secondary`, `.text-tertiary`
- **Border utilities**: `.border-standard`, `.border-light`, `.border-emphasis`
- **Interactive utilities**: `.hover-panel`, `.hover-card`, `.hover-header`, `.hover-surface`
- **Common patterns**: `.surface-elevated`, `.surface-flat`, `.surface-inset`, `.text-label`, `.text-caption`
- **Focus utilities**: `.focus-emphasis`, `.focus-ring`, `.selected`, `.drag`

### components.css
- **Button patterns**: `.button-primary`, `.button-secondary`, `.button-surface`
- **Input patterns**: `.input-field`
- **Legacy patterns**: `.panel`, `.header`, `.card` (for backward compatibility)

## Decision Framework

### When to Use Each Layer

#### Layer 1 (Semantic Colors)
- **Use for**: Brand identity, semantic meaning, state feedback
- **Examples**: Primary actions, success/error states, brand elements
- **Pattern**: `bg-primary`, `text-on-primary`, `bg-success`, `text-on-success`

#### Layer 2 (Structural Colors & Utilities)
- **Use for**: UI structure, reusable patterns, component styling
- **Examples**: Panel backgrounds, borders, text hierarchy, form elements
- **Pattern**: `.bg-panel`, `.text-secondary`, `.surface-elevated`, `.button-primary`

#### Layer 3 (Utility Classes)
- **Use for**: Implementation details, layout, spacing, typography
- **Examples**: Flexbox layouts, padding/margin, responsive design
- **Pattern**: `flex`, `p-4`, `space-x-2`, `text-sm`, `rounded-md`

## Development Guidelines

### 1. Start with Layer 1
- Define semantic meaning first
- Use for brand identity and state feedback
- Keep the set small and focused

### 2. Build Layer 2 Patterns
- Create reusable component classes
- Use for structural decisions
- Maintain clear naming conventions

### 3. Use Layer 3 for Details
- Use Tailwind utilities for implementation
- Focus on layout, spacing, and typography
- Don't override Layer 1 or Layer 2 decisions

### 4. Prefer Utility Classes Over Direct Variables
- **Use**: `.bg-panel`, `.text-secondary`, `.border-standard`
- **Avoid**: `bg-[color:var(--color-panel)]`, `text-[color:var(--color-text-secondary)]`
- **Exception**: One-off custom styling that doesn't warrant a utility class

### 5. Maintain Separation
- Don't use Layer 3 for semantic decisions
- Don't use Layer 1 for structural decisions
- Keep concerns separated and clear

## Benefits

### Cleaner, More Readable Code
- **Before**: `bg-[color:var(--color-panel)] text-[color:var(--color-text-secondary)]`
- **After**: `bg-panel text-secondary`
- **Result**: 50% reduction in class name length, much better readability

### Clear Separation of Concerns
- **Semantic decisions** stay at Layer 1
- **Structural decisions** stay at Layer 2
- **Implementation details** stay at Layer 3

### Maintainable Discipline
- Forces developers to think about the purpose of each styling decision
- Prevents mixing concerns (like using `bg-primary` for a panel background)
- Creates clear patterns that other developers can follow

### Flexible Theming
- Layer 1 handles brand changes
- Layer 2 handles UI pattern changes
- Layer 3 handles implementation changes

### Scalable Architecture
- Easy to add new semantic colors (Layer 1)
- Easy to add new surface patterns (Layer 2)
- Easy to use new Tailwind features (Layer 3)

## Migration Strategy

### Phase 1: Foundation
1. Implement Layer 1 semantic colors
2. Set up dark mode overrides
3. Test theme switching functionality

### Phase 2: Utility Classes
1. ✅ Create Layer 2 utility classes
2. ✅ Replace verbose `color:var()` syntax
3. ✅ Test utility classes in components

### Phase 3: Component Integration
1. ✅ Start with workspace container
2. ✅ Migrate panel components
3. ✅ Update interactive elements
4. ✅ Refine based on real usage

### Phase 4: Optimization
1. Review and refine patterns
2. Optimize for performance
3. Document best practices
4. Train team on architecture

## Testing

### Theme Page Testing
- Test all layers in light mode
- Test all layers in dark mode
- Test theme switching transitions
- Verify color contrast and accessibility

### Component Testing
- Test each component with all relevant color combinations
- Verify "on" colors provide adequate contrast
- Test hover, focus, and active states
- Ensure semantic colors are used appropriately

### Integration Testing
- Test Layer 1 and Layer 2 work together
- Verify dark mode overrides work correctly
- Test responsive behavior in both themes
- Validate performance impact

## Future Enhancements

### Planned Features
- **Custom theme builder**: User-defined color schemes
- **Theme presets**: Pre-built theme collections
- **Animation themes**: Theme-aware animations
- **High contrast mode**: Accessibility-focused variant

### Advanced Patterns
- **Component-level themes**: Allow individual components to override theme
- **Dynamic color generation**: Generate complementary colors automatically
- **Theme export/import**: Share custom themes between users
- **Theme versioning**: Track theme changes over time

## Resources

- [Theme Test Page](/theme-test) - Interactive testing environment
- [Theme Migration Plan](../overview/client/theming-migration-plan.md) - Implementation roadmap
- [Theme Guidance](../patterns/theme-guidance.md) - Detailed usage guidelines
- [Color Palette](../ideation/theme-color-ideas.md) - Color exploration and ideas 