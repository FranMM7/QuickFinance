import { defineStore } from "pinia";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: localStorage.getItem("token") || "", // Initialize with token if available
    isAuthenticated: !!localStorage.getItem("token"), // True if token exists
    user: null as null | { id: string; username:string; roles: string[] }, // Store user info (id, roles, etc.)
  }),

  actions: {
    login(token: string, user: { id: string;username:string; roles: string[] }) {
      this.token = token;
      this.isAuthenticated = true;
      this.user = user;

      // Persist token in localStorage
      localStorage.setItem("token", token);
    },

    logout() {
      this.token = "";
      this.isAuthenticated = false;
      this.user = null;

      // Remove token from localStorage
      localStorage.removeItem("token");
    },

    initialize() {
      // Check if token exists and validate or load user info if needed
      const storedToken = localStorage.getItem("token");
      if (storedToken) {
        this.token = storedToken;
        this.isAuthenticated = true;
        // Optionally fetch user info from the backend to populate `user`
      }
    },
  },
});
