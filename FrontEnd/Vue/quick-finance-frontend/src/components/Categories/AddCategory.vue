<template>
    <div>
        <h2>Add Category</h2>
        <form @submit.prevent="addCategory">
            <input v-model="category.name" placeholder="Category Name" required />
            <input v-model.number="category.budgetLimit" type="number" placeholder="Budget Limit" required />
            <button type="submit">Add</button>
        </form>
    </div>
</template>

<script>
import axios from 'axios';

export default {
    data() {
        return {
            category: {
                name: '',
                budgetLimit: 0
            }
        };
    },
    methods: {
        async addCategory() {
            await axios.post('http://localhost:5000/api/categories', this.category);
            this.category.name = '';
            this.category.budgetLimit = 0;
            this.$emit('categoryAdded'); // Notify parent component to refresh list
        }
    }
};
</script>