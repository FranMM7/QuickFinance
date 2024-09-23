<template>
  <div>
    <h2>Categories</h2>
    <div v-if="loading">Loading categories...</div>
    <div v-else-if="error">{{ error }}</div>
    <ul v-else>
      <!-- Render the categories -->
      <li v-for="category in categories" :key="category.id">
        {{ category.name }}
      </li>
    </ul>
  </div>
</template>

<script>
import { mapState, mapActions } from 'vuex';

export default {
  data() {
    return {
      loading: true, 
      error: null    
    };
  },
  computed: {
    ...mapState(['categories']) // Get the categories from Vuex state
  },
  mounted() {
    this.fetchCategories();
  },
  methods: {
    ...mapActions(['fetchCategories']),
    async fetchCategories() {
      try {
        await this.$store.dispatch('fetchCategories');
      } catch (err) {
        this.error = 'Failed to load categories. Please try again later.';
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>
