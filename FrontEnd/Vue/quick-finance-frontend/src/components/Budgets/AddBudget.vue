<template>
  <div class="container mt-4">
    <form @submit.prevent="submitForm">
      <!-- Budget Section -->
      <h3>Add Budget</h3>

      <div class="row">
        <!-- Disabled Budget ID field -->
        <!-- <div>
          <fieldset disabled>
            <label class="form-label" for="budgetId">Budget ID</label>
            <input class="form-control" id="budgetId" type="text" v-model="budget.id" disabled />
          </fieldset>
        </div> -->

        <!-- Editable title field -->
        <div class="col mt-3">
          <fieldset>
            <label class="form-label" for="title">Title</label>
            <input class="form-control" id="title" type="text" v-model="budget.title" placeholder="Enter title"
              required />
          </fieldset>
        </div>

        <!-- Editable Total Budget field -->
        <div class="col mt-3">
          <fieldset>
            <label class="form-label" for="totalBudget">Total Budget</label>
            <input class="form-control text-end" id="totalBudget" type="number" v-model="budget.totalAllocatedBudget"
              placeholder="Enter total budget" step="0.01" min="0" @change="calculateBalance" required />
            <div class="text-end p-2">
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
      <hr>
      <div class="container">
        <div class="row justify-content-start mt-4">
          <div class="col-auto">
            <button type="submit" class="btn btn-success">Submit Budget</button>
          </div>
          <div class="col-auto">
            <button @click="cancel()" type="button" class="btn btn-secondary">Cancel</button>
          </div>
        </div>
      </div>

    </form>
  </div>
</template>
<script lang="ts">
import { defineComponent, ref, reactive, onMounted } from 'vue';
import { fetchPaymentMethods as apiFetchPaymentMethods, PaymentMethod } from '@/api/services/paymentService';
import { fetchCategoryList, Category } from '@/api/services/categoryService';
import { budgetDTO, addBudget } from '@/api/services/budgetService';
import { ExpensesDTO } from '@/api/services/expensesService';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { useAuthStore } from '@/stores/auth';

export default defineComponent({
  name: 'AddBudget',
  setup() {
    const toast = useToast();
    const router = useRouter();
    const store = useAuthStore();

    const balance = ref(0);
    const budget = reactive<budgetDTO>({
      id: 0,
      createdOn: new Date(),
      updatedOn: null,
      title: '',
      totalAllocatedBudget: 0,
      state: 1,
      userId: store.user?.id || '',
      expensesDTO: [],
    });
    const paymentMethods = ref<PaymentMethod[]>([]);
    const categories = ref<Category[]>([]);

    //methods
    const cancel = () => {
      router.back();
    };

    const calculateBalance = () => {
      if (Array.isArray(budget.expensesDTO)) {
        balance.value = budget.totalAllocatedBudget - budget.expensesDTO.reduce((total, expense) => {
          return total + expense.amount;
        }, 0);
      } else {
        balance.value = 0;
      }
    };

    const markAll = (event: Event) => {
      const input = event.target as HTMLInputElement;
      if (input) {
        const isChecked = input.checked;
        budget.expensesDTO.forEach(expense => {
          expense.isExecuted = isChecked;
        });
      }
    };

    const fetchPaymentMethods = async () => {
      try {
        const response = await apiFetchPaymentMethods(); // Call the API function
        paymentMethods.value = response; // Assign the response to paymentMethods
      } catch (error) {
        console.error('Error fetching payment methods:', error);
      }
    };

    const fetchCategories = async () => {
      try {
        const response = await fetchCategoryList(1);
        categories.value = response || [];
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    };

    const addExpense = () => {
      budget.expensesDTO.push({
        id: 0,
        createdOn: new Date(),
        updatedOn: null,
        description: '',
        budgetId: 0,
        categoryId: 0,
        expenseDueDate: null,
        paymentMethodId: 0,
        amount: 0,
        isExecuted: false,
      });

      calculateBalance();
    };

    const removeExpense = (index: number) => {
      budget.expensesDTO.splice(index, 1);
      calculateBalance(); // Recalculate balance after removing an expense
    };

    const submitForm = async () => {
      try {
        // console.log('Budget to submit:', budget);
        if (!budget.userId) throw new Error('Unable to retreive userID, please login again');

        await addBudget(budget); // Call your API with the modified budget
        toast.success('Record has been saved!'); // Show success notification

        // Optional: Wait for a brief moment before redirecting
        await new Promise(resolve => setTimeout(resolve, 2000)); // Show the notification for 2 seconds

        await router.push('/Budgets'); // Redirect after successful submission
      } catch (error) {
        console.error('Error adding budget:', error);
        toast.error('Failed to save the record.'); // Show error notification
      }
    };

    // Fetch payment methods and categories when the component is mounted
    onMounted(async () => {
      await fetchPaymentMethods();
      await fetchCategories();
      addExpense();
    });

    return {
      balance,
      budget,
      paymentMethods,
      categories,
      cancel,
      calculateBalance,
      markAll,
      fetchPaymentMethods,
      fetchCategories,
      addExpense,
      removeExpense,
      submitForm,
    };
  },
});
</script>
