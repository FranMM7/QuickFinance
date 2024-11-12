<script lang="ts">
import { defineComponent, onMounted, Ref, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';
import { fetchFinanceById, fetchFinanceList, Finance, FinancePageResponse, goToPage } from '@/api/services/financeServices';
import { useToast } from 'vue-toastification';
import { formatDate } from '@/api/services/generalService';
import { useErrorStore } from '@/stores/error';
import { useFinanceStore } from '@/stores/finance';
import { useRouter } from 'vue-router';

export default defineComponent({
    name: "FinanceList",
    components: {
        ListLoader,
        Error
    },
    setup() {
        const toast = useToast()
        const loading = ref<boolean>(true)
        const showLoader = ref<boolean>(false)
        let loaderTimeout: ReturnType<typeof setTimeout>;
        const error = ref<string>('')
        const errorStore = useErrorStore()
        const router = useRouter()

        //data
        const list = ref<Finance[]>([])
        // pagination
        const pageNumber = ref<number>(1);
        const rowsPerPage = ref<number>(10);
        const totalPages = ref<number>(10);
        const next = ref<string>('');
        const prev = ref<string>('');
        const first = ref<string>('');
        const last = ref<string>('');

        const newRecord = () =>{
            router.push({name:'financeAdd'})
        }

        const edit = async (id: number) => {
            try {
                const store = useFinanceStore()
                const response = await fetchFinanceById(id)

                // console.log('edit res:', response)
                if (response) {

                    store.setId(response.id)
                    store.setTitle(response.title)
                    store.setEditMode(true)

                    const expList = response.list.$values
                    if (expList)
                        store.setList(response.list.$values)

                    const incList = response.incomes.$values
                    if (incList)
                        store.setIncomeList(response.incomes.$values)

                    router.push({ name: 'financeEdit' })
                }
                else
                    toast.warning('Record not found')


            } catch (error) {
                console.log('error when fetching data:', error)
                toast.error(error)
            }
        }

        const deleteRecord = (Id: number) => {
            console.log(Id);
        };

        const changePage = (page: number) => {
            if (page >= 1 && page <= totalPages.value) {
                pageNumber.value = page;
                loadingPage();
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

                const response = await goToPage(url) || [];

                list.value = response.data
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


        const loadingPage = async () => {
            try {
                loading.value = true

                showLoader.value = false; // Reset loader visibility
                clearTimeout(loaderTimeout); // Clear any previous timeout

                // Set a timeout to show loader only if loading takes >1 second
                loaderTimeout = setTimeout(() => {
                    if (loading.value) showLoader.value = true;
                }, 1000);

                const response = await fetchFinanceList(pageNumber.value, rowsPerPage.value) || []

                // console.log('res:', response)
                list.value = response.data || []
                totalPages.value = response.totalPages;
                next.value = response.nextPage;
                prev.value = response.previousPage;
                last.value = response.lastPage;
                first.value = response.firstPage;


            } catch (err) {
                error.value = 'Error when loading data'
                console.error(error.value, err);
                toast.error('Unexpected error');
                errorStore.setErrorNotification(error.value, String(err))
            }
            finally {
                loading.value = false
            }
        }

        onMounted(async () => {
            loadingPage()
        });

        return {
            loading,
            error,
            list,
            totalPages,
            rowsPerPage,
            pageNumber,
            formatDate,
            edit,
            goTo,
            validURL,
            changePage,
            loadingPage,
            deleteRecord,
            newRecord

        }
    }
});
</script>

<template>
    <div class="container">
        <div v-if="loading">
            <list-loader />
        </div>
        <div v-else-if="error">
            <Error />
        </div>
        <div v-else class="row">
            <div class="col">
                <h1>Finance Evaluations</h1>
            </div>
            <div class="col text-end">
                <div class="btn-group">
                    <button class="btn btn-success" @click="newRecord">New</button>
                </div>
            </div>
            <hr>
            <table class="table table-striped-columns text-center">
                <thead>
                    <tr>
                        <td>Title</td>
                        <td>Total Expenses</td>
                        <td>Total Incomes</td>
                        <td>Modified On</td>
                        <td>-</td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="itm in list" :key="itm.id">
                        <td>{{ itm.title }}</td>
                        <td>{{ itm.totalExpenses.toFixed(2) }}</td>
                        <td>{{ itm.totalIncomes.toFixed(2) }}</td>
                        <td>{{ formatDate(String(itm.modifiedON)) }}</td>
                        <td class="text-end">
                            <div class="btn-group">
                                <button type="button" class="btn btn-secondary" @click="edit(itm.id)">
                                    <font-awesome-icon :icon="['fas', 'edit']" />
                                </button>
                                <button type="button" class="btn btn-danger" @click="deleteRecord(itm.id)">
                                    <font-awesome-icon :icon="['fas', 'trash']" />
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- pagination -->
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
                    <select id="rowsPerPage" v-model="rowsPerPage" @change="loadingPage" class="page-item page-link">
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

            </div>
        </div>
    </div>
</template>