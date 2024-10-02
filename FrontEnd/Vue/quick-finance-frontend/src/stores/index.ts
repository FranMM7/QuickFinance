// src/store/index.js

import { createStore } from 'vuex';

const store = createStore({
    state: {
        budgetId: null, // Initialize with null or a valid ID
        month: null, // Same for month
        categoryId:null,
        paymentMethodId:null,
        expensesId:null,
    },
    getters: {
        budgetId: (state) => state.budgetId,
        month: (state) => state.month,
    },
    mutations: {
        setBudgetId(state, id) {
            state.budgetId = id;
        },
        setMonth(state, month) {
            state.month = month;
        },
    },
    actions: {
        captureBudgetValues({ commit }, { budgetId, month }) {
            commit('setBudgetId', budgetId); //captures budgetid and assing its value in the muttations 
            commit('setMonth', month);
        },
    },
});

export default store;
