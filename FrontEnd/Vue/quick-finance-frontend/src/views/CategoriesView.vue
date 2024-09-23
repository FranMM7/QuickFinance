<template>
    <div class="container mt-4">
     <h1>Categories</h1>
     <AddCategory/>
     <CategoryList/>
    </div>
  </template>
  
  <script>
import AddCategory from '@/components/Categories/AddCategory.vue';
import CategoryList from '@/components/Categories/CategoryList.vue';

  export default {
    name: 'CategoriesView',
    components: {
        AddCategory,
        CategoryList
  }
  };
  </script>
  