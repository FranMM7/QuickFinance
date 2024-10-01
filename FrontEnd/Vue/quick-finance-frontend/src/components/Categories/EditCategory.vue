<template>
    <div>
      <h2>Edit Category</h2>
      <form @submit.prevent="updateCategory">
        <label for="categoryName">Category Name:</label>
        <input type="text" v-model="categoryName" required />
        <button type="submit">Update Category</button>
      </form>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue';
  
  export default defineComponent({
    name: 'EditCategory',
    data() {
      return {
        categoryName: '',
      };
    },
    created() {
      // Fetch the category details based on the ID from the route params
      const categoryId = this.$route.params.id;
      // Replace this with an actual API call to fetch the category
      this.categoryName = 'Sample Category'; // Example placeholder
    },
    methods: {
      updateCategory() {
        // Logic to update the category (API call or state management)
        console.log('Category Updated:', this.categoryName);
      },
    },
  });
  </script>
  
  <style scoped>
  /* Add your styles here */
  </style>
  