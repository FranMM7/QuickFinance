<template>
    <div>
        <h1>Expenses for Budget {{ budgetId }}</h1>
        <div v-if="loading">Loading expenses...</div>
        <div v-else-if="error">{{ error }}</div>
        <div v-else>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Description</th>
                        <th>Amount</th>
                        <th>Date</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="expense in expenses" :key="expense.id">
                        <td>{{ expense.id }}</td>
                        <td>{{ expense.description }}</td>
                        <td>{{ expense.amount }}</td>
                        <td>{{ formatDate(expense.date) }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
import { fetchExpenses } from '../../api/services/expensesService.js'; // Adjust your API service path

export default {
    name: 'Expenses',
    data() {
        return {
            expenses: [],
            loading: true,
            error: null,
            budgetId: this.$route.params.budgetId, // Access budgetId from route parameters
        };
    },
    async created() {
        try {
            const resp = await fetchExpenses(this.budgetId); // Fetch expenses for the selected budget
            this.expenses = resp;
        } catch (error) {
            this.error = 'Failed to load expenses.';
        } finally {
            this.loading = false;
        }
    },
};
</script>
