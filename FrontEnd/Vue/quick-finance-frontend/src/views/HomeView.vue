<template>
    <div class="container mt-4">
        <h1 class="mb-4">Expenses</h1>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Description</th>
                    <th>Amount</th>
                    <th>Date</th>
                    <th>Payment Method</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="expense in expenses" :key="expense.id">
                    <td>{{ expense.description }}</td>
                    <td>{{ expense.amount }}</td>
                    <td>{{ expense.date }}</td>
                    <td>{{ expense.paymentMethod }}</td>
                </tr>
            </tbody>
        </table>

        <h1 class="mt-5">Categories</h1>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Category Name</th>
                    <th>Budget Limit</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="category in categories" :key="category.id">
                    <td>{{ category.name }}</td>
                    <td>{{ category.budgetLimit }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import { mapState, mapActions } from 'vuex';

export default {
    computed: {
        ...mapState(['expenses', 'categories']) // Map expenses and categories from the store
    },
    methods: {
        ...mapActions(['fetchExpenses', 'fetchCategories']) // Map actions to fetch expenses and categories
    },
    created() {
        this.fetchExpenses(); // Fetch expenses when component is created
        this.fetchCategories(); // Fetch categories when component is created
    }
};
</script>
