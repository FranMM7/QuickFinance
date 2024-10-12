<template>
  <div>
    <div v-if="loading">
      <list-loader />
    </div> <!-- Show the content loader while loading -->
    <div v-else-if="error">
      <Error />
    </div>
    <div v-else>
      <table class="table table-striped text-center">
        <thead>
          <tr>
            <!-- <th>ID</th> -->
            <th>Name</th>
            <th>Modified On</th>
            <th>Budget Limit</th>
            <th>Total Expended</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr class="text-center" v-for="category in categories?.$values || []" :key="category.id">
            <!-- <td>{{ category.id }}</td> -->
            <td>{{ category.name }}</td>
            <td>{{ formatDate(category.modifiedOn) }}</td>
            <td>{{ category.budgetLimit }}</td>
            <td>{{ category.totalExpended }}</td>
            <td>
              <div class="btn-group" role="group">
                <!-- Navigate to edit record page -->
                <button @click="edit(category.id)" type="button" class="btn btn-secondary">
                  <font-awesome-icon :icon="['fas', 'edit']" />
                </button>
                <!-- delete record -->
                <button @click="confirmDelete(category.id)" type="button" class="btn btn-danger">
                  <font-awesome-icon :icon="['fas', 'trash']" />
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import { deleteCategory, fetchCategories } from '../../api/services/categoryService';
import formatDate from '../../App.vue'
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';

export default {
  methods: {
    edit(id) {
      // Store the category id in Vuex
      this.$store.dispatch('getCategoryValues', { categoryId: id });
      // Navigate to the EditCategory route and pass the id as a route param
      this.$router.push({ name: 'editCategory' });
    },
    async confirmDelete(id) {
      const isConfirmed = confirm("Are you sure you want to delete this category?");
      if (isConfirmed) {
        await this.deleteRecord(id);
      }
    },
    async deleteRecord(id) {
      try {
        const response = await deleteCategory(id);
        console.log("delete", response)
        if (response == 200) {
          this.categories.$values = this.categories.$values.filter(category => category.id !== id);
        } else {
          console.log("Record not found")
        }
      } catch (error) {
        console.error("Failed to delete category:", error);
        this.error = 'Failed to delete category.';
      }
    }


  },
  components: {
    ListLoader,
    Error,
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

      const notification = 'Failed to load categories.';

      this.error = error;

      const errorStore = useErrorStore();

      errorStore.setErrorNotification(notification, error);
    } finally {
      this.loading = false; // Always set loading to false
    }
  }
};
</script>