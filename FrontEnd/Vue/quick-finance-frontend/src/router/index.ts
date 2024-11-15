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

const routes = [
  { path: '/', name: 'home', component: HomeView },
  //login & register
  { path: '/login', name: 'Login', component: Login },
  { path: '/register', name: 'Register', component: Register },
  //budgets
  { path: '/budgets', name: 'budgets', component: BudgetsView },
  { path: '/budgets/add', name: 'addBudget', component: AddBudget },
  { path: '/budgets/edit', name: 'editBudget', component: EditBudget },
  { path: '/budgets/expenses', name: 'budgetExpenses', component: BudgetExpenses },

  //shopping
  { path: '/Shopping', name: 'Shopping', component: ShoppingView },
  { path: '/ShoppingList', name: 'ShoppingList', component: ShoppingList },
  { path: '/ShoppingEdit', name: 'ShoppingEdit', component: ShoppingEdit },
  { path: '/ShoppingItemList', name: 'ShoppingItemList', component: ShoppingItemList },
  { path: '/ShoppingAdd', name: 'ShoppingAdd', component: ShoppingAdd },

  //categories
  { path: '/categories', name: 'categories', component: CategoriesView },
  { path: '/categories', component: CategoriesList },
  { path: '/categories/add', name: 'addCategory', component: AddCategory },
  { path: '/categories/edit', name: 'editCategory', component: EditCategory },

  //Finance
  { path: '/finance', name: 'finance', component: FinanceView },
  { path: '/financeList', name: 'financeList', component: FinanceList },
  { path: '/financeEdit', name: 'financeEdit', component: FinanceEdit },
  { path: '/financeAdd', name: 'financeAdd', component: FinanceAdd },

  //settings
  { path: '/settings', name: 'settings', component: SettingsView }
]
//33389632
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL), // Use import.meta.env instead of process.env
  routes
})

export default router
