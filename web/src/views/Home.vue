<template>
  <global-header />
  
  <div class="home row flex-grow-1 align-content-center">
    <div id="carouselExampleControls" class="carousel slide" data-bs-ride="carousel" data-bs-pause="false" data-bs-interval="3000">
      <div class="carousel-inner">
        <div class="carousel-item active">
          <img src="../assets/home1.jpg" class="d-block w-100">
        </div>
        <div class="carousel-item">
          <img src="../assets/home2.jpg" class="d-block w-100">
        </div>
        <div class="carousel-item">
          <img src="../assets/home3.jpg" class="d-block w-100">
        </div>
      </div>

      <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleControls" data-bs-slide="prev">
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="visually-hidden">Previous</span>
      </button>
      <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleControls" data-bs-slide="next">
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="visually-hidden">Next</span>
      </button>
    </div>
  </div>

  <div class="fs-6 fw-light">Stock photos licensed from <a href="http://freepik.com">freepik.com</a>.</div>
  <global-footer />
</template>

<script>
// @ is an alias to /src
import GlobalHeader from '@/components/GlobalHeader.vue'
import GlobalFooter from '@/components/GlobalFooter.vue'

export default {
  name: 'Home',
  components: {
    GlobalHeader,
    GlobalFooter
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only 
<style scoped lang="scss">

</style>-->