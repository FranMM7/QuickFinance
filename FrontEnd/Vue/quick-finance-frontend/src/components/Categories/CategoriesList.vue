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
            <th>Name</th>
            <th>Modified On</th>
            <th>Budget Limit</th>
            <th>Total Expended</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr class="text-center" v-for="category in categories?.$values || []" :key="category.id">
            <td>{{ category.name }}</td>
            <td>{{ category.budgetLimit }}</td>
            <td>{{ category.totalExpended }}</td>
            <td>{{ formatDate(category.modifiedOn) }}</td>
            <td>
              <div class="btn-group" role="group">
                <button @click="edit(category.id)" type="button" class="btn btn-secondary">
                  <font-awesome-icon :icon="['fas', 'edit']" />
                </button>
                <button @click="confirmDelete(category.id)" type="button" class="btn btn-danger">
                  <font-awesome-icon :icon="['fas', 'trash']" />
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Pagination Component -->
      <div class="d-flex justify-content-center mt-4"> <!-- Center the pagination -->
        <ul class="pagination">
          <li :class="['page-item', { disabled: currentPage === 1 }]">
            <a class="page-link" href="#" @click="changePage(currentPage - 1)" aria-label="Previous">
              <span aria-hidden="true">&laquo;</span>
            </a>
          </li>
          <li v-for="page in totalPages" :key="page" :class="['page-item', { active: currentPage === page }]">
            <a class="page-link" href="#" @click="changePage(page)">{{ page }}</a>
          </li>
          <li :class="['page-item', { disabled: currentPage === totalPages }]">
            <a class="page-link" href="#" @click="changePage(currentPage + 1)" aria-label="Next">
              <span aria-hidden="true">&raquo;</span>
            </a>
          </li>
        </ul>
      </div>

    </div>
  </div>
</template>

<script lang="ts">
import { Category, changeCategoryState, deleteCategory, fetchCategories } from '../../api/services/categoryService';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useCategoryStore } from '@/stores/categories';
import { useErrorStore } from '@/stores/error'; // Ensure to import error store if you're using it
import { paginationInfo } from '@/api/services/generalService';

export default {
  components: {
    ListLoader,
    Error,
  },
  name: 'CategoriesList',
  data() {
    return {
      categories: [] as Category[],
      loading: true,
      error: null as string | null,
      currentPage: 1, // Add currentPage for pagination
      rowsPerPage: 10, // Number of rows per page
      totalPages: 10,   // You can calculate this based on your data
    };
  },
  async created() {
    await this.loadCategories();
  },
  methods: {
    async loadCategories() {
      try {
        this.loading = true; // Set loading to true
        const response = await fetchCategories(this.currentPage, this.rowsPerPage);
        this.categories = response || []; // Assign the response
        this.totalPages = await paginationInfo(this.rowsPerPage, 'Category');
      } catch (error) {
        this.error = 'Failed to load categories.';
        const errorStore = useErrorStore();
        errorStore.setErrorNotification(this.error, String(error));
      } finally {
        this.loading = false; // Always set loading to false
      }
    },
    edit(id: number) {
      const storeCategory = useCategoryStore();
      storeCategory.setCategoryId(id);
      this.$router.push({ name: 'editCategory' });
    },
    async confirmDelete(id: number) {
      const isConfirmed = confirm("Are you sure you want to delete this category?");
      if (isConfirmed) {
        await this.deleteRecord(id);
      }
    },
    async deleteRecord(id: number) {
      try {
        const response = await changeCategoryState(id);

        if (response == 200) {
          // Check if categories has $values property before filtering
          if (this.categories && (this.categories as any).$values) {
            // Access the $values array and apply the filter
            (this.categories as any).$values = (this.categories as any).$values.filter(
              (category: Category) => category.id !== id
            );
          } else {
            // If categories is a normal array, apply filter directly
            this.categories = this.categories.filter(
              (category: Category) => category.id !== id
            );
          }
        } else {
          console.log("Record not found");
        }
      } catch (error) {
        console.error("Failed to delete category:", error);
        this.error = 'Failed to delete category.';
        const errorStore = useErrorStore();
        errorStore.setErrorNotification(this.error, String(error));
      }
    },
    changePage(page: number) {
      this.currentPage = page;
      this.loadCategories(); // Reload categories based on the new page
    },
  },
};
</script>
