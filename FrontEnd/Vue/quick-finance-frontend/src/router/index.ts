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
import FinanceView from '@/views/FinanceView.vue'
import ShoppingView from '@/views/ShoppingView.vue'
import ShoppingEdit from '@/components/Shopping/ShoppingEdit.vue'
import ShoppingAdd from '@/components/Shopping/ShoppingAdd.vue'
import ShoppingItemList from '@/components/Shopping/ShoppingItemList.vue'
import FinanceList from '@/components/FinanceAnalysis/FinanceList.vue'
import FinanceEdit from '@/components/FinanceAnalysis/FinanceEdit.vue'
import FinanceAdd from '@/components/FinanceAnalysis/FinanceAdd.vue'
import Login from '@/components/Users/Login.vue'
import Register from '@/components/Users/Register.vue'
import { useAuthStore } from '@/stores/auth'
import Dashboardview from '@/views/Dashboardview.vue'

const routes = [
  { path: '/', name: 'home', component: HomeView },
  //login & register
  { path: '/login', name: 'Login', component: Login },
  { path: '/register', name: 'Register', component: Register },

  //dashboard
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Dashboardview,
    meta: { requiresAuth: true } // Mark as protected
  },

  //budgets
  { path: '/budgets', name: 'budgets', component: BudgetsView, meta: { requiresAuth: true } },
  { path: '/budgets/add', name: 'addBudget', component: AddBudget, meta: { requiresAuth: true } },
  { path: '/budgets/edit', name: 'editBudget', component: EditBudget, meta: { requiresAuth: true } },
  { path: '/budgets/expenses', name: 'budgetExpenses', component: BudgetExpenses, meta: { requiresAuth: true } },

  //shopping
  { path: '/Shopping', name: 'Shopping', component: ShoppingView, meta: { requiresAuth: true } },
  { path: '/ShoppingList', name: 'ShoppingList', component: ShoppingList, meta: { requiresAuth: true } },
  { path: '/ShoppingEdit', name: 'ShoppingEdit', component: ShoppingEdit, meta: { requiresAuth: true } },
  { path: '/ShoppingItemList', name: 'ShoppingItemList', component: ShoppingItemList, meta: { requiresAuth: true } },
  { path: '/ShoppingAdd', name: 'ShoppingAdd', component: ShoppingAdd, meta: { requiresAuth: true } },

  //categories
  { path: '/categories', name: 'categories', component: CategoriesView, meta: { requiresAuth: true } },
  { path: '/categories', component: CategoriesList, meta: { requiresAuth: true } },
  { path: '/categories/add', name: 'addCategory', component: AddCategory, meta: { requiresAuth: true } },
  { path: '/categories/edit', name: 'editCategory', component: EditCategory, meta: { requiresAuth: true } },

  //Finance
  { path: '/finance', name: 'finance', component: FinanceView, meta: { requiresAuth: true } },
  { path: '/financeList', name: 'financeList', component: FinanceList, meta: { requiresAuth: true } },
  { path: '/financeEdit', name: 'financeEdit', component: FinanceEdit, meta: { requiresAuth: true } },
  { path: '/financeAdd', name: 'financeAdd', component: FinanceAdd, meta: { requiresAuth: true } },

  //settings
  { path: '/settings', name: 'settings', component: SettingsView, meta: { requiresAuth: true } },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL), // Use import.meta.env instead of process.env
  routes
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    // Redirect to login if not authenticated
    next({ name: 'Login' })
  } else {
    next() // Allow navigation
  }
})

export default router
