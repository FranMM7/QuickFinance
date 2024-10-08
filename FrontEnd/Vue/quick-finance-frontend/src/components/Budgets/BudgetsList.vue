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

            <!-- Pagination controls -->
            <nav aria-label="Page navigation">
                <ul class="pagination">
                    <li class="page-item" :class="{ disabled: pageNumber === 1 }">
                        <button class="page-link" @click="changePage(pageNumber - 1)" :disabled="pageNumber === 1">
                            Previous
                        </button>
                    </li>
                    <li class="page-item" v-for="page in totalPages" :key="page"
                        :class="{ active: page === pageNumber }">
                        <button class="page-link" @click="changePage(page)">{{ page }}</button>
                    </li>
                    <li class="page-item" :class="{ disabled: pageNumber === totalPages }">
                        <button class="page-link" @click="changePage(pageNumber + 1)"
                            :disabled="pageNumber === totalPages">
                            Next
                        </button>
                    </li>
                </ul>
            </nav>

        </div>
    </div>
</template>

<script>
// Import fetchBudgets function to retrieve budget data from the API
import { fetchBudgets } from '../../api/services/budgetService';
import { ListLoader } from 'vue-content-loader';

export default {
    name: 'BudgetsList',
    data() {
        return {
            budgets: null, // Holds the budget data
            loading: true, // Indicates if data is currently loading
            error: null, // Holds any error messages
            pageNumber: 1, // Current page number
            totalPages: 1, // Total number of pages (default 1)
        };
    },
    methods: {
        async loadBudgets() {
            try {
                this.loading = true; // Set loading state
                const resp = await fetchBudgets(this.pageNumber); // Fetch budgets with current page number
                this.budgets = resp; // Assuming the API returns paginated data under 'items'
                this.totalPages = resp.totalPages; // Assuming the API returns 'totalPages'
            } catch (error) {
                console.error("Failed to load budgets:", error);
                this.error = 'Failed to load budgets.';
            } finally {
                this.loading = false; // Stop loading state
            }
        },
        // Change page and reload budgets
        changePage(page) {
            if (page >= 1 && page <= this.totalPages) {
                this.pageNumber = page;
                this.loadBudgets();
            }
        },
        goToExpenses(budgetId, month) {
            // Store parameters in Vuex
            this.$store.dispatch('captureBudgetValues', { budgetId, month });
            // Navigate to the Expenses route
            this.$router.push({ name: 'Expenses' });
        },
        edit(budgetId) {
            console.log(budgetId);
        }
    },
    async created() {
        this.loadBudgets(); // Fetch budgets on component creation
    },
    components: {
        ListLoader, // Register ContentLoader component
    }
};
</script>
