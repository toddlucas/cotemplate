import type { CoreConfig, WebAppConfig } from './types';

export const defaultCoreConfig: CoreConfig = {
  api: {
    baseUrl: 'http://localhost:8181',
    timeout: 30000,
    retryAttempts: 3,
  },
  features: {
    enableAdvancedFeatures: false,
    enableDebugMode: false,
  },
  ui: {
    theme: 'system',
    language: 'en',
    animations: true,
  },
};

export const defaultWebConfig: WebAppConfig = {
  ...defaultCoreConfig,
  web: {
    storage: {
      localStoragePrefix: 'corpos_',
      sessionStoragePrefix: 'corpos_session_',
    },
    browser: {
      enableNotifications: true,
      enableServiceWorker: false,
    },
    development: {
      enableHotReload: true,
      enableSourceMaps: true,
    },
  },
};
