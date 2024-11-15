<script lang="ts">
import apiClient from "@/api/services/apiClient";
import { defineComponent, ref } from "vue";
import { useToast } from "vue-toastification";
import { AxiosError } from "axios";

export default defineComponent({
    name: "Login",
    setup() {
        const username = ref("");
        const password = ref("");
        const toast = useToast();

        const handleSubmit = async () => {
            try {
                const response = await apiClient.post("/auth/login", {
                    username: username.value,
                    password: password.value,
                });
                console.log("Login Successful:", response.data);
                toast.info("Welcome");

                // Handle token storage (localStorage/sessionStorage)
                localStorage.setItem("token", response.data.token);
            } catch (error) {
                const err = error as AxiosError;
                console.error("Login Failed:", err.response?.data || err.message);
                toast.error("Failed to login. Please check your credentials.");
            }
        };

        return {
            email: username,
            password,
            handleSubmit,
        };
    },
});
</script>


<template>
    <div class="login-container">
        <h2>Login</h2>
        <form @submit.prevent="handleSubmit">
            <div class="form-group">
                <label for="email" class="form-label mt-4">Email address</label>
                <input type="email" class="form-control" id="email" v-model="email" aria-describedby="emailHelp"
                    placeholder="Enter email" required>
                <small id="emailHelp" class="form-text text-muted">We'll never share your email with anyone
                    else.</small>
            </div>
            <div class="form-group">
                <label for="password" class="form-label mt-4">Password</label>
                <input type="password" class="form-control" id="password" placeholder="Password" autocomplete="off"
                    required>
            </div>
            <hr>
            <button type="submit" class="btn btn-primary">Login</button>


        </form>
    </div>
</template>
