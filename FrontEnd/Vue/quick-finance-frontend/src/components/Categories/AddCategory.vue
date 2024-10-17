<template>
  <div class="container">
    <h2>Add New Category</h2>
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
            <input class="form-check-input" type="checkbox" id="typeShoppingList" v-model="category.typeShoppingList" />
            <label class="form-check-label" for="typeShoppingList">Shopping List</label>
          </div>
        </div>

        <div class="container">
          <div class="row justify-content-start"> <!-- Flex alignment -->
            <div class="col-auto"> <!-- 'col-auto' ensures the buttons don't take up the full width -->
              <button type="submit" class="btn btn-primary mt-4">Add Category</button>
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
import { defineComponent, ref } from 'vue';
import { useRouter } from 'vue-router';
import { addCategory, Category } from '@/api/services/categoryService';
import { useToast } from 'vue-toastification';

export default defineComponent({
  methods: {
    cancel() {
      this.$router.back()
    }
  },
  setup() {
    const router = useRouter();
    const category = ref<Category>({
      id: 0,
      createdOn: new Date(), // This is fine as it initializes to the current date.
      updatedOn: null, // Set updatedOn to null directly, not with the type.
      name: '',
      budgetLimit: 0.0,
      typeBudget: true,
      typeFinanceAnalizis: false,
      typeShoppingList: false,
      state: 1
    });


    // Handle form submission
    const submitForm = async () => {
      try {
        const toast = useToast();
        await addCategory(category.value); // Call your service to add the category
        toast.success('Record has been saved!'); // Show success notification
        // Optional: Wait for a brief moment before redirecting
        await new Promise(resolve => setTimeout(resolve, 2000)); // Show the notification for 2 seconds
        await router.push('/categories'); // Navigate back to categories list after adding
      } catch (error) {
        console.error('Error adding category:', error);
      }
    };

    return {
      category,
      submitForm,
    };
  },
});
</script>
