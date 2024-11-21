<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { location } from '@/api/services/locationServices';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { useAuthStore } from '@/stores/auth';
import { useErrorStore } from '@/stores/error';
import { ListLoader } from 'vue-content-loader';
import ErrorCard from '../error/errorCard.vue';

export default defineComponent({
    name: 'LocationsList',
    components: {
        ListLoader,
        ErrorCard
    },
    setup() {
        const router = useRouter()
        const toast = useToast()
        const store = useAuthStore
        const loading = ref<boolean>(true)
        const showLoader = ref<boolean>(false)
        let loaderTimeout: ReturnType<typeof setTimeout>;
        const error = ref<string>('')
        const errorStore = useErrorStore()

        //data
        const list = ref<Location[]>([])

        const loadPage = async () => {
            try {

            } catch (error) {
                console.error('Error while fetching data', error)
            }
        };

        onMounted(async () => {
            loadPage()
        });

        return {
            list,
        };
    }

});
</script>
<template>
    <div>
        LocationsList
    </div>
</template>