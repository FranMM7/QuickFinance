<template>
    <div class="container flex-auto">
        <h1>My Shooping List:</h1>
        <hr>
        <ShoppingList />
    </div>
</template>

<script lang="ts">

import ShoppingList from '@/components/Shopping/shoppingList.vue';
import { defineComponent, onMounted } from 'vue';


export default defineComponent({
    name: 'ShoppingView',

    components: {
        ShoppingList
    },

    setup() {
        onMounted(async () => {

        });

        return {}
    }
});
</script>