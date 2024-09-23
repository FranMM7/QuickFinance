<template>
    <div>
        <h2>Add Category</h2>
        <form @submit.prevent="addCategory">
            <input v-model="category.name" placeholder="Category Name" required />
            <button type="submit">Add</button>
        </form>
    </div>
</template>

<script>
import { mapActions } from 'vuex';

export default {
    data() {
        return {
            category: {
                name: ''
            }
        };
    },
    methods: {
        ...mapActions(['addCategory']), // Map the addCategory action from Vuex

        async addCategory() {
            try {
                await this.$store.dispatch('addCategory', this.category); // Dispatch the action to add a category
                this.category.name = ''; // Clear the input field after adding
                this.$emit('categoryAdded'); // Notify parent component to refresh list
            } catch (error) {
                console.error('Error adding category:', error); // Log any errors
            }
        }
    }
};
</script>
