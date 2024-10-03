<template>
  <div class="container">
    <h2>Edit Category</h2>
    <form @submit.prevent="submitForm">
      <fieldset>
        <legend>Category Details</legend>

        <div class="row">
          <label for="categoryName" class="col-sm-2 col-form-label">Category Name</label>
          <div class="col-sm-10">
            <input type="text" class="form-control" id="categoryName" v-model="category.name" required />
          </div>
        </div>

        <div class="row mt-4">
          <label for="budgetLimit" class="col-sm-2 col-form-label">Budget Limit</label>
          <div class="col-sm-10">
            <input type="number" class="form-control" id="budgetLimit" v-model="category.budgetlimit" />
          </div>
        </div>

        <div class="container">
          <div class="row justify-content-start"> <!-- Flex alignment -->
            <div class="col-auto"> <!-- 'col-auto' ensures the buttons don't take up the full width -->
              <button type="submit" class="btn btn-primary mt-4">Save Changes</button>
            </div>
            <div class="col-auto">
              <button @click="cancel()" type="button" class="btn btn-secondary mt-4">Cancel</button>
            </div>
          </div>
        </div>


      </fieldset>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useStore } from 'vuex'; // Import useStore for Vuex
import { getCategory, editCategory } from '@/api/services/categoryService';

export default defineComponent({
  methods: {
    cancel() {
      this.$router.back();
    }
  },
  setup() {
    const store = useStore(); // Access the Vuex store
    const route = useRoute();
    const router = useRouter();

    // Initialize the category object
    const category = ref({
      id: 0,
      createdOn: new Date(),
      updatedOn: new Date(),
      name: '',
      budgetlimit: 0,
    });

    // Fetch the category details when the component is mounted
    onMounted(async () => {
      const categoryId = store.getters.categoryId; // Get the categoryId from Vuex
      if (!isNaN(categoryId)) { // Check if categoryId is a valid number
        try {
          const categoryData = await getCategory(categoryId);

          // Ensure createdOn and updatedOn are Date objects or fallback to current date
          category.value = {
            ...categoryData,
            createdOn: categoryData.createdOn ? new Date(categoryData.createdOn) : new Date(),
            updatedOn: categoryData.updatedOn ? new Date(categoryData.updatedOn) : new Date(),
          };
        } catch (error) {
          console.error('Error fetching category:', error);
        }
      } else {
        console.error('Invalid category ID:', route.params.id); // Log the error for debugging
      }
    });


    // Handle form submission
    const submitForm = async () => {
      try {
        await editCategory(category.value.id, category.value);
        // Navigate back to the category list or detail page after saving
        await router.push('/categories'); // Update this path as needed
      } catch (error) {
        console.error('Error editing category:', error);
      }
    };

    return {
      category,
      submitForm,
    };
  },
});
</script>