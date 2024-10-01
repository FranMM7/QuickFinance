<template>
  <div class="container">
    <!-- Your HTML content here -->
    <h1>Categories</h1>
    <!-- <AddCategory/> -->
    <CategoriesList/>
  </div>
</template>

<script lang="ts">
import CategoriesList from '@/components/Categories/CategoriesList.vue';
import AddCategory from '@/components/Categories/AddCategory.vue';

export default {
  name: 'CategoriesView',
  components: {
    CategoriesList,
    AddCategory
  },
}
</script>
