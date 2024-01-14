<script setup lang='ts'>
type FormModalProps = {
  title?: string
  visible: boolean
  close: () => void
  ok: () => void
}
const props = defineProps<FormModalProps>()
const okHandle = () => {
  console.log(111);
  props.close()

}
</script>

<template>
  <IxModal :visible="props.visible" @ok="okHandle" @close="props.close" @cancel="close"
    :header="title" :centered="false" :width="400">
    <p>Some contents...</p>
  </IxModal>
</template>

<style scoped></style>