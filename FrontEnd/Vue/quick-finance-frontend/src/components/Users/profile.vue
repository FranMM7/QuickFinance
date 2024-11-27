<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import apiClient, { updateUserInfo, userInfo } from "@/api/services/apiClient";
import { error } from 'console';
import { AxiosError } from 'axios';

const toast = useToast();
const router = useRouter();
const auth = useAuthStore();

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
const submitForm = async () => {
    try {
        // Ensure all required fields are filled
        if (!anonymousData.value) {
            if (!email.value || !name.value || !lastName.value) {
                toast.error('Please fill in all required fields.');
                return;
            }
        } else {
            if (!email.value) {
                toast.error('Please fill in all required fields.');
                return;
            }
        }

        // Create user object
        const user: userInfo = {
            userName: userName.value.trim(),
            email: email.value.trim(),
            anonymousData: anonymousData.value,
            name: name.value.trim(),
            middleName: middleName.value?.trim(),
            lastName: lastName.value.trim(),
        };

        // Make API call to update user info
        const statusCode = await updateUserInfo(user);

        // Handle success response
        if (statusCode === 200) {
            // Store token and user data in auth store
            auth.login(auth.token, {
                id: auth.user?.id || '',
                username: auth.user?.username || '',
                fullName: auth.user?.fullName || '',
                email: email.value,
                anonymousData: anonymousData.value,
                firstName: name.value,
                middleName: middleName.value,
                lastName: lastName.value,
                roles: auth.user?.roles || [],
            });
            toast.success('User information updated successfully!');
            router.push('/dashboard');
        } else {
            toast.error('Unexpected response from the server.');
        }
    } catch (error) {
        // Handle different error scenarios
        if (error instanceof AxiosError && error.response) {
            toast.error(`API Error: ${error.response.data?.message || 'Something went wrong.'}`);
            console.error("API Error:", error.response.data);
        } else if (error instanceof Error) {
            toast.error(`Error: ${error.message}`);
            console.error("Unexpected Error:", error.message);
        } else {
            toast.error('An unknown error occurred. Please try again later.');
            console.error("Unknown Error:", error);
        }
    }
};


const loadPage = async () => {
    try {
        if (!token) {
            console.error("Token is missing.");
            return;
        }
        // console.log('auth.user: ', auth.user)

        userName.value = auth.user?.username || ''
        email.value = auth.user?.email || ''
        anonymousData.value = auth.user?.anonymousData || false

        if (!anonymousData.value) {
            name.value = auth.user?.firstName || ''
            middleName.value = auth.user?.middleName || ''
            lastName.value = auth.user?.lastName || ''
        }

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

    <form class="form" @submit.prevent="submitForm">
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

            <div class="row">
                <label for="staticEmail" class="col-sm-1 col-form-label">Username:</label>
                <div class="col">
                    <input type="text" class="form-control-plaintext" id="staticEmail" v-model="userName" disabled>
                </div>
            </div>
            <div class="row">
                <div class="mb-3">
                    <label class="form-label">Email</label>
                    <div class="form-floating">
                        <input type="text" class="form-control" v-model="email" placeholder="Email" />
                        <label>Enter your Email</label>
                    </div>
                </div>
            </div>

            <div class="row" v-if="!anonymousData">
                <hr>
                <div class="mb-3">
                    <label class="form-label">Name</label>
                    <div class="form-floating">
                        <input type="text" class="form-control" v-model="name" placeholder="Legal Name" />
                        <label>Legal Name</label>
                    </div>
                </div>
                <div class="mb-3">
                    <label class="form-label">Middle Name</label>
                    <div class="form-floating">
                        <input type="text" class="form-control" v-model="middleName" placeholder="Middle Name" />
                        <label>Middle Name</label>
                    </div>
                </div>
                <div class="mb-3">
                    <label class="form-label">Last Name</label>
                    <div class="form-floating">
                        <input type="text" class="form-control" v-model="lastName" placeholder="Last Name" />
                        <label>Last Name</label>
                    </div>
                </div>
            </div>

            <div class="row mb-3 justify-content-end">
                <div class="col-auto">
                    <button class="btn btn-lg btn-primary">Update</button>
                </div>
            </div>

        </fieldset>

    </form>



</template>
