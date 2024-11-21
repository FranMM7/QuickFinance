<template>
  <div class="container fade-in">
    <template v-if="!isEditing">
      <div class="row d-flex">
        <div class="col">
          <h1>Budgets</h1>
        </div>
        <div class="col-auto p-1">
          <button @click="add()" type="button" class="btn btn-lg btn-primary">Create a Budget</button>
        </div>
      </div>
      <hr>
      <BudgetsList />
    </template>
    <template v-else>
      <router-view />
    </template>
  </div>
</template>

<script lang="ts">
import BudgetsList from '@/components/Budgets/BudgetsList.vue';
import { computed, defineComponent } from 'vue';
import { useRoute, useRouter } from 'vue-router';

export default defineComponent({
  name: "BudgetView",
  components: {
    BudgetsList,
  },
  setup() {
    const route = useRoute();
    const router = useRouter()

    // Determine if the user is in "edit" or "add" mode
    const isEditing = computed(() => {
      return route.name === 'addBudget' || route.name === 'editBudget' || route.name ==='budgetExpenses';
    });
    const add = () => {
      router.push({ name: 'addBudget' })
    }
    return {
      add,
      isEditing
    }
  }
});
</script>