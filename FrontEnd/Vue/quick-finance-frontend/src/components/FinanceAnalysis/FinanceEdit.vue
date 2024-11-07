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

        //data
        const financeData = ref<Finance>();
        const financeDetails = ref<FinanceDetails[]>([])
        const title = ref<string>('')

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

                }


                // const data = store.financeRecord

                // if (data) {
                //     financeData.value={
                //         title: data.title,
                //         id: data.id,
                //         state:1,
                //         createdOn:new Date(),
                //         updatedOn:new Date()
                //     }
                //     financeDetails.value = data.financeDetails
                //     title.value = data.title
                //     // financeDetails.value = await
                // }
                // else{
                //     toast.warning("Data was not retrieve")
                // }

            } catch (error) {
                console.log('Error when loading data:', error)
                toast.error(error)
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
            financeDetails
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
            <h1>{{ title }}</h1>
        </form>
    </div>
</template>