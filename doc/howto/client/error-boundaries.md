# Error Boundaries

A comprehensive guide to using error boundaries in the CorpOS React application.

## Overview

Error boundaries are React components that catch JavaScript errors anywhere in the component tree and display a fallback UI instead of crashing the entire application. The CorpOS application includes a robust error boundary system with multiple usage patterns.

## Features

- **Automatic Error Catching**: Catches errors in child components
- **Customizable Fallback UI**: Support for custom error displays
- **Error Recovery**: Built-in reset and reload functionality
- **Error Reporting**: Optional error callback for logging
- **Development Support**: Shows error details in development mode
- **Multiple Usage Patterns**: Class component, hook, and HOC support

## Basic Usage

```tsx
import { ErrorBoundary } from '../components';

function App() {
  return (
    <ErrorBoundary>
      <YourComponent />
    </ErrorBoundary>
  );
}
```

## Advanced Usage

### Custom Fallback UI

```tsx
<ErrorBoundary
  fallback={(error, errorInfo) => (
    <div className="custom-error">
      <h2>Something went wrong</h2>
      <p>{error.message}</p>
    </div>
  )}
>
  <YourComponent />
</ErrorBoundary>
```

### Error Callback

```tsx
<ErrorBoundary
  onError={(error, errorInfo) => {
    // Log to external service
    console.error('Error caught:', error, errorInfo);
  }}
>
  <YourComponent />
</ErrorBoundary>
```

### Reset Key for Recovery

```tsx
const [key, setKey] = useState(0);

<ErrorBoundary resetKey={key}>
  <YourComponent />
</ErrorBoundary>

// Reset error boundary
<button onClick={() => setKey(k => k + 1)}>
  Reset
</button>
```

### Show Error Details

```tsx
<ErrorBoundary showDetails={true}>
  <YourComponent />
</ErrorBoundary>
```

## Hook Usage

For functional components that need error handling:

```tsx
import { useErrorHandler } from '../components';

function MyComponent() {
  const { error, handleError, clearError } = useErrorHandler();

  const riskyOperation = () => {
    try {
      // Some operation that might fail
    } catch (err) {
      handleError(err as Error);
    }
  };

  if (error) {
    return (
      <div>
        <p>Error: {error.message}</p>
        <button onClick={clearError}>Clear Error</button>
      </div>
    );
  }

  return <div>Component content</div>;
}
```

## Higher-Order Component

Wrap components with error boundaries:

```tsx
import { withErrorBoundary } from '../components';

const SafeComponent = withErrorBoundary(YourComponent, {
  showDetails: true,
  onError: (error, errorInfo) => {
    console.log('Error caught by HOC:', error);
  }
});
```

## Props Reference

| Prop | Type | Description |
|------|------|-------------|
| `children` | ReactNode | Components to wrap with error boundary |
| `fallback` | ReactNode \| Function | Custom error UI or function that returns error UI |
| `onError` | Function | Callback when error is caught |
| `resetKey` | any | Key to reset error boundary state |
| `showDetails` | boolean | Show error details in development |

## Best Practices

1. **Wrap at Route Level**: Place error boundaries around route components
2. **Granular Boundaries**: Use multiple boundaries for different sections
3. **Error Logging**: Implement `onError` for production error tracking
4. **User Experience**: Provide clear recovery options in fallback UI
5. **Testing**: Test error scenarios to ensure boundaries work correctly

## Implementation Examples

### Route-Level Error Boundary

```tsx
// In your router configuration
<Route 
  path="/dashboard" 
  element={
    <ErrorBoundary
      onError={(error) => {
        // Log dashboard errors specifically
        analytics.track('dashboard_error', { error: error.message });
      }}
    >
      <Dashboard />
    </ErrorBoundary>
  } 
/>
```

### Feature-Level Error Boundary

```tsx
function UserProfile() {
  return (
    <ErrorBoundary
      fallback={(error) => (
        <div className="profile-error">
          <h3>Profile Loading Failed</h3>
          <p>Unable to load user profile: {error.message}</p>
          <button onClick={() => window.location.reload()}>
            Reload Page
          </button>
        </div>
      )}
    >
      <ProfileContent />
    </ErrorBoundary>
  );
}
```

### Component with Error Handler Hook

```tsx
function DataTable() {
  const { error, handleError, clearError } = useErrorHandler();
  const [data, setData] = useState([]);

  const fetchData = async () => {
    try {
      const response = await api.getData();
      setData(response.data);
    } catch (err) {
      handleError(err as Error);
    }
  };

  if (error) {
    return (
      <div className="data-error">
        <p>Failed to load data: {error.message}</p>
        <button onClick={clearError}>Try Again</button>
      </div>
    );
  }

  return (
    <table>
      {/* table content */}
    </table>
  );
}
```

## Error Boundary Hierarchy

Consider the following hierarchy for optimal error handling:

```
App (Global Error Boundary)
├── Router
│   ├── Route 1 (Route-level Error Boundary)
│   │   ├── Feature 1 (Feature-level Error Boundary)
│   │   └── Feature 2 (Feature-level Error Boundary)
│   └── Route 2 (Route-level Error Boundary)
│       └── Feature 3 (Feature-level Error Boundary)
└── Global Components
```

## Testing Error Boundaries

### Simulating Errors

```tsx
// Component that throws an error for testing
function BuggyComponent({ shouldThrow }: { shouldThrow: boolean }) {
  if (shouldThrow) {
    throw new Error('Test error for error boundary');
  }
  return <div>Working correctly</div>;
}

// Test the error boundary
<ErrorBoundary>
  <BuggyComponent shouldThrow={true} />
</ErrorBoundary>
```

### Testing Error Recovery

```tsx
const [key, setKey] = useState(0);

<ErrorBoundary resetKey={key}>
  <BuggyComponent shouldThrow={key % 2 === 1} />
</ErrorBoundary>

<button onClick={() => setKey(k => k + 1)}>
  Toggle Error State
</button>
```

## Troubleshooting

### Common Issues

1. **Error boundary not catching errors**: Ensure the error boundary is a parent of the component throwing the error
2. **Fallback UI not showing**: Check that the error is actually being thrown (not caught by try-catch)
3. **Reset not working**: Verify the `resetKey` prop is changing when you expect it to

### Debug Mode

Enable detailed error information in development:

```tsx
<ErrorBoundary 
  showDetails={true}
  onError={(error, errorInfo) => {
    console.log('Error details:', error, errorInfo);
  }}
>
  <YourComponent />
</ErrorBoundary>
```

## Related Components

- `ErrorBoundary` - Main error boundary component
- `useErrorHandler` - Hook for functional components
- `withErrorBoundary` - HOC for wrapping components
- `ErrorBoundaryExample` - Comprehensive usage examples

## See Also

- [React Error Boundaries Documentation](https://react.dev/reference/react/Component#catching-rendering-errors-with-an-error-boundary)
- [Error Boundary Best Practices](https://react.dev/learn/keeping-components-pure#where-you-can-cause-side-effects)
