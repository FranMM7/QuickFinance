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
            <input type="number" class="form-control" id="budgetLimit" v-model="category.budgetlimit" required />
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
import { addCategory } from '@/api/services/categoryService';

export default defineComponent({
  methods: {
    cancel() {
      this.$router.back()
    }
  },
  setup() {
    const router = useRouter();
    const category = ref({
      id: 0,
      name: '',
      budgetlimit: 0,
      createdOn: new Date(),
    });

    // Handle form submission
    const submitForm = async () => {
      try {
        await addCategory(category.value); // Call your service to add the category
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
