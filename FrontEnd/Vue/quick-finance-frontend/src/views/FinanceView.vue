<template>
    <div class="container flex-fill">
        <div v-if="recordExist">
            <FinanceEdit />
        </div>
        <div v-else>
            <h1>Finance Analysis</h1>
            <hr>
            <FinanceList />
        </div>
    </div>
</template>
<script lang="ts">
import { fetchFinanceData, FinanceDetails, getExistsData } from '@/api/services/financeServices';
import { Shopping } from '@/api/services/shoppingServices';
import Error from '@/components/error/error.vue';
import FinanceEdit from '@/components/FinanceAnalysis/FinanceEdit.vue';
import FinanceList from '@/components/FinanceAnalysis/FinanceList.vue';
import { useFinanceStore } from '@/stores/finance';
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';

export default defineComponent({
    name: 'FinanceView',
    components: {
        ListLoader,
        Error,
        FinanceList,
        FinanceEdit
    },
    setup() {
        const recordExist = ref<boolean>(false)
        const toast = useToast()
        const store = useFinanceStore()
        const router = useRouter()
        const list = ref<FinanceDetails[]>([])

        const loadPage = async () => {
            try {
                const resp = await fetchFinanceData();
                if (resp) {
                    recordExist.value = true;
                    store.setId(resp.id);
                    store.setTitle(resp.title || ''); // Ensure a non-null string
                    list.value = resp.financeDetails
                    store.setList(Array.isArray(list.value) ? list.value : []);
                    router.push({ name: 'financeEdit' });
                } else {
                    recordExist.value = false;
                }
            } catch (error) {
                console.error('Error when loading data:', error);
                toast.error('Unexpected error');
            }
        };




        onMounted(async () => {
            loadPage()
        });

        return {
            recordExist
        }
    }
});
</script>