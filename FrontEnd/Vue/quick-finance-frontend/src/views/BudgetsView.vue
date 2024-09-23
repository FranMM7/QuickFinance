<template>
    <div class="container mt-4">
      <h1>Budgets</h1>
      <AddBudget />
      <BudgetList />
    </div>
  </template>
  
  <script>
  import AddBudget from '@/components/Budgets/AddBudget.vue';
  import BudgetList from '@/components/Budgets/BudgetList.vue';
  
  export default {
    name: 'BudgetsView',
    components: {
      AddBudget,
      BudgetList
    }
  };
  </script>
  