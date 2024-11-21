<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import { useToast } from 'vue-toastification';
import { useRouter } from 'vue-router';
import { useFinanceStore } from '@/stores/finance';
import { addFinance, FinanceDetails, financeIncome, saveFinanceData } from '@/api/services/financeServices';
import { Category, fetchCategoryList } from '@/api/services/categoryService';
import { useAuthStore } from '@/stores/auth';
import ErrorCard from '../error/errorCard.vue';

export default defineComponent({
    name: 'financeAdd',
    components: {
        ListLoader,
        ErrorCard
    },
    setup() {
        const toast = useToast();
        const router = useRouter();
        const store = useFinanceStore();
        const storeAuth = useAuthStore()
        const loading = ref<boolean>(true);
        const showLoader = ref<boolean>(false);
        let loaderTimeout: ReturnType<typeof setTimeout>;
        const showEditIcon = ref(false);
        const isEditing = ref(true); // Always in edit mode for adding
        const error = ref('');

        // Data
        const financeDetails = ref<FinanceDetails[]>([]);
        const financeIncomes = ref<financeIncome[]>([]);
        const title = ref<string>('');
        const categories = ref<Category[]>([]);
        const totalExpenses = ref<number>(0);
        const totalIncomes = ref<number>(0);

        const expenseCategories: { [key: number]: string } = {
            1: 'Important',
            2: 'Ghost Expense',
            3: 'Ant Expense',
            4: 'Vampire Expense'
        };

        const expenseCategoryIcons: { [key: number]: string } = {
            1: 'exclamation-circle',
            2: 'ghost',
            3: 'bug',
            4: 'skull'
        };

        const getExpenseCategoryIcon = (category: number): string => {
            return expenseCategoryIcons[category] || 'question-circle';
        };

        const showExpenseCategory = (value: number): string => {
            return expenseCategories[value] || 'N/D';
        };

        const fetchCategories = async () => {
            try {
                const userId = storeAuth.user?.id || ''
                const response = await fetchCategoryList(2,userId);
                categories.value = response || [];
            } catch (error) {
                console.error('Error fetching categories:', error);
            }
        };

        const calculateGrandTotal = () => {
            totalExpenses.value = financeDetails.value.reduce((total, item) => total + item.amount, 0);
            totalIncomes.value = financeIncomes.value.reduce((total, item) => total + item.amount, 0);
        };

        const addItem = (table: string) => {
            if (table === 'expenses') {
                financeDetails.value.push({
                    description: '',
                    expenseCategory: 1,
                    amount: 0,
                    categoryId: 0
                });
            } else {
                financeIncomes.value.push({
                    description: '',
                    amount: 0
                });
            }
        };

        const removeItem = (index: number, table: string) => {
            if (table === 'expenses') {
                financeDetails.value.splice(index, 1);
            } else {
                financeIncomes.value.splice(index, 1);
            }
        };

        const goTo = (option: string) => {
            if (option === 'list') {
                router.push({ name: 'financeList' });
            }
        };

        const loadPage = async () => {
            try {
                loading.value = true;
                showLoader.value = false;
                clearTimeout(loaderTimeout);
                loaderTimeout = setTimeout(() => {
                    if (loading.value) showLoader.value = true;
                }, 1000);

                await fetchCategories();
                financeDetails.value = [];
                addItem('expenses')
                financeIncomes.value = [];
                addItem('income')
            } catch (error) {
                console.error('Error loading page:', error);
                toast.error('Unexpected error occurred while loading data');
            } finally {
                loading.value = false;
                showLoader.value = false;
            }
        };

        const submitForm = async () => {
            try {
                const saveData: saveFinanceData = {
                    id: 0,
                    title: title.value,
                    userId:storeAuth.user?.id || '',
                    financeDetails: financeDetails.value,
                    financeIncomes: financeIncomes.value
                };

                if (!saveData.userId) throw new Error('Unable to retreive the userId, please login agian and try again.')
                const response = await addFinance(saveData);
                // console.log(response);

                toast.success('New record has been added!');
                await new Promise((resolve) => setTimeout(resolve, 1000));
                await router.push({ name: 'financeList' });
            } catch (error) {
                console.error('Error when saving data:', error);
                toast.error('Unexpected error occurred while saving data');
            }
        };

        onMounted(async () => {
            await loadPage();
        });

        return {
            showLoader,
            title,
            financeDetails,
            isEditing,
            error,
            submitForm,
            showExpenseCategory,
            categories,
            grandTotal: totalExpenses,
            expenseCategories: Object.entries(expenseCategories),
            goTo,
            addItem,
            removeItem,
            calculateGrandTotal,
            getExpenseCategoryIcon,
            financeIncomes,
            totalIncomes
        };
    }
});
</script>

<template>
    <form @submit.prevent="submitForm">
        <div class="container">
            <div class="row">
                <div class="col">
                    <label for="title" class="form-label">Title:</label>
                    <input type="text" v-model="title" id="title" class="form-control" required>
                </div>
            </div>
        </div>
        <hr>

        <!-- Tabs and Tables for Expenses and Incomes as in the financeEdit component -->
        <div class="container">

            <ul class="nav nav-tabs nav-fill" role="tablist">
                <li class="nav-item" role="presentation">
                    <a class="nav-link active" data-bs-toggle="tab" href="#expenses" aria-selected="true"
                        role="tab">Expenses</a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" data-bs-toggle="tab" href="#incomes" aria-selected="false"
                        role="tab">Incomes</a>
                </li>
            </ul>

            <div class="tab-content" id="tabContent">
                <!-- expenses table  -->
                <div class="tab-pane fade show active" id="expenses" role="tabpanel">

                    <table class="table table-striped text-center" v-if="financeDetails.length > 0">
                        <thead>
                            <tr>
                                <td>Description</td>
                                <td>Category</td>
                                <td>Expense Type</td>
                                <td></td>
                                <td>Amount</td>
                                <td>-</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(item, index) in financeDetails" :key="index">
                                <td>
                                    <template v-if="isEditing">
                                        <input type="text" class="form-control" v-model="item.description">
                                    </template>
                                    <template v-else>
                                        {{ item.description }}
                                    </template>
                                </td>
                                <td>
                                    <select v-model="item.categoryId" class="form-control" :disabled="!isEditing">
                                        <option v-for="category in categories" :key="category.id" :value="category.id">
                                            {{ category.name }}
                                        </option>
                                    </select>
                                </td>
                                <td>
                                    <template v-if="isEditing">
                                        <select v-model="item.expenseCategory" class="form-control">
                                            <option v-for="[key, label] in expenseCategories" :key="key" :value="key">{{
                                                label }}</option>
                                        </select>
                                    </template>
                                    <template v-else>
                                        {{ showExpenseCategory(item.expenseCategory) }}
                                    </template>
                                </td>
                                <td>
                                    <template v-if="!isEditing">
                                        <font-awesome-icon
                                            :icon="['fas', getExpenseCategoryIcon(item.expenseCategory)]" />
                                    </template>
                                </td>

                                <td>
                                    <template v-if="isEditing">
                                        <input v-model="item.amount" class="form-control text-end" type="number"
                                            step="0.01" min="0" @input="calculateGrandTotal" />
                                    </template>
                                    <template v-else>
                                        {{ item.amount.toFixed(2) }}
                                    </template>
                                </td>
                                <td>
                                    <button type="button" class="btn btn-danger" @click="removeItem(index, 'expenses')"
                                        :disabled="!isEditing">
                                        <font-awesome-icon :icon="['fas', 'trash']" />
                                    </button>
                                </td>
                            </tr>

                            <!-- total -->
                            <tr class="table table-info">
                                <td colspan="4">Gran Total:</td>
                                <td>{{ grandTotal.toFixed(2) }}</td>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>
                    <!-- Add New Expense Button -->
                    <div class="mt-3">
                        <button type="button" class="btn btn-primary" @click="addItem('expenses')"
                            :disabled="!isEditing">Add
                            Expense</button>
                    </div>



                </div>

                <!-- income table -->
                <div class="tab-pane fade" id="incomes" role="tabpanel">
                    <table class="table table-striped text-center" v-if="financeIncomes.length > 0">
                        <thead>
                            <tr>
                                <td>Description</td>
                                <td>Amount</td>
                                <td>-</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(item, index) in financeIncomes" :key="index">
                                <td>
                                    <template v-if="isEditing">
                                        <input type="text" class="form-control" v-model="item.description">
                                    </template>
                                    <template v-else>
                                        {{ item.description }}
                                    </template>
                                </td>
                                <td>
                                    <template v-if="isEditing">
                                        <input type="number" class="form-control text-end" v-model="item.amount"
                                            step="0.01" min="0" @input="calculateGrandTotal">
                                    </template>
                                    <template v-else>
                                        {{ item.amount.toFixed(2) }}
                                    </template>
                                </td>
                                <td>
                                    <button type="button" class="btn btn-danger" @click="removeItem(index, 'income')"
                                        :disabled="!isEditing">
                                        <font-awesome-icon :icon="['fas', 'trash']" />
                                    </button>
                                </td>
                            </tr>
                            <tr class="table table-info">
                                <td>Grand Total:</td>
                                <td>{{ totalIncomes.toFixed(2) }}</td>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>

                    <!-- Add New Income Button -->
                    <div class="mt-3">
                        <button type="button" class="btn btn-primary" @click="addItem('income')"
                            :disabled="!isEditing">Add
                            Income</button>
                    </div>
                </div>
            </div><!-- end of tab content -->

        </div><!-- container -->

        <hr />
        <div class="btn btn-group">
            <button type="submit" class="btn btn-primary">Save</button>
            <button type="button" class="btn btn-secondary" @click="goTo('list')">Cancel</button>
        </div>
    </form>

</template>
