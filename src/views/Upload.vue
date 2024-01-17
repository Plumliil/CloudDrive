<script setup lang='ts'>
import useDragUpload from '@/hooks/useDragUpload'
const targetRef = ref(null)
const pRef = ref(null)
const uploadFileData1 = ref<{ files: File[], id: string }>()
const uploadFileData2 = ref<{ files: File[], id: string }>()
const startDragging = (e: any) => { e.preventDefault(); }
const handleDrop = (e: any) => { e.preventDefault(); }
const handleDragOver = (e: any) => { e.preventDefault(); }

onMounted(() => {
  const pRefd = document.getElementById('pRef')
  const targetRefd = document.getElementById('targetRef')
  const { fileList: f1, id: id1 } = useDragUpload(pRefd)
  const { fileList: f2, id: id2 } = useDragUpload(targetRefd)

  watch(() => f1.value, () => {
    uploadFileData1.value = {
      files: f1.value,
      id: id1
    }
  }, {
    deep: true
  })
  watch(() => f2.value, () => {
    uploadFileData2.value = {
      files: f2.value,
      id: id2
    }
  }, {
    deep: true
  })


})

watch([() => uploadFileData1.value, () => uploadFileData2.value], (n, o) => {
  console.log({ n, o });
})

// @mousedown="startDragging"
// @dragover.prevent="handleDragOver"
// @drop.prevent="handleDrop"



</script>
<template>
  <IxCard class="h-full">
    <p ref="pRef" id="pRef" class="shadow-lg w-32 h-32 mx-auto">
      {{ uploadFileData1 }}
    </p>
    <IxCard ref="targetRef" id="targetRef" class="h-64 w-64 mx-auto">
      {{ uploadFileData2 }}
    </IxCard>
  </IxCard>
</template>

<style scoped></style>