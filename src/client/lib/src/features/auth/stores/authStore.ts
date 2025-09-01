import { create } from 'zustand';
import { setAccessToken } from '../../../api';
import * as AuthApi from '../api/authApi';
import { authManager } from '../../../services/auth/authManager';
import {
  logout,
  logoutSession,
  logoutUser,
  getCurrentToken,
  getLoggedInUser,
  getStoredTokenExpiry,
  validateCurrentToken
} from '../../../services/auth/authService';
import type { IdentityUserModel, ProblemDetailsModel, ValidationProblemDetailsModel } from '../../../models';
import type { AccessTokenResponse, LoginRequest, RegisterRequest } from '../../../models/auth';
import type { EventBus } from '../../../platform/events/EventBus';
import { createUserSignedInEvent, createUserSignedOutEvent, createUserSessionRestoredEvent } from '../../../platform/events/types/auth';
import { LOG_CONFIG, LOG_LEVELS } from '../../../platform/logging';

// Login state
interface LoginState {
  status: 'idle' | 'loading' | 'failed';
  tokenResponse: AccessTokenResponse | null;
  error: string | null;
  problem: ProblemDetailsModel | null;
  twoFactorCredentials: { email: string; password: string } | null;
}

// Register state
interface RegisterState {
  status: 'idle' | 'loading' | 'failed';
  success: boolean;
  problem: ValidationProblemDetailsModel | null;
}

// User state
interface UserState {
  status: 'idle' | 'loading' | 'failed';
  userModel: IdentityUserModel<string> | undefined;
  success: boolean;
  problem: ValidationProblemDetailsModel | undefined;
}

// Combined auth store interface
interface AuthStore {
  // Login state
  login: LoginState;

  // Register state
  register: RegisterState;

  // User state
  user: UserState;

  // Login actions
  postLogin: (model: LoginRequest) => Promise<void>;
  resetLoginFlags: () => void;
  logoutSession: () => void;
  setTwoFactorCredentials: (credentials: { email: string; password: string } | null) => void;

  // Register actions
  postRegister: (model: RegisterRequest) => Promise<void>;
  resetRegisterFlags: () => void;

  // User actions
  getUser: () => Promise<void>;
  putUser: (model: IdentityUserModel<string>) => Promise<void>;
  logoutUser: () => void;
  resetUserUpdateFlags: () => void;

  // Global logout (handles both session and user logout)
  logout: () => void;

  // Initialization and timer management
  initialize: (eventBus: EventBus) => void;
  initializeFromStorage: () => void;
  startTokenValidationTimer: () => void;
  stopTokenValidationTimer: () => void;
}

const initialLoginState: LoginState = {
  status: 'idle',
  tokenResponse: null,
  error: null,
  problem: null,
  twoFactorCredentials: null,
};

const initialRegisterState: RegisterState = {
  status: 'idle',
  success: false,
  problem: null,
};

const initialUserState: UserState = {
  status: 'idle',
  userModel: undefined,
  success: false,
  problem: undefined,
};

// ============================================================================
// Event Bus Reference
// ============================================================================

let eventBusRef: EventBus | null = null;

// ============================================================================
// Auth Store
// ============================================================================

export const useAuthStore = create<AuthStore>((set: (partial: Partial<AuthStore> | ((state: AuthStore) => Partial<AuthStore>)) => void, get: () => AuthStore) => {
  // Timer for token validation
  let tokenValidationTimer: NodeJS.Timeout | null = null;

  // Helper function to handle 401 errors - now uses centralized handler
  const handle401Error = () => {
    // Trigger the global 401 handler which will call our registered logout function
    authManager.triggerLogout();
  };

  // Helper function to emit auth events
  const emitAuthEvent = async (event: any) => {
    try {
      if (eventBusRef) {
        await eventBusRef.emit(event);
      }
    } catch (error) {
      if (LOG_CONFIG.AUTH >= LOG_LEVELS.ERROR) console.warn('Failed to emit auth event:', error);
    }
  };

  // Register the logout function with the auth manager
  const logoutFunction = () => {
    const state = get();
    state.logout();
  };

  // Register the logout callback when the store is created
  authManager.registerLogoutCallback(logoutFunction);

  return {
    // Initial state
    login: initialLoginState,
    register: initialRegisterState,
    user: initialUserState,

    // Initialize the auth store with event bus
    initialize: (eventBus: EventBus) => {
      eventBusRef = eventBus;
    },

    // Login actions
    postLogin: async (model: LoginRequest) => {
      set((state) => ({
        login: { ...state.login, status: 'loading' }
      }));

      try {
        const response = await AuthApi.postLogin(model);

        if (response.ok) {
          const tokenResponse = (await response.json()) as AccessTokenResponse;

          sessionStorage.setItem("authAccessToken", tokenResponse.accessToken);
          sessionStorage.setItem("authRefreshToken", tokenResponse.refreshToken);
          // Store absolute expiry time
          const expiresAt = Date.now() + (tokenResponse.expiresIn * 1000);
          sessionStorage.setItem("authTokenExpiry", expiresAt.toString());

          setAccessToken(tokenResponse.accessToken);

          set((state) => ({
            login: {
              ...state.login,
              status: 'idle',
              tokenResponse,
              error: null,
              problem: null,
            }
          }));

          // Get user and emit signed in event
          get().getUser().then(() => {
            const user = get().user.userModel;
            if (user) {
              emitAuthEvent(createUserSignedInEvent(user));
            }
          });
              } else {
          if (response.status === 401) {
            handle401Error();
            return;
          }

          const error = (await response.json()) as ProblemDetailsModel;
          const errorMessage = error.title ?? "Failed to sign in";

          set((state) => ({
            login: {
              ...state.login,
              status: 'failed',
              problem: error,
              error: errorMessage,
            }
          }));
        }
      } catch (_error) {
        set((state) => ({
          login: {
            ...state.login,
            status: 'failed',
            error: 'Network error occurred',
          }
        }));
      }
    },

    resetLoginFlags: () => {
      set((state) => ({
        login: {
          ...state.login,
          status: 'idle',
          error: null,
        }
      }));
    },

    logoutSession: () => {
      logoutSession();

      set((_state) => ({
        login: {
          ...initialLoginState,
        }
      }));
    },

    setTwoFactorCredentials: (credentials) => {
      set((state) => ({
        login: {
          ...state.login,
          twoFactorCredentials: credentials,
        }
      }));
    },

    // Register actions
    postRegister: async (model: RegisterRequest) => {
      set((state) => ({
        register: { ...state.register, status: 'loading' }
      }));

      try {
        const response = await AuthApi.postRegister(model);

        if (response.ok) {
          set((state) => ({
            register: {
              ...state.register,
              status: 'idle',
              success: true,
              problem: null,
            }
                  }));
        } else {
          if (response.status === 401) {
            handle401Error();
            return;
          }

          const problem = (await response.json()) as ValidationProblemDetailsModel;

          set((state) => ({
            register: {
              ...state.register,
              status: 'failed',
              problem,
            }
          }));
        }
      } catch (_error) {
        set((state) => ({
          register: {
            ...state.register,
            status: 'failed',
            problem: {
              title: 'Network error occurred',
              status: 500,
              errors: {},
            } as ValidationProblemDetailsModel,
          }
        }));
      }
    },

    resetRegisterFlags: () => {
      set((state) => ({
        register: {
          ...state.register,
          success: false,
          problem: null,
        }
      }));
    },

    // User actions
    getUser: async () => {
      set((state) => ({
        user: { ...state.user, status: 'loading' }
      }));

      try {
        const response = await AuthApi.getUser();

        if (response.ok) {
          const userModel = (await response.json()) as IdentityUserModel<string>;

          sessionStorage.setItem("authUserModel", JSON.stringify(userModel));

          set((state) => ({
            user: {
              ...state.user,
              status: 'idle',
              userModel,
            }
          }));
        } else {
          if (response.status === 401) {
            handle401Error();
            return;
          }

          set((state) => ({
            user: {
              ...state.user,
              status: 'failed',
            }
          }));
        }
      } catch (_error) {
        set((state) => ({
          user: {
            ...state.user,
            status: 'failed',
          }
        }));
      }
    },

    putUser: async (model: IdentityUserModel<string>) => {
      set((state) => ({
        user: { ...state.user, status: 'loading' }
      }));

      try {
        const response = await AuthApi.putUser(model);

        if (response.ok) {
          const userModel = (await response.json()) as IdentityUserModel<string>;

          sessionStorage.setItem("authUserModel", JSON.stringify(userModel));

          set((state) => ({
            user: {
              ...state.user,
              status: 'idle',
              userModel,
              success: true,
            }
                  }));
        } else {
          if (response.status === 401) {
            handle401Error();
            return;
          }

          const problem = (await response.json()) as ValidationProblemDetailsModel;

          set((state) => ({
            user: {
              ...state.user,
              status: 'failed',
              problem,
            }
          }));
        }
      } catch (_error) {
        set((state) => ({
          user: {
            ...state.user,
            status: 'failed',
            problem: {
              title: 'Network error occurred',
              status: 500,
              errors: {},
            } as ValidationProblemDetailsModel,
          }
        }));
      }
    },

    logoutUser: () => {
      logoutUser();

      set((state) => ({
        user: {
          ...state.user,
          status: 'idle',
          userModel: undefined,
        }
      }));
    },

    resetUserUpdateFlags: () => {
      set((state) => ({
        user: {
          ...state.user,
          success: false,
          problem: undefined,
        }
      }));
    },

    // Global logout function that handles both session and user logout
    logout: () => {
      // Get user before clearing state
      const currentUser = get().user.userModel;

      // Use the consolidated logout function
      logout();

      // Reset all auth state
      set({
        login: initialLoginState,
        register: initialRegisterState,
        user: initialUserState,
      });

      // Emit user signed out event
      if (currentUser) {
        emitAuthEvent(createUserSignedOutEvent(currentUser.id));
      }
    },

    // Initialize auth store from session storage
    initializeFromStorage: () => {
      const token = getCurrentToken();
      const user = getLoggedInUser();
      const expiry = getStoredTokenExpiry();

      if (token && user && expiry && Date.now() < expiry) {
        // Valid session exists - set up the store
        setAccessToken(token);
        set((state) => ({
          user: {
            ...state.user,
            userModel: user,
            status: 'idle',
          }
        }));

        // Start token validation timer
        get().startTokenValidationTimer();

        // Emit user session restored event
        emitAuthEvent(createUserSessionRestoredEvent(user));
      } else {
        // Clear any invalid session data
        logout();
      }
    },

    // Start periodic token validation
    startTokenValidationTimer: () => {
      // Clear any existing timer
      if (tokenValidationTimer) {
        clearInterval(tokenValidationTimer);
      }

      // Start new timer - check every 30 seconds
      tokenValidationTimer = setInterval(() => {
        const isValid = validateCurrentToken();
        if (!isValid) {
          // Token is invalid/expired - logout will be triggered by validateCurrentToken
          get().stopTokenValidationTimer();
        }
      }, 30000);
    },

    // Stop token validation timer
    stopTokenValidationTimer: () => {
      if (tokenValidationTimer) {
        clearInterval(tokenValidationTimer);
        tokenValidationTimer = null;
      }
    },
  };
});
