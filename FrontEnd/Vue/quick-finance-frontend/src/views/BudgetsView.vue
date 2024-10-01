<template>
    <div class="container">
      <h1>Budgets</h1>
      <BudgetsList/>
    </div>
  </template>
  
  <script lang="ts">
import BudgetsList from '@/components/Budgets/BudgetsList.vue';
export default {
    name: "BudgetView",
    components:{
      BudgetsList,
    }
  };
  </script>
  