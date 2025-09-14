import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { OrganizationStore } from './types';
import type { OrganizationModel, OrganizationDetailModel } from '$/models/access';
import { OrganizationMemberRole } from '$/models/access';
import { EntityType, EntityStatus } from '$/models/business';
import { TaskStatus } from '$/models/workflow/task-status';
import { ChecklistStatus } from '$/models/workflow/checklist-status';
import * as organizationApi from '../api/organizationApi';

// Mock API functions - replace with actual API calls
const mockOrganizations: OrganizationModel[] = [
  {
    id: 1,
    name: 'Acme Corporation',
    code: 'ACME',
    parentOrgId: undefined,
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
    parentOrgId: undefined,
    status: 'inactive',
    metadata: 'Secondary organization'
  }
];

// Mock detailed organization data for demonstration
const mockOrganizationDetails: Record<number, OrganizationDetailModel> = {
  1: {
    id: 1,
    name: 'Acme Corporation',
    code: 'ACME',
    parentOrgId: undefined,
    status: 'active',
    metadata: 'Primary organization',
    createdAt: new Date('2024-01-15'),
    updatedAt: new Date('2024-12-19'),
    deletedAt: undefined,
    entities: [
      { id: 1, name: 'Acme Corp LLC', orgId: 1, entityTypeId: EntityType.llc, statusId: EntityStatus.active },
      { id: 2, name: 'Acme Holdings Inc', orgId: 1, entityTypeId: EntityType.c_corp, statusId: EntityStatus.active }
    ],
    members: [
      { id: 1, orgId: 1, personId: 1, roleId: OrganizationMemberRole.admin },
      { id: 2, orgId: 1, personId: 2, roleId: OrganizationMemberRole.viewer }
    ],
    tasks: [
      { id: 1, orgId: 1, name: 'Annual Review', statusId: TaskStatus.todo },
      { id: 2, orgId: 1, name: 'Budget Planning', statusId: TaskStatus.done }
    ],
    checklistInstances: [
      { id: 1, orgId: 1, name: 'Compliance Checklist', statusId: ChecklistStatus.active, createdFromId: 'manual' as any }
    ]
  },
  2: {
    id: 2,
    name: 'Acme Subsidiary',
    code: 'ACME-SUB',
    parentOrgId: 1,
    status: 'active',
    metadata: 'Subsidiary organization',
    createdAt: new Date('2024-03-20'),
    updatedAt: new Date('2024-12-15'),
    deletedAt: undefined,
    entities: [
      { id: 3, name: 'Acme Sub LLC', orgId: 2, entityTypeId: EntityType.llc, statusId: EntityStatus.active }
    ],
    members: [
      { id: 3, orgId: 2, personId: 3, roleId: OrganizationMemberRole.manager }
    ],
    tasks: [
      { id: 3, orgId: 2, name: 'Q4 Report', statusId: TaskStatus.todo }
    ],
    checklistInstances: []
  },
  3: {
    id: 3,
    name: 'Beta Industries',
    code: 'BETA',
    parentOrgId: undefined,
    status: 'inactive',
    metadata: 'Secondary organization',
    createdAt: new Date('2024-02-10'),
    updatedAt: new Date('2024-11-30'),
    deletedAt: undefined,
    entities: [],
    members: [],
    tasks: [],
    checklistInstances: []
  }
};

// For now, we'll use mock data but with real API structure
// This will be replaced when the server endpoints are ready
const useMockData = true;

const mockApi = {
  async getOrganizations(page: number = 0, pageSize: number = 10, search?: string, sorting?: Array<{ id: string; desc: boolean }>): Promise<{ items: OrganizationModel[], totalCount: number }> {
    if (useMockData) {
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

      // Apply sorting if provided
      if (sorting && sorting.length > 0) {
        const sort = sorting[0];
        filtered.sort((a, b) => {
          const aValue = a[sort.id as keyof OrganizationModel];
          const bValue = b[sort.id as keyof OrganizationModel];

          if (aValue === bValue) return 0;
          if (aValue == null) return 1;
          if (bValue == null) return -1;

          const comparison = aValue < bValue ? -1 : 1;
          return sort.desc ? -comparison : comparison;
        });
      }

      const start = page * pageSize;
      const end = start + pageSize;
      const items = filtered.slice(start, end);

      return {
        items,
        totalCount: filtered.length
      };
    } else {
      return await organizationApi.fetchOrganizations(page, pageSize, search, sorting);
    }
  },

  async getOrganization(id: number): Promise<OrganizationDetailModel | null> {
    if (useMockData) {
      await new Promise(resolve => setTimeout(resolve, 300));
      return mockOrganizationDetails[id] || null;
    } else {
      return await organizationApi.fetchOrganizationDetails(id);
    }
  },

  async createOrganization(organization: Omit<OrganizationModel, 'id'>): Promise<OrganizationModel> {
    if (useMockData) {
      await new Promise(resolve => setTimeout(resolve, 500));
      const newOrg: OrganizationModel = {
        ...organization,
        id: Math.max(...mockOrganizations.map(o => o.id)) + 1
      };
      mockOrganizations.push(newOrg);
      return newOrg;
    } else {
      return await organizationApi.createOrganization(organization);
    }
  },

  async updateOrganization(organization: OrganizationModel): Promise<OrganizationModel> {
    if (useMockData) {
      await new Promise(resolve => setTimeout(resolve, 500));
      const index = mockOrganizations.findIndex(org => org.id === organization.id);
      if (index !== -1) {
        mockOrganizations[index] = organization;
      }
      return organization;
    } else {
      return await organizationApi.updateOrganization(organization);
    }
  },

  async deleteOrganization(id: number): Promise<void> {
    if (useMockData) {
      await new Promise(resolve => setTimeout(resolve, 500));
      const index = mockOrganizations.findIndex(org => org.id === id);
      if (index !== -1) {
        mockOrganizations.splice(index, 1);
      }
    } else {
      await organizationApi.deleteOrganization(id);
    }
  }
};

export const useOrganizationStore = create<OrganizationStore>()(
  devtools(
    (set, get) => ({
      // Initial state
      items: [],
      totalCount: 0,
      currentItem: null as OrganizationDetailModel | null,
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
      setItems: (items) => set({ items }),
      setTotalCount: (count) => set({ totalCount: count }),
      setCurrentItem: (item: OrganizationDetailModel | null) => set({ currentItem: item }),

      // Table state actions
      setCurrentPage: (page) => set((state) => ({ ...state, tableState: { ...state.tableState, currentPage: page } })),
      setPageSize: (size) => set((state) => ({ ...state, tableState: { ...state.tableState, pageSize: size } })),
      setSearchTerm: (term) => set((state) => ({ ...state, tableState: { ...state.tableState, searchTerm: term } })),
      setSorting: (sorting) => set((state) => ({ ...state, tableState: { ...state.tableState, sorting } })),
      setSelectedItemId: (id) => set((state) => ({ ...state, tableState: { ...state.tableState, selectedId: id } })),
      setExpandedRows: (rows) => set((state) => ({ ...state, tableState: { ...state.tableState, expandedRows: rows } })),

      // Loading actions
      setLoadingList: (loading) => set({ isLoadingList: loading }),
      setLoadingDetails: (loading) => set({ isLoadingDetails: loading }),

      // Error actions
      setListError: (error) => set({ listError: error }),
      setDetailsError: (error) => set({ detailsError: error }),

      // Reset actions
      resetList: () => set({ items: [], totalCount: 0, listError: null }),
      resetDetails: () => set({ currentItem: null as OrganizationDetailModel | null, detailsError: null }),
      resetTableState: () => set({
        tableState: {
          currentPage: 0,
          pageSize: 10,
          searchTerm: '',
          sorting: [],
          selectedId: undefined,
          expandedRows: []
        }
      }),
      softResetTableState: () => set((state) => ({
        ...state,
        tableState: {
          ...state.tableState,
          currentPage: 0,
          searchTerm: '',
          sorting: []
        }
      })),
      resetSearch: () => set((state) => ({
        ...state,
        tableState: { ...state.tableState, searchTerm: '' }
      })),
      resetPagination: () => set((state) => ({
        ...state,
        tableState: { ...state.tableState, currentPage: 0 }
      })),
      resetSelection: () => set((state) => ({
        ...state,
        tableState: { ...state.tableState, selectedId: undefined, expandedRows: [] }
      })),
      resetAll: () => set({
        items: [],
        totalCount: 0,
        currentItem: null as OrganizationDetailModel | null,
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
        detailsError: null
      }),

      // Table state update actions that don't trigger API calls
      handleSelectedItemChange: (id) => set((state) => ({
        ...state,
        tableState: { ...state.tableState, selectedId: id }
      })),
      handleExpandedRowsChange: (rows) => set((state) => ({
        ...state,
        tableState: { ...state.tableState, expandedRows: rows }
      })),

      // Table state persistence
      saveTableState: () => {
        const state = get();
        const stateToSave = {
          currentPage: state.tableState.currentPage,
          pageSize: state.tableState.pageSize,
          searchTerm: state.tableState.searchTerm,
          sorting: state.tableState.sorting,
          selectedId: state.tableState.selectedId,
          expandedRows: state.tableState.expandedRows
        };
        localStorage.setItem('organizationTableState', JSON.stringify(stateToSave));
      },
      loadTableState: () => {
        try {
          const savedState = localStorage.getItem('organizationTableState');
          if (savedState) {
            const parsedState = JSON.parse(savedState);
            set((state) => ({
              ...state,
              tableState: { ...state.tableState, ...parsedState }
            }));
          }
        } catch (error) {
          console.warn('Failed to load organization table state:', error);
        }
      },
      shouldClearTableState: (currentPath) => !currentPath.startsWith('/organization'),
      clearTableState: () => {
        localStorage.removeItem('organizationTableState');
        set((state) => ({
          ...state,
          tableState: {
            currentPage: 0,
            pageSize: 10,
            searchTerm: '',
            sorting: [],
            selectedId: undefined,
            expandedRows: []
          }
        }));
      },
      loadTableStateForPath: (currentPath) => {
        const state = get();
        if (!currentPath.startsWith('/organization')) {
          state.clearTableState();
        } else {
          state.loadTableState(currentPath);
        }
      },

      // Async actions
      fetchItems: async () => {
        const state = get();
        set({ isLoadingList: true, listError: null });

        try {
          const { items, totalCount } = await mockApi.getOrganizations(
            state.tableState.currentPage,
            state.tableState.pageSize,
            state.tableState.searchTerm || undefined,
            state.tableState.sorting
          );

          set({ items, totalCount });
        } catch (error) {
          set({ listError: error instanceof Error ? error.message : 'Failed to fetch organizations' });
        } finally {
          set({ isLoadingList: false });
        }
      },

      fetchItemDetails: async (id) => {
        set({ isLoadingDetails: true, detailsError: null });

        try {
          const item = await mockApi.getOrganization(id);
          set({ currentItem: item });
        } catch (error) {
          set({ detailsError: error instanceof Error ? error.message : 'Failed to fetch organization details' });
        } finally {
          set({ isLoadingDetails: false });
        }
      },

      // Table state update actions that trigger API calls
      handleSearch: async (searchTerm) => {
        set((state) => ({
          ...state,
          tableState: { ...state.tableState, searchTerm, currentPage: 0 }
        }));
        await get().fetchItems();
      },

      clearSearch: async () => {
        set((state) => ({
          ...state,
          tableState: { ...state.tableState, searchTerm: '', currentPage: 0 }
        }));
        await get().fetchItems();
      },

      handleSortingChange: async (sorting) => {
        set((state) => ({
          ...state,
          tableState: { ...state.tableState, sorting }
        }));
        await get().fetchItems();
      },

      handlePageSizeChange: async (newPageSize) => {
        set((state) => ({
          ...state,
          tableState: { ...state.tableState, pageSize: newPageSize, currentPage: 0 }
        }));
        await get().fetchItems();
      },

      handlePageChange: async (newPage) => {
        set((state) => ({
          ...state,
          tableState: { ...state.tableState, currentPage: newPage }
        }));
        await get().fetchItems();
      },

      // Organization-specific actions
      createOrganization: async (organization) => {
        try {
          await mockApi.createOrganization(organization);
          // Refresh the list to include the new organization
          await get().fetchItems();
        } catch (error) {
          set({ listError: error instanceof Error ? error.message : 'Failed to create organization' });
        }
      },

      updateOrganization: async (organization) => {
        try {
          await mockApi.updateOrganization(organization);
          // Refresh the list to reflect changes
          await get().fetchItems();
        } catch (error) {
          set({ listError: error instanceof Error ? error.message : 'Failed to update organization' });
        }
      },

      deleteOrganization: async (id) => {
        try {
          await mockApi.deleteOrganization(id);
          // Refresh the list to reflect changes
          await get().fetchItems();
        } catch (error) {
          set({ listError: error instanceof Error ? error.message : 'Failed to delete organization' });
        }
      },

      searchOrganizations: async (query) => {
        await get().handleSearch(query);
      },

      getRootOrganizations: async () => {
        // This would be a specific API call for root organizations
        await get().fetchItems();
      },

      getChildOrganizations: async () => {
        // This would be a specific API call for child organizations
        await get().fetchItems();
      }
    }),
    {
      name: 'organization-store'
    }
  )
);
