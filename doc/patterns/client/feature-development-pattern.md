# Feature Development Pattern

## Overview

This document describes the architectural pattern for developing independent features in the client while maintaining consistency with the existing platform architecture. Each feature is designed to be self-contained with its own test page, while leveraging shared platform services (DI, configuration, logging).

## Core Principles

1. **Independence**: Features should be self-contained and independently testable
2. **Platform Integration**: Leverage existing DI, configuration, and logging systems
3. **Testability**: Each feature has a dedicated test page for comprehensive testing
4. **Dashboard Integration**: Features are accessible via a unified dashboard
5. **Consistency**: Follow established patterns and conventions

## Directory Structure

### Feature Module Structure

Each feature follows this standardized structure within `src/client/lib/src/features/`:

```
src/client/lib/src/features/
├── auth/                    # Existing auth feature (reference)
├── ai-chat/                 # Example: AI chat feature
│   ├── api/
│   │   ├── aiChatApi.ts     # API client for the feature
│   │   └── types.ts         # API types and interfaces
│   ├── components/
│   │   ├── ChatInterface.tsx # Main feature component
│   │   ├── MessageList.tsx   # Sub-components
│   │   ├── ChatInput.tsx
│   │   └── ChatMessage.tsx
│   ├── stores/
│   │   └── aiChatStore.ts   # Feature state management
│   ├── views/
│   │   ├── AiChatPage.tsx   # Main feature page
│   │   └── AiChatTestPage.tsx # Comprehensive test page
│   ├── services/
│   │   └── AiChatService.ts # Feature business logic
│   └── di/
│       └── aiChatContainer.ts # DI registration
├── file-manager/            # Future file management feature
└── [other-features]/
```

### Key Directories Explained

- **`api/`**: API client code and type definitions
- **`components/`**: Reusable React components specific to the feature
- **`stores/`**: State management (Zustand stores)
- **`views/`**: Page-level components including test pages
- **`services/`**: Business logic and service layer
- **`di/`**: Dependency injection registration (optional)

## Feature Development Process

### 1. Create Feature Structure

Start by creating the directory structure for your feature:

```bash
mkdir -p src/client/lib/src/features/your-feature/{api,components,stores,views,services,di}
```

### 2. Define Feature API

Create API types and client:

```typescript
// src/client/lib/src/features/your-feature/api/types.ts
export interface YourFeatureRequest {
  // API request types
}

export interface YourFeatureResponse {
  // API response types
}

// src/client/lib/src/features/your-feature/api/yourFeatureApi.ts
export class YourFeatureApi {
  async performAction(request: YourFeatureRequest): Promise<YourFeatureResponse> {
    // API implementation
  }
}
```

### 3. Create Feature Service

Implement business logic with DI integration:

```typescript
// src/client/lib/src/features/your-feature/services/IYourFeatureService.ts
export interface IYourFeatureService {
  performAction(input: string): Promise<string>;
}

// src/client/lib/src/features/your-feature/services/YourFeatureService.ts
import { injectable, inject } from 'inversify';
import { TYPES } from '../../platform/di/types';
import type { IConfigurationService } from '../../platform/config/services/IConfigurationService';

@injectable()
export class YourFeatureService implements IYourFeatureService {
  constructor(
    @inject(TYPES.ConfigurationService) private configService: IConfigurationService
  ) {}

  async performAction(input: string): Promise<string> {
    // Feature business logic
    return `Processed: ${input}`;
  }
}
```

### 4. Register in DI Container

Add feature services to the dependency injection system:

```typescript
// src/client/lib/src/platform/di/types.ts
export const TYPES = {
  // Existing services
  ConfigurationService: Symbol.for('ConfigurationService'),
  
  // Feature services
  YourFeatureService: Symbol.for('YourFeatureService'),
} as const;

// src/client/lib/src/features/your-feature/di/yourFeatureContainer.ts
import { Container } from 'inversify';
import { TYPES } from '../../platform/di/types';
import { YourFeatureService } from '../services/YourFeatureService';

export function registerYourFeatureServices(container: Container): void {
  container.bind<IYourFeatureService>(TYPES.YourFeatureService)
    .to(YourFeatureService)
    .inSingletonScope();
}
```

### 5. Create Feature Components

Build React components for the feature:

```typescript
// src/client/lib/src/features/your-feature/components/YourFeatureInterface.tsx
import React, { useState } from 'react';
import { useContainer } from '../../platform/di/ContainerContext';
import { TYPES } from '../../platform/di/types';
import type { IYourFeatureService } from '../services/IYourFeatureService';

export const YourFeatureInterface: React.FC = () => {
  const container = useContainer();
  const service = container.get<IYourFeatureService>(TYPES.YourFeatureService);
  const [input, setInput] = useState('');
  const [result, setResult] = useState('');

  const handleSubmit = async () => {
    const response = await service.performAction(input);
    setResult(response);
  };

  return (
    <div className="your-feature-interface">
      <input 
        value={input} 
        onChange={(e) => setInput(e.target.value)}
        placeholder="Enter input..."
      />
      <button onClick={handleSubmit}>Process</button>
      {result && <div className="result">{result}</div>}
    </div>
  );
};
```

### 6. Create Test Page

Implement a comprehensive test page following the established pattern:

```typescript
// src/client/lib/src/features/your-feature/views/YourFeatureTestPage.tsx
import React from 'react';
import { YourFeatureInterface } from '../components/YourFeatureInterface';

const YourFeatureTestPage: React.FC = () => {
  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8">Your Feature Test Page</h1>
      
      {/* Feature-specific test controls */}
      <div className="mb-8">
        <h2 className="text-2xl font-semibold mb-4">Test Controls</h2>
        <div className="bg-gray-100 p-4 rounded">
          {/* Configuration controls, test buttons, etc. */}
          <button className="btn btn-primary mr-2">Test Action 1</button>
          <button className="btn btn-secondary mr-2">Test Action 2</button>
          <button className="btn btn-danger">Reset</button>
        </div>
      </div>
      
      {/* Main feature interface */}
      <div className="mb-8">
        <h2 className="text-2xl font-semibold mb-4">Feature Interface</h2>
        <div className="border rounded p-4">
          <YourFeatureInterface />
        </div>
      </div>
      
      {/* Debug/Development tools */}
      <div className="mb-8">
        <h2 className="text-2xl font-semibold mb-4">Debug Tools</h2>
        <div className="bg-gray-100 p-4 rounded">
          <h3 className="font-semibold mb-2">Configuration</h3>
          <pre className="text-sm bg-white p-2 rounded">
            {/* Display current configuration */}
          </pre>
          
          <h3 className="font-semibold mb-2 mt-4">Logs</h3>
          <div className="bg-white p-2 rounded text-sm">
            {/* Display feature logs */}
          </div>
        </div>
      </div>
    </div>
  );
};

export default YourFeatureTestPage;
```

### 7. Add Configuration Integration

Extend the platform configuration system:

```typescript
// src/client/lib/src/platform/config/types/core.ts
export interface CoreConfig {
  // Existing config
  api: ApiConfig;
  features: FeatureConfig;
  
  // Feature-specific config
  yourFeature?: YourFeatureConfig;
}

export interface YourFeatureConfig {
  enabled: boolean;
  apiEndpoint: string;
  timeout: number;
}

// src/client/lib/src/platform/config/defaults.ts
export const defaultCoreConfig: CoreConfig = {
  // Existing defaults
  api: { /* ... */ },
  features: { /* ... */ },
  
  // Feature defaults
  yourFeature: {
    enabled: true,
    apiEndpoint: 'https://api.example.com/your-feature',
    timeout: 30000,
  },
};
```

### 8. Update Routing

Add the test page to the routing system:

```typescript
// src/client/lib/src/routes/index.tsx
import YourFeatureTestPage from '../features/your-feature/views/YourFeatureTestPage';

const PlatformRoutes = () => (
  <Routes>
    <Route element={<Layout />}>
      <Route index element={<Dash />} />
      
      {/* Feature test pages */}
      <Route path="test/auth" element={<AuthViewsIndex />} />
      <Route path="test/your-feature" element={<YourFeatureTestPage />} />
      
      {/* Existing routes */}
      <Route path="signin" element={<Login />} />
      {/* ... other routes */}
    </Route>
  </Routes>
);
```

### 9. Create Dashboard Card

Add a feature card to the main dashboard by updating `Dash.tsx`:

```typescript
// src/client/lib/src/components/dashboard/FeatureCard.tsx
import React from 'react';
import { Link } from 'react-router-dom';

interface FeatureCardProps {
  title: string;
  description: string;
  status: 'active' | 'inactive' | 'error';
  testPagePath: string;
  miniPreview?: ReactNode;
  icon?: ReactNode;
}

export const FeatureCard: React.FC<FeatureCardProps> = ({
  title,
  description,
  status,
  testPagePath,
  miniPreview,
  icon
}) => {
  const statusColors = {
    active: 'bg-green-100 text-green-800',
    inactive: 'bg-gray-100 text-gray-800',
    error: 'bg-red-100 text-red-800'
  };

  return (
    <div className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center">
          {icon && <div className="mr-3">{icon}</div>}
          <h3 className="text-lg font-semibold">{title}</h3>
        </div>
        <span className={`px-2 py-1 rounded-full text-xs font-medium ${statusColors[status]}`}>
          {status}
        </span>
      </div>
      
      <p className="text-gray-600 mb-4">{description}</p>
      
      {miniPreview && (
        <div className="mb-4 p-3 bg-gray-50 rounded">
          {miniPreview}
        </div>
      )}
      
      <Link 
        to={testPagePath}
        className="inline-flex items-center text-blue-600 hover:text-blue-800"
      >
        Open Test Page →
      </Link>
    </div>
  );
};

// src/client/lib/src/Dash.tsx (updated)
import React from 'react';
import { Link } from 'react-router-dom';
import { isUserAuthenticated } from './hooks/AuthHooks';
import { FeatureCard } from './components/dashboard/FeatureCard';
import { YourFeatureInterface } from './features/your-feature/components/YourFeatureInterface';

export function Dash() {
  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">Dashboard</h1>
        <div>
          {isUserAuthenticated() ? (
            <Link to="/signout" className="text-blue-600 hover:text-blue-800">Sign out</Link>
          ) : (
            <Link to="/signin" className="text-blue-600 hover:text-blue-800">Sign in</Link>
          )}
        </div>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <FeatureCard
          title="Authentication"
          description="User authentication and account management"
          status="active"
          testPagePath="/test/auth"
        />
        
        <FeatureCard
          title="Your Feature"
          description="Description of your feature"
          status="active"
          testPagePath="/test/your-feature"
          miniPreview={<YourFeatureInterface />}
        />
        
        {/* Add more feature cards */}
      </div>
    </div>
  );
}
```

## Testing Strategy

### Test Page Requirements

Each feature test page should include:

1. **Test Controls**: Buttons and inputs for testing different scenarios
2. **Main Interface**: The actual feature interface for testing
3. **Debug Tools**: Configuration display, logs, state inspection
4. **Documentation**: Usage instructions and examples

### Testing Levels

1. **Unit Tests**: Test individual components and services
2. **Integration Tests**: Test feature with platform services
3. **Test Page**: Manual testing and debugging interface
4. **Dashboard Integration**: End-to-end feature workflow

## Configuration Integration

### Environment Variables

Add feature-specific environment variables:

```bash
# Your feature configuration
VITE_YOUR_FEATURE_ENABLED=true
VITE_YOUR_FEATURE_API_ENDPOINT=https://api.example.com/your-feature
VITE_YOUR_FEATURE_TIMEOUT=30000
```

### Configuration Provider

Extend the environment provider to handle feature config:

```typescript
// src/client/lib/src/platform/config/providers/EnvironmentProvider.ts
function parseEnvironmentConfig(env: Record<string, any> = (import.meta as any).env || {}) {
  const config: Record<string, any> = {};
  
  // Existing config parsing...
  
  // Your feature configuration
  if (env.VITE_YOUR_FEATURE_ENABLED !== undefined) {
    if (!config.yourFeature) config.yourFeature = {};
    config.yourFeature.enabled = env.VITE_YOUR_FEATURE_ENABLED === 'true';
  }
  
  if (env.VITE_YOUR_FEATURE_API_ENDPOINT) {
    if (!config.yourFeature) config.yourFeature = {};
    config.yourFeature.apiEndpoint = env.VITE_YOUR_FEATURE_API_ENDPOINT;
  }
  
  return config;
}
```

## Best Practices

### 1. Service Design
- Use dependency injection for all services
- Implement interfaces for testability
- Leverage platform configuration system
- Use platform logging for consistency

### 2. Component Design
- Keep components focused and reusable
- Use TypeScript for type safety
- Follow established naming conventions
- Implement proper error handling

### 3. Testing
- Create comprehensive test pages
- Include debug tools and logging
- Test edge cases and error scenarios
- Document testing procedures

### 4. Documentation
- Document feature purpose and usage
- Include configuration options
- Provide usage examples
- Maintain API documentation

## Example: AI Chat Feature

Here's a complete example of how the AI Chat feature would be implemented:

### Directory Structure
```
src/client/lib/src/features/ai-chat/
├── api/
│   ├── aiChatApi.ts
│   └── types.ts
├── components/
│   ├── ChatInterface.tsx
│   ├── MessageList.tsx
│   ├── ChatInput.tsx
│   └── ChatMessage.tsx
├── stores/
│   └── aiChatStore.ts
├── views/
│   ├── AiChatPage.tsx
│   └── AiChatTestPage.tsx
├── services/
│   └── AiChatService.ts
└── di/
    └── aiChatContainer.ts
```

### Key Implementation Points

1. **Service Registration**: Register `AiChatService` in DI container
2. **Configuration**: Add AI chat settings to platform config
3. **Test Page**: Comprehensive testing interface with chat history, settings, debug tools
4. **Dashboard Card**: Mini chat interface preview with status indicator
5. **Routing**: Add `/test/ai-chat` route for test page access

## Conclusion

This pattern provides a structured approach to feature development that:
- Maintains independence between features
- Leverages existing platform services
- Provides comprehensive testing capabilities
- Integrates seamlessly with the dashboard
- Follows established conventions and best practices

By following this pattern, new features can be developed independently while maintaining consistency with the overall architecture and providing excellent developer experience through dedicated test pages. 