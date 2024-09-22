<template>
  <div>
    <h2>Expenses</h2>
    <ul>
      <li v-for="expense in expenses" :key="expense.id">
        {{ expense.name }} - {{ expense.amount }}
      </li>
    </ul>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      expenses: []
    };
  },
  mounted() {
    this.fetchExpenses();
  },
  methods: {
    async fetchExpenses() {
      const response = await axios.get('http://localhost:5000/api/expenses');
      this.expenses = response.data;
    }
  }
};
</script>
