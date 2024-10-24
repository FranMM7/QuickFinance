<template>
    <div class="container mt-4">
        <div v-if="loading">
            <list-loader />
        </div>
        <div v-else-if="error">
            <Error />
        </div>
        <div v-else class="row">
            <div class="col">
                <h1>{{ budget.id }} - {{ budget.title }}</h1>
            </div>
            <div class="col-auto text-lg-end">
                <div class="row">Total Allocated Budget: {{ budget.totalAllocatedBudget }}</div>
                <div class="row">Balance: {{ totalBalance }}</div>
            </div>
            <hr>

            <!-- table paginated expenses  -->
            <table class="table table-striped">
                <thead class="table-primary">
                    <tr class="text-center">
                        <td>Category</td>
                        <td>Description</td>
                        <td>Amount</td>
                        <td>Due Date</td>
                        <td>Payment Type</td>
                        <td>Is executed</td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="record in expenses" :key="record.id">
                        <td>{{ record.category }}</td>
                        <td>{{ record.description }}</td>
                        <td class="text-end">{{ record.amount }}</td>
                        <td class="text-center">{{ fortmatDate(String(record.expenseDueDate)) }}</td>
                        <td class="text-end">{{ record.paymentMethod }}</td>
                        <td class="text-center">
                            <input v-model="record.executed" class="form-check-input" type="checkbox" />
                        </td>
                    </tr>
                    <tr class="table-info">
                        <td colspan="2">Total:</td>
                        <td class="text-end">{{ sumExpenses }}</td>
                        <td colspan="3"></td>
                    </tr>
                </tbody>
            </table>

            <!-- Pagination Component -->
            <div class="row">
                <div class="col-auto">

                    <ul class="pagination">
                        <li :class="['page-item', { disabled: pageNumber === 1 }]">
                            <a class="page-link" href="#" @click="changePage(pageNumber - 1)" aria-label="Previous">
                                <span aria-hidden="true">&laquo;</span>
                            </a>
                        </li>
                        <li v-for="page in totalPages" :key="page"
                            :class="['page-item', { active: pageNumber === page }]">
                            <a class="page-link" href="#" @click="changePage(page)">{{ page }}</a>
                        </li>
                        <li :class="['page-item', { disabled: pageNumber === totalPages }]">
                            <a class="page-link" href="#" @click="changePage(pageNumber + 1)" aria-label="Next">
                                <span aria-hidden="true">&raquo;</span>
                            </a>
                        </li>
                    </ul>
                </div>

                <!-- Row Selection Dropdown -->
                <div class="col-auto text-sm-end">
                    <div class="row mb-3">
                        <div class="col-auto text-end text-primary">
                            <!-- <label for="rowsPerPage">Rows per page:</label> -->
                            <select id="rowsPerPage" v-model="rowsPage" @change="loadPage" class="form-select ">
                                <option :value="5">5</option>
                                <option :value="10">10</option>
                                <option :value="20">20</option>
                                <option :value="50">50</option>
                            </select>
                        </div>
                    </div>
                </div>

            </div><!-- end pagination -->

        </div><!-- row -->

    </div><!-- container mt-4 -->
</template>

<script lang="ts">
import { Budget, getBudget } from '@/api/services/budgetService';
import { Expenses, fetchExpenses } from '@/api/services/expensesService';
import { useBudgetStore } from '@/stores/budgets';
import { defineComponent, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useErrorStore } from '@/stores/error';
import { paginationInfo } from '@/api/services/generalService';

export default defineComponent({
    name: "BudgetExpenses",
    components: {
        ListLoader,
        Error,
    },
    setup() {

        const loading = ref<boolean>(true);
        const error = ref<String>('');
        const router = useRouter();
        const toast = useToast();
        const budgetStore = useBudgetStore();

        const budget = ref<Budget>({
            id: 0,
            title: '',
            totalAllocatedBudget: 0,
            state: 1
        });

        const expenses = ref<Expenses[]>([]);
        const totalBalance = ref<number>(0);
        const sumExpenses = ref<number>(0);
        const pageNumber = ref<number>(1);
        const rowsPage = ref<number>(10);
        const totalPages = ref<number>(1);

        // Function to calculate the total values
        const calculateValues = () => {
            try {
                if (Array.isArray(expenses.value)) {
                    const total = budget.value.totalAllocatedBudget || 0;
                    sumExpenses.value = expenses.value.reduce((bal, expense) => bal + expense.amount, 0);
                    totalBalance.value = total - sumExpenses.value;
                }
            } catch (error) {
                sumExpenses.value = 0;
                toast.error(error);
            }
        };

        // Function to format dates
        const fortmatDate = (dateString: string) => {
            const date = new Date(dateString);
            return date.toLocaleDateString();
        };

        // Function to handle page changes
        const changePage = (page: number) => {
            if (page >= 1 && page <= totalPages.value) {
                pageNumber.value = page;
                loadPage();
            }
        };

        // Function to load the page with expenses
        const loadPage = async () => {
            try {
                loading.value = true;
                await new Promise(resolve => setTimeout(resolve, 1000)); // Show the notification for 2 seconds

                const id = budgetStore.getBudgetId || 0;
                if (id === 0) {
                    toast.warning('Unable to retrieve ID');
                    await router.push('/budgets');
                } else {
                    const record = await getBudget(id);
                    budget.value = record;

                    // Fetch expenses directly as an array
                    expenses.value = await fetchExpenses(id, pageNumber.value, rowsPage.value);
                    totalPages.value = await paginationInfo(rowsPage.value, 'Expenses');

                    console.log('details:', {
                        record,
                        expenses: expenses.value,
                        totalPages: totalPages.value,
                        pageNumber: pageNumber.value,
                    });

                    calculateValues();
                }
            } catch (err) {
                error.value = 'Failed to load Budget Expenses Details';
                console.log('Error msg:', err);
                const errorStore = useErrorStore();
                errorStore.setErrorNotification(String(error.value), String(error));
            } finally {
                loading.value = false;
            }
        };

        // Initialized on mounted
        onMounted(() => {
            loadPage();
        });

        return {
            budget,
            expenses,
            loading,
            error,
            totalBalance,
            sumExpenses,
            pageNumber,
            rowsPage,
            totalPages,
            fortmatDate,
            changePage, // Add changePage function here to use it in your template
            loadPage,
        };
    }
});
</script>
