<template>
  <div id="app" class="d-flex flex-column min-vh-100">
    <Navbar />

    <!-- Main Content Section -->
    <div class="container-xxl bd-gutter mt-3 my-md-4 bd-layout flex-grow-1">
      <router-view />
    </div>

    <!-- Footer Section -->
    <footer class="bg-light py-3 bg-dark-subtle">
      <div class="container text-center">
        <p>&copy; 2024 Quick Finance. All rights reserved.</p>
      </div>
    </footer>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Navbar from "./components/Navbar/Navbar.vue";

export default defineComponent({
  name: "App",
  components: {
    Navbar,
  },
  setup() { }
});
</script>

<style>
@import './assets/custom.css';
</style>
