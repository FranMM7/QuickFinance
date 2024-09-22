<template>
    <div>
        <h2>Add Expense</h2>
        <form @submit.prevent="addExpense">
            <input v-model="expense.name" placeholder="Expense Name" required />
            <input v-model.number="expense.amount" type="number" placeholder="Amount" required />
            <button type="submit">Add</button>
        </form>
    </div>
</template>

<script>
import axios from 'axios';

export default {
    data() {
        return {
            expense: {
                name: '',
                amount: 0
            }
        };
    },
    methods: {
        async addExpense() {
            await axios.post('http://localhost:5000/api/expenses', this.expense);
            this.expense.name = '';
            this.expense.amount = 0;
            this.$emit('expenseAdded'); // Notify parent component to refresh list
        }
    }
};
</script>