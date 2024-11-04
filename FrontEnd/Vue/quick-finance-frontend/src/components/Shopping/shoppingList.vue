<template>
    <div class="container">
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
                        <td> {{ 
                        formatDate(String(record.modifiedOn)) 
                        }}</td>
                        <td class="text-end">
                            <div class="btn-group">
                                <button type="button" class="btn btn-primary">
                                    <font-awesome-icon :icon="['fas', 'table-list']" />
                                </button>
                                <button type="button" class="btn btn-secondary" @click="edit(record.id)">
                                    <font-awesome-icon :icon="['fas', 'edit']" />
                                </button>
                                <button type="button" class="btn btn-danger" @click="deleteRecord(record.id)">
                                    <font-awesome-icon :icon="['fas', 'trash']" />
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- Pagination Component -->
            <div class="d-flex justify-content-center mt-4"> <!-- Center the pagination -->
                <ul class="pagination">
                    <li :class="['page-item', { disabled: pageNumber === 1 }]">
                        <a class="page-link" href="#" @click="changePage(pageNumber - 1)" aria-label="Previous">
                            <span aria-hidden="true">&laquo;</span>
                        </a>
                    </li>
                    <li v-for="page in totalPages" :key="page" :class="['page-item', { active: pageNumber === page }]">
                        <a class="page-link" href="#" @click="changePage(page)">{{ page }}</a>
                    </li>
                    <li :class="['page-item', { disabled: pageNumber === totalPages }]">
                        <a class="page-link" href="#" @click="changePage(pageNumber + 1)" aria-label="Next">
                            <span aria-hidden="true">&raquo;</span>
                        </a>
                    </li>
                </ul>

                <!-- Row Selection Dropdown -->
                <div class="col-auto text-sm-end">
                    <div class="row mb-3">
                        <div class="col-auto text-end text-primary">
                            <!-- <label for="rowsPerPage">Rows per page:</label> -->
                            <select id="rowsPerPage" v-model="rowsPerPage" @change="loadPage" class="form-select ">
                                <option :value="5">5</option>
                                <option :value="10">10</option>
                                <option :value="20">20</option>
                                <option :value="50">50</option>
                            </select>
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, Ref, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useRoute, useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { fetchShoppingInfo, Shopping, ShoppingDTO } from '@/api/services/shoppingServices';
import { useErrorStore } from '@/stores/error';
import { useShoppingStore } from '@/stores/shopping';

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

        // pagination
        const pageNumber = ref<number>(1);
        const rowsPerPage = ref<number>(5);
        const totalPages = ref<number>(10);
        const next = ref<string>('');
        const prev = ref<string>('');
        const first = ref<string>('');
        const last = ref<string>('');

        const granTotal = ref<number>(0);


        //methods
        const cancel = () => {
            router.back();
        };

        const edit = (Id: number) => {
            const store = useShoppingStore();
            store.setShoppingId(Id);
            router.push({ name: 'ShoppingEdit' });
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
        const validURL = (option: 'F' | 'L' | 'N' | 'P'): boolean => {
            switch (option) {
                case 'P':
                    return !!prev.value;
                case 'F':
                    return !!first.value;
                case 'L':
                    return !!last.value;
                case 'N':
                    return !!next.value;
                default:
                    return false;
            }
        };

        const goTo = async (option: 'F' | 'L' | 'N' | 'P') => {
            try {
                loading.value = true;

                const opt: { [key: string]: Ref<string> } = {
                    'F': first,
                    'L': last,
                    'N': next,
                    'P': prev
                };

                const url = opt[option].value;

                // const response = await goToPage(url);
                // categories.value = response.data;
                // totalPages.value = response.totalPages;
                // next.value = response.nextPage;
                // prev.value = response.previousPage;
                // last.value = response.lastPage;
                // first.value = response.firstPage;
            } catch (err) {
                error.value = 'Failed to load budget list';
                console.error('Error loading budgets:', err);
            } finally {
                loading.value = false;
            }
        }
        const loadPage = async () => {
            try {
                loading.value = true;
                await new Promise(resolve => setTimeout(resolve, 1000)); // Show the notification for 1 seconds

                const records = await fetchShoppingInfo(pageNumber.value, rowsPerPage.value);
                ShoppingList.value = records.data


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
            rowsPerPage,
            pageNumber,
            ShoppingList,
            error,
            loading,
            granTotal,
            cancel,
            edit,
            deleteRecord,
            formatDate,
            changePage,
            loadPage,
        }
    }
});
</script>