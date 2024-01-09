<script setup lang='ts'>
import { RadioData } from '@idux/components/radio'
import FileTable from '@/components/FileTable/index.vue'
import FileList from '@/components/FileList/index.vue'
const route = useRoute();
const searchValue = ref('')
const visible = ref(false)
const value = ref('beijing')
console.log('route', route);
watch(() => route.query.type, (n) => {
  console.log('new type', n);
})
const fileDisplayType: RadioData[] = [
  { key: 'list', label: '列表' },
  { key: 'table', label: '网格' },
  { key: 'timeLine', label: '时间线' },
]

const onChange = console.log
</script>

<template>
  <IxRow>
    <IxCol class="flex justify-start" :span="12">
      <IxSpace align="center">
        <IxButton mode="primary" class="bg-blue-600 text-white hover:bg-blue-500 hover:text-white duration-500">上传
        </IxButton>
        <IxButton mode="primary" class="bg-blue-600 text-white hover:bg-blue-500 hover:text-white duration-500">下载
        </IxButton>
      </IxSpace>
    </IxCol>
    <IxCol class="flex justify-end pr-4" :span="12">
      <IxSpace align="center">
        <IxInput v-model:value="searchValue">
          <template #addonBefore class="bg-none">
            <IxIcon name="search" />
          </template>
        </IxInput>
        <IxPopover :visible="visible" trigger="manual"  placement="bottomEnd">
          <template #content>
            <IxSpace vertical align="center">
              <div class="flex justify-around items-center">
                <p class="pr-3 pb-1">展示方式:</p>
                <IxRadioGroup v-model:value="value" :dataSource="fileDisplayType" @change="onChange">
                </IxRadioGroup>
              </div>
            </IxSpace>

          </template>
          <IxButton @Click="visible = !visible" class="border-none">
            <IxIcon name="setting" />
          </IxButton>
        </IxPopover>
      </IxSpace>
    </IxCol>
  </IxRow>
  <FileTable />
  <FileList />
  <div>File</div>
</template>

<style scoped></style>