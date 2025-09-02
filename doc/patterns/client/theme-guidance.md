# Theme System Guidance

## Overview

This document provides guidance for the theme system, which follows Material Design principles with CSS custom properties and Tailwind CSS v4 integration.

## Color System Architecture

### Core Design Principles

1. **Material Design Foundation**: Uses surface colors with "on" qualifiers for text/icons
2. **CSS Custom Properties**: Dynamic theming with CSS variables
3. **Tailwind Integration**: Leverages `@theme` block for utility class generation
4. **Dark Mode Support**: Automatic light/dark theme switching
5. **Semantic Gap Detection**: Identify and suggest new theme colors when semantic needs aren't met

### Semantic Gap Detection

When working with the theme system, if you notice a semantic gap where existing colors don't adequately represent the intended meaning, **suggest a new theme color type** rather than silently substituting an inferior alternative.

#### Examples of Semantic Gaps:
- **Interactive States**: Need for hover, active, pressed, focus states
- **Status Variations**: Different levels of success, warning, error states
- **Surface Interactions**: Panel hover, card active states
- **Feedback States**: Drag and drop, selection, disabled states

#### When to Suggest New Colors:
- ✅ **Good**: "We need `--color-primary-hover` for button hover states"
- ✅ **Good**: "We need `--color-disabled` for inactive form elements"
- ❌ **Avoid**: "I'll use `--color-neutral` for disabled states" (suboptimal semantic match)

#### Process for Adding New Colors:
1. **Identify the semantic gap** in the current color system
2. **Propose the new color variable** with clear semantic meaning
3. **Define both light and dark mode values**
4. **Update the theme documentation**
5. **Apply consistently across components**

### Color Token Structure

#### Core Color Roles (Theme Anchors)
```css
--color-primary: #284b63ff;    /* Main brand accent */
--color-secondary: #3c6e71ff;  /* Supporting accent */
--color-accent: #708A58;       /* Highlight color */
--color-neutral: #d9d9d9ff;    /* UI surfaces and text */
```

#### "On" Color Qualifiers
```css
--color-on-primary: #fff;      /* Text/icons on primary */
--color-on-secondary: #fff;    /* Text/icons on secondary */
--color-on-neutral: #000;      /* Text/icons on neutral */
--color-on-accent: #fff;       /* Text/icons on accent */
```

#### Semantic State Colors
```css
--color-success: #548c2fff;    /* Success messages */
--color-warning: #ffd449ff;    /* Warning messages */
--color-error: #f9a620ff;      /* Error messages */
--color-info: #a8d5e2ff;       /* Info messages */
```

#### "On" Semantic Colors
```css
--color-on-success: #fff;
--color-on-warning: #000;
--color-on-error: #000;
--color-on-info: #000;
```

#### Interactive State Colors
```css
--color-primary-hover: #3e7397ff;    /* Primary button hover */
--color-primary-active: #203c4eff;    /* Primary button active */
--color-primary-pressed: #182d3bff;   /* Primary button pressed */
--color-primary-focus: #6099beff;     /* Primary button focus */

--color-disabled: #ccccccff;          /* Disabled elements */
--color-on-disabled: #666666ff;       /* Text on disabled */

--color-drag: #3b82f6ff;              /* Drag and drop state */
--color-on-drag: #ffffff;             /* Text on drag state */
--color-selected: #3b82f6ff;          /* Selection state */
--color-on-selected: #ffffff;         /* Text on selection */
```

#### Structural Colors
```css
--color-surface: #fff;          /* Main background */
--color-on-surface: #000;      /* Text on surface */
--color-ambient: #fff;         /* Default background */
--color-on-ambient: #000;      /* Default text */
--color-default: #000;         /* Default text color */
--color-muted: #888;           /* Muted text */
```

## Usage Guidelines

### When to Use Each Color Role

#### Primary Colors
- **`primary`**: Main CTAs, brand elements, key actions
- **`secondary`**: Supporting actions, secondary buttons
- **`accent`**: Highlights, special features, attention-grabbing elements
- **`neutral`**: Backgrounds, borders, subtle UI elements

#### Semantic Colors
- **`success`**: Confirmations, successful operations, positive feedback
- **`warning`**: Cautionary messages, attention needed
- **`error`**: Errors, destructive actions, critical issues
- **`info`**: Informational messages, tips, secondary feedback

#### Surface Colors
- **`surface`**: Main content areas, cards, modals
- **`ambient`**: Page backgrounds, default surfaces
- **`muted`**: De-emphasized text, disabled states

### "On" Color Usage

Always use the appropriate "on" color for text and icons:

```html
<!-- ✅ Correct: Use on-primary for text on primary background -->
<button class="bg-primary text-on-primary">Primary Button</button>

<!-- ✅ Correct: Use on-surface for text on surface background -->
<div class="bg-surface text-on-surface">Content</div>

<!-- ❌ Incorrect: Don't use generic text colors on colored backgrounds -->
<button class="bg-primary text-white">Primary Button</button>
```

## Implementation Patterns

### CSS Custom Properties

#### Adding New Colors
1. Define in `@theme` block:
```css
@theme {
  --color-new-color: #value;
  --color-on-new-color: #value;
}
```

2. Use in components:
```html
<div class="bg-new-color text-on-new-color">Content</div>
```

#### Dark Mode Overrides
```css
.dark {
  --color-new-color: #dark-value;
  --color-on-new-color: #dark-value;
}
```

### Tailwind Integration

#### Generated Classes
The `@theme` block automatically generates:
- `bg-primary`, `bg-secondary`, `bg-accent`, etc.
- `text-on-primary`, `text-on-secondary`, `text-on-accent`, etc.
- `border-primary`, `border-secondary`, etc.

#### Usage Examples
```html
<!-- Background colors -->
<div class="bg-primary">Primary background</div>
<div class="bg-surface">Surface background</div>

<!-- Text colors -->
<div class="text-on-primary">Text on primary</div>
<div class="text-on-surface">Text on surface</div>

<!-- Semantic colors -->
<div class="bg-success text-on-success">Success message</div>
<div class="bg-warning text-on-warning">Warning message</div>
```

## Component Theming Patterns

### Buttons
```html
<!-- Primary button -->
<button class="bg-primary text-on-primary hover:bg-primary/90">
  Primary Action
</button>

<!-- Secondary button -->
<button class="bg-secondary text-on-secondary hover:bg-secondary/90">
  Secondary Action
</button>

<!-- Surface button -->
<button class="bg-surface text-on-surface border border-neutral">
  Surface Action
</button>
```

### Cards
```html
<div class="bg-surface text-on-surface rounded-lg shadow-sm">
  <h3 class="text-lg font-semibold">Card Title</h3>
  <p class="text-muted">Card content</p>
</div>
```

### Alerts
```html
<!-- Success alert -->
<div class="bg-success text-on-success p-3 rounded-md">
  Operation completed successfully
</div>

<!-- Warning alert -->
<div class="bg-warning text-on-warning p-3 rounded-md">
  Please review your input
</div>

<!-- Error alert -->
<div class="bg-error text-on-error p-3 rounded-md">
  An error occurred
</div>
```

### Form Elements
```html
<!-- Input field -->
<input class="bg-surface text-on-surface border border-neutral rounded-md" />

<!-- Select dropdown -->
<select class="bg-surface text-on-surface border border-neutral rounded-md">
  <option>Select option</option>
</select>
```

## Accessibility Guidelines

### Contrast Ratios
- **Primary text**: Minimum 4.5:1 contrast ratio
- **Large text**: Minimum 3:1 contrast ratio
- **UI elements**: Minimum 3:1 contrast ratio

### Color Independence
- Never rely solely on color to convey information
- Use icons, text, or patterns in addition to color
- Ensure information is accessible in grayscale

### Focus States
- Always provide visible focus indicators
- Use consistent focus styling across components
- Ensure focus indicators meet contrast requirements

## Testing Guidelines

### Theme Switching
1. Test all components in light mode
2. Test all components in dark mode
3. Test theme switching transitions
4. Verify color contrast in both themes

### Component Testing
1. Test each component with all relevant color combinations
2. Verify "on" colors provide adequate contrast
3. Test hover, focus, and active states
4. Ensure semantic colors are used appropriately

### Accessibility Testing
1. Use color contrast checkers
2. Test with screen readers
3. Verify keyboard navigation
4. Test with color blindness simulators

## Future Enhancements

### Planned Additions
- **Interactive states**: Hover, focus, active colors
- **Border colors**: Dedicated border color tokens
- **Shadow colors**: Elevation-based shadow tokens
- **Overlay colors**: Modal and tooltip backgrounds
- **Disabled states**: Inactive element colors

### Advanced Features
- **Custom theme builder**: User-defined color schemes
- **Theme presets**: Pre-built theme collections
- **Animation themes**: Theme-aware animations
- **High contrast mode**: Accessibility-focused variant

## Implementation Checklist

### For New Components
- [ ] Use appropriate surface colors for backgrounds
- [ ] Apply correct "on" colors for text/icons
- [ ] Test in both light and dark themes
- [ ] Verify contrast ratios meet accessibility standards
- [ ] Use semantic colors for state feedback
- [ ] Ensure keyboard navigation works properly

### For Theme Updates
- [ ] Update CSS custom properties in `@theme` block
- [ ] Add dark mode overrides if needed
- [ ] Test generated Tailwind classes
- [ ] Update component examples
- [ ] Verify accessibility compliance
- [ ] Document new color usage patterns

## Resources

- [Material Design Color System](https://m2.material.io/design/color/the-color-system.html)
- [WCAG Color Contrast Guidelines](https://www.w3.org/WAI/WCAG21/quickref/#contrast-minimum)
- [Tailwind CSS v4 Documentation](https://tailwindcss.com/docs)
- [CSS Custom Properties Guide](https://developer.mozilla.org/en-US/docs/Web/CSS/Using_CSS_custom_properties) 