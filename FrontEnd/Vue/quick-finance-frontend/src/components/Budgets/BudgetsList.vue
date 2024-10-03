<template>
    <div>
        <div v-if="loading">
            <list-loader />
        </div>
        <div v-else-if="error">{{ error }}</div>
        <div v-else>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Month</th>
                        <th>Budget</th>
                        <th>Total Expended</th>
                        <th colspan="2">Modified On</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="budget in budgets?.$values || []" :key="budget.id">
                        <td>{{ budget.id }}</td>
                        <td>{{ budget.month }}</td>
                        <td>{{ budget.totalBudget }}</td>
                        <td>{{ budget.executedBudget }}</td>
                        <td>{{ formatDate(budget.modifiedOn) }}</td>
                        <td>
                            <div class="btn-group">
                                <!-- Navigate to expenses page -->
                                <button @click="goToExpenses(budget.id, budget.month)" type="button"
                                    class="btn btn-primary">
                                    Expenses <font-awesome-icon :icon="['fas', 'table-list']" />
                                </button>
                                <!-- Navigate to edit record page -->
                                <button @click="edit(budget.id)" type="button" class="btn btn-secondary">
                                    <font-awesome-icon :icon="['fas', 'edit']" />
                                </button>
                                <!-- Delete record  -->
                                <button type="button" class="btn btn-danger">
                                    <font-awesome-icon :icon="['fas', 'trash']" />
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>


<script>
// Import fetchBudgets function to retrieve budget data from the API
import { fetchBudgets } from '../../api/services/budgetService.js';
import {
    ContentLoader,
    FacebookLoader,
    CodeLoader,
    BulletListLoader,
    InstagramLoader,
    ListLoader,
} from 'vue-content-loader';

export default {
    methods: {
        goToExpenses(budgetId, month) {
            // Store parameters in Vuex
            this.$store.dispatch('captureBudgetValues', { budgetId, month });
            // Navigate to the Expenses route
            this.$router.push({ name: 'Expenses' });
        },
        edit(budgetId) {
            console.log(budgetId)
        }
    },
    components: {
        ContentLoader,
        FacebookLoader,
        CodeLoader,
        BulletListLoader,
        InstagramLoader,
        ListLoader, // Register ContentLoader component
    },
    name: 'BudgetsList',
    data() {
        return {
            budgets: null, // Holds the budget data
            loading: true, // Indicates if data is currently loading
            error: null, // Holds any error messages
        };
    },
    async created() {
        try {
            this.loading = true; // Set loading to true while fetching data

            // Introduce a 2-second delay for the loader effect
            // await new Promise(resolve => setTimeout(resolve, 2000));

            const resp = await fetchBudgets(); // Fetch budgets from the API
            this.budgets = resp; // Assign fetched data to budgets
        } catch (error) {
            console.error("Failed to load budgets:", error); // Log error to the console
            this.error = 'Failed to load budgets.'; // Set error message
        } finally {
            this.loading = false; // Always set loading to false after fetching data
        }
    },
};
</script>
