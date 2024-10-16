<template>
    <div class="container mt-4">
      <form @submit.prevent="submitForm">
        <!-- Budget Section -->
        <h3>Add Budget</h3>
  
        <!-- Disabled Budget ID field -->
        <div>
          <fieldset disabled>
            <label class="form-label" for="budgetId">Budget ID</label>
            <input class="form-control" id="budgetId" type="text" v-model="budget.id" disabled />
          </fieldset>
        </div>
  
        <!-- Editable title field -->
        <div class="mt-3">
          <fieldset>
            <label class="form-label" for="title">title</label>
            <input class="form-control" id="title" type="text" v-model="budget.title" placeholder="Enter title" />
          </fieldset>
        </div>
  
        <!-- Editable Total Budget field -->
        <div class="mt-3">
          <fieldset>
            <label class="form-label" for="totalBudget">Total Budget</label>
            <input class="form-control" id="totalBudget" type="number" v-model="budget.totalAllocatedBudget"
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
              <!-- <td><input v-model="expense.category.name" class="form-control" type="text" /></td> -->
              <td><input v-model="expense.dueDate" class="form-control" type="date" /></td>
              <td><input v-model="expense.amount" class="form-control" type="number" /></td>
              <!-- <td><input v-model="expense.paymentMethod.name" class="form-control" type="text" /></td> -->
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
  export default {
    name: 'EditBudget',
    data() {
      return {
        budget: {
          id: 0, // Assuming this is auto-generated
          createdOn: new Date(),
          updatedOn: null,
          title: '',
          totalAllocatedBudget: 0,
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
      };
    },
    methods: {
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
        // axios.post('/api/budget', this.budget)
        //   .then(response => {
        //     console.log('Budget submitted:', response.data);
        //   })
        //   .catch(error => {
        //     console.error('Error submitting budget:', error);
        //   });
      },
    },
  };
  </script>
  
  <style scoped>
  /* Add any styles you need for this component */
  </style>