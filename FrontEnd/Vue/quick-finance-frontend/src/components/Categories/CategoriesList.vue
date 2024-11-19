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
            <th>Total Budget Expended</th>
            <th>Total Budget Executed</th>
            <th>Total Shopping Expended</th>
            <th>Modified On</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr class="text-center" v-for="category in categories || []" :key="category.id">
            <td>{{ category.name }}</td>
            <td>{{ category.budgetLimit.toFixed(2) }}</td>
            <td>{{ category.budgetsTotalExpended.toFixed(2) }}</td>
            <td>{{ category.budgetsTotalExpendedExecuted.toFixed(2) }}</td>
            <td>{{ category.shoppingTotalExpended.toFixed(2) }}</td>
            <td>{{ formatDate(String(category.modifiedOn)) }}</td>
            <td>
              <div class="btn-group" role="group">
                <button @click="edit(category.id)" type="button" class="btn btn-secondary">
                  <font-awesome-icon :icon="['fas', 'edit']" />
                </button>

                <div v-if="!category.inUse">
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
          <li :class="['page-item', { disabled: !validURL('F') }]">
            <a class="page-link" href="#" @click.prevent="goTo('F')">First</a>
          </li>
          <li :class="['page-item', { disabled: !validURL('P') }]">
            <a class="page-link" href="#" @click.prevent="goTo('P')">Previous</a>
          </li>
          <li v-for="page in totalPages" :key="page" :class="['page-item', { active: page === currentPage }]">
            <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
          </li>

          <!-- Row Selection Dropdown -->
          <label for="rowsPerPage" class="page-item page-link disabled">View:</label>
          <select id="rowsPerPage" v-model="rowsPerPage" @change="loadCategories" class="page-item page-link">
            <option :value="5">5</option>
            <option :value="10">10</option>
            <option :value="20">20</option>
            <option :value="50">50</option>
          </select>

          <li :class="['page-item', { disabled: !validURL('N') }]">
            <a class="page-link" href="#" @click.prevent="goTo('N')">Next</a>
          </li>
          <li :class="['page-item', { disabled: !validURL('L') }]">
            <a class="page-link" href="#" @click.prevent="goTo('L')">Last</a>
          </li>
        </ul>

      </div> <!-- Pagination Component -->

    </div>
  </div>
</template>

<script lang="ts">
import { Category, categoryList, changeCategoryState, deleteCategory, fetchCategories, goToPage } from '../../api/services/categoryService';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useCategoryStore } from '@/stores/categories';
import { useErrorStore } from '@/stores/error'; // Ensure to import error store if you're using it
import { defineComponent, ref, onMounted, Ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

export default defineComponent({
  name: 'CategoriesList',
  components: {
    ListLoader,
    Error,
  },
  setup() {
    const categories = ref<categoryList[]>([]);
    const loading = ref<boolean>(true);
    const error = ref<string>('');
    const router = useRouter();
    const store = useAuthStore();

    // pagination
    const currentPage = ref<number>(1);
    const rowsPerPage = ref<number>(10);
    const totalPages = ref<number>(10);
    const next = ref<string>('');
    const prev = ref<string>('');
    const first = ref<string>('');
    const last = ref<string>('');

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

    const validURL = (option: 'F' | 'L' | 'N' | 'P'): boolean => {
      switch (option) {
        case 'P':
          return !!prev.value;
        case 'F':
          return !!first.value;
        case 'L':
          return !!last.value;
        case 'N':
          return !!next.value;
        default:
          return false;
      }
    };

    const goTo = async (option: 'F' | 'L' | 'N' | 'P') => {
      try {
        loading.value = true;

        const opt: { [key: string]: Ref<string> } = {
          'F': first,
          'L': last,
          'N': next,
          'P': prev
        };

        const url = opt[option].value;

        const response = await goToPage(url);
        categories.value = response.data;
        totalPages.value = response.totalPages;
        next.value = response.nextPage;
        prev.value = response.previousPage;
        last.value = response.lastPage;
        first.value = response.firstPage;
      } catch (err) {
        error.value = 'Failed to load budget list';
        console.error('Error loading budgets:', err);
      } finally {
        loading.value = false;
      }
    };

    const loadCategories = async () => {
      try {
        loading.value = true;
        const userId = store.user?.id || ''
        const response = await fetchCategories(userId, currentPage.value, rowsPerPage.value);
        console.log('userId:', userId, response)
        categories.value = response.data; // Directly assign the flat array of categories

        totalPages.value = response.totalPages;
        next.value = response.nextPage;
        prev.value = response.previousPage;
        last.value = response.lastPage;
        first.value = response.firstPage;
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
      confirmDelete,
      goTo,
      validURL
    }
  }
});
</script>
