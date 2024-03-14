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
import { itemapi } from '@/api'

import { Validators, useFormGroup } from '@idux/cdk/forms'
import { TreeNode } from '@idux/components/tree/src/types'
import requestHandler, { get, post } from '@/request'
import { CreateFolderRqType, GetFileRpType, GetFileRqType, ResponseDataType } from '@/api/type'

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
const dataSource = ref<FileDataType[] | null>([])
const activeRouteType = ref<LocationQueryValue | LocationQueryValue[] | FileType>(fileStore.displayType)
const rootMenuSubKeys: VKey[] = ['mine', 'recycle', 'share']
const expandedKeys = ref<VKey[]>(['mine'])
const message = useMessage()
const selectKeys = ref<any[]>([])
const selectRows = ref<any[]>([])
const selectBtnFlag = computed(() => selectKeys.value.length > 0)
const { confirm } = useModal()
const showRightFileDetailFlag = ref<boolean>(false) // 展示右侧详情
const rightFileDetailInfo = ref<FileDataType>()

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
  { key: 'LeafName', label: '文件名', disabled: true },
  { key: 'FileType', label: '类型' },
  { key: 'FileSize', label: '大小' },
  { key: 'CreatedDate', label: '创建日期' },
  { key: 'ChangeDate', label: '修改日期' },
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
const getData = async (type: 'All' | 'Image' | 'Docs' | 'Video' | 'Audio' | 'Other', PageIndex: number, PageSize = 10) => {
  const { IsSuccess, Data, Message } = await get<GetFileRpType[], GetFileRqType>(itemapi.getFiles, {
    FileType: type,
    PageIndex: PageIndex,
    PageSize: PageSize
  });
  dataSource.value = IsSuccess ? Data : []
  !IsSuccess && message.error(Message)
}
onMounted(async () => {
  dragControllerDiv()
  getData('All', 1)
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
const okcreateFolderHandle = async () => {
  if (!folderCreateForm.valid.value) {
    return message.info('请填写文件夹名称!')
  } else {
    const createFolderRes: ResponseDataType<any> = await requestHandler<ResponseDataType<boolean>, CreateFolderRqType>(itemapi.createFolder, "post", { FolderName: folderCreateForm.getValue().folderName, ParentFolderId: null });
    console.log('createFolderRes', createFolderRes);
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
const clickMenuHandle = (options: MenuClickOptions) => {

  switch (options.key) {
    case '/file?type=all':
      getData('All', 1)
      break;
    case '/file?type=image':
      getData('Image', 1)
      break;
    case '/file?type=docs':
      getData('Docs', 1)
      break;
    case '/file?type=video':
      getData('Video', 1)
      break;
    case '/file?type=audio':
      getData('Audio', 1)
      break;
    case '/file?type=other':
      getData('Other', 1)
      break;
    // case '/file?type=recycle':
    //   getData('Recycle', 1)
    //   break;
    // case '/file?type=share':
    //   getData('Share', 1)
    //   break;
  }
  // /file?type=all
}

const showRightFileDetailFn = (flag: boolean) => {
  showRightFileDetailFlag.value = flag
}

const setSelectedRow = (record: any) => {
  showRightFileDetailFlag.value = true
  rightFileDetailInfo.value = record
}

</script>
<template>
  <IxLayout id="container" class="m-0 p-0">
    <IxLayoutSider v-if="fileStore.siderState === 'show'"
      class="left-content flex flex-col justify-between bg-white m-0 p-0">
      <IxMenu @click="clickMenuHandle" :expandedKeys="expandedKeys" @update:expandedKeys="onExpandedChange"
        mode="inline" :dataSource="menuData">
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
              1
            </IxBreadcrumbItem>
            <IxBreadcrumbItem>
              2
            </IxBreadcrumbItem>
            <IxBreadcrumbItem>
              3
            </IxBreadcrumbItem>
            <IxBreadcrumbItem>
              4
            </IxBreadcrumbItem>
          </IxBreadcrumb>
        </IxCol>
        <IxCol style="display: flex;;justify-content: flex-end;" :span="12">
          <IxSpace style="min-width: 430px;justify-content: end;" class="flex overflow-x-auto pr-4 bg-red">
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
          <IxSpace align="center" style="min-width: 276px;">
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
      <div class="flex mt-3">
        <FileList ref="tableRef" :setSelectData="setSelectData" v-if="fileStore.displayType === 'table'"
          :type="activeRouteType" :dataSource="dataSource" :setSelectedRow="setSelectedRow" />
        <FileTable :setSelectedRow="setSelectedRow" ref="listRef" :setSelectData="setSelectData"
          :move-handle="() => moveFileVisibleChange(true)" :delete-handle="() => deleteFileHandle()"
          :rename-handle="() => renameVisibleChange(true)" :share-handle="() => shareFileVisibleChange(true)"
          :key="fileStore.columnsType.join(',')" v-else-if="fileStore.displayType === 'list'" :type="activeRouteType"
          :dataSource="dataSource || []" />
        <TimeLine :setSelectedRow="setSelectedRow" v-else-if="fileStore.displayType === 'timeLine'"
          :type="activeRouteType" :dataSource="dataSource || []" />
        <IxCard v-if="showRightFileDetailFlag" borderless
          :header="{ title: '文件详情', suffix: 'double-right', onSuffixClick: () => showRightFileDetailFn(false) }"
          style="width: 360px;">
          <IxEmpty v-if="!rightFileDetailInfo" description="选中文件/文件夹，查看详情" />
          <IxDesc v-else header="单列数据" col="1" labelWidth="56px">
            <IxDescItem label="文件名">{{rightFileDetailInfo.LeafName}}</IxDescItem>
            <IxDescItem label="类型">禁用</IxDescItem>
            <IxDescItem label="操作人">saas</IxDescItem>
            <IxDescItem label="描述信息">优先保证网络会议带宽的使用</IxDescItem>
            <IxDescItem label="优先级">高</IxDescItem>
            <IxDescItem label="更新时间">2022-02-20 16:29</IxDescItem>
          </IxDesc>
        </IxCard>
        <IxIcon class="hover:color-#74ABFF" style="margin-top: 15px;padding-right:10px;font-size: 16px;cursor: pointer;"
          v-if="!showRightFileDetailFlag" @click="() => showRightFileDetailFn(true)" name="double-left" />
      </div>
    </IxLayoutContent>
  </IxLayout>
  <IxModal :destroyOnHide="true" :visible="createFolderVisible" @ok="okcreateFolderHandle" header="新建文件夹"
    @cancel="createFolderVisibleChange(false)" @close="createFolderVisibleChange(false)" :centered="false" :width="400">
    <IxForm :control="folderCreateForm">
      <IxFormItem label="文件夹名" required>
        <IxInput control="folderName"></IxInput>
      </IxFormItem>
    </IxForm>
  </IxModal>
  <IxModal :destroyOnHide="true" :visible="renameVisible" @ok="okrenameHandle" header="文件重命名"
    @cancel="renameVisibleChange(false)" @close="renameVisibleChange(false)" :centered="false" :width="400">
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