<template>
    <div class="container">
        <div v-if="showLoader">
            <ListLoader />
        </div>
        <div v-if="error">
            <ErrorCard />
        </div>
        <div v-else>
            <div class="row">
                <div class="col-auto">
                    <h1>{{ shoppingData?.description }}</h1>
                </div>
                <div class="col text-end">
                    <h1>Total: {{ grandTotal.toFixed(2) }}</h1>
                </div>
            </div>

            <div class="row">
                <h3>Group by:</h3>
                <div class="col">
                    <div class="form-check form-switch">
                        <input class="form-check-input" type="checkbox" id="groupByCategory" v-model="gCategory"
                            @change="setGroupBy('category')" :checked="gCategory" />
                        <label class="form-check-label" for="groupByCategory">Category</label>
                    </div>
                </div>
                <div class="col">
                    <div class="form-check form-switch">
                        <input class="form-check-input" type="checkbox" id="groupByLocation" v-model="gLocation"
                            @change="setGroupBy('location')" :checked="gLocation" />
                        <label class="form-check-label" for="groupByLocation">Location</label>
                    </div>
                </div>
                <div class="col">
                    <div class="form-check form-switch">
                        <input class="form-check-input" type="checkbox" id="groupByNone" v-model="none"
                            @change="setGroupBy('none')" :checked="none" />
                        <label class="form-check-label" for="groupByNone">None</label>
                    </div>
                </div>
            </div>
            <hr>

            <table class="table table-striped text-center">
                <thead>
                    <tr>
                        <td v-if="selectedGroup === 'category'">Category</td>
                        <td v-if="selectedGroup === 'location'">Location</td>
                        <td>Brand</td>
                        <td>Description</td>
                        <td>Qty</td>
                        <td>Amount</td>
                        <td>SubTotal</td>
                    </tr>
                </thead>
                <tbody>
                    <template v-for="group in groupedData" :key="group.category">
                        <tr>
                            <td v-if="selectedGroup != 'none'" :rowspan="group.items.length + 1">{{ group.category }}
                            </td>
                        </tr>
                        <tr v-for="item in group.items" :key="item.id">
                            <td>{{ item.brand }}</td>
                            <td>{{ item.itemName }}</td>
                            <td>{{ item.quantity }}</td>
                            <td>{{ item.amount.toFixed(2) }}</td>
                            <td>{{ item.subTotal.toFixed(2) }}</td>
                        </tr>
                    </template>
                    <tr>
                        <td :colspan="selectedGroup === 'none' ? 4 : 5" class="text-left">Grand Total</td>
                        <td class="text-end">{{ grandTotal.toFixed(2) }}</td>
                    </tr>
                </tbody>
            </table>

            <div class="d-flex justify-content-center mt-4">
                <div class="btn-group" role="group" aria-label="Basic example">
                    <button @click="edit(shoppingData?.id || 0)" type="button" class="btn btn-primary">Edit</button>
                    <button @click="print()" type="button" class="btn btn-info">Print</button>
                    <button @click="cancel()" type="button" class="btn btn-light">Return</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { useErrorStore } from '@/stores/error';
import { defineComponent, onMounted, ref } from 'vue';
import { ListLoader } from 'vue-content-loader';
import { useShoppingStore } from '@/stores/shopping';
import { getShoppingById, ShoppingData, ShoppingList } from '@/api/services/shoppingServices';
import { groupDataByColumns } from '@/api/services/generalService';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { faL } from '@fortawesome/free-solid-svg-icons';
import ErrorCard from '../error/errorCard.vue';

export default defineComponent({
    name: 'ShoppingItemList',
    components: {
        ListLoader,
        ErrorCard
    },
    setup() {
        const loading = ref<boolean>(true);
        const showLoader = ref<boolean>(false);
        let loaderTimeout: ReturnType<typeof setTimeout>;
        const error = ref<string>('');

        const shoppingData = ref<ShoppingData>();
        const groupedData = ref<{ category: string, items: ShoppingList[] }[]>([]);
        const grandTotal = ref<number>(0);
        
        const selectedGroup = ref<'category' | 'location' | 'none'>('category');
        const gCategory = ref<boolean>(true)
        const gLocation = ref<boolean>(false)
        const none = ref<boolean>(false)

        const router = useRouter();
        const toast = useToast();

        const edit = async (id: number) => {
            if (id === 0) toast.warning('Unable to capture ID');
            const store = useShoppingStore();
            store.setShoppingId(id);
            router.push({ name: 'ShoppingEdit' });
        }

        const print = () => {
            console.log('print');
        }

        const cancel = () => {
            router.back();
        }

        const setGroupBy = (option: 'category' | 'location' | 'none') => {
            gCategory.value = option === 'category';
            gLocation.value = option === 'location';
            none.value = option === 'none';
            selectedGroup.value = option;
            loadPage();
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

                const shoppingStore = useShoppingStore();
                const shoppingId = shoppingStore.getShoppingId;

                if (shoppingId !== null) {
                    const res = await getShoppingById(shoppingId);
                    shoppingData.value = res.shoppingData;
                    const shoppingItems = res.data.$values ?? [];

                    if (selectedGroup.value !== 'none') {
                        const grouped = groupDataByColumns<ShoppingList>(shoppingItems, [selectedGroup.value]);
                        groupedData.value = Object.entries(grouped).map(([category, items]) => ({
                            category,
                            items
                        }));
                    } else {
                        groupedData.value = [{ category: '', items: shoppingItems }];
                    }

                    grandTotal.value = shoppingItems.reduce((total, item) => total + item.subTotal, 0);

                } else {
                    toast.error('ID was not retrieved');
                    router.back();
                }
            } catch (err) {
                error.value = 'Failed to load Shopping Details';
                console.error('Error:', err);
                const errorStore = useErrorStore();
                errorStore.setErrorNotification(error.value, String(err));
            } finally {
                loading.value = false;
            }
        };

        onMounted(() => {
            loadPage();
        });

        return {
            loading,
            showLoader,
            error,
            shoppingData,
            groupedData,
            grandTotal,
            edit,
            print,
            cancel,
            setGroupBy,
            selectedGroup,
            gCategory,
            gLocation,
            none,
        };
    }
});
</script>
