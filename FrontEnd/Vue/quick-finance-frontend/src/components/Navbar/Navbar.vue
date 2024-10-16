<template>
  <nav class="navbar navbar-expand-lg bg-dark" data-bs-theme="dark">
    <div class="container-fluid">
      <router-link class="navbar-brand" to="/">Quick Finance App</router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarColor02"
        aria-controls="navbarColor02" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarColor02">
        <ul class="navbar-nav me-auto">
          <li class="nav-item">
            <router-link to="/" class="nav-link" exact-active-class="active"> <font-awesome-icon :icon="['fas', 'house']" /> Home</router-link>
          </li>
          <li class="nav-item">
            <router-link to="/budgets" class="nav-link"><font-awesome-icon :icon="['fas', 'wallet']" /> Budgets</router-link>
          </li>
          <li class="nav-item">
            <router-link to="/Categories" class="nav-link"> <font-awesome-icon :icon="['fas', 'list-ul']" /> Categories</router-link>
          </li>
          <li class="nav-item">
            <router-link to="/finance" class="nav-link"> <font-awesome-icon :icon="['fas', 'coins']" /> Finance Analizis</router-link>
          </li>
          <li class="nav-item">
            <router-link to="/shoppinglist" class="nav-link"> <font-awesome-icon :icon="['fas', 'cart-shopping']" /> Shopping List</router-link>
          </li>
          <li class="nav-item">
            <router-link to="/Settings" class="nav-link"> <font-awesome-icon :icon="['fas', 'gear']" /> Settings</router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script>
export default {
  name: "Navbar",
};
</script>
