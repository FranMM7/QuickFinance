<script lang="ts">
import { useAuthStore } from '@/stores/auth';
import { defineComponent, ref } from 'vue';
import { useRouter } from 'vue-router';


export default defineComponent({
    name: 'ListLocation',
    components: {},
    setup() {
        const store = useAuthStore()
        const router = useRouter()
    
    }
})
</script>

<template>
    <div>
        List
    </div>
</template>