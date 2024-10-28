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
                    <h1>{{ shoppingRecord.description }}</h1>
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
                    <tr v-for="record in sortedShoppingList" :key="record.id">
                        <td>{{ record.category }}</td>
                        <td>{{ record.description }}</td>
                        <td>{{ record.quantity }}</td>
                        <td>{{ record.amount }}</td>
                        <td>{{ record.subtotal }}</td>
                    </tr>
                    <tr>
                        <td colspan="4" class="text-left">Gran Total</td>
                        <td class="text-end">{{ grandTotal }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="ts">
import { useErrorStore } from '@/stores/error';
import { defineComponent, onMounted, ref, computed } from 'vue';
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
        const shoppingRecord = ref<ShoppingData>({
            id: 0,
            createdOn: '',
            updatedOn: null,
            description: '',
            state: 1,
            shoppingLists: {
                $id: '',
                $values: []
            },
        });

        // Calculate grand total
        const grandTotal = computed(() => {
            return shoppingRecord.value.shoppingLists.$values.reduce((total, item) => total + item.subtotal, 0);
        });

        // Sort shopping list by category
        const sortedShoppingList = computed(() => {
            return shoppingRecord.value.shoppingLists.$values.slice().sort((a, b) => a.category.localeCompare(b.category));
        });

        // Load shopping record data
        const loadPage = async () => {
            try {
                loading.value = true;
                const shoppingStore = useShoppingStore();
                const shoppingId = shoppingStore.getShoppingId;

                if (shoppingId !== null) {
                    shoppingRecord.value = await getShoppingById(shoppingId);
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
            shoppingRecord,
            grandTotal,
            sortedShoppingList
        };
    }
});
</script>
