<template>
  <div class="container">
    <div class="jumbotron">
      <h1 class="display-3">Quick Finance</h1>
      <hr>
      <p class="lead">QuickFinance is a simple personal finance tracker that allows users to manage their expenses,
        budgets,
        categories, payment methods, and budget limits efficiently. This application is designed to help users keep
        track
        of their financial activities and gain insights into their spending habits.</p>
    </div>
    <hr>

    <div class="row m-1 p-1">
      <div v-if="loading">
        <ListLoader />
      </div>
      <div v-else-if="error">{{ error }}</div>
      <div v-else>
        <!-- Loop through the budgetInfo array and display each budget -->

        <!-- warning card -->
        <div class="row">
          <h3>Month with the highest Expenses</h3>
          <div v-for="(budget, index) in highetsExpenses" :key="index" class="card border-warning mb"
            style="max-width: 20rem; cursor: pointer;" @click="goToExpenses(budget.BudgetId, budget.Month)">
            <div class=" card-header">{{ budget.Month }}</div>
            <div class="card-body">
              <h4 class="card-title">Budget: {{ budget.TotalBudget }}</h4>
              <p class="card-text">Expenses: {{ budget.Expenses }} | Saving: {{ budget.Saving }}</p>
            </div>
          </div>
        </div>
        <hr>

        <!-- info cards -->
        <div class="row">
          <h3>Last 5 Months</h3>
          <div v-for="(budget, index) in budgetInfo" :key="index" class="card border-info mb"
            style="max-width: 20rem; margin-right: 5px;cursor: pointer;"
            @click="goToExpenses(budget.BudgetId, budget.Month)">
            <div class="card-header">{{ budget.Month }}</div>
            <div class="card-body">
              <h4 class="card-title">Budget: {{ budget.TotalBudget }}</h4>
              <p class="card-text">Expenses: {{ budget.Expenses }} | Saving: {{ budget.Saving }}</p>
            </div>
          </div>
        </div>

      </div> <!-- else div -->

    </div><!-- row -->

  </div> <!-- container -->

</template>

<script lang="ts">
import { BudgetSumary, getBudgetInfo } from '@/api/services/budgetService';
import { useBudgetStore } from '@/stores/budgets';
import { ListLoader } from 'vue-content-loader';

export default {
  name: 'homeView',
  components: {
    ListLoader,
  },
  data() {
    return {
      budgetInfo: [] as BudgetSumary[], // Correctly type budgetInfo as an array of Budget objects
      highetsExpenses: [] as BudgetSumary[], // Correctly type highetsExpenses as an array of Budget objects
      loading: true,
      error: null as string | null, // Allow error to be a string or null
    };
  },
  methods: {
    goToExpenses(budgetId: number, month: string) {
      const budgetStore = useBudgetStore(); // Access the store here
      // Call the new captureBudgetValues method
      budgetStore.captureBudgetValues(budgetId, month);
      // Navigate to the Expenses route
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
        this.highetsExpenses = resp.MonthWithHighestExpenses || [];
      } catch (error) {
        console.error("Failed to load budgets:", error);
        this.error = 'Failed to load budgets.';
      } finally {
        this.loading = false;
      }
    }
  },

  async created() {
    this.loadBudgetInfo();
  }
}
</script>
