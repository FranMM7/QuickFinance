<template>
    <div class="container">        
        <ExpensesList  />
    </div>
</template>

<script lang="ts">
import ExpensesList from '@/components/Expenses/ExpensesList.vue';
export default {
    name: 'Expenses',
    components: {
        ExpensesList,
    },
};
</script>
