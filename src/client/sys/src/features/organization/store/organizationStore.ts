import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { OrganizationStore } from './types';
import type { OrganizationModel } from '$/models/organization-model';
import * as actions from './actions';

// Mock API functions - replace with actual API calls
const mockOrganizations: OrganizationModel[] = [
  {
    id: 1,
    name: 'Acme Corporation',
    code: 'ACME',
    parentOrgId: null,
    status: 'active',
    metadata: 'Primary organization'
  },
  {
    id: 2,
    name: 'Acme Subsidiary',
    code: 'ACME-SUB',
    parentOrgId: 1,
    status: 'active',
    metadata: 'Subsidiary organization'
  },
  {
    id: 3,
    name: 'Beta Industries',
    code: 'BETA',
    parentOrgId: null,
    status: 'inactive',
    metadata: 'Secondary organization'
  }
];

const mockApi = {
  async getOrganizations(page: number = 0, pageSize: number = 10, search?: string): Promise<{ items: OrganizationModel[], totalCount: number }> {
    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 500));

    let filtered = mockOrganizations;

    if (search) {
      const searchLower = search.toLowerCase();
      filtered = mockOrganizations.filter(org =>
        org.name.toLowerCase().includes(searchLower) ||
        org.code?.toLowerCase().includes(searchLower) ||
        org.status?.toLowerCase().includes(searchLower)
      );
    }

    const start = page * pageSize;
    const end = start + pageSize;
    const items = filtered.slice(start, end);

    return {
      items,
      totalCount: filtered.length
    };
  },

  async getOrganization(id: number): Promise<OrganizationModel | null> {
    await new Promise(resolve => setTimeout(resolve, 300));
    return mockOrganizations.find(org => org.id === id) || null;
  },

  async createOrganization(organization: Omit<OrganizationModel, 'id'>): Promise<OrganizationModel> {
    await new Promise(resolve => setTimeout(resolve, 500));
    const newOrg: OrganizationModel = {
      ...organization,
      id: Math.max(...mockOrganizations.map(o => o.id)) + 1
    };
    mockOrganizations.push(newOrg);
    return newOrg;
  },

  async updateOrganization(organization: OrganizationModel): Promise<OrganizationModel> {
    await new Promise(resolve => setTimeout(resolve, 500));
    const index = mockOrganizations.findIndex(org => org.id === organization.id);
    if (index !== -1) {
      mockOrganizations[index] = organization;
    }
    return organization;
  },

  async deleteOrganization(id: number): Promise<void> {
    await new Promise(resolve => setTimeout(resolve, 500));
    const index = mockOrganizations.findIndex(org => org.id === id);
    if (index !== -1) {
      mockOrganizations.splice(index, 1);
    }
  }
};

export const useOrganizationStore = create<OrganizationStore>()(
  devtools(
    (set, get) => ({
      // Initial state
      items: [],
      totalCount: 0,
      currentItem: null,
      tableState: {
        currentPage: 0,
        pageSize: 10,
        searchTerm: '',
        sorting: [],
        selectedId: undefined,
        expandedRows: []
      },
      isLoadingList: false,
      isLoadingDetails: false,
      listError: null,
      detailsError: null,

      // Data actions
      setItems: (items) => set((state) => actions.setItems(state, items)),
      setTotalCount: (count) => set((state) => actions.setTotalCount(state, count)),
      setCurrentItem: (item) => set((state) => actions.setCurrentItem(state, item)),

      // Table state actions
      setCurrentPage: (page) => set((state) => actions.setCurrentPage(state, page)),
      setPageSize: (size) => set((state) => actions.setPageSize(state, size)),
      setSearchTerm: (term) => set((state) => actions.setSearchTerm(state, term)),
      setSorting: (sorting) => set((state) => actions.setSorting(state, sorting)),
      setSelectedItemId: (id) => set((state) => actions.setSelectedItemId(state, id)),
      setExpandedRows: (rows) => set((state) => actions.setExpandedRows(state, rows)),

      // Loading actions
      setLoadingList: (loading) => set((state) => actions.setLoadingList(state, loading)),
      setLoadingDetails: (loading) => set((state) => actions.setLoadingDetails(state, loading)),

      // Error actions
      setListError: (error) => set((state) => actions.setListError(state, error)),
      setDetailsError: (error) => set((state) => actions.setDetailsError(state, error)),

      // Reset actions
      resetList: () => set((state) => actions.resetList(state)),
      resetDetails: () => set((state) => actions.resetDetails(state)),
      resetTableState: () => set((state) => actions.resetTableState(state)),
      softResetTableState: () => set((state) => actions.softResetTableState(state)),
      resetSearch: () => set((state) => actions.resetSearch(state)),
      resetPagination: () => set((state) => actions.resetPagination(state)),
      resetSelection: () => set((state) => actions.resetSelection(state)),
      resetAll: () => set((state) => actions.resetAll(state)),

      // Table state update actions that don't trigger API calls
      handleSelectedItemChange: (id) => set((state) => actions.handleSelectedItemChange(state, id)),
      handleExpandedRowsChange: (rows) => set((state) => actions.handleExpandedRowsChange(state, rows)),

      // Table state persistence
      saveTableState: () => set((state) => actions.saveTableState(state)),
      loadTableState: (currentPath) => set((state) => actions.loadTableState(state, currentPath)),
      shouldClearTableState: (currentPath) => actions.shouldClearTableState(get(), currentPath),
      clearTableState: () => set((state) => actions.clearTableState(state)),
      loadTableStateForPath: (currentPath) => set((state) => actions.loadTableStateForPath(state, currentPath)),

      // Async actions
      fetchItems: async () => {
        const state = get();
        set((state) => actions.setLoadingList(state, true));
        set((state) => actions.setListError(state, null));

        try {
          const { items, totalCount } = await mockApi.getOrganizations(
            state.tableState.currentPage,
            state.tableState.pageSize,
            state.tableState.searchTerm || undefined
          );

          set((state) => actions.setItems(state, items));
          set((state) => actions.setTotalCount(state, totalCount));
        } catch (error) {
          set((state) => actions.setListError(state, error instanceof Error ? error.message : 'Failed to fetch organizations'));
        } finally {
          set((state) => actions.setLoadingList(state, false));
        }
      },

      fetchItemDetails: async (id) => {
        set((state) => actions.setLoadingDetails(state, true));
        set((state) => actions.setDetailsError(state, null));

        try {
          const item = await mockApi.getOrganization(id);
          set((state) => actions.setCurrentItem(state, item));
        } catch (error) {
          set((state) => actions.setDetailsError(state, error instanceof Error ? error.message : 'Failed to fetch organization details'));
        } finally {
          set((state) => actions.setLoadingDetails(state, false));
        }
      },

      // Table state update actions that trigger API calls
      handleSearch: async (searchTerm) => {
        set((state) => actions.setSearchTerm(state, searchTerm));
        set((state) => actions.setCurrentPage(state, 0));
        await get().fetchItems();
      },

      clearSearch: async () => {
        set((state) => actions.setSearchTerm(state, ''));
        set((state) => actions.setCurrentPage(state, 0));
        await get().fetchItems();
      },

      handleSortingChange: async (sorting) => {
        set((state) => actions.setSorting(state, sorting));
        await get().fetchItems();
      },

      handlePageSizeChange: async (newPageSize) => {
        set((state) => actions.setPageSize(state, newPageSize));
        set((state) => actions.setCurrentPage(state, 0));
        await get().fetchItems();
      },

      handlePageChange: async (newPage) => {
        set((state) => actions.setCurrentPage(state, newPage));
        await get().fetchItems();
      },

      // Organization-specific actions
      createOrganization: async (organization) => {
        try {
          const newOrg = await mockApi.createOrganization(organization);
          // Refresh the list to include the new organization
          await get().fetchItems();
        } catch (error) {
          set((state) => actions.setListError(state, error instanceof Error ? error.message : 'Failed to create organization'));
        }
      },

      updateOrganization: async (organization) => {
        try {
          await mockApi.updateOrganization(organization);
          // Refresh the list to reflect changes
          await get().fetchItems();
        } catch (error) {
          set((state) => actions.setListError(state, error instanceof Error ? error.message : 'Failed to update organization'));
        }
      },

      deleteOrganization: async (id) => {
        try {
          await mockApi.deleteOrganization(id);
          // Refresh the list to reflect changes
          await get().fetchItems();
        } catch (error) {
          set((state) => actions.setListError(state, error instanceof Error ? error.message : 'Failed to delete organization'));
        }
      },

      searchOrganizations: async (query) => {
        await get().handleSearch(query);
      },

      getRootOrganizations: async () => {
        // This would be a specific API call for root organizations
        await get().fetchItems();
      },

      getChildOrganizations: async (parentId) => {
        // This would be a specific API call for child organizations
        await get().fetchItems();
      }
    }),
    {
      name: 'organization-store'
    }
  )
);
