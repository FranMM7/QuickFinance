<template>
    <div>
        <h1>{{ budgetTitle }}</h1>
        <hr>
        <div v-if="loading.value">
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
                    <tr v-for="expense in expenses" :key="expense.id" class="text-center">
                        <td>{{ expense.description }}</td>
                        <td>{{ expense.category }}</td>
                        <td>{{ expense.amount }}</td>
                        <td>{{ expense.formattedDueDate }}</td>
                        <td><input v-model="expense.isExecuted" class="form-check-input text-center" type="checkbox"
                                style="font-size: x-large" disabled /></td>
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
<script lang="ts">
import { defineComponent, ref, computed, onMounted } from 'vue';
import { ListLoader } from 'vue-content-loader';
import { useExpensesStore } from '@/stores/expenses';
import { useBudgetStore } from '@/stores/budgets';
import { fetchExpenses as apiExpenses, Expenses } from '@/api/services/expensesService';
import { useErrorStore } from '@/stores/error';
import Error from '../error/error.vue';

export default defineComponent({
    name: 'ExpensesList',
    components: {
        ListLoader,
        Error,
    },
    setup() {
        const expenses = ref<Expenses[]>([]); // Ensure Expense is defined
        const loading = ref(true);
        const error = ref<Error | null>(null);
        const budgetStore = useBudgetStore();
        const expenseStore = useExpensesStore();
        const errorStore = useErrorStore();
        const rowsPage = 10;
        const pageNumber = 1;
        const budgetTitle = ref<String | null>(null);

        const edit = (expenseId: number) => {
            // Logic to edit the expense
        };

        const deleteExpense = (expenseId: number) => {
            // Logic to delete the expense
        };

        onMounted(async () => {
            try {
                loading.value = true;
                const budgetId = budgetStore.getBudgetId || 0; // Handle null budgetId
                const bTitle = budgetStore.getMonth;

                const resp = await apiExpenses(budgetId, pageNumber, rowsPage);
                expenses.value = resp; // Access the expenses correctly
                budgetTitle.value = bTitle;
            } catch (err: any) {
                error.value = err;
                const notification = 'Failed to load expenses';
                errorStore.setErrorNotification(notification, err.message); // Adjust as necessary
            } finally {
                loading.value = false;
            }
        });


        return {
            expenses,
            budgetTitle,
            loading,
            error,
            pageNumber,
            rowsPage,
            edit,
            deleteExpense,
        };
    },

});
</script>
