<template>
    <div class="row align-self-end">
        <Login />
    </div>
    <div class="row">
        <nav class="navbar border-bottom pb-0">
            <router-link class="navbar-brand" to="/">
                <img src="../assets/corpsite_logo.png" height="40" class="d-inline-block" alt="{{ orgname? orgname : 'Corpsite' }} Logo">
                {{ orgname? orgname : 'Corpsite' }}
            </router-link>
            <ul class="nav-buttons mt-3">
                <li class="btn btn-m btn-primary py-0"><router-link class="nav-item nav-link" to="/products">Products</router-link></li>
                <li class="btn btn-m btn-primary mx-3 py-0"><router-link class="nav-item nav-link" to="/about">About Us</router-link></li>
                <li class="btn btn-m btn-primary py-0"><router-link class="nav-item nav-link" to="/employees">Our Team</router-link></li>
            </ul>
        </nav>
    </div>
</template>

<script>
import Login from "@/components/Login.vue";

export default {
  name: 'GlobalHeader',
  components: {
      Login
  },
  props: {
      orgname: String
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
    .nav-item {
        color: white;
    }
    .nav-buttons .router-link-exact-active {
        color: #ffe347;
    }
</style>
