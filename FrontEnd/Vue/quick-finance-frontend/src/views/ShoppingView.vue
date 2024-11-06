<template>
    <div class="container flex-auto">
        <h1>My Shooping List:</h1>
        <button @click="add()" type="button" class="btn btn-primary">New Record</button>
        <hr>
        <ShoppingList />
    </div>
</template>

<script lang="ts">

import ShoppingAdd from '@/components/Shopping/ShoppingAdd.vue';
import ShoppingList from '@/components/Shopping/shoppingList.vue';
import { defineComponent, onMounted } from 'vue';
import { useRouter } from 'vue-router';


export default defineComponent({
    name: 'ShoppingView',

    components: {
        ShoppingList,
        ShoppingAdd
    },

    setup() {
        const router = useRouter()
        const add = () => {
            router.push({ name: 'ShoppingAdd' })
        }
        onMounted(async () => {
        });

        return {
            add,
        }
    }
});
</script>