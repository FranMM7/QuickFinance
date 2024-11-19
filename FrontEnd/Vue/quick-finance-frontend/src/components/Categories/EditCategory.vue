<template>
  <div class="container">
    <h2>Edit Category</h2>
    <form @submit.prevent="submitForm" v-if="!loading">
      <fieldset>
        <legend>Category Details</legend>

        <div class="row">
          <div class="row">
            <label for="categoryName" class="col-sm-2 col-form-label">Category Name</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" id="categoryName" v-model="category.name" required />
            </div>
          </div>

          <div class="row mt-4">
            <label for="budgetLimit" class="col-sm-2 col-form-label">Budget Limit</label>
            <div class="col-sm-10">
              <input type="number" class="form-control" id="budgetLimit" v-model="category.budgetLimit" step="0.01"
                min="0" />
            </div>
          </div>

          <legend>Types:</legend>
          <div class="row p-4" style="font-size: xx-large"> <!-- Add some margin for spacing -->
            <div class="col-auto form-check form-switch">
              <input class="form-check-input" type="checkbox" id="typeBudgets" v-model="category.typeBudget" />
              <label class="form-check-label" for="typeBudgets">Budgets</label>
            </div>
            <div class="col-auto form-check form-switch">
              <input class="form-check-input" type="checkbox" id="typeFinanceAnalysis"
                v-model="category.typeFinanceAnalizis" />
              <label class="form-check-label" for="typeFinanceAnalysis">Finance Analysis</label>
            </div>
            <div class="col-auto form-check form-switch">
              <input class="form-check-input" type="checkbox" id="typeShoppingList"
                v-model="category.typeShoppingList" />
              <label class="form-check-label" for="typeShoppingList">Shopping List</label>
            </div>
          </div>

          <div class="container">
            <div class="row justify-content-start">
              <div class="col-auto">
                <button type="submit" class="btn btn-primary mt-4">Save Changes</button>
              </div>
              <div class="col-auto">
                <button @click="cancel()" type="button" class="btn btn-secondary mt-4">Cancel</button>
              </div>
            </div>
          </div>
        </div>
      </fieldset>
    </form>

    <div v-else-if="loading" class="loading">
      <list-loader />
    </div>

    <div v-else-if="error">
      <Error />
    </div>
  </div>
</template>


<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { getCategory, editCategory, Category } from '@/api/services/categoryService';
import { useCategoryStore } from '@/stores/categories';
import { ListLoader } from 'vue-content-loader';
import { useToast } from 'vue-toastification';
import { useAuthStore } from '@/stores/auth';

export default defineComponent({
  setup() {
    const router = useRouter();
    const categoryStore = useCategoryStore();
    const loading = ref<boolean>(true); // Loading state
    const error = ref<string | null>(null); // Error state
    const store = useAuthStore();

    // Initialize the category object
    const category = ref<Category>({
      id: 0,
      createdOn: new Date(),
      updatedOn: new Date(),
      name: '',
      budgetLimit: 0.0,
      typeBudget: true, // Add any other properties that are required
      typeFinanceAnalizis: false,
      typeShoppingList: false,
      state: 1,
      userId: store.user?.id || ''
    });

    // Fetch the category details when the component is mounted
    onMounted(async () => {
      const categoryId = categoryStore.getCategoryId || 0; // Use default value if null

      loading.value = true;

      if (categoryId === null || categoryId === undefined) {
        console.error('Category ID is null or undefined');
        return;
      }

      if (!isNaN(categoryId)) {
        try {
          const categoryData = await getCategory(categoryId);
          category.value = categoryData; // Update here
        } catch (error) {
          console.error('Error fetching category:', error);
        }
      } else {
        console.error('Invalid category ID:', categoryId); // Log the error for debugging
      }

      loading.value = false;
    });

    // Handle form submission
    const submitForm = async () => {
      try {
        const toast = useToast();
        if (!category.value.userId) throw new Error('Unable to retrieve the UserID, please login again and try again later.')
        await editCategory(category.value.id, category.value);
        toast.success('Record has been saved!'); // Show success notification
        // await new Promise(resolve => setTimeout(resolve, 2000)); // Show the notification for 2 seconds
        // Navigate back to the category list or detail page after saving
        await router.push('/categories'); // Update this path as needed
      } catch (error) {
        console.error('Error editing category:', error);
      }
    };

    const cancel = () => {
      router.back();
    };

    return {
      category,
      loading,
      error,
      submitForm,
      cancel,
    };
  },
});
</script>
