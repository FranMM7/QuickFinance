<template>
  <div class="container">
    <div class="jumbotron">
      <h1 class="display-3">Quick Finance</h1>
      <hr />
      <p class="lead">
        QuickFinance is a simple personal finance tracker that allows users to manage their expenses,
        budgets, categories, payment methods, and budget limits efficiently. This application is
        designed to help users keep track of their financial activities and gain insights into their
        spending habits.
      </p>
    </div>
    <!-- <hr /> -->

  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { BudgetSumary, getBudgetInfo } from '@/api/services/budgetService';
import Error from '@/components/error/error.vue';
import { useBudgetStore } from '@/stores/budgets';
import { useErrorStore } from '@/stores/error';
import { ListLoader } from 'vue-content-loader';

export default defineComponent({
  name: 'homeView',
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
        await new Promise(resolve => setTimeout(resolve, 1000));
        const resp = await getBudgetInfo();
        budgetInfo.value = resp.BudgetTop5 || [];
        highestExpenses.value = resp.RecordWithHighestExpenses || [];
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
});
</script>
