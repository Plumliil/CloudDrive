<script setup lang='ts'>
import { RadioData } from '@idux/components/radio'
import { MenuClickOptions, type MenuData } from '@idux/components/menu'
import { VKey } from '@idux/cdk/utils'
import { CheckboxData } from '@idux/components/checkbox'
import { useMessage } from '@idux/components/message'
import FileTable from '@/components/FileTable/index.vue'
import FileList from '@/components/FileList/index.vue'
import TimeLine from '@/components/TimeLine/index.vue'
import { useFileStoreWithOut } from '@/store';
import { ColumnType, FileDataType, FileDisPlayType, FileType } from '@/type'
import { LocationQueryValue } from 'vue-router'
import { folderList } from '@/mock'
import { useModal } from '@idux/components/modal'

const fileData: FileDataType[] = [
  {
    key: 0,
    name: '文件夹',
    type: 'folder',
    size: '1024 KB',
    deleteDate: ['2023-01-01', '2023-02-15'],
    changeDate: ['2023-01-01', '2023-02-15']
  },
  {
    key: 1,
    name: 'Document1',
    type: 'doc',
    size: '1024 KB',
    deleteDate: ['2023-01-01', '2023-02-15'],
    changeDate: ['2023-01-01', '2023-02-15']
  },
  {
    key: 2,
    name: 'Image1',
    type: 'jpg',
    size: '2048 KB',
    deleteDate: ['2023-03-10'],
    changeDate: ['2023-03-10']
  },
  {
    key: 3,
    name: 'Spreadsheet1',
    type: 'xlsx',
    size: '512 KB',
    deleteDate: ['2023-05-20'],
    changeDate: ['2023-05-20']
  },
  {
    key: 4,
    name: 'Video1',
    type: 'mp4',
    size: '4096 KB',
    deleteDate: ['2023-07-08'],
    changeDate: ['2023-07-08']
  },
  {
    key: 5,
    name: 'Presentation1',
    type: 'ppt',
    size: '3072 KB',
    deleteDate: ['2023-09-15'],
    changeDate: ['2023-09-15']
  },
  {
    key: 6,
    name: 'CodeFile1',
    type: 'ts',
    size: '256 KB',
    deleteDate: ['2023-11-01'],
    changeDate: ['2023-11-01']
  },
  {
    key: 7,
    name: 'Document2',
    type: 'doc',
    size: '1024 KB',
    deleteDate: ['2023-01-01', '2023-02-15'],
    changeDate: ['2023-01-01', '2023-02-15']
  },
  {
    key: 8,
    name: 'Image2',
    type: 'jpg',
    size: '2048 KB',
    deleteDate: ['2023-03-10'],
    changeDate: ['2023-03-10']
  },
  {
    key: 9,
    name: 'Spreadsheet2',
    type: 'xlsx',
    size: '512 KB',
    deleteDate: ['2023-05-20'],
    changeDate: ['2023-05-20']
  },
  {
    key: 10,
    name: 'Video2',
    type: 'mp4',
    size: '4096 KB',
    deleteDate: ['2023-07-08'],
    changeDate: ['2023-07-08']
  },
  {
    key: 11,
    name: 'Presentation2',
    type: 'ppt',
    size: '3072 KB',
    deleteDate: ['2023-09-15'],
    changeDate: ['2023-09-15']
  },
  {
    key: 12,
    name: 'CodeFile2',
    type: 'ts',
    size: '256 KB',
    deleteDate: ['2023-11-01'],
    changeDate: ['2023-11-01']
  },
  {
    key: 33,
    name: 'Spreadsheet17',
    type: 'xlsx',
    size: '512 KB',
    changeDate: ['2024-05-20'],
    deleteDate: ['2024-03-10']
  },
  {
    key: 34,
    name: 'Video17',
    type: 'mp4',
    size: '4096 KB',
    changeDate: ['2024-07-08'],
    deleteDate: ['2024-05-20']
  },
  {
    key: 35,
    name: 'Presentation17',
    type: 'ppt',
    size: '3072 KB',
    changeDate: ['2024-09-15'],
    deleteDate: ['2024-07-08']
  },
  {
    key: 36,
    name: 'CodeFile17',
    type: 'ts',
    size: '256 KB',
    changeDate: ['2024-11-01'],
    deleteDate: ['2024-09-15']
  },
  {
    key: 37,
    name: 'Document18',
    type: 'doc',
    size: '1024 KB',
    changeDate: ['2025-01-01', '2025-02-15'],
    deleteDate: ['2025-03-10', '2025-04-20']
  },
  {
    key: 38,
    name: 'Image18',
    type: 'jpg',
    size: '2048 KB',
    changeDate: ['2025-03-10'],
    deleteDate: ['2025-01-01', '2025-02-15']
  },
  {
    key: 39,
    name: 'Spreadsheet18',
    type: 'xlsx',
    size: '512 KB',
    changeDate: ['2025-05-20'],
    deleteDate: ['2025-03-10']
  },
  {
    key: 40,
    name: 'Video18',
    type: 'mp4',
    size: '4096 KB',
    changeDate: ['2025-07-08'],
    deleteDate: ['2025-05-20']
  },
  {
    key: 41,
    name: 'Presentation18',
    type: 'ppt',
    size: '3072 KB',
    changeDate: ['2025-09-15'],
    deleteDate: ['2025-07-08']
  },
  {
    key: 42,
    name: 'CodeFile18',
    type: 'ts',
    size: '256 KB',
    changeDate: ['2025-11-01'],
    deleteDate: ['2025-09-15']
  }
];
import { Validators, useFormGroup } from '@idux/cdk/forms'
import { TreeNode } from '@idux/components/tree/src/types'

const { required } = Validators

const folderCreateForm = useFormGroup({
  folderName: ['', required],
})
const renameFileForm = useFormGroup({
  newName: ['', required],
})
const folderShareForm = useFormGroup({
  validityData: ['', required],
  shouldCode: ['n', required],
  password: ['', required],
})


const route = useRoute();
const listRef = ref();
const tableRef = ref();
const fileStore = useFileStoreWithOut()
const searchValue = ref('') // Search keyword
const visible = ref(false) // Display column visible
const createFileVisible = ref<boolean>(false)
const uploadFileVisible = ref<boolean>(false)
const shareFileVisible = ref<boolean>(false)
const dataSource = ref<FileDataType[]>(fileData)
const activeRouteType = ref<LocationQueryValue | LocationQueryValue[] | FileType>(fileStore.displayType)
const rootMenuSubKeys: VKey[] = ['mine', 'recycle', 'share']
const expandedKeys = ref<VKey[]>(['mine'])
const message = useMessage()
const selectKeys = ref<any[]>([])
const selectRows = ref<any[]>([])
const selectBtnFlag = computed(() => selectKeys.value.length > 0)
const { confirm } = useModal()


const onExpandedChange = (keys: VKey[]) => {
  const lastExpandedKey = keys.find(key => !expandedKeys.value.includes(key))
  if (rootMenuSubKeys.indexOf(lastExpandedKey!) === -1) {
    expandedKeys.value = keys
  } else {
    expandedKeys.value = lastExpandedKey ? [lastExpandedKey] : []
  }
}
const menuData: MenuData[] = [
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
  { key: '/file?type=recycle ', icon: 'delete', label: '回收站' },
  { key: '/file?type=share', icon: 'send', label: '我的分享' },
]
const fileDisplayType: RadioData[] = [
  { key: 'list', label: '列表' },
  { key: 'table', label: '网格' },
  { key: 'timeLine', label: '时间线' },
]
const fileDisplayTypeChangeHandle = (value: FileDisPlayType) => {
  fileStore.changeState({
    key: 'displayType',
    value
  })
}
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
  activeRouteType.value = n
  dataSource.value = []
})
const columnsTypes = ref<ColumnType[]>(fileStore.columnsType)

const columnsTypesData: CheckboxData[] = [
  { key: 'name', label: '文件名', disabled: true },
  { key: 'type', label: '类型' },
  { key: 'size', label: '大小' },
  { key: 'changeDate', label: '修改日期' },
  { key: 'deleteDate', label: '删除日期' },
]

const dragControllerDiv = () => {
  const resize: any = document.getElementsByClassName('resize')
  const boxDom = document.getElementById('container')
  const leftDom: any = document.getElementsByClassName('left-content')
  const rightDom: any = document.getElementsByClassName('right-content')

  if (!boxDom) return
  resize[0].onmousedown = function (e: any) {
    // 拖拽区 开始的距离  403
    const startX = e.clientX
    // 左边大小 放入 resize
    resize[0].left = resize[0].offsetLeft
    document.onmousemove = function (ee) {
      // 拖拽区 结束的距离
      const endX = ee.clientX
      // 移动的距离 （endx-startx）=移动的距离。resize[0].left+移动的距离=左边区域最后的宽度
      let leftWidth = resize[0].left + (endX - startX)
      // 右边最大宽度
      const maxWidth = boxDom.clientWidth - resize[0].offsetWidth
      /* 设置 左边 最小值 */
      if (leftWidth < 224)
        leftWidth = 224
      if (leftWidth > maxWidth - 224)
        leftWidth = maxWidth - 224
      // 设置拖拽条 距离左侧区域的宽度
      resize[0].style.left = leftWidth
      // 设置 左边宽度
      leftDom[0].style.width = `${leftWidth}px`
      // 设置右边宽度
      rightDom[0].style.width = `${boxDom.clientWidth - leftWidth - 10}px`
    }
    document.onmouseup = function () {
      document.onmousemove = null
      document.onmouseup = null
    }

    return false
  }
}

onMounted(() => {
  dragControllerDiv()
})

const siderShowHandle = (flag: boolean) => {
  fileStore.changeState({
    key: 'siderState',
    value: flag ? 'show' : 'hidden'
  })
}
const columnsTypeChangeHandle = (value: ColumnType[]) => {
  fileStore.changeState({
    key: 'columnsType',
    value
  })
}

// ----------- 文件操作 ---------------  //

const createFolderVisible = ref(false)
const renameVisible = ref(false)
const moveFileVisible = ref(false)
const curSelectFolder = ref<VKey | undefined>()

// 创建文件夹
const createFolderVisibleChange = (flag: boolean) => {
  createFolderVisible.value = flag
}
const okcreateFolderHandle = () => {
  if (!folderCreateForm.valid.value) {
    return message.info('请填写文件夹名称!')
  } else {
    createFolderVisibleChange(false)
    setTimeout(() => {
      folderCreateForm.reset()
    }, 500);
  }
}
// 重命名
const renameVisibleChange = (flag: boolean) => {
  renameVisible.value = flag
}
const okrenameHandle = () => {
  if (!folderCreateForm.valid.value) {
    return message.info('请填写文件夹名称!')
  } else {
    renameVisibleChange(false)
    setTimeout(() => {
      renameFileForm.reset()
    }, 500);
  }
}
// 移动文件
const moveFileVisibleChange = (flag: boolean) => {
  moveFileVisible.value = flag
}
const okMoveFileHandle = () => {
  moveFileVisibleChange(false)
  console.log('curSelectFolder', curSelectFolder.value);

  curSelectFolder.value = undefined
}
const selectTargetFolder = (evt: Event, node: TreeNode) => {
  console.log('evt, node', { evt, node });
  curSelectFolder.value = node.key
}


// 删除

const deleteFileHandle = () => {
  confirm({
    title: '警告', content: '是否删除选中文件!', onOk: () => {
      console.log('selectData', {
        sk: selectKeys.value,
        sr: selectRows.value
      });

    }
  })
}


// 分享
const shareFileVisibleChange = (flag: boolean) => {
  shareFileVisible.value = flag
}
const okShareFileHandle = () => {
  console.log('share handle');
  console.log('folderShareForm', folderShareForm.valueRef, selectKeys.value);

  shareFileVisibleChange(false)
}

const setSelectData = (sKeys: any, sRows: any) => {
  selectKeys.value = sKeys
  selectRows.value = sRows
}


// 新建

const createButtons: MenuData[] = [
  { type: 'item', key: 'folder', label: '文件夹' },
  {
    type: 'sub', key: 'online', label: '在线文档',
    children: [
      { key: 'excel', label: 'Excel' },
      { key: 'word', label: 'Word' },
      { key: 'ppt', label: 'PowerPoint ' },
    ],
  }
]
const createHandle = (options: MenuClickOptions) => {
  if (options.key === 'folder') {
    createFolderVisibleChange(true)
  }
}

// 上传
const uploadButtons: MenuData[] = [
  { type: 'item', key: 'file', label: '文件' },
  { type: 'item', key: 'folder', label: '文件夹' },
  { type: 'item', key: 'drag', label: '拖拽上传' },
]
const uploadHandle = (options: MenuClickOptions) => {
  console.log('options', options);
}

</script>
<template>
  <IxLayout id="container" class="m-0 p-0">
    <IxLayoutSider v-if="fileStore.siderState === 'show'"
      class="left-content flex flex-col justify-between bg-white m-0 p-0">
      <IxMenu :expandedKeys="expandedKeys" @update:expandedKeys="onExpandedChange" mode="inline" :dataSource="menuData">
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
    <div class="resize">
      <IxIcon style="background-color: #EEEEEE;" v-if="fileStore.siderState === 'show'" @click="siderShowHandle(false)"
        name="collapse" class="py-8 text-black rounded-md rounded-tl-none rounded-bl-none" />
      <IxIcon style="background-color: #EEEEEE;" v-else @click="siderShowHandle(true)" name="expand"
        class="py-8 text-black rounded-md rounded-tl-none rounded-bl-none" />
    </div>
    <IxLayoutContent class="right-content p-0 m-0 flex-1">
      <IxRow class="mx-3 mt-3 p-2 flex justify-center items-center">
        <IxCol class="flex justify-start" :span="12">
          <IxBreadcrumb>
            <IxBreadcrumbItem>
              巴黎
            </IxBreadcrumbItem>
            <IxBreadcrumbItem>
              北京
            </IxBreadcrumbItem>
            <IxBreadcrumbItem>
              热情的岛屿
            </IxBreadcrumbItem>
            <IxBreadcrumbItem>
              土耳其
            </IxBreadcrumbItem>
          </IxBreadcrumb>
        </IxCol>
        <IxCol style="display: flex;;justify-content: flex-end;" :span="12">
          <IxSpace align="center" class="overflow-x-auto pr-4">
            <IxDropdown v-model:visible="uploadFileVisible" trigger="click">
              <IxButton mode="primary">
                上传
                <IxIcon name="down" :rotate="uploadFileVisible ? -180 : 0" size="16px" style="margin-left: 4px">
                </IxIcon>
              </IxButton>
              <template #overlay>
                <IxMenu :dataSource="uploadButtons" @click="uploadHandle" :selectable="false"></IxMenu>
              </template>
            </IxDropdown>
            <IxButton v-if="selectBtnFlag" mode="primary" @click="shareFileVisibleChange(true)">分享</IxButton>
            <IxButton v-if="selectBtnFlag" mode="primary" @click="deleteFileHandle">删除</IxButton>
            <IxDropdown v-model:visible="createFileVisible" trigger="click">
              <IxButton mode="primary">
                新建
                <IxIcon name="down" :rotate="createFileVisible ? -180 : 0" size="16px" style="margin-left: 4px">
                </IxIcon>
              </IxButton>
              <template #overlay>
                <IxMenu :dataSource="createButtons" @click="createHandle" :selectable="false"></IxMenu>
              </template>
            </IxDropdown>
            <IxButton v-if="selectBtnFlag" mode="primary" @click="moveFileVisibleChange(true)">移动文件</IxButton>
          </IxSpace>
          <IxSpace align="center">
            <IxInput v-model:value="searchValue">
              <template #addonBefore class="bg-none">
                <IxIcon name="search" />
              </template>
            </IxInput>
            <!-- <IxPopover :visible="visible" trigger="click" placement="bottomEnd"> -->
            <IxPopover :visible="visible" trigger="manual" placement="bottomEnd">
              <template #content>
                <IxSpace vertical align="start">
                  <div class="flex justify-around items-center">
                    <p class="pr-3 pb-1">展示方式:</p>
                    <IxRadioGroup v-model:value="fileStore.displayType"
                      :dataSource="fileStore.routeType === 'image' ? fileDisplayType : fileDisplayType.slice(0, 2)"
                      @change="fileDisplayTypeChangeHandle">
                    </IxRadioGroup>
                  </div>
                  <div v-if="fileStore.displayType === 'list'" class="flex justify-around items-center">
                    <p class="pr-3 pb-1">展示列:</p>
                    <IxCheckboxGroup @Change="columnsTypeChangeHandle" v-model:value="columnsTypes"
                      :dataSource="columnsTypesData"></IxCheckboxGroup>
                  </div>
                  <!-- <div v-if="fileStore.displayType !== 'list'" class="flex justify-around items-center">
                    <p class="pr-3 pb-1">图标大小:</p>
                    <IxSlider class="w-32" v-model:value="iconSize" :disabled="false"></IxSlider>
                  </div> -->
                </IxSpace>
              </template>
              <IxButton @Click="visible = !visible" class="border-none">
                <IxIcon name="setting" />
              </IxButton>
            </IxPopover>
          </IxSpace>
        </IxCol>
      </IxRow>
      <FileTable ref="tableRef" :setSelectData="setSelectData" v-if="fileStore.displayType === 'table'"
        :type="activeRouteType" :dataSource="dataSource" />
      <FileList ref="listRef" :setSelectData="setSelectData" :move-handle="() => moveFileVisibleChange(true)"
        :delete-handle="() => deleteFileHandle()" :rename-handle="()=>renameVisibleChange(true)" :share-handle="() => shareFileVisibleChange(true)"
        :key="fileStore.columnsType.join(',')" v-else-if="fileStore.displayType === 'list'" :type="activeRouteType"
        :dataSource="dataSource" />
      <TimeLine v-else-if="fileStore.displayType === 'timeLine'" :type="activeRouteType" :dataSource="dataSource" />
      <IxModal :destroyOnHide="true" :visible="createFolderVisible" @ok="okcreateFolderHandle" header="新建文件夹"
        @cancel="createFolderVisibleChange(false)" @close="createFolderVisibleChange(false)" :centered="false"
        :width="400">
        <IxForm :control="folderCreateForm">
          <IxFormItem label="文件夹名" required>
            <IxInput control="folderName"></IxInput>
          </IxFormItem>
        </IxForm>
      </IxModal>
      <IxModal :destroyOnHide="true" :visible="renameVisible" @ok="okrenameHandle" header="文件重命名"
        @cancel="renameVisibleChange(false)" @close="renameVisibleChange(false)" :centered="false"
        :width="400">
        <IxForm :control="renameFileForm">
          <IxFormItem label="文件名" required>
            <IxInput control="newName"></IxInput>
          </IxFormItem>
        </IxForm>
      </IxModal>
      <IxModal :destroyOnHide="true" :visible="shareFileVisible" @ok="okShareFileHandle" header="文件分享"
        @cancel="shareFileVisibleChange(false)" @close="shareFileVisibleChange(false)" :centered="false" :width="400">
        <IxForm :control="folderShareForm">
          <IxFormItem label="链接有效期至" required>
            <IxDatePicker control="validityData" type="datetime" clearable></IxDatePicker>
          </IxFormItem>
          <IxFormItem label="是否需要提取码">
            <IxRadioGroup control="shouldCode">
              <IxRadio value="y">是</IxRadio>
              <IxRadio value="n">否</IxRadio>
            </IxRadioGroup>
          </IxFormItem>
          <IxFormItem v-if="folderShareForm.valueRef.value.shouldCode === 'y'" label="提取码" required>
            <IxInput control="password"></IxInput>
          </IxFormItem>
        </IxForm>
      </IxModal>
      <IxModal :destroyOnHide="true" :visible="moveFileVisible" @ok="okMoveFileHandle" header="移动文件"
        @cancel="moveFileVisibleChange(false)" @close="moveFileVisibleChange(false)" :centered="false" :width="600">
        <div style="height: 400px; overflow-y: scroll;">
          <IxProTree :style="{ height: '400px' }" :height="321" :dataSource="folderList" @nodeClick="selectTargetFolder">
            <template #suffix>
              <IxSpace>
                <IxIcon name="plus" @click="createFolderVisibleChange(true)" />
              </IxSpace>
            </template>
          </IxProTree>
        </div>
      </IxModal>
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

:deep(.ix-tree-node) {
  margin: 3px 0;
}

#container {
  display: flex;

}

.resize {
  width: 10px;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}
</style>