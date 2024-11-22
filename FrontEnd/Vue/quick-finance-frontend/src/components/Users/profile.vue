<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import apiClient from "@/api/services/apiClient";
import { error } from 'console';
import { AxiosError } from 'axios';

const toast = useToast();
const router = useRouter();
const auth = useAuthStore();
const userinfo = ref<any>();
const userName = ref('John Doe');
const name = ref('')
const middleName = ref('')
const lastName = ref('')
const email = ref('')
const anonymousData = ref<boolean>(true)
const token = auth.token;

//methods 
const setAnynimousData = (value: boolean) => {
    anonymousData.value = value
}
const submitForm = async () => { }

const loadPage = async () => {
    try {
        if (!token) {
            console.error("Token is missing.");
            return;
        }

        console.log("API Base URL:", import.meta.env.VITE_API_BASE_URL);
        console.log("Token:", token);
        console.log("Authorization Header:", `Bearer ${token}`);

        const response = await apiClient.get("/auth/getInfo", {
            baseURL: import.meta.env.VITE_API_BASE_URL,
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });

        userinfo.value = response.data;
        email.value = userinfo.value.email;

        console.log("User Info:", userinfo.value);
    } catch (error) {
        if (error instanceof AxiosError && error.response) {
            console.error("API Error:", error.response.data);
        } else if (error instanceof Error) {
            console.error("Unexpected Error:", error.message);
        } else {
            console.error("An unknown error occurred.");
        }
    }
};


// Lifecycle hooks
onMounted(() => {
    loadPage();
});
</script>
<template>

    <form>
        <fieldset>
            <div class="row">
                <div class="col">
                    <legend>User Information</legend>
                </div>
                <div class="col-auto">
                    <div class="row p-1 text-center">
                        <div class="form-check form-switch">
                            <label class="form-check-label" for="anonymousData">Set my information private</label>
                            <input class="form-check-input" type="checkbox" id="anonymousData"
                                @change="setAnynimousData(anonymousData)" v-model="anonymousData">
                        </div>
                    </div>
                </div>
            </div>
            <label class="form-label">Username</label>
            <div class="form-floating">
                <input type="text" class="form-control" v-model="userName" placeholder="Username" />
                <label>Enter your username</label>
            </div>
            <div>
                <label for="exampleInputEmail1" class="form-label mt-4">Email address</label>
                <input type="email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp"
                    placeholder="Enter email" v-model="email">
                <small id="emailHelp" class="form-text text-muted">We'll never share your email with anyone
                    else.</small>
            </div>



            <div class="row" v-if="!anonymousData">
                <hr>
                <div>
                    <label class="form-label">Name</label>
                    <div class="form-floating">
                        <input type="text" class="form-control" v-model="name" placeholder="Legal Name" />
                        <label>Legal Name</label>
                    </div>
                </div>
                <div>
                    <label class="form-label">Middle Name</label>
                    <div class="form-floating">
                        <input type="text" class="form-control" v-model="middleName" placeholder="Middle Name" />
                        <label>Middle Name</label>
                    </div>
                </div>
                <div>
                    <label class="form-label">Last Name</label>
                    <div class="form-floating">
                        <input type="text" class="form-control" v-model="lastName" placeholder="Last Name" />
                        <label>Last Name</label>
                    </div>
                </div>
            </div>

        </fieldset>

    </form>



</template>
