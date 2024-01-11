<script setup lang='ts'>
// import { FileDisplayPropsType, } from '@/type';
import { getFileIcon } from '@/utils/file.ts'
// const props = withDefaults(defineProps<FileDisplayPropsType>(), {})
const pageIndex = ref(6)
const listData: Record<string, unknown>[] = []
const arr = [
  "file",
  "unknow",
  "svg",
  "image",
  "music",
  "audio",
  "video",
  "doc",
  "word",
  "excel",
  "eml",
  "gif",
  "mov",
  "ppt",
  "rar",
  "pdf",
  "png",
  "zip",
  "broke"
]
for (let i = 0; i < 30; i++) {
  listData.push({
    title: `${i}`,
    content: 'content',
    type: arr[i]
  })
}
</script>

<template>
  <IxSpace vertical block class="bg-white p-3 overflow-y-scroll overflow-x-hidden pb-16"
    style="width: 98.6%;margin: 0 0.7%;height: calc(100% - 72px);">
    <IxList :grid="{ gutter: [20, 10], column: 3, xs: 1, sm: 4, md: 6, lg: 8, xl: 8 }">
      <IxList-item class="flex justify-center items-center" v-for="item in listData" :key="item.title">
        <div class="w-32 h-32 text-center">
          <IxImage class="w-16 h-16" :src="getFileIcon(`${item.title}.${item.type}`)" :preview="false" />
          <p>{{ `${item.title}.${item.type}` }}</p>
        </div>
      </IxList-item>
    </IxList>
    <IxSpace vertical class="pgtion">
      <IxPagination style="width: 460px;" :pageIndex="pageIndex" :total="500" />
    </IxSpace>
  </IxSpace>
</template>

<style scoped>
.pgtion {
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  padding: 12px 0;
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #FFFFFF;
}
</style>