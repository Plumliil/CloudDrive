import { defineStore } from 'pinia'
import { store } from '@/store'
// import { getToken } from '@/utils/http/cookie'

type StateType = {
  displayType: 'list' | 'table' | 'timeLine'
}
const useFileStore = defineStore({
  id: 'file',
  persist: true,
  state: () => {
    const state: StateType = {
      displayType: 'list'
    }
    return state
  },
  actions: {
    changeState({ key, value }: { key: keyof StateType, value: any }) {
      this[key] = value
    }
  },
})

export default function useFileStoreWithOut() {
  return useFileStore(store)
}


