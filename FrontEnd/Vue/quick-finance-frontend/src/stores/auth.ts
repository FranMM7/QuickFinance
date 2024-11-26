import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || '', // Initialize with token if available
    isAuthenticated: !!localStorage.getItem('token'), // True if token exists
    user: JSON.parse(localStorage.getItem('user') || 'null') as null | {
      id: string
      username: string
      fullName: string
      anonymousData: boolean
      firstName: string
      middleName: string
      lastName: string
      roles: string[]
    } // Store user info
  }),

  actions: {
    login(
      token: string,
      user: {
        id: string
        username: string
        fullName: string
        anonymousData: boolean
        firstName: string
        middleName: string
        lastName: string
        roles: string[]
      }
    ) {
      this.token = token
      this.isAuthenticated = true
      this.user = user

      // Persist token and user in localStorage
      localStorage.setItem('token', token)
      localStorage.setItem('user', JSON.stringify(user))
    },

    logout() {
      this.token = ''
      this.isAuthenticated = false
      this.user = null

      // Remove token and user from localStorage
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    },

    initialize() {
      // Check if token exists and validate or load user info if needed
      const storedToken = localStorage.getItem('token')
      const storedUser = localStorage.getItem('user')

      if (storedToken && storedUser) {
        this.token = storedToken
        this.user = JSON.parse(storedUser)
        this.isAuthenticated = true
      } else {
        this.logout() // Clear state if no valid token or user data is found
      }
    }
  }
})
