<script setup lang='ts'>
import vueLogo from "@/assets/vue.svg";
</script>

<template>
  <IxSpace vertical block class="p-3 bg-white overflow-y-scroll"
    style="width: 98.6%;margin: 0 0.7%;height: calc(100% - 72px);">
    <IxTimeline>
      <IxTimelineItem v-for="item in [1, 2, 3, 4, 5, 6, 7]" :key="item">
        <h1>2021-02-10</h1>
        <IxSpace vertical block class="pt-3">
          <IxSpace :size="[8, 16]" align="center">
            <IxSpace v-for="i in [1, 2, 3, 4, 1, 1, 1, 1, , 1, 1, , 1, 1, 1, , 1, 1, 1]" vertical align="center">
              <IxImage :src="vueLogo" alt="logo" loading="lazy" />
              文件名 : {{ i }}
            </IxSpace>
          </IxSpace>
        </IxSpace>
      </IxTimelineItem>
    </IxTimeline>
  </IxSpace>
</template>

<style scoped></style>