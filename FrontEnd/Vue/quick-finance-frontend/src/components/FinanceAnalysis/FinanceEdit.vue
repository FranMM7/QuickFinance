<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import { useToast } from 'vue-toastification';
import { useRouter } from 'vue-router';
import { useFinanceStore } from '@/stores/finance';
import { editFinance, Finance, FinanceDetails, financeIncome, FinancePageResponse, saveFinanceData } from '@/api/services/financeServices';
import { title } from 'process';
import { Category, fetchCategoryList } from '@/api/services/categoryService';
import { useAuthStore } from '@/stores/auth';
import ErrorCard from '../error/errorCard.vue';

export default defineComponent({
    name: 'financeEdit',
    component: {
        ListLoader,
        ErrorCard
    },
    setup() {
        const toast = useToast()
        const router = useRouter()
        const store = useFinanceStore()
        const authStore = useAuthStore()
        const loading = ref<boolean>(true)
        const showLoader = ref<boolean>(false)
        let loaderTimeout: ReturnType<typeof setTimeout>;
        const showEditIcon = ref(false);
        const isEditing = ref(false);
        const error = ref('')

        //data
        const id = ref<number>(0)
        const financeDetails = ref<FinanceDetails[]>([])
        const financeIncomes = ref<financeIncome[]>([])
        const title = ref<string>('')
        const categories = ref<Category[]>([]);
        const totalExpenses = ref<number>(0)
        const totalIncomes = ref<number>(0)

        const expenseCategories: { [key: number]: string } = {
            1: 'Important',
            2: 'Ghost Expense',
            3: 'Ant Expense',
            4: 'Vampire Expense'
        };


        const expenseCategoryIcons: { [key: number]: string } = {
            1: 'exclamation-circle', // Icon for 'Important'
            2: 'ghost',               // Icon for 'Ghost Expense'
            3: 'bug',                 // Icon for 'Ant Expense'
            4: 'skull'                // Icon for 'Vampire Expense'
        };


        // This function returns the correct icon for a given category
        const getExpenseCategoryIcon = (category: number): string => {
            return expenseCategoryIcons[category] || 'question-circle'; // Default icon if category is missing
        };


        const showExpenseCategory = (value: number): string => {
            return expenseCategories[value] || 'N/D';
        };

        const fetchCategories = async () => {
            try {
                const userId = authStore.user?.id || ''
                const response = await fetchCategoryList(2,userId);
                categories.value = response || [];
            } catch (error) {
                console.error('Error fetching categories:', error);
            }
        };

        // Calculate the grand total based on item subtotals
        const calculateGrandTotal = () => {
            totalExpenses.value = financeDetails.value.reduce((total, item) => total + item.amount, 0);
            totalIncomes.value = financeIncomes.value.reduce((total, item) => total + item.amount, 0);
        };

        // Add a new item to the list
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
        const enableEdit = (visible: boolean) => {
            isEditing.value = visible;
        };

        const goTo = (option: string) => {
            switch (option) {
                case 'list':
                    router.push({ name: 'financeList' })
                    break;
                case 'new':
                    router.push({ name: 'financeAdd' })
                    break;
                default:
                    break;
            }
        }

        const loadPage = async () => {
            try {

                loading.value = true;
                showLoader.value = false; // Reset loader visibility
                clearTimeout(loaderTimeout); // Clear any previous timeout

                // Set a timeout to show loader only if loading takes >1 second
                loaderTimeout = setTimeout(() => {
                    if (loading.value) showLoader.value = true;
                }, 1000);

                fetchCategories()

                id.value = store.id || 0
                title.value = store.strTitle
                enableEdit(store.editMode)

                if (id.value != 0) {

                    const expenseList = store.list

                    if (expenseList.length > 0)
                        financeDetails.value = expenseList

                    const incomeLists = store.incomesList
                    if (incomeLists.length > 0)
                        financeIncomes.value = incomeLists

                    // console.log('expenses:', expenseList, 'income:', incomeLists)
                    calculateGrandTotal()
                }
                else {
                    toast.warning('Fail to retrieve data')
                    router.push({ name: 'financeList' })
                }

            } catch (error) {
                console.log('Error when loading data:', error)
                toast.error('Unexpected error occurr while loading data')
            }
            finally {
                loading.value = false;
                showLoader.value = false;
            }
        }

        const submitForm = async () => {
            try {
                const saveData: saveFinanceData = {
                    id: id.value,
                    title: title.value,
                    userId:authStore.user?.id || '',
                    financeDetails: financeDetails.value,
                    financeIncomes: financeIncomes.value
                }

                const response = await editFinance(id.value, saveData)

                // console.log(response)

                toast.success('Record has been saved!');

                await new Promise((r) => setTimeout(r, 1000));

                await router.push({ name: 'finance' })

            } catch (error) {
                console.log('Error when saving data:', error)
                toast.error('Unexpected error occurr while saving data')
            }
        }

        onMounted(async () => {
            loadPage()
        });

        return {
            showLoader,
            title,
            financeDetails,
            enableEdit,
            showEditIcon,
            isEditing,
            error,
            submitForm,
            showExpenseCategory,
            categories,
            grandTotal: totalExpenses,
            expenseCategories: Object.entries(expenseCategories), // Convert object to array of [key, value] pairs for template usage
            goTo,
            addItem,
            removeItem,
            calculateGrandTotal,
            getExpenseCategoryIcon,
            financeIncomes,
            totalIncomes
        }
    }
})
</script>

<template>
    <div v-if="showLoader">
        <ListLoader />
    </div>
    <div v-if="error">
        <Error />
    </div>
    <div v-else>
        <form @submit.prevent="submitForm">
            <div class="container">
                <div class="row">
                    <div class="col">
                        <template v-if="!isEditing">
                            <h1>{{ title }}</h1>
                            <!-- Buttons positioned below the title in view mode -->
                            <div class="row p-1">
                                <div class="col text-start">
                                    <div class="btn-group">
                                        <button class="btn btn-primary" @click="enableEdit(true)"
                                            :disabled="isEditing">Edit</button>
                                        <button class="btn btn-secondary" @click="enableEdit(false)"
                                            :disabled="!isEditing">Cancel</button>
                                        <button class="btn btn-secondary" @click="goTo('list')">List</button>
                                        <button class="btn btn-success" @click="goTo('new')">New</button>
                                    </div>
                                </div>
                            </div>
                        </template>

                        <template v-else>
                            <label for="title" class="form-label">Title:</label>
                            <div class="d-flex align-items-center">
                                <input type="text" v-model="title" id="title" class="form-control" required>
                                <!-- Buttons positioned next to the input in edit mode -->
                                <div class="btn-group">
                                    <button class="btn btn-primary" @click="enableEdit(true)"
                                        :disabled="isEditing">Edit</button>
                                    <button class="btn btn-secondary" @click="enableEdit(false)"
                                        :disabled="!isEditing">Cancel</button>
                                    <button class="btn btn-secondary" @click="goTo('list')">List</button>
                                    <button class="btn btn-success" @click="goTo('new')">New</button>
                                </div>
                            </div>
                        </template>
                    </div>

                    <div class="col text-end" v-if="!isEditing">
                        <div class="row">
                            <h4>Total Expenses: {{ grandTotal.toFixed(2) }}</h4>
                        </div>
                        <hr>
                        <div class="row">
                            <h4>Total Incomes: {{ totalIncomes.toFixed(2) }}</h4>
                        </div>
                    </div>
                </div>
            </div>


            <hr>
            <div class="container">

                <ul class="nav nav-tabs na" role="tablist">
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
                                            <option v-for="category in categories" :key="category.id"
                                                :value="category.id">
                                                {{ category.name }}
                                            </option>
                                        </select>
                                    </td>
                                    <td>
                                        <template v-if="isEditing">
                                            <select v-model="item.expenseCategory" class="form-control">
                                                <option v-for="[key, label] in expenseCategories" :key="key"
                                                    :value="key">{{
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
                                        <button type="button" class="btn btn-danger"
                                            @click="removeItem(index, 'expenses')" :disabled="!isEditing">
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
                                            <input type="number" class="form-control text-end" v-model="item.amount" step="0.01"
                                                min="0" @input="calculateGrandTotal">
                                        </template>
                                        <template v-else>
                                            {{ item.amount.toFixed(2) }}
                                        </template>
                                    </td>
                                    <td>
                                        <button type="button" class="btn btn-danger"
                                            @click="removeItem(index, 'income')" :disabled="!isEditing">
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
            <hr>
            <button type="submit" class="btn btn-primary" :disabled="!isEditing">Save</button>
        </form>
    </div>
</template>