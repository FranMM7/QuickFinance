<template>
  <div class="container mt-4">
    <form @submit.prevent="submitForm">
      <!-- Budget Section -->
      <h3>Edit {{budget.title}}</h3>

      <!-- Disabled Budget ID field -->
      <div>
        <fieldset disabled>
          <label class="form-label" for="budgetId">Budget ID</label>
          <input class="form-control" id="budgetId" type="text" v-model="budget.id" disabled />
        </fieldset>
      </div>

      <div class="row">

        <!-- Editable title field -->
        <div class="col mt-3">
          <fieldset>
            <label class="form-label" for="title">Title</label>
            <input class="form-control" id="title" type="text" v-model="budget.title" placeholder="Enter title" />
          </fieldset>
        </div>
  
        <!-- Editable Total Budget field -->
        <div class="col mt-3">
          <fieldset>
            <label class="form-label" for="totalBudget">Total Budget</label>
            <input class="form-control" id="totalBudget" type="number" v-model="budget.totalAllocatedBudget"
              placeholder="Enter total budget" @change="calculateBalance" required />
            <div class="text-end">
              <label for="totalbudget" class="form-label">Balance: {{ balance }}</label>
            </div>
          </fieldset>
        </div>
      </div>

      <hr />

      <!-- Expenses Section -->
      <h4>Expenses</h4>
      <table class="table table-striped">
        <thead>
          <tr>
            <th style="width: 30%;">Description</th>
            <th style="width: 20%;">Category</th>
            <th>Due Date</th>
            <th>Amount</th>
            <th style="width: 10%;">Payment Method</th>
            <th>Executed</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(expense, index) in budget.expensesDTO" :key="index">
            <td><input v-model="expense.description" class="form-control" type="text" /></td>
            <td>
              <select v-model="expense.categoryId" class="form-control">
                <option v-for="category in categories" :key="category.id" :value="category.id">
                  {{ category.name }}
                </option>
              </select>
            </td>
            <td><input v-model="expense.expenseDueDate" class="form-control" type="date" /></td>
            <td><input v-model="expense.amount" class="form-control text-end" type="number" step="0.01" min="0"
                @change="calculateBalance" /></td>
            <td>
              <select v-model="expense.paymentMethodId" class="form-control">
                <option v-for="method in paymentMethods" :key="method.id" :value="method.id"> {{
                  method.paymentMethodName }}
                </option>
              </select>
            </td>
            <td><input v-model="expense.isExecuted" class="form-check-input text-center" type="checkbox"
                style="font-size: x-large" /></td>
            <td>
              <button type="button" class="btn btn-danger" @click="removeExpense(index)">Remove</button>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Mark all as executed -->
      <div class="mt-3">
        <div class="col-auto form-check form-switch">
          <input class="form-check-input" type="checkbox" id="typeShoppingList" @click="markAll($event)" />
          <label class="form-check-label" for="typeShoppingList">Mark all as Executed</label>
        </div>
      </div>

      <!-- Add New Expense Button -->
      <div class="mt-3">
        <button type="button" class="btn btn-primary" @click="addExpense">Add Expense</button>
      </div>

      <!-- Submit Button -->
      <div class="mt-4">
        <button type="submit" class="btn btn-success">Submit Budget</button>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { budgetDTO, editBudget, fetchBudgets, getBudget } from '@/api/services/budgetService';
import { Category, fetchCategoryList } from '@/api/services/categoryService';
import { ExpensesDTO } from '@/api/services/expensesService';
import { PaymentMethod, fetchPaymentMethods } from '@/api/services/paymentService';
import router from '@/router';
import { useBudgetStore } from '@/stores/budgets';
import { title } from 'process';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';

export default {
  name: 'EditBudget',
  data() {
    return {
      balance: 0,
      budgetId: 0,
      budget: {
        id: 0,
        createdOn: new Date(),
        updatedOn: null,
        title: '',
        totalAllocatedBudget: 0,
        state: 1,
        expensesDTO: [] as ExpensesDTO[], // Initialize as an empty array
      } as budgetDTO,
      paymentMethods: [] as PaymentMethod[],
      categories: [] as Category[],
    };
  },
  methods: {
    async calculateBalance() {
      // Ensure the expensesDTO array exists and is not null/undefined
      if (Array.isArray(this.budget.expensesDTO)) {
        // Use reduce to accumulate the balance
        this.balance = this.budget.totalAllocatedBudget - this.budget.expensesDTO.reduce((balance, expense) => {
          return balance + expense.amount;
        }, 0);
      } else {
        this.balance = 0;  // Default to 0 if expensesDTO is not an array
      }
    },
    async markAll(event: Event) {
      const input = event.target as HTMLInputElement; // Type assertion to HTMLInputElement

      if (input) {
        const isChecked = input.checked; // Now 'checked' is properly recognized
        this.budget.expensesDTO.forEach(expense => {
          expense.isExecuted = isChecked; // Set all expenses based on checkbox state
        });
      }
    },
    async fetchPaymentMethods() {
      try {
        const response = await fetchPaymentMethods(); // Now it retrieves the array from $values
        this.paymentMethods = response; // Assign the array to paymentMethods
      } catch (error) {
        console.error('Error fetching payment methods:', error);
      }
    },


    async fetchCategories() {
      try {
        const response = await fetchCategoryList(1);
        this.categories = response || [];
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    },
    addExpense() {
      this.budget.expensesDTO.push({
        id: 0,
        createdOn: new Date(),
        updatedOn: null, // This can remain as null
        description: '',
        budgetId: 0,
        categoryId: 0,
        expenseDueDate: null,
        paymentMethodId: 0,
        amount: 0,
        isExecuted: false,
      });
    },
    removeExpense(index: number) {
      this.budget.expensesDTO.splice(index, 1);
    },
    async submitForm() {
      const toast = useToast();
      const router = useRouter();

      try {
        console.log('Submitting Budget:', this.budget);

        await editBudget(this.budgetId, this.budget);

        toast.success('Record has been saved!')

        await new Promise(r => setTimeout(r, 1000));

        await router.push('/Budgets');
      } catch (error) {
        console.error('Error adding budget:', error);
        toast.error('Failed to save the record.'); // Show error notification
      }
    },
  },
  async created() {
    const budgetStore = useBudgetStore();
    const toast = useToast();
    this.budgetId = budgetStore.getBudgetId || 0;

    if (this.budgetId == 0) {
      toast.warning('Unable to retrive ID');
      await router.push('/Budgets');
    } else {
      this.budget = await getBudget(this.budgetId)
      await this.fetchPaymentMethods();
      await this.fetchCategories();
      this.calculateBalance();
    }


  },
};
</script>

<style scoped>
/* Add any styles you need for this component */
</style>