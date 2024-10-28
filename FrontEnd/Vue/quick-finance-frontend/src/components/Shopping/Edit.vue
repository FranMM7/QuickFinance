<template>
    <div class="container">
        <div v-if="loading">
            <ListLoader />
        </div>
        <div v-if="error">
            <Error />
        </div>
        <div v-else>
            <!-- Content for successful loading -->
        </div>
    </div>
</template>

<script lang="ts">
import { useErrorStore } from '@/stores/error';
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useShoppingStore } from '@/stores/shopping';
import { getShoppingById, ShoppingDTO } from '@/api/services/shoppingServices';

export default defineComponent({
    name: 'EditShopping',
    components: {
        ListLoader,
        Error
    },
    setup() {
        const loading = ref<Boolean>(true);
        const error = ref<string>('');
        const Id = ref<number>(0); // Default to 0
        const record = ref<ShoppingDTO>;

        const loadPage = async () => {
            try {
                loading.value = true;
                const shoppingStore = useShoppingStore();
                const shoppingId = shoppingStore.getShoppingId; // Access the getter without parentheses

                // Handle the possibility of shoppingId being null
                Id.value = shoppingId !== null ? shoppingId : 0; // Use 0 if null

                // record = await getShoppingById(Id.value);


            } catch (err) {
                error.value = 'Failed to load Budget Expenses Details';
                console.log('Error msg:', err);
                const errorStore = useErrorStore();
                errorStore.setErrorNotification(String(error.value), String(error));
            } finally {
                loading.value = false;
            }
        };

        // Initialize the component by loading the page
        onMounted(() => {
            loadPage();
        });

        return {
            loading,
            error,
            Id, // Include Id in the return to use it in the template if needed
        };
    }
});
</script>
