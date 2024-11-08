<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useToast } from 'vue-toastification';
import { useRouter } from 'vue-router';
import { useFinanceStore } from '@/stores/finance';
import { Finance, FinanceDetails } from '@/api/services/financeServices';
import { title } from 'process';

export default defineComponent({
    name: 'financeEdit',
    component: {
        ListLoader,
        Error
    },
    setup() {
        const toast = useToast()
        const router = useRouter()
        const store = useFinanceStore()
        const loading = ref<boolean>(true)
        const showLoader = ref<boolean>(false)
        let loaderTimeout: ReturnType<typeof setTimeout>;
        const showEditIcon = ref(false);
        const isEditingTitle = ref(false);

        //data
        const financeData = ref<Finance>();
        const financeDetails = ref<FinanceDetails[]>([])
        const title = ref<string>('')


        const editTitle = (visible: boolean) => {
            isEditingTitle.value = visible;
        };

        const loadPage = async () => {
            try {

                loading.value = true;
                showLoader.value = false; // Reset loader visibility
                clearTimeout(loaderTimeout); // Clear any previous timeout

                // Set a timeout to show loader only if loading takes >1 second
                loaderTimeout = setTimeout(() => {
                    if (loading.value) showLoader.value = true;
                }, 1000);

                const id = store.id || 0
                title.value = store.strTitle

                if (id != 0) {
                    const list = store.list
                    // financeDetails.value = list.$values.financeDetails

                }
                else {
                    toast.warning('Fail to retrieve data')
                }

            } catch (error) {
                console.log('Error when loading data:', error)
                toast.error('Unexpected error occurr while loading data')
            }
            finally {
                loading.value = false;
                showLoader.value = false;
            }
        }

        onMounted(async () => {
            loadPage()
        });

        return {
            showLoader,
            title,
            financeDetails,
            editTitle,
            showEditIcon,
            isEditingTitle
        }
    }
})
</script>

<template>
    <div v-if="showLoader">
        <ListLoader />
    </div>
    <div v-if="error">
        <Error />
    </div>
    <div v-else>
        <form action="submit">
            <div class="row">
                <div class="col">
                    <div @mouseenter="showEditIcon = true" @mouseleave="showEditIcon = false"
                        class="d-flex align-items-center">
                        <h1 v-if="!isEditingTitle">{{ title }}</h1>
                        <input v-else type="text" v-model="title" class="form-control" required>

                        <!-- Edit Icon -->
                        <button v-if="showEditIcon && !isEditingTitle" @click="editTitle(true)"
                            class="btn btn-link p-0 ms-2">
                            <font-awesome-icon :icon="['fas', 'pencil-alt']" />
                        </button>

                        <!-- Cancel -->
                        <button v-if="showEditIcon && isEditingTitle" @click="editTitle(false)"
                            class="btn btn-danger p-0 ms-2">
                            <font-awesome-icon :icon="['fas', 'ban']" />
                        </button>
                    </div>
                </div>
                <div class="col-auto">
                    <div class="btn-group">
                        <button class="btn btn-primary">Edit</button>
                        <button class="btn btn-secondary">Cancel</button>
                    </div>
                </div>
            </div>
            <hr>
            <div>
                <table class="table table-stripted" v-if="financeDetails.length > 0">
                    <thead>
                        <tr>
                            <td>Description</td>
                            <td>Category</td>
                            <td>Expense Type</td>
                            <td>Amount</td>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- <tr v-for="(item, index) in itemsList" :key="index"> -->
                        <tr v-for="(item, index) in financeDetails" :key="index">
                            <td>{{ item.description }}</td>
                            <td>{{ item.category }}</td>
                            <td>{{ item.expenseCategory }}</td>
                            <td>{{ item.amount }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </form>
    </div>
</template>