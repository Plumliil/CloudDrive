<script setup lang='ts'>
import { RadioData } from '@idux/components/radio'
import { type MenuData } from '@idux/components/menu'
import { VKey } from '@idux/cdk/utils'
import FileTable from '@/components/FileTable/index.vue'
import FileList from '@/components/FileList/index.vue'
import TimeLine from '@/components/TimeLine/index.vue'
import { useFileStoreWithOut } from '@/store';
const route = useRoute();
const fileStore = useFileStoreWithOut()
const searchValue = ref('')
const visible = ref(false)
console.log('route', route);
watch(() => route.query.type, (n, o) => {
  fileStore.changeState({
    key: 'routeType',
    value: n === 'image' ? 'image' : 'other'
  })
  if (o === 'image') {
    fileStore.changeState({
      key: 'displayType',
      value: 'list'
    })
  }
})
const rootMenuSubKeys: VKey[] = ['mine', 'recycle', 'share']
const expandedKeys = ref<VKey[]>(['mine'])
const onExpandedChange = (keys: VKey[]) => {
  const lastExpandedKey = keys.find(key => !expandedKeys.value.includes(key))
  if (rootMenuSubKeys.indexOf(lastExpandedKey!) === -1) {
    expandedKeys.value = keys
  } else {
    expandedKeys.value = lastExpandedKey ? [lastExpandedKey] : []
  }
}
const dataSource: MenuData[] = [
  {
    type: 'sub', key: 'mine', icon: 'folder-open', label: '我的文件',
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
  <IxLayout class="m-0 p-0">
    <IxLayoutSider class="flex flex-col justify-between bg-white m-0 p-0">
      <IxMenu :expandedKeys="expandedKeys" @update:expandedKeys="onExpandedChange" mode="inline" :dataSource="dataSource">
        <template #itemLabel="item">
          <router-link :to="item.key">
            <span>{{ item.label }}</span>
          </router-link>
        </template>
      </IxMenu>
      <div class="flex w-full flex-col p-1 mb-3">
        <IxProgress :percent="75" />
        <div class="flex items-center justify-between">
          <span>储存</span>
          <span>50mb / 100mb</span>
        </div>
      </div>
    </IxLayoutSider>
    <IxLayoutContent class="p-0 m-0">
      <IxRow class="bg-white m-3 p-2">
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
            <IxPopover :visible="visible" trigger="click" placement="bottomEnd">
              <template #content>
                <IxSpace vertical align="center">
                  <div class="flex justify-around items-center">
                    <p class="pr-3 pb-1">展示方式:</p>
                    <IxRadioGroup v-model:value="fileStore.displayType"
                      :dataSource="fileStore.routeType === 'image' ? fileDisplayType : fileDisplayType.slice(0, 2)"
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
      <FileTable v-if="fileStore.displayType === 'table'" />
      <FileList v-else-if="fileStore.displayType === 'list'" />
      <TimeLine v-else-if="fileStore.displayType === 'timeLine'" />
    </IxLayoutContent>
  </IxLayout>
</template>

<style scoped>
:deep(.ix-layout-content) {
  padding: 0;
}

:deep(.ix-pro-layout-content) {
  padding: 0;
}
</style>