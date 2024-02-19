import { defineStore } from 'pinia'
import { store } from '@/store'
import { CommonStoreType } from '../type'
// import { getToken } from '@/utils/http/cookie'

const useCommonStore = defineStore({
  id: 'common',
  persist: true,
  state: ():CommonStoreType => ({
    curRoute: '/',
  }),
  actions: {
    changeState<K extends keyof CommonStoreType>(payload: { key: K, value: CommonStoreType[K] }) {
      this.$patch({ [payload.key]: payload.value })
    }
  },
})

export default function useCommonStoreWithOut() {
  return useCommonStore(store)
}


