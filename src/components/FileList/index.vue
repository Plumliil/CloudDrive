<script lang="ts" setup>
// import requestHandler from '@/request';
import { FileDataType, FileDisplayPropsType } from '@/type';
import { TableColumn, TableColumnSelectable } from '@idux/components/table'
import { IxTag } from '@idux/components/tag'

const props = withDefaults(defineProps<FileDisplayPropsType>(), {})

const selectableColumn = reactive<TableColumnSelectable<FileDataType>>({
  type: 'selectable',
  align: 'center',
  multiple: true,
  showIndex: false,
  onChange: (selectedKeys, selectedRows) => console.log(selectedKeys, selectedRows),
})

const columns: TableColumn<FileDataType>[] = [
  selectableColumn,
  {
    title: '文件名',
    dataKey: 'name',
    customCell: 'name',
  },
  {
    title: '类型',
    dataKey: 'type',
    width: 200,
  },
  {
    title: '修改日期',
    dataKey: 'dateChanged',
    width: 200,
    customCell: ({ value }: { value: any }) =>
      value.map((tag: string) => {
        let color = tag.length > 5 ? 'warning' : 'success'
        if (tag === 'loser') {
          color = 'error'
        }
        return h(IxTag, { color }, { default: () => tag.toUpperCase() })
      }),
  },
]

const selectedRowKeys = ref([1])
onMounted(() => {
  console.log('props.type',props.type);
  // const { data } = await requestHandler("/checkFile", "post", { hash })
})

</script>

<template>
  <IxSpace vertical class="pt-4" style="width: 98.6%;background-color: #fff;margin: 0 0.7%;margin-top: -10px;">
    <IxTable v-model:selectedRowKeys="selectedRowKeys" :columns="columns" :dataSource="props.dataSource">
      <template #name="{ value }">
        <IxButton mode="link">{{ value }}</IxButton>
      </template>
      <template #action="{ record }">
        <IxButtonGroup :gap="8" mode="link" separator="|">
          <IxButton>Invite {{ record.name }}</IxButton>
          <IxButton>Delete</IxButton>
        </IxButtonGroup>
      </template>
    </IxTable>
  </IxSpace>
</template>

<style scoped>
:deep(.ix-table-table) {
  height: calc(100vh - 193px);
}
</style>