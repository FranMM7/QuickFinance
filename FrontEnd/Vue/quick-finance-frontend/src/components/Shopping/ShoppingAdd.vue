<template >
    <div>
        Add Shopping
    </div>
</template>


<script lang="ts">
import { useErrorStore } from '@/stores/error';
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useShoppingStore } from '@/stores/shopping';
import { getShoppingById, ShoppingData, ShoppingLists } from '@/api/services/shoppingServices';

export default defineComponent({
    name: 'ShoppingAdd',
    components: {
        ListLoader,
        Error
    },
    setup() {
        const loading = ref<Boolean>(true);
        const error = ref<string>('');
        const Id = ref<number>(0);

        const list = ref<ShoppingLists>({
            $id: '',
            $values: []
        });

        const record = ref<ShoppingData>({
            id: 0,
            createdOn: '',
            updatedOn: null,
            description: '',
            state: 1,
            shoppingLists: list.value,
        });

        const loadPage = async () => {
            try {
                loading.value = true;
                
                // Fetch record from the API if needed
                // record.value = await getShoppingById(Id.value);

            } catch (err) {
                error.value = 'Failed to Shopping Details';
                console.log('Error msg:', err);
                const errorStore = useErrorStore();
                errorStore.setErrorNotification(String(error.value), String(error));
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
            Id,
            record,
        };
    }
});
</script>
