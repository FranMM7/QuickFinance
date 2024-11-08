<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useToast } from 'vue-toastification';
import { useRouter } from 'vue-router';
import { useFinanceStore } from '@/stores/finance';
import { Finance, FinanceDetails } from '@/api/services/financeServices';
import { title } from 'process';
import { Category, fetchCategoryList } from '@/api/services/categoryService';

export default defineComponent({
    name: 'financeEdit',
    component: {
        ListLoader,
        Error
    },
    setup() {
        const toast = useToast()
        const router = useRouter()
        const store = useFinanceStore()
        const loading = ref<boolean>(true)
        const showLoader = ref<boolean>(false)
        let loaderTimeout: ReturnType<typeof setTimeout>;
        const showEditIcon = ref(false);
        const isEditing = ref(false);
        const error = ref('')

        //data
        const financeData = ref<Finance>();
        const financeDetails = ref<FinanceDetails[]>([])
        const title = ref<string>('')
        const categories = ref<Category[]>([]);
        const grandTotal = ref<number>(0)

        const expenseCategories: { [key: number]: string } = {
            1: 'Important',
            2: 'Ghost Expense',
            3: 'Ant Expense',
            4: 'Vampire Expense'
        };

        const showExpenseCategory = (value: number): string => {
            return expenseCategories[value] || 'N/D';
        };

        const fetchCategories = async () => {
            try {
                const response = await fetchCategoryList(2);
                categories.value = response || [];
            } catch (error) {
                console.error('Error fetching categories:', error);
            }
        };

        // Calculate the grand total based on item subtotals
        const calculateGrandTotal = () => {
            grandTotal.value = financeDetails.value.reduce((total, item) => total + item.amount, 0);
        };
        // Add a new item to the list
        const addItem = () => {
            financeDetails.value.push({
                description: '',
                expenseCategory: 1,
                amount: 0,
                categoryId: 0
            });
        };

        const removeItem = (index: number) => {
            financeDetails.value.splice(index, 1);
        };
        const enableEdit = (visible: boolean) => {
            isEditing.value = visible;
        };

        const goTo = () => {
            router.push({ name: 'financeList' })
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

                const id = store.id || 0
                title.value = store.strTitle
                enableEdit(store.editMode)

                if (id != 0) {
                    const list = store.list
                    console.log('list:', list)
                    if (list.length > 0)
                        financeDetails.value = list

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
                // const updatedOn = new Date()
                // const editedRecord: shoppingDataSave = {
                //     id: shoppingData.value?.id || 0,
                //     description: description.value,
                //     state: shoppingData.value?.state || 1,
                //     updatedOn: updatedOn,
                //     ShoppingLists: itemsList.value

                // }

                // // console.log('submit form:', editedRecord)

                // await editShopping(editedRecord.id, editedRecord)

                // toast.success('Record has been saved!');
                // await new Promise((r) => setTimeout(r, 1000));
                // await router.push('/Shopping');
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
            grandTotal,
            expenseCategories: Object.entries(expenseCategories), // Convert object to array of [key, value] pairs for template usage
            goTo,
            addItem,
            removeItem,
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
            <div class="row">
                <div class="col">
                    <div class="d-flex align-items-center">
                        <h1 v-if="!isEditing">{{ title }}</h1>
                        <input v-else type="text" v-model="title" class="form-control" required>
                    </div>
                </div>
                <div class="col-auto">
                    <div class="btn-group">
                        <button class="btn btn-primary" @click="enableEdit(true)" :disabled="isEditing">Edit</button>
                        <button class="btn btn-secondary" @click="enableEdit(false)"
                            :disabled="!isEditing">Cancel</button>
                        <button class="btn btn-secondary" @click="goTo">List</button>
                    </div>
                </div>
            </div>
            <hr>
            <div>
                <table class="table table-stripted" v-if="financeDetails.length > 0">
                    <thead>
                        <tr>
                            <td>Description</td>
                            <td>Category</td>
                            <td>Expense Type</td>
                            <td>Amount</td>
                            <td>-</td>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- <tr v-for="(item, index) in itemsList" :key="index"> -->
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
                            <td>{{ item.amount.toFixed(2) }}</td>
                            <td>
                                <button type="button" class="btn btn-danger" @click="removeItem(index)" :disabled="!isEditing">
                                    <font-awesome-icon :icon="['fas', 'trash']" />
                                </button>
                            </td>
                        </tr>

                        <!-- total -->
                        <tr class="table-info">
                            <td colspan="3">Gran Total:</td>
                            <td>{{ grandTotal.toFixed(2) }}</td>
                            <td></td>
                        </tr>
                    </tbody>
                </table>


                <!-- Add New Expense Button -->
                <div class="mt-3">
                    <button type="button" class="btn btn-primary" @click="addItem" :disabled="!isEditing">Add Expense</button>
                </div>
            </div>

            <hr>
            <button type="submit" class="btn btn-primary" :disabled="!isEditing">Save</button>
        </form>
    </div>
</template>