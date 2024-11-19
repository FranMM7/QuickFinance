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
                        <td>Grand Total</td>
                        <td>Modified On</td>
                        <td></td>
                    </tr>
                </thead>
                <tbody>
                    <tr class="text-center" v-for="record in ShoppingList || []" :key="record.id">
                        <td> {{ record.description }} </td>
                        <td> {{ record.grandTotal.toFixed(2) }} </td>
                        <td> {{ formatDate(String(record.modifiedOn)) }}</td>
                        <td class="text-end">
                            <div class="btn-group">
                                <button type="button" class="btn btn-primary" @click="view(record.id)">
                                    <font-awesome-icon :icon="['fas', 'table-list']" />
                                </button>
                                <button type="button" class="btn btn-success" @click="clone(record.id)">
                                    <font-awesome-icon :icon="['fas', 'clone']" />
                                </button>
                                <button type="button" class="btn btn-secondary" @click="edit(record.id)">
                                    <font-awesome-icon :icon="['fas', 'edit']" />
                                </button>
                                <button type="button" class="btn btn-warning" disabled>
                                    <font-awesome-icon :icon="['fas', 'share-nodes']" />
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
                    <li :class="['page-item', { disabled: !validURL('F') }]">
                        <a class="page-link" href="#" @click.prevent="goTo('F')">First</a>
                    </li>
                    <li :class="['page-item', { disabled: !validURL('P') }]">
                        <a class="page-link" href="#" @click.prevent="goTo('P')">Previous</a>
                    </li>
                    <li v-for="page in totalPages" :key="page" :class="['page-item', { active: page === pageNumber }]">
                        <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
                    </li>

                    <!-- Row Selection Dropdown -->
                    <label for="rowsPerPage" class="page-item page-link disabled">View:</label>
                    <select id="rowsPerPage" v-model="rowsPerPage" @change="loadPage" class="page-item page-link">
                        <option :value="5">5</option>
                        <option :value="10">10</option>
                        <option :value="20">20</option>
                        <option :value="50">50</option>
                    </select>

                    <li :class="['page-item', { disabled: !validURL('N') }]">
                        <a class="page-link" href="#" @click.prevent="goTo('N')">Next</a>
                    </li>
                    <li :class="['page-item', { disabled: !validURL('L') }]">
                        <a class="page-link" href="#" @click.prevent="goTo('L')">Last</a>
                    </li>
                </ul>

            </div> <!-- Pagination Component -->

        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, Ref, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { fetchShoppingInfo, Shopping, goToPage, getCloneShopping } from '@/api/services/shoppingServices';
import { useErrorStore } from '@/stores/error';
import { useShoppingStore } from '@/stores/shopping';
import { useAuthStore } from '@/stores/auth';

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
        const store = useAuthStore();

        const ShoppingList = ref<Shopping[]>([]);

        // pagination
        const pageNumber = ref<number>(1);
        const rowsPerPage = ref<number>(10);
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

        const clone = async (id: number) => {
            try {
                const response = await getCloneShopping(id);
                toast.success("Record cloned");
                edit(response.id);  
            } catch (err) {
                error.value = 'Failed to clone record';
                console.error('Error loading budgets:', err);
            }
        };


        const edit = (Id: number) => {
            const store = useShoppingStore();
            store.setShoppingId(Id);
            router.push({ name: 'ShoppingEdit' });
        };

        const view = (id: number) => {
            const store = useShoppingStore();
            store.setShoppingId(id);
            router.push({ name: 'ShoppingItemList' });
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

                const response = await goToPage(url);

                ShoppingList.value = response.data;
                totalPages.value = response.totalPages;
                next.value = response.nextPage;
                prev.value = response.previousPage;
                last.value = response.lastPage;
                first.value = response.firstPage;
            } catch (err) {
                error.value = 'Failed to load shopping list';
                console.error('Error loading shopping:', err);
            } finally {
                loading.value = false;
            }
        };

        const loadPage = async () => {
            try {
                loading.value = true;
                await new Promise(resolve => setTimeout(resolve, 1000)); // Show the notification for 1 seconds

                const response = await fetchShoppingInfo(store.user?.id || '',pageNumber.value, rowsPerPage.value);

                ShoppingList.value = response.data
                totalPages.value = response.totalPages;
                next.value = response.nextPage;
                prev.value = response.previousPage;
                last.value = response.lastPage;
                first.value = response.firstPage;
            } catch (err) {
                error.value = 'Failed to load shopping Details';
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
            goTo,
            validURL,
            view,
            clone
        }
    }
});
</script>