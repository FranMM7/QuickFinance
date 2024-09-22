<template>
  <div>
    <h2>Categories</h2>
    <ul>
      <li v-for="category in categories" :key="category.id">
        {{ category.name }} - Limit: {{ category.budgetLimit }}
      </li>
    </ul>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      categories: []
    };
  },
  mounted() {
    this.fetchCategories();
  },
  methods: {
    async fetchCategories() {
      const response = await axios.get('http://localhost:5000/api/categories');
      this.categories = response.data;
    }
  }
};
</script>
