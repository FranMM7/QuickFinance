<template>
    <div class="container flex-fill">
       <h1>Finance Analysis</h1>
       <hr>
       <FinanceList/>
    </div>
</template>
<script lang="ts">
import Error from '@/components/error/error.vue';
import FinanceList from '@/components/FinanceAnalysis/FinanceList.vue';
import { defineComponent, onMounted } from 'vue';
import { ListLoader } from 'vue-content-loader';

export default defineComponent({
    name: 'FinanceView',
    components: {
        ListLoader,
        Error,
        FinanceList
    },
    setup() {
        onMounted(async () => {

        });

        return {}
    }
});
</script>