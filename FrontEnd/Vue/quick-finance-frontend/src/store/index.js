import { createStore } from 'vuex';

// Define your state
const state = {
    expenses: [], // Array to store expenses
    budgets: [], // Array to store budgets
    categories: [] // Array to store categories
};

// Define your mutations
const mutations = {
    SET_EXPENSES(state, expenses) {
        state.expenses = expenses; // Set the expenses in the state
    },
    SET_BUDGETS(state, budgets) {
        state.budgets = budgets; // Set the budgets in the state
    },
    SET_CATEGORIES(state, categories) {
        state.categories = categories; // Set the categories in the state
    }
};

// Define your base URL for API calls
const BASE_URL = 'http://localhost:7271/api';

// Define your actions
const actions = {
    fetchExpenses({ commit }) {
        // Fetch expenses from the API
        return fetch(`${BASE_URL}/Expenses`)
            .then(response => {
                // Check if the response is ok (status in the range 200-299)
                if (!response.ok) {
                    console.error('Network response was not ok:', response); // Log the response if not ok
                    throw new Error('Network response was not ok');
                }
                return response.json(); // Parse the JSON from the response
            })
            .then(data => {
                console.log('Fetched expenses:', data); // Log the fetched expenses
                commit('SET_EXPENSES', data); // Commit the expenses to the state
            })
            .catch(error => {
                console.error('Error fetching expenses:', error); // Log any errors that occur
            });
    },
    fetchBudgets({ commit }) {
        // Fetch budgets from the API
        return fetch(`${BASE_URL}/Budgets`)
            .then(response => {
                if (!response.ok) {
                    console.error('Network response was not ok:', response); // Log the response if not ok
                    throw new Error('Network response was not ok');
                }
                return response.json(); // Parse the JSON from the response
            })
            .then(data => {
                console.log('Fetched budgets:', data); // Log the fetched budgets
                commit('SET_BUDGETS', data); // Commit the budgets to the state
            })
            .catch(error => {
                console.error('Error fetching budgets:', error); // Log any errors that occur
            });
    },
    fetchCategories({ commit }) {
        // Fetch categories from the API
        return fetch(`${BASE_URL}/Categories`)
            .then(response => {
                if (!response.ok) {
                    console.error('Network response was not ok:', response); // Log the response if not ok
                    throw new Error('Network response was not ok');
                }
                return response.json(); // Parse the JSON from the response
            })
            .then(data => {
                console.log('Fetched categories:', data); // Log the fetched categories
                commit('SET_CATEGORIES', data); // Commit the categories to the state
            })
            .catch(error => {
                console.error('Error fetching categories:', error); // Log any errors that occur
            });
    }
};

// Create the Vuex store
const store = createStore({
    state,
    mutations,
    actions
});

export default store;
