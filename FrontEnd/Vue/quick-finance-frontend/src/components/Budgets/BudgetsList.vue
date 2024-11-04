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
          <li :class="['page-item', { disabled: !validURL('F') }]">
            <a class="page-link" href="#" @click.prevent="goTo('F')">First</a>
          </li>
          <li :class="['page-item', { disabled: !validURL('P') }]">
            <a class="page-link" href="#" @click.prevent="goTo('P')">Previous</a>
          </li>
          <li v-for="page in totalPages" :key="page" :class="['page-item', { active: page === pageNumber }]">
            <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
          </li>

          <!-- Row Selection Dropdown -->
          <label for="rowsPerPage" class="page-item page-link disabled">View:</label>
          <select id="rowsPerPage" v-model="rowsPage" @change="loadPage" class="page-item page-link">
            <option :value="5">5</option>
            <option :value="10">10</option>
            <option :value="20">20</option>
            <option :value="50">50</option>
          </select>

          <li :class="['page-item', { disabled: !validURL('N') }]">
            <a class="page-link" href="#" @click.prevent="goTo('N')">Next</a>
          </li>
          <li :class="['page-item', { disabled: !validURL('L') }]">
            <a class="page-link" href="#" @click.prevent="goTo('L')">Last</a>
          </li>
        </ul>

      </div> <!-- Pagination Component -->

    </div>
  </div>
</template>

<script lang="ts">
import { fetchBudgets, BudgetList, editBudget, goToPage } from '../../api/services/budgetService';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useErrorStore } from '@/stores/error';
import { useBudgetStore } from '@/stores/budgets';
import { defineComponent, ref, onMounted, Ref } from 'vue';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: 'BudgetList',
  setup() {
    const loading = ref<boolean>(true);
    const error = ref<string>('');
    const budgets = ref<BudgetList[]>([]);
    const pageNumber = ref<number>(1);
    const rowsPage = ref<number>(10);
    const totalPages = ref<number>(1);
    const router = useRouter();
    const next = ref<string>('');
    const prev = ref<string>('');
    const first = ref<string>('');
    const last = ref<string>('');


    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    };

    const goToExpenses = (budgetId: number, title: string) => {
      // Store parameters in penia
      const budgeStore = useBudgetStore();
      budgeStore.captureBudgetValues(budgetId, title)

      // Navigate to the Expenses route
      router.push({ name: 'budgetExpenses' });
    };
    const edit = (budgetId: number) => {
      const storeBudget = useBudgetStore();
      storeBudget.setBudgetId(budgetId);
      router.push({ name: 'editBudget' });
    };

    const loadPage = async () => {
      try {
        loading.value = true;
        const response = await fetchBudgets(pageNumber.value, rowsPage.value);
        budgets.value = response.data;
        totalPages.value = response.totalPages;
        next.value = response.nextPage
        prev.value = response.previousPage
        last.value = response.lastPage
        first.value = response.firstPage
      } catch (err) {
        error.value = 'Failed to load budget list';
        console.error('Error loading budgets:', err);
      } finally {
        loading.value = false;
      }
    };

    const changePage = (newPage: number) => {
      if (newPage >= 1 && newPage <= totalPages.value) {
        pageNumber.value = newPage;
        loadPage();
      }
    };

    const validURL = (option: 'F' | 'L' | 'N' | 'P'): boolean => {
      switch (option) {
        case 'P':
          return !!prev.value;
        case 'F':
          return !!first.value;
        case 'L':
          return !!last.value;
        case 'N':
          return !!next.value;
        default:
          return false;
      }
    };
    const goTo = async (option: 'F' | 'L' | 'N' | 'P') => {
      try {
        loading.value = true;

        const opt: { [key: string]: Ref<string> } = {
          'F': first,
          'L': last,
          'N': next,
          'P': prev
        };

        const url = opt[option].value;

        const response = await goToPage(url);

        budgets.value = response.data;
        totalPages.value = response.totalPages;
        next.value = response.nextPage;
        prev.value = response.previousPage;
        last.value = response.lastPage;
        first.value = response.firstPage;
      } catch (err) {
        error.value = 'Failed to load budget list';
        console.error('Error loading budgets:', err);
      } finally {
        loading.value = false;
      }
    };


    onMounted(() => {
      loadPage();
    });

    return {
      loading,
      error,
      budgets,
      pageNumber,
      rowsPage,
      totalPages,
      changePage,
      loadPage,
      formatDate,
      goToExpenses,
      edit,
      goTo,
      validURL
    };
  },
});
</script>
