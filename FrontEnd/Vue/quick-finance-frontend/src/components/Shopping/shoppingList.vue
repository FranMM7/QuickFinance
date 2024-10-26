<template>
    <div class="container flex">
        <div v-if="loading">
            <list-loader />
        </div>
        <div v-else-if="error">
            <Error />
        </div>
        <div v-else class="row">
          

            <table class="table table-striped text-center">
                <thead>
                    <tr class="text-center">
                        <td>Description</td>
                        <td>Modified On</td>
                        <td></td>
                    </tr>
                </thead>
                <tbody>
                    <tr class="text-center" v-for="record in ShoppingList || []" :key="record.id">
                        <td> {{ record.description }} </td>
                        <td> {{ formatDate(String(record.modifiedOn)) }}</td>
                        <td class="text-end">
                            <div class="btn-group">
                                <button type="button" class="btn btn-primary">View</button>
                                <button type="button" class="btn btn-secondary">Edit</button>
                                <button type="button" class="btn btn-danger">Delete</button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- Pagination Component -->
            <div>
                <ul class="pagination">
                    <li :class="['page-item', { disabled: currentPage === 1 }]">
                        <a class="page-link" href="#" @click="changePage(currentPage - 1)" aria-label="Previous">
                            <span aria-hidden="true">&laquo;</span>
                        </a>
                    </li>
                    <li v-for="page in totalPages" :key="page" :class="['page-item', { active: currentPage === page }]">
                        <a class="page-link" href="#" @click="changePage(page)">{{ page }}</a>
                    </li>
                    <li :class="['page-item', { disabled: currentPage === totalPages }]">
                        <a class="page-link" href="#" @click="changePage(currentPage + 1)" aria-label="Next">
                            <span aria-hidden="true">&raquo;</span>
                        </a>
                    </li>
                </ul>
            </div>

        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useRoute, useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { fetchShoppingInfo, Shopping, ShoppingDTO } from '@/api/services/shoppingServices';
import { useErrorStore } from '@/stores/error';

export default defineComponent({
    name: 'ShoppingList',
    components: {
        ListLoader,
        Error,
    },
    setup() {

        const loading = ref<boolean>(true);
        const error = ref<String>('');
        const router = useRouter();
        const toast = useToast();

        const ShoppingList = ref<Shopping[]>([]);

        const pageNumber = ref<number>(1);
        const rowsPage = ref<number>(10);
        const totalPages = ref<number>(1);

        const granTotal = ref<number>(0);


        //methods
        const cancel = () => {
            router.back();
        };

        const edit = (Id: number) => {
            console.log(Id);
        };

        const deleteRecord = (Id: number) => {
            console.log(Id);
        };

        const formatDate = (dateString: string) => {
            const date = new Date(dateString);
            return date.toLocaleDateString();
        };
        const changePage = (page: number) => {
            if (page >= 1 && page <= totalPages.value) {
                pageNumber.value = page;
                loadPage();
            }
        };

        const loadPage = async () => {
            try {
                loading.value = true;
                await new Promise(resolve => setTimeout(resolve, 1000)); // Show the notification for 1 seconds

                const records = await fetchShoppingInfo(pageNumber.value, rowsPage.value);
                ShoppingList.value = records


            } catch (err) {
                error.value = 'Failed to load Budget Expenses Details';
                console.log('Error msg:', err);
                const errorStore = useErrorStore();
                errorStore.setErrorNotification(String(error.value), String(error));
            }
            finally {
                loading.value = false;
            }
        }

        //initilized on mounted
        onMounted(() => {
            loadPage();
        });

        return {
            totalPages,
            rowsPage,
            pageNumber,
            ShoppingList,
            error,
            loading,
            granTotal,
            cancel,
            edit,
            formatDate,
            changePage,
        }
    }
});
</script>