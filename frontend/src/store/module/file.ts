import { defineStore } from 'pinia'
import { store } from '@/store'
import { FileStoreType } from '@/type'
// import { getToken } from '@/utils/http/cookie'
const useFileStore = defineStore({
  id: 'file',
  persist: true,
  state: (): FileStoreType => ({
    displayType: 'list',
    columnsType: ['name', 'type', 'size', 'changeDate', 'deleteDate'],
    routeType: 'other',
    siderState: 'show'
  }),
  actions: {
    changeState<K extends keyof FileStoreType>(payload: { key: K, value: FileStoreType[K] }) {
      this.$patch({ [payload.key]: payload.value })
    }
  },
})

export default function useFileStoreWithOut() {
  return useFileStore(store)
}


