import { defineStore } from "pinia";

export const useCategoryStore = defineStore('category', {
    //state
    state:() => ({
        categoryId: null as number | null,
       
    }),

    //getters
    getters:{
        getCategoryId: (state) => state.categoryId,
    },

    //actions
    actions: {
        setCategoryId(id:number |null) {
            this.categoryId=id;
        },
       
    },
});