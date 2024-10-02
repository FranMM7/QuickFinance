<template>
    <div class="container">
        <h1>Expenses</h1>
        <ExpensesList />
    </div>
</template>

<script lang="ts">
import ExpensesList from '@/components/Expenses/ExpensesList.vue';
export default {
    name: 'Expenses',
    components: {
        ExpensesList
    },
}
</script>