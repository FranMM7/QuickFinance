import { createRouter, createWebHistory } from 'vue-router'; // Import functions to set up routing
import HomeView from '@/views/HomeView.vue';
import BudgetsView from '@/views/BudgetsView.vue';
import CategoriesView from '@/views/CategoriesView.vue';
import CategoriesList from '../components/Categories/CategoriesList.vue';
import AddCategory from '../components/Categories/AddCategory.vue';
import EditCategory from '../components/Categories/EditCategory.vue';

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView,
  },
  {
    path: '/budgets',
    name: 'budgets',
    component: BudgetsView,
  },
  {
    path: '/categories',
    name: 'categories',
    component: CategoriesView,
  },
  { path: '/categories', component: CategoriesList },
  { path: '/categories/add', component: AddCategory },
  { path: '/categories/edit/:id', name: 'edit-category', component: EditCategory },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL), // Use import.meta.env instead of process.env
  routes,
});

export default router;
