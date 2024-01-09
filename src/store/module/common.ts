import { defineStore } from 'pinia'
import { store } from '@/store'
// import { getToken } from '@/utils/http/cookie'

type StateType = {
  curRoute: '/' | '/file' | '/doc'
}
const useCommonStore = defineStore({
  id: 'common',
  persist: true,
  state: () => {
    const state: StateType = {
      curRoute: '/'
    }
    return state
  },
  actions: {
    changeState({ key, value }: { key: keyof StateType, value: any }) {
      this[key] = value
    }
  },
})

export default function useCommonStoreWithOut() {
  return useCommonStore(store)
}


