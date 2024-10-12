<template>
  <div class="container mt-4">
    <form @submit.prevent="submitForm">
      <!-- Budget Section -->
      <h3>Add Budget</h3>

      <!-- Disabled Budget ID field -->
      <!-- <div>
        <fieldset disabled>
          <label class="form-label" for="budgetId">Budget ID</label>
          <input class="form-control" id="budgetId" type="text" v-model="budget.id" disabled />
        </fieldset>
      </div> -->

      <!-- Editable Month field -->
      <div class="mt-3">
        <fieldset>
          <label class="form-label" for="month">Month</label>
          <input class="form-control" id="month" type="text" v-model="budget.month" placeholder="Enter month" />
        </fieldset>
      </div>

      <!-- Editable Total Budget field -->
      <div class="mt-3">
        <fieldset>
          <label class="form-label" for="totalBudget">Total Budget</label>
          <input class="form-control" id="totalBudget" type="number" v-model="budget.totalBudget"
            placeholder="Enter total budget" />
        </fieldset>
      </div>

      <hr />

      <!-- Expenses Section -->
      <h4>Expenses</h4>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>Description</th>
            <th>Category</th>
            <th>Due Date</th>
            <th>Amount</th>
            <th>Payment Method</th>
            <th>Executed</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(expense, index) in budget.expenses" :key="index">
            <td><input v-model="expense.description" class="form-control" type="text" /></td>
            <td>
              <select v-model="expense.categoryId" class="form-control">
                <option v-for="category in categories" :key="category.id" :value="category.id">
                  {{ category.name }}
                </option>
              </select>
            </td>
            <td><input v-model="expense.dueDate" class="form-control" type="date" /></td>
            <td><input v-model="expense.amount" class="form-control" type="number" /></td>
            <td>
              <select v-model="expense.paymentMethodId" class="form-control">
                <option v-for="method in paymentMethods" :key="method.id" :value="method.id">
                  {{ method.name }}
                </option>
              </select>
            </td>
            <td><input v-model="expense.executed" class="form-check-input" type="checkbox" /></td>
            <td>
              <button type="button" class="btn btn-danger" @click="removeExpense(index)">Remove</button>
            </td>
          </tr>
        </tbody>
      </table>

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
import { fetchPaymentMethods, PaymentMethod } from '@/api/services/paymentService';
import { fetchCategoryList, Category } from '@/api/services/categoryService';

export default {
  name: 'AddBudget',
  data() {
    return {
      budget: {
        id: 0,
        createdOn: new Date(),
        updatedOn: null,
        month: '',
        totalBudget: 0,
        expenses: [
          {
            id: 0,
            createdOn: new Date(),
            updatedOn: null,
            budgetId: 0,
            budget: '',
            description: '',
            categoryId: 0,
            dueDate: '',
            paymentMethodId: 0,
            amount: 0,
            executed: false,
          },
        ],
      },
      paymentMethods: [] as PaymentMethod[],
      categories: [] as Category[], // Specifying the type for categories
    };
  },
  methods: {
    async fetchPaymentMethods() {
      try {
        this.paymentMethods = await fetchPaymentMethods();
      } catch (error) {
        console.error('Error fetching payment methods:', error);
      }
    },
    async fetchCategories() {
      try {
        this.categories = await fetchCategoryList(); 
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    },
    addExpense() {
      this.budget.expenses.push({
        id: 0,
        createdOn: new Date(),
        updatedOn: null,
        budgetId: this.budget.id,
        budget: '',
        description: '',
        categoryId: 0,
        dueDate: '',
        paymentMethodId: 0,
        amount: 0,
        executed: false,
      });
    },
    removeExpense(index: number) {
      this.budget.expenses.splice(index, 1);
    },
    submitForm() {
      console.log('Submitting Budget:', this.budget);
      // Perform the API call to submit the form data
    },
  },
  async created() {
    await this.fetchPaymentMethods();
    await this.fetchCategories(); // Fetch categories on component creation
  },
};
</script>