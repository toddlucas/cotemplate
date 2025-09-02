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
import ListSearch from '../../../components/ListSearch';
import TableFooter from '../../../components/TableFooter';
import {
  useUserStore,
  selectItems,
  selectIsLoadingList,
  selectListError,
  selectCurrentPage,
  selectPageSize,
  selectTotalPages,
  selectSorting,
  selectSearchTerm
} from '../store';
import './user.module.css';
import type { IdentityUserModel } from '$/models/identity-user-model';

const ActionColumn = ({ id }: { id: string }) => {
  return (
    <div className="actions">
      <Link className="text-decoration-none me-2 fa fa-file-alt" title="View" to={`/identity/user/${id}`}></Link>
      <a className="text-decoration-none me-2 fa fa-pencil" title="Edit" href={`/identity/user/${id}/edit`}></a>
    </div>
  );
}

const columnHelper = createColumnHelper<IdentityUserModel<string>>();

const UserList = () => {
  const location = useLocation();

  // Get state from store
  const users = useUserStore(selectItems);
  const isLoading = useUserStore(selectIsLoadingList);
  const error = useUserStore(selectListError);
  const currentPage = useUserStore(selectCurrentPage);
  const pageSize = useUserStore(selectPageSize);
  const totalPages = useUserStore(selectTotalPages);
  const sorting = useUserStore(selectSorting);
  const searchTerm = useUserStore(selectSearchTerm);
  void searchTerm; // Unused

  // Get actions from store
  const {
    fetchItems: fetchUsers,
    handleSearch,
    handleSortingChange,
    handlePageSizeChange,
    handlePageChange,
    shouldClearTableState,
    softResetTableState
  } = useUserStore();

  useEffect(() => {
    const fromPath = location.state?.from;

    // Clear location.state, which gets set by the Link component, but gets cached.
    window.history.replaceState({}, '')

    if (shouldClearTableState(fromPath)) {
      softResetTableState();
    }

    // Fetch users (will use cached state if available)
    fetchUsers();
  }, []); // Only run on mount

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
      columnHelper.accessor('userName', {
        id: 'name',
        header: 'User Name',
        cell: info => (
          <div>{info.getValue()}</div>
        ),
        enableSorting: true,
      }),
      columnHelper.accessor('id', {
        id: 'id',
        header: 'ID',
        cell: info => (
          <div>
            <small className="text-muted">{info.getValue()}</small>
          </div>
        ),
        enableSorting: true,
      }),
      columnHelper.accessor('email', {
        id: 'email',
        header: 'Email',
        cell: info => (
          <div>
            <small className="text-muted">{info.getValue()}</small>
          </div>
        ),
        enableSorting: true,
      }),
      // columnHelper.accessor('isActive', {
      //   id: 'is_active',
      //   header: 'Status',
      //   cell: info => (
      //     <span className={`badge ${info.getValue() ? 'bg-success' : 'bg-secondary'}`}>
      //       {info.getValue() ? 'Active' : 'Inactive'}
      //     </span>
      //   ),
      //   enableSorting: true,
      // }),
      // columnHelper.accessor('information.createdAt', {
      //   id: 'createdat',
      //   header: 'Created',
      //   cell: info => {
      //     const date = info.getValue();
      //     if (!date) return '-';

      //     // Format the date to be more readable
      //     return new Date(date).toLocaleDateString();
      //   },
      //   enableSorting: true,
      // }),
      columnHelper.display({
        id: 'actions',
        header: 'Actions',
        cell: info => (
          <ActionColumn id={info.row.original.id.toString()} />
        ),
      }),
    ],
    []
  );

  /* Client-side pagination */
  /*
  const table = useReactTable({
    data,
    columns,
    state: {
      sorting,
    },
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
  });
  */

  /* Server-side pagination */
  const table = useReactTable({
    data: users,
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
    <>
      <header>
        <h1>Users</h1>
        <ListSearch onSearch={handleSearchChange} placeholder="Search by user name" />
      </header>

      <main>
        {isLoading && <div className="loading">Loading...</div>}
        {error && <div className="error">{error}</div>}

        {users.length > 0 && !isLoading && (
          <div className="table-container">
            <table className="table">
              <thead>
                {table.getHeaderGroups().map(headerGroup => (
                  <tr key={headerGroup.id}>
                    {headerGroup.headers.map(header => (
                      <th
                        key={header.id}
                        onClick={header.column.getToggleSortingHandler()}
                        className={header.column.getCanSort() ? 'sortable' : ''}
                      >
                        {flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                        {{
                          asc: ' 🔼',
                          desc: ' 🔽',
                        }[header.column.getIsSorted() as string] ?? null}
                      </th>
                    ))}
                  </tr>
                ))}
              </thead>
              <tbody>
                {table.getRowModel().rows.map(row => (
                  <tr key={row.id}>
                    {row.getVisibleCells().map(cell => (
                      <td key={cell.id}>
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </td>
                    ))}
                  </tr>
                ))}
              </tbody>
            </table>

            <TableFooter
              currentPage={currentPage}
              totalPages={totalPages}
              pageSize={pageSize}
              onPageChange={handlePageChange}
              onPageSizeChange={handlePageSizeChangeWrapper}
            />
          </div>
        )}
      </main>
    </>
  );
};

export default UserList;
