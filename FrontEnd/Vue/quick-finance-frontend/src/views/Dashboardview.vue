<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { BudgetSumary, getBudgetInfo } from '@/api/services/budgetService';
import Error from '@/components/error/error.vue';
import { useBudgetStore } from '@/stores/budgets';
import { useErrorStore } from '@/stores/error';
import { ListLoader } from 'vue-content-loader';
import { useAuthStore } from '@/stores/auth';


export default defineComponent({
    name: 'DashBoard',
    components: {
        ListLoader,
        Error
    },
    setup() {
        const budgetInfo = ref<BudgetSumary[]>([]);
        const highestExpenses = ref<BudgetSumary[]>([]);
        const loading = ref(true);
        const error = ref<string | null>(null);

        const budgetStore = useBudgetStore();
        const errorStore = useErrorStore();
        const router = useRouter();
        const store = useAuthStore();

        const scrollLeft = () => {
            const container = document.querySelector('.card-container');
            if (container) {
                container.scrollBy({
                    left: -container.clientWidth,
                    behavior: 'smooth'
                });
            }
        };

        const scrollRight = () => {
            const container = document.querySelector('.card-container');
            if (container) {
                container.scrollBy({
                    left: container.clientWidth,
                    behavior: 'smooth'
                });
            }
        };

        const goToExpenses = (budgetId: number, title: string) => {
            budgetStore.captureBudgetValues(budgetId, title);
            router.push({ name: 'budgetExpenses' });
        };

        const loadBudgetInfo = async () => {
            try {
                loading.value = true;
                // await new Promise(resolve => setTimeout(resolve, 1000));
                const userId = store.user?.id || ''
                const resp = await getBudgetInfo(userId);
                budgetInfo.value = resp.BudgetTop5 || [];
                highestExpenses.value = resp.MonthWithHighestExpenses || [];
            } catch (err: unknown) {
                if (err instanceof Error) {
                    error.value = err.message;
                    errorStore.setErrorNotification(error.value, err);
                } else {
                    error.value = 'Failed to load budgets';
                }
            } finally {
                loading.value = false;
            }
        };

        onMounted(async () => {
            await loadBudgetInfo();
        });

        return {
            budgetInfo,
            highestExpenses,
            loading,
            error,
            scrollLeft,
            scrollRight,
            goToExpenses
        };

    }
})

</script>

<template>
    <div class="container">
        <div class="row m-1 p-1">
            <div v-if="loading">
                <ListLoader />
            </div>
            <div v-else-if="error">
                <Error />
            </div>
            <div v-else>
                <div class="row" v-if="highestExpenses">
                    <h3>Budget with the Highest Expenses</h3>

                    <div v-for="(budget, index) in highestExpenses" :key="index" class="card btn border-warning"
                        style="max-width: 20rem; cursor: pointer;" @click="goToExpenses(budget.BudgetId, budget.Title)">
                        <div class="card-header">{{ budget.Title }}</div>
                        <div class="card-body">
                            <h4 class="card-title">Budget: {{ budget.TotalAllocatedBudget }}</h4>
                            <p class="card-text">Expenses: {{ budget.Expenses }} | Saving: {{ budget.Saving }}</p>
                        </div>
                    </div>
                </div>
                <hr />

                <div class="row">
                    <h3>Last 5 Budgets</h3>

                    <div class="slideshow-container">
                        <div class="card-container">
                            <div v-for="(budget, index) in budgetInfo" :key="index" class="card btn word border-info"
                                style="max-width: 20rem; cursor: pointer;"
                                @click="goToExpenses(budget.BudgetId, budget.Title)">
                                <div class="card-header">{{ budget.Title }}</div>
                                <div class="card-body">
                                    <h4 class="card-title">Budget: {{ budget.TotalAllocatedBudget }}</h4>
                                    <p class="card-text">Expenses: {{ budget.Expenses }}</p>
                                    <p class="card-text">Saving: {{ budget.Saving }}</p>
                                </div>
                            </div>
                        </div>
                        <!-- Next and previous buttons -->
                        <!-- <a class="prev" @click="scrollLeft">&#10094;</a> -->
                        <!-- <a class="next" @click="scrollRight">&#10095;</a> -->
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>