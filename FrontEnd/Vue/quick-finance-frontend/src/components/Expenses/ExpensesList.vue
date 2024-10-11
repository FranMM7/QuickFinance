<template>
    <div>
        <h1>{{ monthValue }} Budget</h1>
        <hr>
        <div v-if="loading">
            <ListLoader />
        </div>
        <div v-else-if="error">{{ error }}</div>
        <div v-else>
            <table class="table table-striped">
                <thead>
                    <tr class="text-center">
                        <!-- <th>Id</th> -->
                        <th>Description</th>
                        <th>Category</th>                
                        <th>Amount</th>
                        <th>Due Date</th>
                        <th>Executed</th>
                        <th>Modified on</th>
                        <th>-</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="text-center" v-for="expense in expenses?.$values || []" :key="expense.id">
                        <!-- <td>{{ expense.id }}</td> -->
                        <td>{{ expense.description }}</td>
                        <td>{{ expense.category }}</td>
                        <td>{{ expense.amount }}</td>
                        <td>{{ formatDate(expense.dueDate) }}</td>
                        <td>{{ expense.executed }}</td>
                        <td>{{ formatDate(expense.modifiedOn) }}</td>
                        <td class="button-group">
                            <button @click="edit(expense.id)" type="button" class="btn btn-secondary">
                                <font-awesome-icon :icon="['fas', 'edit']" />
                            </button>
                            <button type="button" class="btn btn-danger">
                                <font-awesome-icon :icon="['fas', 'trash']" />
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
import { ListLoader } from 'vue-content-loader';
import { fecthExpenses } from '../../api/services/expensesService'; // Adjust your API service path

export default {
    name: 'ExpensesList',
    components: {
        ListLoader,
    },
   
    data() {
        return {
            expenses: [],
            loading: true,
            error: null,
        };
    },
    computed:{
        budgetId() {
            return this.$store.getters.budgetId; // Get budgetId from Vuex
        },
        month() {
            return this.$store.getters.month; // Get month from Vuex
        },
    },
    async created() {
        try {
            // Introduce a 1-second delay for the loader effect
            await new Promise(resolve => setTimeout(resolve, 1000));
            
            console.log('Budget ID:', this.budgetId); // Log the budgetId

            const resp = await fecthExpenses(this.budgetId); // Fetch expenses for the selected budget
            this.expenses = resp;
        } catch (error) {
            this.error = 'Failed to load expenses.';
        } finally {
            this.loading = false;
        }
    },
    methods:{
        edit(id){
            console.log(id);
        }
    }
};
</script>