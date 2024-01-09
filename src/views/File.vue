<script setup lang='ts'>
import { RadioData } from '@idux/components/radio'
import { type MenuData } from '@idux/components/menu'
import FileTable from '@/components/FileTable/index.vue'
import FileList from '@/components/FileList/index.vue'
import { useFileStoreWithOut } from '@/store';
const route = useRoute();
const fileListStore = useFileStoreWithOut()
const searchValue = ref('')
const visible = ref(false)
console.log('route', route);
watch(() => route.query.type, (n) => {
  console.log('new type', n);
})

const dataSource: MenuData[] = [
  {
    type: 'sub', key: 'main', icon: 'folder-open', label: '我的文件',
    children: [
      { key: '/file?type=all', icon: 'folder', label: '全部' },
      { key: '/file?type=image', icon: 'file-image', label: '图片' },
      { key: '/file?type=docs', icon: 'audit', label: '文档' },
      { key: '/file?type=video', icon: 'play-circle', label: '视频' },
      { key: '/file?type=audio', icon: 'customer-service', label: '音乐' },
      { key: '/file?type=other', icon: 'expand-all', label: '其他' },
    ],
  },
  { key: 'recycle ', icon: 'delete', label: '回收站' },
  { key: 'share', icon: 'send', label: '我的分享' },
]

const fileDisplayType: RadioData[] = [
  { key: 'list', label: '列表' },
  { key: 'table', label: '网格' },
  { key: 'timeLine', label: '时间线' },
]

const fileDisplayTypeChangeHandle = (value: string) => {
  console.log('fileListStore', fileListStore.displayType, value);
  fileListStore.changeState({
    key: 'displayType',
    value
  })
}
</script>

<template>
  <IxLayout>
    <IxLayoutSider class="flex flex-col justify-between">
      <IxMenu mode="inline" :dataSource="dataSource"></IxMenu>
      <div class="flex w-full flex-col p-1 mb-3">
        <IxProgress :percent="75" />
        <div class="flex items-center justify-between">
          <span>储存</span>
          <span>50mb / 100mb</span>
        </div>
      </div>
    </IxLayoutSider>
    <IxLayoutContent>
      <IxRow>
        <IxCol class="flex justify-start" :span="12">
          <IxSpace align="center">
            <IxButton mode="primary" class="text-blue-500 active:text-white hover:text-white">上传</IxButton>
            <IxButton mode="primary" class="text-blue-500 active:text-white hover:text-white">新建文件夹</IxButton>
            <IxButton mode="primary" class="text-blue-500 active:text-white hover:text-white">新建在线文档</IxButton>
            <IxButton mode="primary" class="text-blue-500 active:text-white hover:text-white">批量删除</IxButton>
            <IxButton mode="primary" class="text-blue-500 active:text-white hover:text-white">批量移动</IxButton>
            <IxButton mode="primary" class="text-blue-500 active:text-white hover:text-white">下载</IxButton>
            <IxButton mode="primary" class="text-blue-500 active:text-white hover:text-white">批量下载</IxButton>
            <IxButton mode="primary" class="text-blue-500 active:text-white hover:text-white">分享</IxButton>
          </IxSpace>
        </IxCol>
        <IxCol class="flex justify-end pr-4" :span="12">
          <IxSpace align="center">
            <IxInput v-model:value="searchValue">
              <template #addonBefore class="bg-none">
                <IxIcon name="search" />
              </template>
            </IxInput>
            <IxPopover :visible="visible" trigger="manual" placement="bottomEnd">
              <template #content>
                <IxSpace vertical align="center">
                  <div class="flex justify-around items-center">
                    <p class="pr-3 pb-1">展示方式:</p>
                    <IxRadioGroup v-model:value="fileListStore.displayType" :dataSource="fileDisplayType"
                      @change="fileDisplayTypeChangeHandle">
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
    </IxLayoutContent>
  </IxLayout>
</template>

<style scoped></style>