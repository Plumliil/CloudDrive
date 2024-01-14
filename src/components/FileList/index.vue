<script lang="ts" setup>
// import requestHandler from '@/request';
import { getFileIcon } from '@/utils/file.ts'
import { useFileStoreWithOut } from '@/store';
import { FileDataType, FileDisplayPropsType } from '@/type';
import { menusEvent } from 'vue3-menus';
import { TableColumn, TableColumnSelectable, TablePagination } from '@idux/components/table'
const fileStore = useFileStoreWithOut()
const curRightSelect = ref<any>()
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
  ...(fileStore.columnsType.includes('name') ? [{
    title: '文件名',
    dataKey: 'name',
    customCell: 'name',
  }] : []),
  ...(fileStore.columnsType.includes('type') ? [{
    title: '类型',
    dataKey: 'type',
    width: 200,
    customCell: 'type',
  }] : []),
  ...(fileStore.columnsType.includes('size') ? [{
    title: '大小',
    dataKey: 'size',
    width: 200,
    customCell: 'size',
  }] : []),
  ...(fileStore.columnsType.includes('changeDate') ? [{
    title: '修改日期',
    dataKey: 'changeDate',
    width: 200,
    customCell: 'changeDate',
  }] : []),
  ...(fileStore.columnsType.includes('deleteDate') ? [{
    title: '删除日期',
    dataKey: 'deleteDate',
    width: 200,
    customCell: 'changeDate',
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
        props.renameHandle && props.renameHandle()
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
function rightClick(event: MouseEvent, record: any) {
  menusEvent(event, menus.value, null);
  curRightSelect.value = record
  event.preventDefault();
}
</script>

<template>
  <IxSpace vertical class="pt-4" style="width: 98.6%;background-color: #fff;margin: 0 0.7%;margin-top: -10px;">
    <IxTable v-model:selectedRowKeys="selectedRowKeys" :pagination="pagination" :columns="columns"
      :dataSource="props.dataSource">
      <template #name="{ record }">
        <div class="flex justify-start items-center" @click.stop @contextmenu="(e) => rightClick(e, record)">
          <IxImage :src="getFileIcon(`${record.name}.${record.type}`)" :preview="false" />
          <IxButton mode="link">{{ record.name }}</IxButton>
        </div>
      </template>
      <template #type="{ record }">
        <div class="p-2" @click.stop @contextmenu="(e) => rightClick(e, record)">{{ record.type }}</div>
      </template>
      <template #size="{ record }">
        <div class="p-2" @click.stop @contextmenu="(e) => rightClick(e, record)">{{ record.type }}</div>
      </template>
      <template #changeDate="{ record }">
        <div class="p-2" @click.stop @contextmenu="(e) => rightClick(e, record)">{{ record.changeDate.join(',') }}</div>
      </template>
      <template #deleteDate="{ record }">
        <div class="p-2" @click.stop @contextmenu="(e) => rightClick(e, record)">{{ record.deleteDate.join(',') }}</div>
      </template>
    </IxTable>
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
</style>