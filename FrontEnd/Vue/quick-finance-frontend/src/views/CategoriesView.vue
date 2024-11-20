<template>
  <div class="container">

    <template v-if="!isEditing">
      <div class="row d-flex">
        <div class="col">
          <h1>Categories</h1>
        </div>
        <div class="col-auto p-1">
          <button @click="addCategory" type="button" class="btn btn-lg btn-primary">
            Add Category
          </button>
        </div>
      </div>
      <hr>
      <!-- Conditionally render the list -->
      <CategoriesList />
    </template>

    <!-- Router view for dynamic child components -->
    <template v-else>
      <router-view />
    </template>

  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import CategoriesList from '@/components/Categories/CategoriesList.vue';

export default defineComponent({
  name: 'CategoriesView',
  components: {
    CategoriesList
  },
  setup() {
    const route = useRoute();
    const router = useRouter();

    // Determine if the user is in "edit" or "add" mode
    const isEditing = computed(() => {
      return route.name === 'addCategory' || route.name === 'editCategory';
    });

    const addCategory = () => {
      router.push({ name: 'addCategory' });
    };

    return {
      isEditing,
      addCategory
    };
  }
});
</script>
