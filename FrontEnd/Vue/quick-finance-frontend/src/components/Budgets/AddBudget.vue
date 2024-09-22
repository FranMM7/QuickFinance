<template>
    <div>
        <h2>Add Budget</h2>
        <form @submit.prevent="addBudget">
            <input v-model="budget.name" placeholder="Budget Name" required />
            <input v-model.number="budget.totalBudget" type="number" placeholder="Total Budget" required />
            <button type="submit">Add</button>
        </form>
    </div>
</template>

<script>
import axios from 'axios';

export default {
    data() {
        return {
            budget: {
                name: '',
                totalBudget: 0
            }
        };
    },
    methods: {
        async addBudget() {
            await axios.post('http://localhost:5000/api/budgets', this.budget);
            this.budget.name = '';
            this.budget.totalBudget = 0;
            this.$emit('budgetAdded'); // Notify parent component to refresh list
        }
    }
};
</script>