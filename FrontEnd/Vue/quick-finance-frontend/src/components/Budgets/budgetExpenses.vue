<template>
    <div class="container mt-4">
        <div v-if="loading">
            <list-loader />
        </div>
        <div v-else-if="error">
            <Error />
        </div>
        <div v-else class="row">

            <div class="col" id="print">
                <h1>{{ budget.id }} - {{ budget.title }}</h1>
            </div>
            <div class="col-auto text-lg-end">
                <div class="row" style="text-align: right;">Total Allocated Budget: {{
                    budget.totalAllocatedBudget.toFixed(2)
                    }}
                </div>
                <div class="row" style="text-align: right;">Balance: {{ totalBalance.toFixed(2) }}</div>
            </div>

            <hr>

            <!-- table paginated expenses  -->
            <table class="table table-striped" id="printTable">
                <thead class="table-primary">
                    <tr class="text-center">
                        <td>Category</td>
                        <td>Description</td>
                        <td>Amount</td>
                        <td>Due Date</td>
                        <td>Payment Type</td>
                        <td>Is Executed</td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="record in expenses" :key="record.id">
                        <td>{{ record.category }}</td>
                        <td>{{ record.description }}</td>
                        <td class="text-end">{{ record.amount.toFixed(2) }}</td>
                        <td class="text-center">{{ fortmatDate(String(record.expenseDueDate)) }}</td>
                        <td class="text-end">{{ record.paymentMethod }}</td>
                        <td class="text-center">
                            <input v-model="record.isExecuted" class="form-check-input" type="checkbox" disabled="true" />
                        </td>
                    </tr>
                    <tr class="table-info">
                        <td colspan="2">Total:</td>
                        <td class="text-end">{{ sumExpenses.toFixed(2) }}</td>
                        <td colspan="3"></td>
                    </tr>
                </tbody>
            </table>
            <!-- Pagination Component -->
            <div class="d-flex justify-content-center mt-4"> <!-- Center the pagination -->
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

                    <div class="col-auto">
                        <div class="btn-group" role="group" aria-label="Basic example">
                            <button @click="edit(budget.id)" type="button" class="btn btn-secondary">Edit</button>
                            <button @click="print()" type="button" class="btn btn-info">Print</button>
                            <button @click="cancel()" type="button" class="btn btn-danger">Cancel</button>
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

        const cancel = () => {
            router.back();
        };

        const edit = (id: number) => {
            const storeBudget = useBudgetStore();
            storeBudget.setBudgetId(id);
            router.push({ name: 'editBudget' });
        };

        const print = () => {
            // Open a new window
            const printWindow = window.open('', '_blank');

            // Check if the print window opened successfully
            if (printWindow) {
                // Create print styles to hide everything except #print
                const printStyles = `
      <style>
        @media print {
          body * { visibility: hidden; }
          #print, #print * { visibility: visible; }
          #print { position: absolute; top: 0; left: 0; }
        }
      </style>
    `;

                // Inject print styles and content into the new window
                printWindow.document.write(`
      <html>
        <head>
          <title>Print Budget</title>
          ${printStyles}
        </head>
        <body>
          ${document.getElementById('print')?.outerHTML}
          ${document.getElementById('printTable')?.outerHTML}
        </body>
      </html>
    `);
                printWindow.document.close();
                printWindow.print();
            } else {
                console.error("Failed to open print window. Please check popup blocker settings.");
            }
        };



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
                    totalPages.value = await paginationInfo(rowsPage.value, 'Expenses', id);

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
            cancel,
            edit,
            print
        };
    }
});
</script>
