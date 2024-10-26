import { createRouter, createWebHistory } from 'vue-router' // Import functions to set up routing
import HomeView from '@/views/HomeView.vue'
import BudgetsView from '@/views/BudgetsView.vue'
import CategoriesView from '@/views/CategoriesView.vue'
import CategoriesList from '../components/Categories/CategoriesList.vue'
import AddCategory from '../components/Categories/AddCategory.vue'
import EditCategory from '../components/Categories/EditCategory.vue'
import path from 'path'
import SettingsView from '@/views/SettingsView.vue'
import AddBudget from '@/components/Budgets/AddBudget.vue'
import EditBudget from '@/components/Budgets/editBudget.vue'
import BudgetExpenses from '@/components/Budgets/budgetExpenses.vue'
import ShoppingList from '@/components/Shopping/shoppingList.vue'

const routes = [
  { path: '/', name: 'home', component: HomeView },
  //budgets
  { path: '/budgets', name: 'budgets', component: BudgetsView },
  { path: '/budgets/add', name: 'addBudget', component: AddBudget },
  { path: '/budgets/edit', name: 'editBudget', component: EditBudget },
  { path: '/budgets/expenses', name: 'budgetExpenses', component: BudgetExpenses },

  //shopping
  { path: '/ShoppingList', name: 'ShoppingList', component: ShoppingList },

  //categories
  { path: '/categories', name: 'categories', component: CategoriesView },
  { path: '/categories', component: CategoriesList },
  { path: '/categories/add', name: 'addCategory', component: AddCategory },
  { path: '/categories/edit', name: 'editCategory', component: EditCategory },

  //Finance
  { path: '/finance', name: 'finance', component: HomeView },

  //shopping list
  { path: '/shoppinglist', name: 'shoppinglist', component: HomeView },

  //settings
  { path: '/settings', name: 'settings', component: SettingsView }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL), // Use import.meta.env instead of process.env
  routes
})

export default router
