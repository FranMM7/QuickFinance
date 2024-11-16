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
                    username: username.value, // Ensure this matches the backend's expectations
                    password: password.value,
                });
                console.log("Login Successful:", response.data);
                toast.info("Welcome");

                // Store the token securely
                localStorage.setItem("token", response.data.token);
            } catch (error) {
                const err = error as AxiosError;
                console.error("Login Failed:", err.response?.data || err.message);
                toast.error("Failed to login. Please check your credentials.");
            }
        };


        return {
            username,
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
            <div>
                <label class="form-label mt-4">Please enter your credentials</label>
                <div class="form-floating mb-3">
                    <input type="text" class="form-control" id="floatingInput" v-model="username"
                        placeholder="Username">
                    <label for="floatingInput">Username</label>
                </div>
                <div class="form-floating">
                    <input type="password" class="form-control" id="floatingPassword" v-model="password"
                        placeholder="Password" autocomplete="off">
                    <label for="floatingPassword">Password</label>
                </div>

            </div>
            <hr>
            <button type="submit" class="btn btn-primary">Login</button>


        </form>
    </div>
</template>
