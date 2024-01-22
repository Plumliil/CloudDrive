<script setup lang='ts'>
import homeImg from '@/assets/home.jpg'
</script>

<template>
  <div class="home-container">
    <IxImage :src="homeImg" style="width: 80%;" :preview="false" />
  </div>
</template>

<style scoped>
.home-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: calc(100vh - 108px);
  background-color: #FEFEFE;
}
</style>