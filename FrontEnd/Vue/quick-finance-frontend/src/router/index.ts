import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import Dashboardview from '@/views/Dashboardview.vue'
import BudgetsView from '@/views/BudgetsView.vue'
import CategoriesView from '@/views/CategoriesView.vue'
import CategoriesList from '../components/Categories/CategoriesList.vue'
import AddCategory from '../components/Categories/AddCategory.vue'
import EditCategory from '../components/Categories/EditCategory.vue'
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
import Profile from '@/components/Users/profile.vue'
import Notfound from '@/components/404/notfound.vue'

const routes = [
  { path: '/:pathMatch(.*)*', name: 'NotFound', component: Notfound },

  { path: '/', name: 'home', component: HomeView },

  // login & register
  { path: '/login', name: 'Login', component: Login },
  { path: '/register', name: 'Register', component: Register },

  // Protected routes
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Dashboardview,
    meta: { requiresAuth: true }
  },
  {
    path: '/profile',
    name: 'Profile',
    component: Profile,
    meta: { requiresAuth: true }
  },
  {
    path: '/budgets',
    name: 'budgets',
    component: BudgetsView,
    meta: { requiresAuth: true },
    children: [
      { path: 'add', name: 'addBudget', component: AddBudget },
      { path: 'edit', name: 'editBudget', component: EditBudget },
      { path: 'expenses', name: 'budgetExpenses', component: BudgetExpenses }
    ]
  },

  // Shopping routes
  {
    path: '/shopping',
    name: 'Shopping',
    component: ShoppingView,
    meta: { requiresAuth: true },
    children: [
      { path: 'list', name: 'ShoppingList', component: ShoppingList },
      { path: 'edit', name: 'ShoppingEdit', component: ShoppingEdit },
      { path: 'add', name: 'ShoppingAdd', component: ShoppingAdd },
      { path: 'items', name: 'ShoppingItemList', component: ShoppingItemList }
    ]
  },

  // Categories routes
  {
    path: '/categories',
    name: 'categories',
    component: CategoriesView,
    meta: { requiresAuth: true },
    children: [
      { path: 'list', component: CategoriesList },
      { path: 'add', name: 'addCategory', component: AddCategory },
      { path: 'edit', name: 'editCategory', component: EditCategory }
    ]
  },

  // Finance routes
  {
    path: '/finance',
    name: 'finance',
    component: FinanceView,
    meta: { requiresAuth: true },
    children: [
      { path: 'list', name: 'childFinanceList', component: FinanceList },
      { path: 'edit', name: 'financeEdit', component: FinanceEdit },
      { path: 'add', name: 'childfinanceAdd', component: FinanceAdd }
    ]
  },

  {
    path: '/financeList',
    name: 'financeList',
    component: FinanceList,
    meta: { requiresAuth: true }
  },
  {
    path: '/financeAdd',
    name: 'financeAdd',
    component: FinanceAdd,
    meta: { requiresAuth: true }
  },

  // Settings route
  { path: '/settings', name: 'settings', component: SettingsView, meta: { requiresAuth: true } }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

// Global navigation guard
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()

  // Check if the route requires authentication
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next({ name: 'Login' }) // Redirect to login if not authenticated
  } else {
    next() // Proceed to the route
  }
})

export default router
