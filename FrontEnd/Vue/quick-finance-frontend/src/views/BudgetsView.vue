<template>
    <div class="container">
      <h1>Budgets</h1>
      <button @click="add()" type="button" class="btn btn-primary">Create a Budget</button>
      <hr>
      <BudgetsList/>
    </div>
  </template>
  
  <script lang="ts">
import BudgetsList from '@/components/Budgets/BudgetsList.vue';
export default {
    name: "BudgetView",
    components:{
      BudgetsList,
    },
    methods:{
      add(){
        this.$router.push({name:'addBudget'})
      },
    },
  };
  </script>
  