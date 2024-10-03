<template>
  <div class="container">
    <!-- Your HTML content here -->
    <h1>Categories</h1>
    <hr>
    <div>
      <button @click="addCategory()" type="button" class="btn btn-primary">Add Category</button>
    </div>
    <hr>
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
  methods:{
    addCategory(){
      this.$router.push({name:'addCategory'})
    }
  }
}
</script>
