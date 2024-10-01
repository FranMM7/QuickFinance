<template>
    <div>
      <h2>Add Category</h2>
      <form @submit.prevent="addCategory">
        <label for="categoryName">Category Name:</label>
        <input type="text" v-model="categoryName" required />
        <button type="submit">Add Category</button>
      </form>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue';
  
  export default defineComponent({
    name: 'AddCategory',
    data() {
      return {
        categoryName: '',
      };
    },
    methods: {
      addCategory() {
        // Logic to add the category (API call or state management)
        console.log('Category Added:', this.categoryName);
        this.categoryName = ''; // Clear the input
      },
    },
  });
  </script>
  
  <style scoped>
  /* Add your styles here */
  </style>
  