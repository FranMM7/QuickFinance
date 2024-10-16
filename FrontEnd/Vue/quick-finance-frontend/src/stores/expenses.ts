import { defineStore } from "pinia";

export const useExpensesStore = defineStore('expenses', {
    //state
    state: () => ({
        expenseId: null as number | null,
    }),

    //Getters
    getters: {
        getExpenseId: (state) => state.expenseId,
    },

    //actions
    actions: {
        setExpenseId(id: number | null) {
            this.expenseId = id;
        },
    },

});