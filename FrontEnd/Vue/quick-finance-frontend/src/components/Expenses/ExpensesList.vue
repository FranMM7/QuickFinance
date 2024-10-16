<template>
    <div>
        <h1>{{ month }} Budget</h1>
        <hr>
        <div v-if="loading">
            <ListLoader />
        </div>
        <div v-else-if="error">
            <Error />
        </div>
        <div v-else>
            <table class="table table-striped">
                <thead>
                    <tr class="text-center">
                        <th>Description</th>
                        <th>Category</th>
                        <th>Amount</th>
                        <th>Due Date</th>
                        <th>Executed</th>
                        <th>Modified on</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="expense in formattedExpenses" :key="expense.id" class="text-center">
                        <td>{{ expense.description }}</td>
                        <td>{{ expense.category }}</td>
                        <td>{{ expense.amount }}</td>
                        <td>{{ expense.formattedDueDate }}</td>
                        <td>{{ expense.executed }}</td>
                        <td>{{ expense.formattedModifiedOn }}</td>
                        <td>
                            <button @click="edit(expense.id)" type="button" class="btn btn-secondary">
                                <font-awesome-icon :icon="['fas', 'edit']" />
                            </button>
                            <button @click="deleteExpense(expense.id)" type="button" class="btn btn-danger">
                                <font-awesome-icon :icon="['fas', 'trash']" />
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
import { mapGetters } from 'vuex';
import { ListLoader } from 'vue-content-loader';
import { useExpensesStore } from '@/stores/expenses';
import { useBudgetStore } from '@/stores/budgets';
import { fecthExpenses } from '@/api/services/expensesService';
import { useErrorStore } from '@/stores/error';
import Error from '../error/error.vue';

export default {
    name: 'ExpensesList',
    components: {
        ListLoader,
        Error
    },
    data() {
        return {
            expenses: [],
            loading: true,
            error: null,
        };
    },
    computed: {
        formattedExpenses() {
            return this.expenses.map(expense => ({
                ...expense,
                formattedDueDate: this.formatDate(expense.dueDate),
                formattedModifiedOn: this.formatDate(expense.modifiedOn),
            }));
        },
    },
    async created() {
        try {
            const budgetStore = useBudgetStore();

            this.budgetId = budgetStore.getBudgetId;
            this.month = budgetStore.getMonth;

            const resp = await fecthExpenses(this.budgetId);
            this.expenses = resp?.$values || [];
        } catch (error) {
            const notification = 'Failed to load expenses'

            this.error = error;

            const errorStore = useErrorStore();

            errorStore.setErrorNotification(notification, error);

        } finally {
            this.loading = false;
        }
    },
    methods: {
        formatDate(date) {
            return new Date(date).toLocaleDateString();
        },
        edit(id) {
            const expenseStore = useExpensesStore();
            expenseStore.getExpenseId(id);
            // this.$router.push({ name: 'editExpense', params: { id } });
        },
        deleteExpense(id) {
            // Implement the delete functionality here.
            console.log('Delete expense with id:', id);
        },
    },
};
</script>
