/**
 * Standardized API error class for consistent error handling across all API clients
 */
export class ApiError extends Error {
  public readonly status: number;
  public readonly statusText: string;
  public readonly url?: string;
  public readonly response?: Response;

  // ProblemDetails properties (RFC 7807) - set when available in response
  public readonly problemType?: string;
  public readonly title?: string;
  public readonly detail?: string;
  public readonly instance?: string;

  constructor(
    message: string,
    status: number,
    statusText: string = '',
    url?: string,
    response?: Response,
    problemType?: string,
    title?: string,
    detail?: string,
    instance?: string
  ) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.statusText = statusText;
    this.url = url;
    this.response = response;
    this.problemType = problemType;
    this.title = title;
    this.detail = detail;
    this.instance = instance;
  }

    /**
   * Factory method to create ApiError from Response
   */
  static async fromResponse(response: Response, customMessage?: string): Promise<ApiError> {
    const url = response.url;
    const status = response.status;
    const statusText = response.statusText;

    let message = customMessage || `HTTP ${status}: ${statusText}`;
    let problemType: string | undefined;
    let title: string | undefined;
    let detail: string | undefined;
    let instance: string | undefined;

    // Try to extract error details from response body
    try {
      const body = await response.clone().text();
      if (body) {
        // Try to parse as JSON for more detailed error info
        try {
          const json = JSON.parse(body);

          // Extract ProblemDetails fields if present (RFC 7807)
          problemType = json.type;
          title = json.title;
          detail = json.detail;
          instance = json.instance;

          // Message priority: custom > detail > title > message > statusText
          if (!customMessage) {
            message = detail || title || json.message || `HTTP ${status}: ${statusText}`;
          }
        } catch {
          // If not JSON, use the text body if it's short enough
          if (body.length < 200) {
            message = body;
          }
        }
      }
    } catch {
      // Ignore errors when trying to read response body
    }

    return new ApiError(message, status, statusText, url, response, problemType, title, detail, instance);
  }

  // Helper function to extract meaningful error message from various error types
  static extractErrorMessage(error: unknown): string {
    if (error instanceof ApiError) {
      // Use problem details if available, otherwise fall back to the error message
      if (error.isProblemDetails && error.detail) {
        return error.detail;
      } else if (error.isProblemDetails && error.title) {
        return error.title;
      } else {
        return error.message;
      }
    } else if (error instanceof Error) {
      return error.message;
    }
    return 'An unknown error occurred';
  }

  /**
   * Check if this is a specific HTTP status error
   */
  isStatus(status: number): boolean {
    return this.status === status;
  }

  /**
   * Check if this is a not found error
   */
  isNotFound(): boolean {
    return this.status === 404;
  }

  /**
   * Check if this is an unauthorized error
   */
  isUnauthorized(): boolean {
    return this.status === 401;
  }

  /**
   * Check if this is a forbidden error
   */
  isForbidden(): boolean {
    return this.status === 403;
  }

  /**
   * Check if this is a client error (4xx)
   */
  isClientError(): boolean {
    return this.status >= 400 && this.status < 500;
  }

  /**
   * Check if this is a server error (5xx)
   */
  isServerError(): boolean {
    return this.status >= 500 && this.status < 600;
  }

  /**
   * Check if this error contains RFC 7807 Problem Details information
   */
  get isProblemDetails(): boolean {
    return this.problemType !== undefined || this.title !== undefined || this.detail !== undefined;
  }
}
