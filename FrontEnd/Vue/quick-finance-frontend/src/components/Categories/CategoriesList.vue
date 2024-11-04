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
            <th>Budget Limit</th>
            <th>Total Expended</th>
            <th>Modified On</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr class="text-center" v-for="category in categories || []" :key="category.id">
            <td>{{ category.name }}</td>
            <td>{{ category.budgetLimit }}</td>
            <td>{{ category.totalExpended }}</td>
            <td>{{ formatDate(String(category.modifiedOn)) }}</td>
            <td>
              <div class="btn-group" role="group">
                <button @click="edit(category.id)" type="button" class="btn btn-secondary">
                  <font-awesome-icon :icon="['fas', 'edit']" />
                </button>

                <div v-if="!category.expenseCount">
                  <button @click="confirmDelete(category.id)" type="button" class="btn btn-danger">
                    <font-awesome-icon :icon="['fas', 'trash']" />
                  </button>
                </div>

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
        <br>
        <!-- Row Selection Dropdown -->
        <div class="col-auto text-sm-end">
          <div class="row mb-3">
            <div class="col-auto text-end text-primary">
              <!-- <label for="rowsPerPage">Rows per page:</label> -->
              <select id="rowsPerPage" v-model="rowsPerPage" @change="loadCategories" class="form-select ">
                <option :value="5">5</option>
                <option :value="10">10</option>
                <option :value="20">20</option>
                <option :value="50">50</option>
              </select>
            </div>
          </div>
        </div>

      </div>

    </div>
  </div>
</template>

<script lang="ts">
import { Category, categoryList, changeCategoryState, deleteCategory, fetchCategories } from '../../api/services/categoryService';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useCategoryStore } from '@/stores/categories';
import { useErrorStore } from '@/stores/error'; // Ensure to import error store if you're using it
import { paginationInfo } from '@/api/services/generalService';
import { defineComponent, ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: 'CategoriesList',
  components: {
    ListLoader,
    Error,
  },
  setup() {
    const categories = ref<categoryList[]>([]);
    const list = ref<[]>();
    const loading = ref<boolean>(true);
    const error = ref<string>('');
    const currentPage = ref<number>(1);
    const rowsPerPage = ref<number>(10);
    const totalPages = ref<number>(10);
    const router = useRouter();

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    };

    const edit = (id: number) => {
      const storeCategory = useCategoryStore();
      storeCategory.setCategoryId(id);
      router.push({ name: 'editCategory' });
    };

    const confirmDelete = async (id: number) => {
      const isConfirmed = confirm("Are you sure you want to delete this category?");
      if (isConfirmed) {
        await deleteRecord(id);
      }
    };

    const deleteRecord = async (id: number) => {
      try {
        const response = await changeCategoryState(id);

        if (response === 200) {
          categories.value = categories.value.filter(
            (category: categoryList) => category.id !== id
          );
        } else {
          console.log("Record not found");
        }
      } catch (err) {
        console.error("Failed to delete category:", err);
        error.value = 'Failed to delete category.';
        const errorStore = useErrorStore();
        errorStore.setErrorNotification(error.value, String(err));
      }
    };

    const changePage = (page: number) => {
      currentPage.value = page;
      loadCategories(); // Reload categories based on the new page
    };

    const loadCategories = async () => {
      try {
        loading.value = true;
        const response = await fetchCategories(currentPage.value, rowsPerPage.value);
        categories.value = response.data; // Directly assign the flat array of categories

        totalPages.value = response.totalPages;
      } catch (err) {
        error.value = 'Failed to load categories.';
        const errorStore = useErrorStore();
        errorStore.setErrorNotification(error.value, String(err));
      } finally {
        loading.value = false;
      }
    };


    onMounted(() => {
      loadCategories();
    });

    return {
      loading,
      error,
      totalPages,
      currentPage,
      rowsPerPage,
      categories,
      loadCategories,
      formatDate,
      edit,
      changePage,
      confirmDelete
    }
  }
});
</script>
