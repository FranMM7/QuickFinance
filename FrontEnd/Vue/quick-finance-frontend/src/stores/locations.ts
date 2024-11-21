import { defineStore } from 'pinia'

export const useLocationStore = defineStore('location', {
  //state
  state: () => ({
    locationId: null as number | null
  }),

  //getters
  getters: {
    getLocationId: (state) => state.locationId
  },

  //actions
  actions: {
    setLocationId(id: number | null) {
      this.locationId = id
    }
  }
})
