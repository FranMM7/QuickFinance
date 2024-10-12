import { defineStore } from "pinia";

export const useErrorStore = defineStore('errors', {
    //state
    state: () => ({
        notification: null as string | null,
        error: null as string | null,
    }),

    //getters
    getters: {
        getError: (state) => state.error,
        getNotification: (state) => state.notification,
    },

    //actions
    actions: {
        setErrorNotification(notification: string | null, error: string | null) {
            this.notification = notification;
            this.error = error;
        }
    }
});