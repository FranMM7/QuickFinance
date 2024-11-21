import { defineStore } from 'pinia'

export interface lSettings {
  id?: number // Optional for POST requests where the ID might be auto-generated.
  settingsName: string
  jsonValue: string
  userId: string
}

export const useSettingsStore = defineStore('settings', {
  // State with an initial value of null or an empty object
  state: () => ({
    setting: null as lSettings | null // Null by default, as we don't have the settings yet
  }),

  // Getters to retrieve the settings
  getters: {
    getSettings: (state): lSettings | null => state.setting // Returns the current settings or null
  },

  // Actions to handle API calls and manage the settings
  actions: {
    // Set settings manually (e.g., for testing or initial setup)
    setSetting(setting: lSettings) {
      this.setting = setting
    }
  }
})
