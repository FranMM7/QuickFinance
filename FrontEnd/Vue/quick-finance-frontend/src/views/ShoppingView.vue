<template>

    <template v-if="!isEditing">
        <div class="row d-flex">
            <div class="col">
                <h1>My Shooping List:</h1>
            </div>
            <div class="col-auto p-1">
                <button @click="add()" type="button" class="btn btn-lg btn-primary">New Record</button>
            </div>
        </div>
        <hr>
        <ShoppingList />
    </template>

    <!-- Router view for dynamic child components -->
    <template v-else>
        <router-view />
    </template>
</template>

<script lang="ts">

import ShoppingAdd from '@/components/Shopping/ShoppingAdd.vue';
import ShoppingList from '@/components/Shopping/shoppingList.vue';
import { computed, defineComponent, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';


export default defineComponent({
    name: 'ShoppingView',

    components: {
        ShoppingList,
        ShoppingAdd
    },

    setup() {
        const router = useRouter()
        const route = useRoute()

        const isEditing = computed(() => {
            return route.name === 'ShoppingAdd' || route.name === 'ShoppingEdit' || route.name === 'ShoppingItemList'
        })

        const add = () => {
            router.push({ name: 'ShoppingAdd' })
        }

        onMounted(async () => {
        });

        return {
            add,
            isEditing
        }
    }
});
</script>