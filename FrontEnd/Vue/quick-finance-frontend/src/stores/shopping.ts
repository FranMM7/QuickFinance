import { defineStore } from "pinia";

export const useShoppingStore = defineStore('shopping', {
    // state
    state: () => ({
        shoppingId: null as number | null, // shoppingId can be a number or null
    }),

    // getters
    getters: {
        getShoppingId: (state) => state.shoppingId, // Returns the current shoppingId
    },

    // actions
    actions: {
        setShoppingId(Id: number | null) {
            this.shoppingId = Id; // Set the shoppingId in state
        },
    },
});
