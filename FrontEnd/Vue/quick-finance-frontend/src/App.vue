<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png" />
    <HomeView />
  </div>
</template>

<script>
import HomeView from './views/HomeView.vue'; // Update the path as necessary

export default {
  name: 'App',
  components: {
    HomeView,
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
