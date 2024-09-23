import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import BudgetsView from '../views/BudgetsView.vue';
import CategoriesView from '../views/CategoriesView.vue';
import SettingsView from '../views/SettingsView.vue';

const routes = [
  { path: '/', name: 'Home', component: HomeView },
  { path: '/budgets', name: 'Budgets', component: BudgetsView },
  { path: '/categories', name: 'Categories', component: CategoriesView },
  { path: '/settings', name: 'Settings', component: SettingsView },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
