// src/stores/themeStore.ts
import { defineStore } from 'pinia';

export const useThemeStore = defineStore({
  id: 'theme',
  state: () => ({
    currentTheme: localStorage.getItem('theme') || 'default',
  }),
  actions: {
    setTheme(theme: string) {
      this.currentTheme = theme;
      localStorage.setItem('theme', theme);
      this.loadTheme(theme);
    },
    loadTheme(theme: string) {
      const link = document.getElementById('theme-link') as HTMLLinkElement;
      if (link) {
        link.href = theme === 'default' 
          ? 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css' 
          : `https://bootswatch.com/5/${theme}/bootstrap.min.css`;

          // https://bootswatch.com/5/cosmo/bootstrap.min.css
      }
    },
  },
});
