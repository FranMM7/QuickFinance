<template>
    <div>
        <h2>Budgets</h2>
        <ul>
            <li v-for="budget in budgets" :key="budget.id">
                {{ budget.name }} - Total: {{ budget.totalBudget }}
            </li>
        </ul>
    </div>
</template>

<script>
import axios from 'axios';

export default {
    data() {
        return {
            budgets: []
        };
    },
    mounted() {
        this.fetchBudgets();
    },
    methods: {
        async fetchBudgets() {
            const response = await axios.get('http://localhost:5000/api/budgets');
            this.budgets = response.data;
        }
    }
};
</script>