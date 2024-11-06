<template>
    <div class="container">
        <div v-if="showLoader">
            <ListLoader />
        </div>
        <div v-if="error">
            <Error />
        </div>
        <div v-else>
            <form @submit.prevent="submitForm">


                <div class="row">
                    <div class="col-auto">
                        <h1>{{ shoppingData?.description }}</h1>
                        <h4>Modified On: {{ formatDate(String(shoppingData?.modifiedOn)) }}</h4>
                    </div>
                    <div class="col text-end">
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
                                <input type="number" class="form-control text-end" step="1" min="1"
                                    v-model="item.quantity" @input="calculateSubtotal(item)" />
                            </td>
                            <td>
                                <input v-model="item.amount" class="form-control text-end" type="number" step="0.01"
                                    min="0" @input="calculateSubtotal(item)" />
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

        </div> <!-- else -->
    </div> <!-- container -->
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useShoppingStore } from '@/stores/shopping';
import { editShopping, getShoppingById, ShoppingData, shoppingDataSave, ShoppingList } from '@/api/services/shoppingServices';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { useErrorStore } from '@/stores/error';
import { Category, categoryList, fetchCategoryList } from '@/api/services/categoryService';
import { fetchlocation, location } from '@/api/services/locationServices';
import { formatDate } from '@/api/services/generalService';

export default defineComponent({
    name: 'ShoppingEdit',
    components: {
        ListLoader,
        Error
    },
    setup() {
        const loading = ref<boolean>(true);
        const showLoader = ref<boolean>(false);
        let loaderTimeout: ReturnType<typeof setTimeout>;
        const error = ref<string>('');
        const router = useRouter();
        const toast = useToast();

        //data
        const shoppingData = ref<ShoppingData>();
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
                const response = await fetchlocation();
                locations.value = response || []
            } catch (error) {
                console.error('Error fetching locations:', error);
            }
        };

        // Load shopping record data
        const loadPage = async () => {
            try {
                loading.value = true;
                showLoader.value = false; // Reset loader visibility
                clearTimeout(loaderTimeout); // Clear any previous timeout

                // Set a timeout to show loader only if loading takes >1 second
                loaderTimeout = setTimeout(() => {
                    if (loading.value) showLoader.value = true;
                }, 1000);

                const shoppingStore = useShoppingStore();
                const shoppingId = shoppingStore.getShoppingId;

                fetchCategories();
                fetchLocations();

                if (shoppingId !== null) {
                    const res = await getShoppingById(shoppingId);
                    shoppingData.value = res.shoppingData;
                    itemsList.value = res.data.$values ?? []; // Extract the array of ShoppingList items

                    // Calculate initial grand total
                    itemsList.value.forEach(item => {
                        item.subTotal = item.quantity * item.amount;
                    });
                    calculateGrandTotal();

                } else {
                    toast.error('ID was not retrieved');
                    router.back();
                }
            } catch (err) {
                error.value = 'Failed to load Shopping Details';
                console.error('Error:', err);
                const errorStore = useErrorStore();
                errorStore.setErrorNotification(error.value, String(err));
            } finally {
                loading.value = false;
            }
        };

        const submitForm = async () => {
            try {
                const updatedOn = new Date()
                const editedRecord: shoppingDataSave = {
                    id: shoppingData.value?.id || 0,
                    description: shoppingData.value?.description || '',
                    state: shoppingData.value?.state || 1,
                    updatedOn: updatedOn,
                    ShoppingLists: itemsList.value

                }

                console.log('submit form:', editedRecord)

                await editShopping(editedRecord.id, editedRecord)

                toast.success('Record has been saved!');
                await new Promise((r) => setTimeout(r, 1000));
                await router.push('/Shopping');
            } catch (error) {
                console.error('Error adding budget:', error);
                toast.error('Failed to save the record.');
            }
        }

        onMounted(() => {
            loadPage();
        });

        return {
            loading,
            showLoader,
            error,
            shoppingData,
            itemsList,
            grandTotal,
            categories,
            locations,
            addItem,
            calculateSubtotal,
            cancel,
            removeItem,
            formatDate,
            submitForm
        };
    }
});
</script>
