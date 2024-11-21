<script lang="ts">
import { fetchFinanceData, FinanceDetails, financeList, getExistsData } from '@/api/services/financeServices';
import { Shopping } from '@/api/services/shoppingServices';
import FinanceEdit from '@/components/FinanceAnalysis/FinanceEdit.vue';
import FinanceList from '@/components/FinanceAnalysis/FinanceList.vue';
import { useAuthStore } from '@/stores/auth';
import { useFinanceStore } from '@/stores/finance';
import { computed, defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import { useRoute, useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';

export default defineComponent({
    name: 'FinanceView',
    components: {
        ListLoader,
        FinanceList,
        FinanceEdit
    },
    setup() {
        const recordExist = ref<boolean>(false)
        const toast = useToast()
        const store = useFinanceStore()
        const router = useRouter()
        const route = useRoute()
        const authStore = useAuthStore()

        const loading = ref<boolean>(true)
        const showLoader = ref<boolean>(false)
        let loaderTimeout: ReturnType<typeof setTimeout>;

        const isEditing = computed(() => {
            return route.name === 'financeAdd' || route.name === 'financeEdit'
        })

        const viewList = computed(() => {
            return route.name === 'FinanceList'
        })

        const loadPage = async () => {
            try {
                loading.value = true
                showLoader.value = false; // Reset loader visibility
                clearTimeout(loaderTimeout); // Clear any previous timeout

                // Set a timeout to show loader only if loading takes >1 second
                loaderTimeout = setTimeout(() => {
                    if (loading.value) showLoader.value = true;
                }, 1000);

                const userId = authStore.user?.id || '';

                const resp = await fetchFinanceData(userId);
                // console.log('resp: ', resp)
                if (resp != null) {
                    recordExist.value = true;
                    store.setId(resp.id);
                    store.setTitle(resp.title || ''); // Ensure a non-null string
                    store.setEditMode(false)
                    const expensesList = resp.list.$values || []
                    if (expensesList)
                        store.setList(expensesList);

                    const incomeList = resp.incomes.$values || []
                    if (incomeList)
                        store.setIncomeList(incomeList)

                    router.push({ name: 'financeEdit' });
                } else {
                    recordExist.value = false;
                }
            } catch (error) {
                console.error('Error when loading data:', error);
                toast.error('Unexpected error');
            }
            finally {
                loading.value = false
            }
        };


        onMounted(async () => {
            loadPage()
        });

        return {
            recordExist,
            showLoader,
            isEditing,
            viewList
        }
    }
});
</script>

<template>
    <div class="container flex-fill fade-in">
        <div v-if="showLoader">
            <ListLoader />
        </div>
        <div v-else>
            <div v-if="recordExist">
                <template v-if="!viewList">
                    <FinanceEdit />
                </template>
                <template v-else>
                    <router-view />
                </template>
            </div>
            <div v-else>
                <template v-if="!isEditing">
                    <FinanceList />
                </template>
                <template v-else>
                    <router-view />
                </template>
            </div>
        </div>
    </div>
</template>
