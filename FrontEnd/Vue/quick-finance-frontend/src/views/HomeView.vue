<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import errorCard from '@/components/error/errorCard.vue';
import { ListLoader } from 'vue-content-loader';

export default defineComponent({
  name: 'homeView',
  components: {
    ListLoader,
    errorCard
  },
  setup() {
    onMounted(async () => { });

    return {};
  }
});
</script>

<template>
  <div class="container fade-in">
    <div class="jumbotron">
      <h1 class="display-3">Quick Finance</h1>
      <hr />
      <p class="lead">
        QuickFinance is a simple personal finance tracker that allows users to manage their expenses,
        budgets, categories, payment methods, and budget limits efficiently. This application is
        designed to help users keep track of their financial activities and gain insights into their
        spending habits.
      </p>
    </div>
    <!-- <hr /> -->

  </div>
</template>
