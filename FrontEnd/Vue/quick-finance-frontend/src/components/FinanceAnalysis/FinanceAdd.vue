<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';

export default defineComponent({
    name: 'financeAdd',
    components: {},
    setup() {
        const loading = ref<boolean>(false)

        onMounted(async () => {
            // loadPage()
        });

        return {}
    }
})
</script>
<template>
    <div>
        new
    </div>
</template>