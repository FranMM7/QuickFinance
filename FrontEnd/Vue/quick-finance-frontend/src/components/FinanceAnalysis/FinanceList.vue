<script lang="ts">
import { defineComponent, onMounted } from 'vue';
import { ListLoader } from 'vue-content-loader';
import Error from '../error/error.vue';


export default defineComponent({
    name: "FinanceList",
    components: {
        ListLoader,
        Error
    },
    setup() {
        onMounted(async () => {

        });

        return {}
    }
});
</script>

<template>
    <div>list</div>
</template>