<template>
    <div class="accordion card border-danger mb-3 error-card" id="accordionError">
        <div class="accordion-item">
            <h2 class="accordion-header" id="headingOne">
                <button class="accordion-button collapsed btn btn-danger" type="button" data-bs-toggle="collapse"
                    data-bs-target="#collapseOne" aria-expanded="false" aria-controls="collapseOne">
                    {{ notification }}
                </button>
            </h2>
            <div id="collapseOne" class="accordion-collapse collapse danger" aria-labelledby="headingOne"
                data-bs-parent="#accordionError">
                <div class="accordion-body">
                    <p class="card-text">{{ error }}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { useErrorStore } from '@/stores/error';

export default {
    name: 'ErrorCardAccordion',
    computed: {
        notification() {
            const errorStore = useErrorStore();
            return errorStore.notification;
        },
        error() {
            const errorStore = useErrorStore();
            return errorStore.error;
        }
    }
};
</script>