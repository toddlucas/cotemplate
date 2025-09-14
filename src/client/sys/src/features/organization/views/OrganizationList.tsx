import { useEffect, useMemo } from 'react';
import { Link, useLocation } from 'react-router-dom';
import {
  useReactTable,
  getCoreRowModel,
  getSortedRowModel,
  flexRender,
  createColumnHelper,
  type OnChangeFn,
  type PaginationState,
  type SortingState,
  type Updater,
} from '@tanstack/react-table';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '$/components/ui/table';
import { Button } from '$/components/ui/button';
import TableFooter from '$/components/ui/tables/TableFooter';
import {
  useOrganizationStore,
  selectItems,
  selectIsLoadingList,
  selectListError,
  selectCurrentPage,
  selectPageSize,
  selectTotalPages,
  selectSorting,
  selectSearchTerm
} from '../store';
import type { OrganizationModel } from '$/models/organization-model';

const ActionColumn = ({ id }: { id: number }) => {
  return (
    <div className="flex items-center gap-2">
      <Button variant="ghost" size="icon" asChild>
        <Link
          title="View"
          to={`/organization/${id}`}
        >
          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
          </svg>
        </Link>
      </Button>
      <Button variant="ghost" size="icon" asChild>
        <Link
          title="Edit"
          to={`/organization/${id}/edit`}
        >
          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
          </svg>
        </Link>
      </Button>
      <Button variant="ghost" size="icon" asChild>
        <Link
          title="Delete"
          to={`/organization/${id}/delete`}
        >
          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
          </svg>
        </Link>
      </Button>
    </div>
  );
};

const StatusBadge = ({ status }: { status?: string }) => {
  const getStatusColor = (status?: string) => {
    switch (status?.toLowerCase()) {
      case 'active':
        return 'bg-green-100 text-green-800';
      case 'inactive':
        return 'bg-red-100 text-red-800';
      case 'pending':
        return 'bg-yellow-100 text-yellow-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  return (
    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getStatusColor(status)}`}>
      {status || 'Unknown'}
    </span>
  );
};

const columnHelper = createColumnHelper<OrganizationModel>();

const OrganizationList = () => {
  const location = useLocation();

  // Get state from store
  const organizations = useOrganizationStore(selectItems);
  const isLoading = useOrganizationStore(selectIsLoadingList);
  const error = useOrganizationStore(selectListError);
  const currentPage = useOrganizationStore(selectCurrentPage);
  const pageSize = useOrganizationStore(selectPageSize);
  const totalPages = useOrganizationStore(selectTotalPages);
  const sorting = useOrganizationStore(selectSorting);
  const searchTerm = useOrganizationStore(selectSearchTerm);
  void searchTerm; // Unused

  // Get actions from store
  const {
    fetchItems: fetchOrganizations,
    handleSearch,
    handleSortingChange,
    handlePageSizeChange,
    handlePageChange,
    shouldClearTableState,
    softResetTableState
  } = useOrganizationStore();

  useEffect(() => {
    const fromPath = location.state?.from;

    // Clear location.state, which gets set by the Link component, but gets cached.
    window.history.replaceState({}, '')

    if (shouldClearTableState(fromPath)) {
      softResetTableState();
    }

    // Fetch organizations (will use cached state if available)
    fetchOrganizations();
  }, [shouldClearTableState, softResetTableState, fetchOrganizations, location.state?.from]); // Only run on mount

  const setPagination: OnChangeFn<PaginationState> = (updaterOrValue: Updater<PaginationState>) => {
    if (typeof updaterOrValue === 'function') {
      // Handle function updater
      const newPagination = updaterOrValue({ pageIndex: currentPage, pageSize });
      handlePageChange(newPagination.pageIndex);
    } else {
      // Handle direct value
      handlePageChange(updaterOrValue.pageIndex);
    }
  }

  const handleSearchChange = (searchTerm: string) => {
    handleSearch(searchTerm);
  };

  const handleSortingChangeWrapper: OnChangeFn<SortingState> = (updaterOrValue: Updater<SortingState>) => {
    if (typeof updaterOrValue === 'function') {
      const newSorting = updaterOrValue(sorting);
      handleSortingChange(newSorting);
    } else {
      handleSortingChange(updaterOrValue);
    }
  };

  const handlePageSizeChangeWrapper = (newPageSize: number) => {
    handlePageSizeChange(newPageSize);
  };

  const columns = useMemo(
    () => [
      columnHelper.accessor('name', {
        id: 'name',
        header: 'Organization Name',
        cell: info => (
          <div className="font-medium">{info.getValue()}</div>
        ),
        enableSorting: true,
      }),
      columnHelper.accessor('code', {
        id: 'code',
        header: 'Code',
        cell: info => (
          <div className="text-sm font-mono bg-muted px-2 py-1 rounded">
            {info.getValue() || 'N/A'}
          </div>
        ),
        enableSorting: true,
      }),
      columnHelper.accessor('status', {
        id: 'status',
        header: 'Status',
        cell: info => (
          <StatusBadge status={info.getValue()} />
        ),
        enableSorting: true,
      }),
      columnHelper.accessor('parentOrgId', {
        id: 'parentOrgId',
        header: 'Parent Organization',
        cell: info => {
          const parentId = info.getValue();
          if (!parentId) return <span className="text-muted-foreground">Root Organization</span>;

          // Find parent organization name
          const parentOrg = organizations.find(org => org.id === parentId);
          return (
            <div className="text-sm">
              {parentOrg ? parentOrg.name : `ID: ${parentId}`}
            </div>
          );
        },
        enableSorting: false,
      }),
      columnHelper.accessor('metadata', {
        id: 'metadata',
        header: 'Description',
        cell: info => (
          <div className="text-sm text-muted-foreground max-w-xs truncate">
            {info.getValue() || 'No description'}
          </div>
        ),
        enableSorting: false,
      }),
      columnHelper.display({
        id: 'actions',
        header: 'Actions',
        cell: info => (
          <ActionColumn id={info.row.original.id} />
        ),
      }),
    ],
    [organizations]
  );

  /* Server-side pagination */
  const table = useReactTable({
    data: organizations,
    columns,
    // Tell TanStack this is manual pagination and sorting
    manualPagination: true,
    manualSorting: true,
    // Total count from API
    pageCount: totalPages,
    // Current page and sorting state
    state: {
      pagination: {
        pageIndex: currentPage,
        pageSize: pageSize,
      },
      sorting,
    },
    // Handle page and sorting changes
    onPaginationChange: setPagination,
    onSortingChange: handleSortingChangeWrapper,
    // Required even for manual pagination/sorting
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
  });

  return (
    <div className="min-h-screen bg-background">
      {/* Header */}
      <div className="bg-card shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="py-6">
            <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
              <div>
                <h1 className="text-2xl font-bold">Organizations</h1>
                <p className="mt-1 text-sm text-muted-foreground">
                  Manage organizations and their hierarchy
                </p>
              </div>
              <div className="flex items-center gap-4">
                <input
                  type="text"
                  placeholder="Search organizations..."
                  className="px-3 py-2 border border-input rounded-md bg-background text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:border-transparent"
                  onChange={(e) => handleSearchChange(e.target.value)}
                />
                <Button asChild>
                  <Link to="/organization/new">
                    <svg className="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                    </svg>
                    New Organization
                  </Link>
                </Button>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Loading State */}
        {isLoading && (
          <div className="flex items-center justify-center py-12">
            <div className="flex items-center space-x-3">
              <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-primary"></div>
              <span className="text-muted-foreground">Loading organizations...</span>
            </div>
          </div>
        )}

        {/* Error State */}
        {error && (
          <div className="bg-destructive/10 border border-destructive/20 rounded-lg p-4 mb-6">
            <div className="flex">
              <div className="flex-shrink-0">
                <svg className="h-5 w-5 text-destructive" viewBox="0 0 20 20" fill="currentColor">
                  <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                </svg>
              </div>
              <div className="ml-3">
                <h3 className="text-sm font-medium text-destructive">Error loading organizations</h3>
                <div className="mt-2 text-sm text-destructive">{error}</div>
              </div>
            </div>
          </div>
        )}

        {/* Organizations Table */}
        {organizations.length > 0 && !isLoading && (
          <div className="bg-card shadow-sm rounded-lg border overflow-hidden">
            <Table>
              <TableHeader>
                {table.getHeaderGroups().map(headerGroup => (
                  <TableRow key={headerGroup.id}>
                    {headerGroup.headers.map(header => (
                      <TableHead
                        key={header.id}
                        onClick={header.column.getToggleSortingHandler()}
                        className={header.column.getCanSort() ? 'cursor-pointer hover:bg-muted/50' : ''}
                      >
                        <div className="flex items-center space-x-1">
                          <span>
                            {flexRender(
                              header.column.columnDef.header,
                              header.getContext()
                            )}
                          </span>
                          {header.column.getCanSort() && (
                            <span className="text-muted-foreground">
                              {{
                                asc: '↑',
                                desc: '↓',
                              }[header.column.getIsSorted() as string] ?? '↕'}
                            </span>
                          )}
                        </div>
                      </TableHead>
                    ))}
                  </TableRow>
                ))}
              </TableHeader>
              <TableBody>
                {table.getRowModel().rows.map((row) => (
                  <TableRow key={row.id}>
                    {row.getVisibleCells().map(cell => (
                      <TableCell key={cell.id}>
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
              </TableBody>
            </Table>

            {/* Table Footer with Pagination */}
            <div className="bg-muted/50 px-6 py-3 border-t">
              <TableFooter
                currentPage={currentPage}
                totalPages={totalPages}
                pageSize={pageSize}
                onPageChange={handlePageChange}
                onPageSizeChange={handlePageSizeChangeWrapper}
                showPageInfo={true}
                adjacentPages={2}
                pageSizeOptions={[10, 20, 30, 40, 50]}
              />
            </div>
          </div>
        )}

        {/* Empty State */}
        {organizations.length === 0 && !isLoading && !error && (
          <div className="text-center py-12">
            <svg className="mx-auto h-12 w-12 text-muted-foreground" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
            </svg>
            <h3 className="mt-2 text-sm font-medium">No organizations found</h3>
            <p className="mt-1 text-sm text-muted-foreground">
              Get started by creating a new organization.
            </p>
            <div className="mt-6">
              <Button asChild>
                <Link to="/organization/new">
                  <svg className="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                  </svg>
                  New Organization
                </Link>
              </Button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default OrganizationList;
