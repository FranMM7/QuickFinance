<template>
    <div class="container flex-fill">
       <h1>Finance Analysis</h1>
    </div>
</template>
<script lang="ts">
import Error from '@/components/error/error.vue';
import { defineComponent, onMounted } from 'vue';
import { ListLoader } from 'vue-content-loader';

export default defineComponent({
    name: 'FinanceView',
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