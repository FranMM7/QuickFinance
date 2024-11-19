<template>
    <form @submit.prevent="submitForm">


        <div class="row">
            <div class="col mt-3">
                <fieldset>
                    <label class="form-label" for="title">Title</label>
                    <input class="form-control" id="title" type="text" v-model="description" placeholder="Enter title"
                        required />
                </fieldset>
            </div>
            <div class="col text-end py-4">
                <h1>Total: {{ grandTotal }}</h1>
            </div>
        </div>
        <hr>

        <table class="table table-striped text-center">
            <thead>
                <tr>
                    <td>Location</td>
                    <td>Category</td>
                    <td style="width: 25%">Description</td>
                    <td>Brand</td>
                    <td style="width: 10%;">Qty</td>
                    <td style="width: 13%;">Amount</td>
                    <td>SubTotal</td>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in itemsList" :key="index">
                    <td>
                        <select v-model="item.locationId" class="form-control">
                            <option v-for="location in locations" :key="location.id" :value="location.id">
                                {{ location.name }}
                            </option>

                        </select>
                    </td>
                    <td>
                        <select v-model="item.categoryId" class="form-control">
                            <option v-for="category in categories" :key="category.id" :value="category.id">
                                {{ category.name }}
                            </option>
                        </select>
                    </td>
                    <td>
                        <input type="text" class="form-control" v-model="item.itemName">
                    </td>
                    <td>
                        <input type="text" class="form-control" v-model="item.brand">
                    </td>
                    <td>
                        <input type="number" class="form-control text-end" step="1" min="1" v-model="item.quantity"
                            @input="calculateSubtotal(item)" />
                    </td>
                    <td>
                        <input v-model="item.amount" class="form-control text-end" type="number" step="0.01" min="0"
                            @input="calculateSubtotal(item)" />
                    </td>
                    <td>{{ item.subTotal.toFixed(2) }}</td>
                    <td>
                        <button type="button" class="btn btn-danger" @click="removeItem(index)">
                            <font-awesome-icon :icon="['fas', 'trash']" />
                        </button>
                    </td>
                </tr>

                <!-- Grand Total row -->
                <tr class="table-info">
                    <td :rowspan="1" colspan="6" class="text-left">Grand Total</td>
                    <td class="text-end">{{ grandTotal.toFixed(2) }}</td>
                </tr>
            </tbody>
        </table>


        <!-- Add New Expense Button -->
        <div class="mt-3">
            <button type="button" class="btn btn-primary" @click="addItem">Add Expense</button>
        </div>

        <!-- Submit Button -->
        <hr>
        <div class="container">
            <div class="row justify-content-start mt-4">
                <div class="col-auto">
                    <button type="submit" class="btn btn-success">Submit</button>
                </div>
                <div class="col-auto">
                    <button @click="cancel()" type="button" class="btn btn-secondary">Cancel</button>
                </div>
            </div>
        </div>

    </form>
</template>


<script lang="ts">
import { useErrorStore } from '@/stores/error';
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useShoppingStore } from '@/stores/shopping';
import { addShopping, getShoppingById, ShoppingData, shoppingDataSave, ShoppingList } from '@/api/services/shoppingServices';
import { useRoute, useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { Category, fetchCategoryList } from '@/api/services/categoryService';
import { fetchlocation, location } from '@/api/services/locationServices';
import { formatDate } from '@/api/services/generalService';
import { useAuthStore } from '@/stores/auth';

export default defineComponent({
    name: 'ShoppingAdd',
    components: {
        ListLoader,
        Error
    },
    setup() {
        const loading = ref<Boolean>(true);
        const error = ref<string>('');
        const router = useRouter();
        const toast = useToast();
        const store = useAuthStore()

        //data
        const Id = ref<number>(0);
        const description = ref<string>('')
        const itemsList = ref<ShoppingList[]>([]);
        const grandTotal = ref<number>(0);
        const categories = ref<Category[]>([])
        const locations = ref<location[]>([])

        // Methods
        const cancel = () => {
            router.back();
        };

        // Calculate subtotal for an item
        const calculateSubtotal = (item: ShoppingList) => {
            item.subTotal = item.quantity * item.amount;
            calculateGrandTotal();
        };

        // Calculate the grand total based on item subtotals
        const calculateGrandTotal = () => {
            grandTotal.value = itemsList.value.reduce((total, item) => total + item.subTotal, 0);
        };

        // Add a new item to the list
        const addItem = () => {
            itemsList.value.push({
                id: 0,
                locationId: 0,
                location: '',
                categoryId: 0,
                category: '',
                itemName: '',
                brand: '',
                quantity: 1,
                amount: 0,
                subTotal: 0,
            });
        };

        const removeItem = (index: number) => {
            itemsList.value.splice(index, 1);
        };


        const fetchCategories = async () => {
            try {
                const response = await fetchCategoryList(3);
                categories.value = response || [];
            } catch (error) {
                console.error('Error fetching categories:', error);
            }
        };

        const fetchLocations = async () => {
            try {
                const response = await fetchlocation(store.user?.id || '');
                locations.value = response || []
            } catch (error) {
                console.error('Error fetching locations:', error);
            }
        };


        const loadPage = async () => {
            try {
                loading.value = true;
                addItem()
                fetchCategories();
                fetchLocations();
            } catch (err) {
                error.value = 'Failed to Shopping Details';
                console.log('Error msg:', err);
                const errorStore = useErrorStore();
                errorStore.setErrorNotification(String(error.value), String(error));
            } finally {
                loading.value = false;
            }
        };

        const submitForm = async () => {
            try {
                const userId = store.user?.id || ''
                if (!userId) throw new Error('Unable to retreive userId, please login again and try again.')
                const updatedOn = new Date()
                const newRecord: shoppingDataSave = {
                    id: 0,
                    description: description.value,
                    state: 1,
                    updatedOn: updatedOn,
                    userId: userId,
                    ShoppingLists: itemsList.value
                }

                console.log('submit form:', newRecord)

                await addShopping(newRecord)

                toast.success('Record has been saved!');
                await new Promise((r) => setTimeout(r, 1000));
                await router.push('/Shopping');

            } catch (error) {
                console.error('Error adding budget:', error);
                toast.error('Failed to save the record.');
            }
        };

        onMounted(() => {
            loadPage();
        });

        return {
            loading,
            error,
            Id,
            description,
            itemsList,
            grandTotal,
            categories,
            locations,
            submitForm,
            formatDate,
            cancel,
            addItem,
            removeItem,
            calculateSubtotal
        };
    }
});
</script>
