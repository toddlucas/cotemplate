import { LOG_LEVELS } from './constants';

/**
 * Get debug configuration from environment variables
 * This function allows for better testing by accepting environment values
 */
function getLogConfig(env: Record<string, any> = (import.meta as any).env || {}) {
  return {
    CONFIG: parseInt(env.VITE_LOG_CONFIG || '0') || LOG_LEVELS.NONE,
    DI: parseInt(env.VITE_LOG_DI || '0') || LOG_LEVELS.NONE,
    EVENT: parseInt(env.VITE_LOG_EVENT || '0') || LOG_LEVELS.NONE,
    PLATFORM: parseInt(env.VITE_LOG_PLATFORM || '0') || LOG_LEVELS.NONE,
    AUTH: parseInt(env.VITE_LOG_AUTH || '0') || LOG_LEVELS.NONE,
  } as const;
}

/**
 * Environment-based debug configuration
 * These are build-time constants that enable tree-shaking
 */
export const LOG_CONFIG = getLogConfig();

// Export the function for testing
export { getLogConfig };
