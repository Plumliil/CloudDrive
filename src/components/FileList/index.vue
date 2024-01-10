<script lang="ts" setup>
import { FileDataType } from '@/type';
import { TableColumn, TableColumnSelectable } from '@idux/components/table'
import { IxTag } from '@idux/components/tag'


const selectableColumn = reactive<TableColumnSelectable<FileDataType>>({
  type: 'selectable',
  align: 'center',
  disabled: record => record.key === 4,
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

const fullData: FileDataType[] = []
for (let index = 0; index < 30; index++) {
  fullData.push({
    key: index,
    name: `Edrward ${index}`,
    type: `London Park no. ${index}`,
    dateChanged: ['nice', 'developer'],
  })
}

const data = ref<FileDataType[]>(fullData)

const selectedRowKeys = ref([1])


</script>

<template>
  <IxSpace vertical class="pt-4" style="width: 98.6%;background-color: #fff;margin: 0 0.7%;margin-top: -10px;">
    <IxTable v-model:selectedRowKeys="selectedRowKeys" :columns="columns" :dataSource="data">
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