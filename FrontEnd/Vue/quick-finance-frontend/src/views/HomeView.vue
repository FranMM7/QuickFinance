<template>
  <div class="container">
    <div class="jumbotron">
      <h1 class="display-3">Quick Finance</h1>
      <hr>
      <p class="lead">
        QuickFinance is a simple personal finance tracker that allows users to manage their expenses,
        budgets, categories, payment methods, and budget limits efficiently. This application is designed to help users
        keep
        track of their financial activities and gain insights into their spending habits.
      </p>
    </div>
    <hr>

    <div class="row m-1 p-1">
      <div v-if="loading">
        <ListLoader />
      </div>
      <div v-else-if="error">
        <Error />
      </div>
      <div v-else>
        <div class="row" v-if="highestExpenses">
          <h3>Budget with the highest Expenses</h3>
          <div v-for="(budget, index) in highestExpenses" :key="index" class="card border-warning mb"
            style="max-width: 20rem; cursor: pointer;" @click="goToExpenses(budget.BudgetId, budget.Title)">
            <div class="card-header">{{ budget.Title }}</div>
            <div class="card-body">
              <h4 class="card-title">Budget: {{ budget.TotalAllocatedBudget }}</h4>
              <p class="card-text">Expenses: {{ budget.Expenses }} | Saving: {{ budget.Saving }}</p>
            </div>
          </div>
        </div>
        <hr>

        <div class="row">
          <h3>Last 5 Budgets</h3>
          <div v-for="(budget, index) in budgetInfo" :key="index" class="card border-info mb"
            style="max-width: 20rem; margin-right: 5px; cursor: pointer;"
            @click="goToExpenses(budget.BudgetId, budget.Title)">
            <div class="card-header">{{ budget.Title }}</div>
            <div class="card-body">
              <h4 class="card-title">Budget: {{ budget.TotalAllocatedBudget }}</h4>
              <p class="card-text">Expenses: {{ budget.Expenses }} | Saving: {{ budget.Saving }}</p>
            </div>
          </div>
        </div>

      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { BudgetSumary, getBudgetInfo } from '@/api/services/budgetService';
import Error from '@/components/error/error.vue';
import { useBudgetStore } from '@/stores/budgets';
import { useErrorStore } from '@/stores/error';
import { error } from 'console';
import { ListLoader } from 'vue-content-loader';

export default {
  name: 'homeView',
  components: {
    ListLoader,
    Error
  },
  data() {
    return {
      budgetInfo: [] as BudgetSumary[], // Type budgetInfo as an array of Budget objects
      highestExpenses: [] as BudgetSumary[], // Fixed typo: 'highetsExpenses' to 'highestExpenses'
      loading: true,
      error: null as string | null, // Explicitly type error as string | null
    };
  },
  methods: {
    goToExpenses(budgetId: number, title: string) {
      const budgetStore = useBudgetStore();
      budgetStore.captureBudgetValues(budgetId, title);
      this.$router.push({ name: 'Expenses' });
    },
    async loadBudgetInfo() {
      try {
        this.loading = true;

        // Introduce a 1-second delay for the loader effect
        await new Promise(resolve => setTimeout(resolve, 1000));
        const resp = await getBudgetInfo();

        // Ensure the response is properly assigned
        this.budgetInfo = resp.BudgetTop5 || [];
        this.highestExpenses = resp.RecordWithHighestExpenses || []; // Fixed typo
      } catch (error: unknown) {  // Explicitly typing the error
        if (error instanceof Error) {
          this.error = error.message;  // Safely access the message
          const errorStore = useErrorStore();
          errorStore.setErrorNotification(this.error, error); // Pass the original error object
        } else {
          this.error = 'Failed to load budgets';  // Fallback error message
        }
      } finally {
        this.loading = false;
      }
    }


  },

  async created() {
    await this.loadBudgetInfo();
  }
}
</script>
