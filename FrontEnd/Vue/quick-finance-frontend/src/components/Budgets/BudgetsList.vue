<template>
  <div class="container">
    <div v-if="loading">
      <list-loader />
    </div>
    <div v-else-if="error">
      <Error />
    </div>
    <div v-else>
      <table class="table table-striped">
        <thead>
          <tr>
            <!-- <th>Id</th> -->
            <th>Budget Title</th>
            <th>Allocated Budget</th>
            <th>Total Expended</th>
            <th colspan="2">Modified On</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="budget in budgets || []" :key="budget.id">
            <!-- <td>{{ budget.id }}</td> -->
            <td>{{ budget.title }}</td>
            <td>{{ budget.totalAllocatedBudget }}</td>
            <td>{{ budget.executedBudget }}</td>
            <td>{{ formatDate(String(budget.modifiedOn)) }}</td>
            <td>
              <div class="btn-group">
                <button @click="goToExpenses(budget.id, budget.title)" type="button" class="btn btn-primary">
                  Expenses <font-awesome-icon :icon="['fas', 'table-list']" />
                </button>
                <button @click="edit(budget.id)" type="button" class="btn btn-secondary">
                  <font-awesome-icon :icon="['fas', 'edit']" />
                </button>
                <button type="button" class="btn btn-danger">
                  <font-awesome-icon :icon="['fas', 'trash']" />
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Pagination Component -->
      <div class="d-flex justify-content-center mt-4"> <!-- Center the pagination -->
        <ul class="pagination">
          <li :class="['page-item', { disabled: currentPage === 1 }]">
            <a class="page-link" href="#" @click="changePage(currentPage - 1)" aria-label="Previous">
              <span aria-hidden="true">&laquo;</span>
            </a>
          </li>
          <li v-for="page in totalPages" :key="page" :class="['page-item', { active: currentPage === page }]">
            <a class="page-link" href="#" @click="changePage(page)">{{ page }}</a>
          </li>
          <li :class="['page-item', { disabled: currentPage === totalPages }]">
            <a class="page-link" href="#" @click="changePage(currentPage + 1)" aria-label="Next">
              <span aria-hidden="true">&raquo;</span>
            </a>
          </li>
        </ul>
      </div>
      
    </div>
  </div>
</template>

<script lang="ts">
import { fetchBudgets, BudgetList, editBudget } from '../../api/services/budgetService';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useErrorStore } from '@/stores/error';
import { useBudgetStore } from '@/stores/budgets';

export default {
  name: 'BudgetsList',
  components: {
    ListLoader,
    Error,
  },
  data() {
    return {
      budgets: [] as BudgetList[], // Holds the budget data
      loading: true, // Indicates if data is currently loading
      error: null as string | null,  // Allows both null and string // Holds any error messages
      currentPage: 1, // Tracks the current page
      rowsPerPage: 10, // Number of rows per page
      totalPages: 1, // Total pages for pagination
    };
  },
  async created() {
    await this.loadBudgets(); // Fetch initial budgets
  },
  methods: {
    formatDate(dateString: string) {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    },
    goToExpenses(budgetId: number, title: string) {
      // Store parameters in penia
      const budgeStore = useBudgetStore();
      budgeStore.captureBudgetValues(budgetId, title)

      // Navigate to the Expenses route
      this.$router.push({ name: 'budgetExpenses' });
    },
    edit(budgetId: number) {
      const storeBudget = useBudgetStore();
      storeBudget.setBudgetId(budgetId);
      this.$router.push({name:'editBudget'});
    },
    async loadBudgets() {
      try {
        this.loading = true;
        const response = await fetchBudgets(this.currentPage, this.rowsPerPage);

        // Map over the response to ensure modifiedOn is a Date object
        this.budgets = response;

      } catch (err) {
        this.error = 'Failed to load budgets.';
        const errorStore = useErrorStore();
        errorStore.setErrorNotification('Failed to load budgets', String(err));
      } finally {
        this.loading = false;
      }
    },
    changePage(page: number) {
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page;
        this.loadBudgets();
      }
    },
  }

};
</script>
