<script lang="ts" setup>
import requestHandler from '@/request';
import { formatDate, formatFileSize, getFileIcon, getFileTypeByIndex } from '@/utils/file.ts'
import { useFileStoreWithOut } from '@/store';
import { useDragUploadv2 } from '@/hooks'
import { FileDataType, FileDisplayPropsType } from '@/type';
import { menusEvent } from 'vue3-menus';
import { TableColumn, TableColumnSelectable, TablePagination } from '@idux/components/table'
import { useFormControl } from '@idux/cdk';
import { useMessage } from '@idux/components';
const fileStore = useFileStoreWithOut()
const curRightSelect = ref<FileDataType>()
const curUploadKey = ref<number | undefined>(-1)
const tableFileList = ref<File[]>([])
const dragUploadVisible = ref<boolean>(false)
const monseoverData = ref<FileDataType>()
const renameRow = ref<number | string>()
const message = useMessage()
const props = withDefaults(defineProps<FileDisplayPropsType>(), {})
const selectableColumn = reactive<TableColumnSelectable<FileDataType>>({
  type: 'selectable',
  align: 'center',
  multiple: true,
  showIndex: false,
  onChange: (selectedKeys, selectedRows) => {
    props.setSelectData && props.setSelectData(selectedKeys, selectedRows)
  },
})
const setPagination = (pageIndex: number, pageSize: number) => {
  // 如果修改了 pageSize, 应该把 pageIndex 重置为 1
  if (pagination.pageSize !== pageSize) {
    pagination.pageIndex = 1
    pagination.pageSize = pageSize
  } else {
    pagination.pageIndex = pageIndex
  }
  // fetchData(pagination.pageIndex, pagination.pageSize)
}

const pagination = reactive<TablePagination>({
  showSizeChanger: true,
  onChange: setPagination,
})
const columns = ref<TableColumn<FileDataType>[]>([
  selectableColumn,
  ...(fileStore.columnsType.includes('LeafName') ? [{
    title: '文件名',
    dataKey: 'LeafName',
    customCell: 'LeafName',
  }] : []),

  ...(fileStore.columnsType.includes('LeafName') ? [{
    title: '类型',
    dataKey: 'FileType',
    width: 200,
    customCell: ({ value }: { value: number }) => getFileTypeByIndex(value)
  }] : []),
  ...(fileStore.columnsType.includes('LeafName') ? [{
    title: '大小',
    dataKey: 'FileSize',
    width: 200,
    customCell: ({ value }: { value: string }) => formatFileSize(parseInt(value))
  }] : []),
  ...(fileStore.columnsType.includes('LeafName') ? [{
    title: '创建日期',
    dataKey: 'CreatedDate',
    width: 200,
    customCell: ({ value }: { value: string }) => formatDate(value)
  }] : []),
  ...(fileStore.columnsType.includes('LeafName') ? [{
    title: '修改日期',
    dataKey: 'UpdatedDate',
    width: 200,
    customCell: ({ value }: { value: string }) => formatDate(value)
  }] : []),
])
const selectedRowKeys = ref([])
onMounted(() => {
  setPagination(1, 20)
})

const menus = shallowRef({
  menus: [
    {
      label: "查看",
      tip: '    ',
      click: () => {
        console.log('查看');
      }
    },
    {
      label: "删除",
      tip: '    ',
      click: () => {
        props.deleteHandle && props.deleteHandle()
      }
    },
    {
      label: "移动",
      tip: '    ',
      click: () => {
        props.moveHandle && props.moveHandle()
      }
    },
    {
      label: "重命名",
      tip: '    ',
      click: () => {
        // props.renameHandle && props.renameHandle()
        console.log('重命名', curRightSelect.value?.Id);
        renameRow.value = curRightSelect.value?.Id
        onEdit(curRightSelect.value, 'name')
      }
    },
    {
      label: "分享",
      tip: '    ',
      click: () => {
        props.shareHandle && props.shareHandle()
      }
    },
    {
      label: "下载",
      tip: '    ',
      click: () => {
        props.downHandle && props.downHandle()
      }
    },
  ]
});
function rightClick(event: MouseEvent, record: FileDataType) {
  menusEvent(event, menus.value, null);
  curRightSelect.value = record
  event.preventDefault();
}

const mouseoverHandle = (record: FileDataType) => {
  monseoverData.value = record
}

const cancelHandle = () => {
  monseoverData.value = undefined
  tableFileList.value = []
}

const showUploadTip = (record: FileDataType) => {
  curUploadKey.value = record.Id
}

watch(() => monseoverData.value, async (n: FileDataType | undefined) => {
  if (!n) return
  curUploadKey.value = n?.Id
})


const dragoverHandle = (e: Event) => {
  e.preventDefault();
}
const dropHandle = (e: DragEvent) => {
  useDragUploadv2(e).then((fileList: File[] | undefined) => {
    if (monseoverData.value?.FileType === 9 && fileList) {
      console.log('dropHandle fileList', fileList);
      tableFileList.value = fileList
      dragUploadVisible.value = true
    } else {
      tableFileList.value = []
    }
  })
  curUploadKey.value = -1
  e.preventDefault();
}

const submitHandle = async () => {
  const res: any = await requestHandler("/api/file", "get")
  if (res.success) {
    console.log('tableFileList.value 临时上传成功', tableFileList.value, res);
    Promise.all(tableFileList.value.map(async (file: File) => {
      const formData = new FormData();
      formData.append('files', file);
      return Promise.resolve(requestHandler("/api/file/upload", "post", formData))
    })).then((res: any) => {
      console.log('upload all', res);

    })
  }

}

const goPreview = (record: FileDataType) => {
  window.open(`/preview?id=${record.Id}`, '_black')
  console.log('record', record);

}


const editControl = useFormControl<string | number | undefined>(undefined)
const validatorMap = {
  name: [],
}

const onEdit = (record: any, type: 'name') => {
  record.editable = type
  editControl.setValue(record[type])
  editControl.setValidators(validatorMap[type])
}


const cancleSaveNewName = () => {
  console.log('cancleSaveNewName');
  renameRow.value = -1
}
const saveNewName = (record: any, type: 'name' | 'age' | 'address') => {
  if (editControl.valid.value) {
    // 发起请求，成功后刷新数据, 最好只更新当前行的数据，
    record[type] = editControl.getValue()
    record.editable = undefined
    message.success("重命名成功~")
  } else {
    editControl.markAsDirty()
  }
  renameRow.value = -1
}
const emits = defineEmits(['setSelectedRow'])
const setSelectedRow = (_: Event, record: any) => {
  console.log(record);

  props.setSelectedRow && props.setSelectedRow(record)
}
</script>

<template>
  <IxSpace vertical class="pt-4" style="width: 98.6%;background-color: #fff;margin: 0 0.7%;margin-top: -10px;">
    <IxTable id="uploadTable" @dragover="dragoverHandle" @drop="dropHandle" v-model:selectedRowKeys="selectedRowKeys"
      :pagination="pagination" :columns="columns" :dataSource="dataSource">
      <template #LeafName="{ record }">
        <div style="height: 100%;width: 100%;" @dragover="() => showUploadTip(record)"
          @drop="() => mouseoverHandle(record)" class="flex justify-start items-center" @click.stop
          @contextmenu="(e) => rightClick(e, record)" @click="(e) => setSelectedRow(e, record)">
          <IxImage :src="getFileIcon(`${record.LeafName}.${record.type}`)" :preview="false" />
          <IxButton mode="link" @click="() => goPreview(record)">{{ record.LeafName }}
          </IxButton>
          <IxFormItem v-if="record.editable === 'name' && renameRow === record.key" messageTooltip>
            <IxInput :control="editControl">
              <template #suffix>
                <IxIcon class="cancle" name="close" @click="cancleSaveNewName" />
                <IxIcon class="save" name="check" @click="saveNewName(record, 'name')" />
              </template>
            </IxInput>
          </IxFormItem>
          <span v-if="record.type === 'folder' && dragUploadVisible"
            :class="['pl-16', curUploadKey === record.key ? 'text-gray-400' : 'text-white']">上传到此处</span>
        </div>
      </template>
    </IxTable>
    <IxModal v-model:visible="dragUploadVisible" header="文件上传" @ok="submitHandle" @close="cancelHandle"
      @cancel="cancelHandle">
      <a-upload v-model:file-list="tableFileList" />
    </IxModal>
  </IxSpace>
</template>

<style scoped>
:deep(.ix-table) {
  height: calc(100vh - 193px);
  overflow: auto;
}

:deep(.ix-image-inner) {
  width: 20px;
  min-width: 10px;
  min-height: 20px;
  height: 20px
}

:deep(.ix-image) {
  width: 36px;
  min-width: 36px;
  min-height: 20px;
  height: 20px
}

:deep(.ix-pagination) {
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
}

.cancle {
  padding: 3px;
  font-weight: 900;
  transition: .5s all;

}

.save {
  padding: 3px;
  font-size: 14px;
  font-weight: 900;
  transition: .5s all;

}
</style>