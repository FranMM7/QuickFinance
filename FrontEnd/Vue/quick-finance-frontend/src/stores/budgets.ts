// src/stores/budget.ts
import { defineStore } from 'pinia';

export const useBudgetStore = defineStore('budget', {
  // State
  state: () => ({
    budgetId: null as number | null,
    month: null as string | null,
    categoryId: null as number | null,
    paymentMethodId: null as number | null,
    expensesId: null as number | null,
  }),

  // Getters
  getters: {
    getBudgetId: (state) => state.budgetId,
    getMonth: (state) => state.month,
    getCategoryId: (state) => state.categoryId,
  },

  // Actions
  actions: {
    setBudgetId(id: number | null) {
      this.budgetId = id;
    },
    setMonth(month: string | null) {
      this.month = month;
    },
    setCategoryId(id: number | null) {
      this.categoryId = id;
    },

    // Accept two arguments instead of an object
    captureBudgetValues(budgetId: number | null, month: string | null) {
      this.setBudgetId(budgetId);
      this.setMonth(month);
    },

    getCategoryValues(categoryId: number | null) {
      this.setCategoryId(categoryId);
    },
  },
});
