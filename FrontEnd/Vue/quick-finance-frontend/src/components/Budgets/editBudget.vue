<template>
  <div class="container mt-4">
    <form @submit.prevent="submitForm">
      <!-- Budget Section -->
      <h3>Edit {{ budget?.title }}</h3>

      <!-- Disabled Budget ID field -->
      <div>
        <fieldset disabled>
          <small class="form-label text-end" for="budgetId">ID: {{ budget.id }}</small>

          <!-- <input id="budgetId" class="form-control" type="text" :value="budget.id" @input="updateBudgetId" disabled /> -->

        </fieldset>
      </div>

      <div class="row">
        <!-- Editable title field -->
        <div class="col mt-3">
          <fieldset>
            <label class="form-label" for="title">Title</label>
            <input class="form-control" id="title" type="text" :value="budget.title" @input="updateBudgetTitle($event)"
              placeholder="Enter title" required />
          </fieldset>
        </div>

        <!-- Editable Total Budget field -->
        <div class="col mt-3">
          <fieldset>
            <label class="form-label" for="totalBudget">Total Budget</label>
            <input class="form-control text-end" id="totalBudget" type="number" :value="budget.totalAllocatedBudget"
              @input="updateTotalAllocatedBudget($event)" placeholder="Enter total budget" required />
            <div class="text-end p-2">
              <label for="totalbudget" class="form-label">Balance: {{ balance }}</label>
            </div>
          </fieldset>
        </div>
      </div>

      <hr />

      <!-- Expenses Section -->
      <h4>Expenses</h4>
      <table class="table table-striped" v-if="expensesDTO && expensesDTO.length > 0">
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
          <tr v-for="(expense, index) in expensesDTO" :key="index">
            <td><input v-model="expense.description" class="form-control" type="text" /></td>
            <td>
              <select v-model="expense.categoryId" class="form-control">
                <option v-for="category in categories" :key="category.id" :value="category.id">
                  {{ category.name }}
                </option>
              </select>
            </td>
            <td><input id="expenseDueDate" v-model="formattedExpenseDueDate" class="form-control" type="date" /></td>
            <td><input v-model="expense.amount" class="form-control text-end" type="number" step="0.01" min="0"
                @change="calculateBalance" /></td>
            <td>
              <select v-model="expense.paymentMethodId" class="form-control">
                <option v-for="method in paymentMethods" :key="method.id" :value="method.id"> {{
                  method.paymentMethodName }}
                </option>
              </select>
            </td>
            <td>
              <input v-model="expense.isExecuted" class="form-check-input text-center" type="checkbox"
                style="font-size: x-large" />
            </td>
            <td>
              <button type="button" class="btn btn-danger" @click="removeExpense(index)">Remove</button>
            </td>
          </tr>

          <!-- total row -->
          <tr class="table-info">
            <td scope="row" colspan=3>Total:</td>
            <td class="text-end">{{ totalAmount }}</td>
            <td scope="row" colspan=3></td>
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
import { defineComponent, ref, computed, onMounted, reactive } from 'vue';
import { Budget, budgetDTO, editBudget, getBudget } from '@/api/services/budgetService';
import { Category, fetchCategoryList } from '@/api/services/categoryService';
import { ExpensesDTO } from '@/api/services/expensesService';
import { PaymentMethod, fetchPaymentMethods as apiFetchPaymentMethods } from '@/api/services/paymentService';
import { useRouter } from 'vue-router';
import { useBudgetStore } from '@/stores/budgets';
import { useToast } from 'vue-toastification';
import { useI18n } from 'vue-i18n';
import { useAuthStore } from '@/stores/auth';

export default defineComponent({
  name: 'EditBudget',
  setup() {
    const router = useRouter();
    const toast = useToast();
    const budgetStore = useBudgetStore();
    const store = useAuthStore();

    // Reactive state properties
    const balance = ref<number>(0);
    const totalAmount = ref<number>(0);
    const budgetId = ref<number>(0);
    const budget = ref<Budget>({  // Initialize budget with a default object
      id: 0,
      title: '',
      totalAllocatedBudget: 0,
      state: 0,
      userId:store.user?.id || ''
    });
    const expensesDTO = ref<ExpensesDTO[]>([]);
    const paymentMethods = ref<PaymentMethod[]>([]);
    const categories = ref<Category[]>([]);

    const expense = ref<ExpensesDTO>({
      id: 0,
      description: '',
      budgetId: 0,
      categoryId: 0,
      expenseDueDate: new Date(),
      paymentMethodId: 0,
      amount: 0,
      isExecuted: false,
      createdOn: new Date(),
      updatedOn: new Date(),
    });


    const formattedExpenseDueDate = computed({
      get() {
        return formatDate(expense.value.expenseDueDate);
      },
      set(value: string) {
        expense.value.expenseDueDate = new Date(value);
      },
    });

    // Methods
    const cancel = () => {
      router.back();
    };

    const formatDate = (date: Date | null | undefined): string => {
      // Check if the date is null or undefined
      if (!date) {
        return 'N/A'; // or some default value
      }

      // Attempt to create a Date object
      const parsedDate = new Date(date);

      // Check if the date is valid
      if (isNaN(parsedDate.getTime())) {
        return 'Invalid date'; // Handle invalid date scenario
      }

      // Return formatted date string
      return parsedDate.toISOString().split('T')[0]; // Formats as YYYY-MM-DD
    };

    const updateBudgetTitle = (event: Event) => {
      const target = event.target as HTMLInputElement | null;
      if (target && budget.value) {
        budget.value.title = target.value; // Use string directly
      }
    };

    const updateTotalAllocatedBudget = (event: Event) => {
      const target = event.target as HTMLInputElement | null;
      if (target && budget.value) {
        budget.value.totalAllocatedBudget = Number(target.value); // Ensure to convert to number
      }
    };

    const calculateBalance = () => {
      if (Array.isArray(expensesDTO.value)) {
        const total = budget.value?.totalAllocatedBudget || 0;
        balance.value = total - expensesDTO.value.reduce((bal, expense) => bal + expense.amount, 0);
        totalAmount.value = expensesDTO.value.reduce((bal, expense) => bal + expense.amount, 0);;
      } else {
        balance.value = 0;
      }
    };

    const markAll = (event: Event) => {
      const input = event.target as HTMLInputElement;
      const isChecked = input.checked;
      expensesDTO.value.forEach((expense) => {
        expense.isExecuted = isChecked;
      });
    };

    const fetchPaymentMethods = async () => {
      try {
        const response = await apiFetchPaymentMethods();
        paymentMethods.value = response;
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
      expensesDTO.value.push({
        id: 0,
        description: '',
        budgetId: budgetId.value, // Set budgetId to the current budgetId
        categoryId: 0,
        expenseDueDate: null,
        paymentMethodId: 0,
        amount: 0,
        isExecuted: false,
        createdOn: new Date(), // Set createdOn to the current date
        updatedOn: new Date(), // Set updatedOn to the current date
      });
    };


    const removeExpense = (index: number) => {
      expensesDTO.value.splice(index, 1);
    };

    const submitForm = async () => {
      try {
        const editedBudget: budgetDTO = {
          id: budget.value.id,
          title: budget.value.title,
          totalAllocatedBudget: budget.value.totalAllocatedBudget,
          state: budget.value.state,
          userId:store.user?.id || '',
          expensesDTO: expensesDTO.value,
        };

        // console.log('Submitting Budget:', editedBudget);
        await editBudget(budgetId.value, editedBudget);
        toast.success('Record has been saved!');

        await new Promise((r) => setTimeout(r, 1000));
        await router.push('/Budgets');
      } catch (error) {
        console.error('Error adding budget:', error);
        toast.error('Failed to save the record.');
      }
    };

    onMounted(async () => {
      budgetId.value = budgetStore.getBudgetId || 0;

      if (budgetId.value === 0) {
        toast.warning('Unable to retrieve ID');
        await router.push('/Budgets');
      } else {
        const record = await getBudget(budgetId.value);
        budget.value = record;
        expensesDTO.value = record.expensesDTO;
        // console.log('budget:', budget.value);

        await fetchPaymentMethods();
        await fetchCategories();
        calculateBalance();
      }
    });

    return {
      budget,
      expensesDTO,
      balance,
      budgetId,
      paymentMethods,
      categories,
      formattedExpenseDueDate,
      totalAmount,
      cancel,
      markAll,
      calculateBalance,
      fetchPaymentMethods,
      fetchCategories,
      addExpense,
      removeExpense,
      submitForm,
      updateBudgetTitle,
      updateTotalAllocatedBudget,
    };
  },
});
</script>