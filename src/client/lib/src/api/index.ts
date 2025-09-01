import { authManager } from '../services/auth/authManager';

export { ApiError } from './ApiError';

const APPLICATION_JSON = 'application/json';

let schemeHost: string;
let accessToken: string;

export const setSchemeHost = (host: string) => schemeHost = host;
export const setAccessToken = (token: string) => accessToken = token;

export const apiUrl = (path: string) => buildPath(schemeHost, path);

export const get = (input: RequestInfo | URL, signal?: AbortSignal) =>
  fetchJson(input, 'GET', signal);

export const post = <T>(input: RequestInfo | URL, body: T, signal?: AbortSignal) =>
  fetchJsonBody(input, 'POST', body, signal);

export const put = <T>(input: RequestInfo | URL, body: T, signal?: AbortSignal) =>
  fetchJsonBody(input, 'PUT', body, signal);

export const patch = <T>(input: RequestInfo | URL, body: T, signal?: AbortSignal) =>
  fetchJsonBody(input, 'PATCH', body, signal);

export const del = (input: RequestInfo | URL, signal?: AbortSignal) =>
  fetchJson(input, 'DELETE', signal);

export const postMultipart = (input: RequestInfo | URL, formData: FormData, signal?: AbortSignal) =>
  fetchMultipart(input, 'POST', formData, signal);

export const putMultipart = (input: RequestInfo | URL, formData: FormData, signal?: AbortSignal) =>
  fetchMultipart(input, 'PUT', formData, signal);

const fetchJson = (input: RequestInfo | URL, method: string, signal?: AbortSignal) => {
  const headers: HeadersInit = {
    Accept: APPLICATION_JSON,
  };

  const init: RequestInit = {
    headers,
    method,
    signal,
  };

  return fetchAuth(input, init);
};

const fetchJsonBody = <T>(input: RequestInfo | URL, method: string, body: T, signal?: AbortSignal) => {
  const headers: HeadersInit = {
    Accept: APPLICATION_JSON,
    'Content-Type': APPLICATION_JSON
  };

  const init: RequestInit = {
    headers,
    method,
    body: JSON.stringify(body),
    signal,
  };

  return fetchAuth(input, init);
};

const fetchMultipart = (input: RequestInfo | URL, method: string, formData: FormData, signal?: AbortSignal) => {
  const headers: HeadersInit = {
    Accept: APPLICATION_JSON,
    // Don't set Content-Type for FormData - browser will set it with boundary
  };

  const init: RequestInit = {
    headers,
    method,
    body: formData,
    signal,
  };

  return fetchAuth(input, init);
};

export const fetchAuth = async (input: RequestInfo | URL, init?: RequestInit) => {
  init ||= {};
  init.headers ||= {};

  setAuthorizationHeader(init.headers as Record<string, string>);

  const response = await fetch(resolveUrl(input), init);

  // Global 401 error handling
  if (response.status === 401) {
    authManager.triggerLogout();
  }

  return response;
};

const setAuthorizationHeader = (headers: Record<string, string>) => {
  if (accessToken) {
    headers['Authorization'] = 'Bearer ' + accessToken;
  }
};

const resolveUrl = (input: RequestInfo | URL) => {
  // We go against the host itself with absolute paths.
  if (typeof input === 'string') {
    if (input.startsWith('/')) {
      input = apiUrl(input);
    }
  }

  return input;
};

// https://stackoverflow.com/questions/29855098/is-there-a-built-in-javascript-function-similar-to-os-path-join/46427607#46427607
const buildPath = (...args: string[]) => {
  return args
    .map((part, i) => {
      if (!part) {
        throw new Error("Path segment isn't defined.");
      }

      if (i === 0) {
        return part.trim().replace(/[\/]*$/g, '');
      } else {
        return part.trim().replace(/(^[\/]*|[\/]*$)/g, '');
      }
    })
    .filter(x => x.length)
    .join('/');
};
