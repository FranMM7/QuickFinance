<template>
    <div class="container">
        <div v-if="loading">
            <ListLoader />
        </div>
        <div v-if="error">
            <Error />
        </div>
        <div v-else>
            <div class="row">
                <div class="col-auto">
                    <h1>{{ shoppingData?.description }}</h1>
                </div>
                <div class="col text-end">
                    <h1>Total: {{ grandTotal }}</h1>
                </div>
            </div>
            <hr>

            <table class="table table-striped text-center">
                <thead>
                    <tr>
                        <td>Category</td>
                        <td>Description</td>
                        <td>Qty</td>
                        <td>Amount</td>
                        <td>SubTotal</td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="group in groupedData" :key="group.category">
                        <!-- Category row -->
                        <td :rowspan="group.items.length + 1">{{ group.category }}</td>
                        <td colspan="4"></td>
                        <!-- Item rows for each category -->
                <tbody>
                    <tr v-for="item in group.items" :key="item.id">
                        <td>{{ item.itemName }}</td>
                        <td>{{ item.quantity }}</td>
                        <td>{{ item.amount }}</td>
                        <td>{{ item.subTotal }}</td>
                    </tr>
                </tbody>
                </tr>
                <!-- Grand Total row -->
                <tr>
                    <td colspan="4" class="text-left">Grand Total</td>
                    <td class="text-end">{{ grandTotal }}</td>
                </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="ts">
import { useErrorStore } from '@/stores/error';
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useShoppingStore } from '@/stores/shopping';
import { getShoppingById, ShoppingData, ShoppingList } from '@/api/services/shoppingServices';

export default defineComponent({
    name: 'ShoppingEdit',
    components: {
        ListLoader,
        Error
    },
    setup() {
        const loading = ref<boolean>(true);
        const error = ref<string>('');
        const shoppingData = ref<ShoppingData>();
        const groupedData = ref<{ category: string, items: any[] }[]>([]);
        const grandTotal = ref<number>(0);
        // const shoppingRecord = ref<any>({});

        const groupByCategory = (data: ShoppingList[]) => {
            const groups: { [key: string]: ShoppingList[] } = {}; // Define groups to hold ShoppingList items

            // Iterate over each item in the data
            data.forEach(item => {
                const category = item.category || 'N/D'; // Use 'N/D' if category is null
                if (!groups[category]) {
                    groups[category] = []; // Initialize the array for this category
                }
                groups[category].push(item); // Add item to the corresponding category group
            });

            // Convert to an array of objects with category and items
            groupedData.value = Object.keys(groups).map(category => ({
                category,
                items: groups[category],
            }));

            // Calculate grand total by summing the subTotal of each item
            grandTotal.value = data.reduce((total, item) => total + item.subTotal, 0);
        };

        // Load shopping record data
        const loadPage = async () => {
            try {
                loading.value = true;
                const shoppingStore = useShoppingStore();
                const shoppingId = shoppingStore.getShoppingId;

                if (shoppingId !== null) {
                    const res = await getShoppingById(shoppingId);
                    console.log('res', res.shoppingData)
                    shoppingData.value = res.shoppingData
                    // Ensure you're accessing the correct array
                    const shoppingItems = res.data; // This is an array of ShoppingListItem
                    groupByCategory(shoppingItems); // Pass the array to the group function
                } else {
                    throw new Error('No shopping ID provided');
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

        onMounted(() => {
            loadPage();
        });

        return {
            loading,
            error,
            shoppingData,
            groupedData,
            grandTotal,
        };
    }
});
</script>
