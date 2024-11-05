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
                        <td>Brand</td>
                        <td>Description</td>
                        <td>Qty</td>
                        <td>Amount</td>
                        <td>SubTotal</td>
                    </tr>
                </thead>
                <tbody>
                    <slot v-for="group in groupedData" :key="group.category">

                        <!-- Category row -->
                        <tr>
                            <td :rowspan="group.items.length + 1">{{ group.category }}</td>
                        </tr>

                        <!-- Item rows for each category -->
                        <tr v-for="item in group.items" :key="item.id">
                            <td>{{ item.brand }}</td>
                            <td>{{ item.itemName }}</td>
                            <td>{{ item.quantity }}</td>
                            <td>{{ item.amount }}</td>
                            <td>{{ item.subTotal }}</td>
                        </tr>

                    </slot>
                    <!-- Grand Total row -->
                    <tr>
                        <td :rowspan="1" colspan="5" class="text-left">Grand Total</td>
                        <td class="text-end">{{ grandTotal }}</td>
                    </tr>
                </tbody>

            </table>
            <!-- buttons -->
            <div class="d-flex justify-content-center mt-4">
                <div class="btn-group" role="group" aria-label="Basic example">
                    <button @click="edit(shoppingData?.id || 0)" type="button" class="btn btn-primary">Edit</button>
                    <button @click="print()" type="button" class="btn btn-info">Print</button>
                    <button @click="cancel()" type="button" class="btn btn-light">Return</button>
                </div>

            </div>
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
import { groupDataByColumns } from '@/api/services/generalService';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';

export default defineComponent({
    name: 'ShoppingItemList',
    components: {
        ListLoader,
        Error
    },
    setup() {
        const loading = ref<boolean>(true);
        const error = ref<string>('');
        const shoppingData = ref<ShoppingData>();
        const groupedData = ref<{ category: string, items: ShoppingList[] }[]>([]);
        const grandTotal = ref<number>(0);
        // const shoppingRecord = ref<any>({});

        const router = useRouter();
        const toast = useToast();

        const edit = async (id: number) => {
            if (id == 0)
                toast.warning('Unable to capture ID');

            const store = useShoppingStore();
            store.setShoppingId(id);
            router.push({ name: 'ShoppingEdit' });
        }

        const print = () => {
            console.log('print')
        }

        const cancel = () => {
            router.back();
        }

        // Load shopping record data
        const loadPage = async () => {
            try {
                loading.value = true;
                const shoppingStore = useShoppingStore();
                const shoppingId = shoppingStore.getShoppingId;

                if (shoppingId !== null) {
                    const res = await getShoppingById(shoppingId);
                    // console.log('res', res.shoppingData);
                    shoppingData.value = res.shoppingData;

                    // Ensure you're accessing the correct array
                    const shoppingItems = res.data.$values ?? []; // Extract the array of ShoppingList items

                    // Group by category
                    const groupedByCategory = groupDataByColumns<ShoppingList>(shoppingItems, ["category"]);

                    // Transform grouped data to the expected format for `groupedData`
                    groupedData.value = Object.entries(groupedByCategory).map(([category, items]) => ({
                        category,
                        items
                    }));

                    // Calculate grand total
                    grandTotal.value = shoppingItems.reduce((total, item) => total + item.subTotal, 0);

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

        onMounted(() => {
            loadPage();
        });

        return {
            loading,
            error,
            shoppingData,
            groupedData,
            grandTotal,
            edit,
            print,
            cancel
        };
    }
});
</script>
