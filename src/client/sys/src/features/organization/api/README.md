# Organization API

This directory contains the API layer for the organization feature.

## Files

- `organizationApi.ts` - API functions for CRUD operations on organizations

## Usage

The organization store uses these API functions through a mock layer that can be easily switched to real API calls.

### Switching from Mock to Real API

To switch from mock data to real API calls:

1. Open `src/client/sys/src/features/organization/store/organizationStore.ts`
2. Change `const useMockData = true;` to `const useMockData = false;`

### API Endpoints

The following endpoints are expected to be implemented on the server:

- `GET /api/organization/list` - List organizations with pagination, search, and sorting
- `GET /api/organization/{id}` - Get organization details
- `POST /api/organization` - Create a new organization
- `PUT /api/organization` - Update an existing organization
- `DELETE /api/organization/{id}` - Delete an organization

### Pagination Support

The client-side pagination is already implemented and ready. When the server endpoints are ready, they should support:

- `take` - Number of items per page
- `skip` - Number of items to skip (for pagination)
- `search` - Search term for filtering
- `column` - Column to sort by
- `direction` - Sort direction ('asc' or 'desc')

### Error Handling

All API functions include proper error handling and will throw meaningful error messages that are displayed to the user.
