<template>
  <global-header />
  <h1 class="pt-5">About Us</h1>
  <div class="about row flex-grow-1 align-content-left">
      <div style="width: 25%; float:left; margin-left:200px; margin-top:10px;">
          <h4>
              Corpsite is a company that operates with the consumer in mind.
              We are a fast growing business with big goals for the future.
          </h4>
      </div>
      <div style="width: 25%; float: left; margin-left:100px; margin-top:10px;">
          <p>
              <img src="../assets/office.jpg" height="200" />
          </p>
      </div>
  </div>

 <div>
    <h4>
      Headquartered in Columbus, OH.  Servicing clients nationwide.  
    </h4>

     <div class="map"> 
         <img src="../assets/US-1-map.png" height="450"/>
     </div>
  </div>
  <global-footer />
</template>

<script>
import GlobalHeader from '@/components/GlobalHeader.vue'
import GlobalFooter from '@/components/GlobalFooter.vue'

export default {
  name: 'About',
  components: {
    GlobalHeader,
    GlobalFooter
  }
}
</script>