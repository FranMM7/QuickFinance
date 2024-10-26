import { defineStore } from "pinia";

export const useShoppingStore = defineStore('shopping', {

    //state 
    state:() => ({
        shoppingId:null as number | null,        
    }),

    //getters
    getters: {
        getShoppingId: (state) => state.shoppingId,
    },

    //actions
    actions: {
        setShoppingId(Id:number | null) {
            this.shoppingId = Id;
        },
    },
});