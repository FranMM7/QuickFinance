<template>
  <div id="app">
    <Navbar />

    <div class="container-xxl bd-gutter mt-3 my-md-4 bd-layout">
      <router-view />
    </div>
  </div>
</template>

<script>
import Navbar from "./components/Navbar/Navbar.vue";

export default {
  name: "App",
  components: {
    Navbar,
  },
};
</script>
