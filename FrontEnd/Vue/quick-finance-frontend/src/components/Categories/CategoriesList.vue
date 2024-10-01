<template>
  <div>
    <div v-if="loading">
      <list-loader />
    </div> <!-- Show the content loader while loading -->
    <div v-else-if="error">{{ error }}</div>
    <div v-else>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Modified On</th>
            <th colspan="2">Expended</th>
            <th>-</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="category in categories?.$values || []" :key="category.id">
            <td>{{ category.id }}</td>
            <td>{{ category.name }}</td>
            <td>{{ formatDate(category.modifiedOn) }}</td>
            <td class="text-end">{{ category.totalExpended }}</td>
            <td></td>
            <td class="btn-group">
              <button type="button" class="btn btn-secondary"><font-awesome-icon :icon="['fas', 'edit']" /></button>
              <button type="button" class="btn btn-danger"><font-awesome-icon :icon="['fas', 'trash']" /></button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import { fetchCategories } from '../../api/services/categoryService';
import formatDate from '../../App.vue'
import {
  ContentLoader,
  FacebookLoader,
  CodeLoader,
  BulletListLoader,
  InstagramLoader,
  ListLoader,
} from 'vue-content-loader';


export default {
  components: {
    ContentLoader,
    FacebookLoader,
    CodeLoader,
    BulletListLoader,
    InstagramLoader,
    ListLoader, // Register ContentLoader component
  },
  name: 'CategoriesList',
  data() {
    return {
      categories: null,
      loading: true,
      error: null,
    };
  },
  async created() {
    try {
      this.loading = true; // Set loading to true
      const response = await fetchCategories();
      this.categories = response; // Assign the response
    } catch (error) {
      console.error("Failed to load categories:", error);
      this.error = 'Failed to load categories.';
    } finally {
      this.loading = false; // Always set loading to false
    }
  },
};
</script>
