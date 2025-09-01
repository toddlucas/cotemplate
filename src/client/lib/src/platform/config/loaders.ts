import { MainConfigurationService } from './services/MainConfigurationService';
import { MemoryProvider, WebProvider, EnvironmentProvider } from './providers';
import type { WebAppConfig } from './types';
import type { ConfigurationService } from './services/ConfigurationService';

/**
 * Load configuration service for web application
 */
export async function loadWebConfigService(): Promise<ConfigurationService> {
  const providers = [
    new EnvironmentProvider(), // VITE_* variables
    new WebProvider(), // localStorage
    new MemoryProvider(), // Defaults
  ];

  const configService = new MainConfigurationService(providers);
  await configService.load();

  return configService;
}

/**
 * Load configuration for web application
 */
export async function loadWebConfig(): Promise<WebAppConfig> {
  const configService = await loadWebConfigService();

  return configService.getConfig() as WebAppConfig;
}
