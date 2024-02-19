import { defineStore } from 'pinia'
import { store } from '@/store'
import { UserStoreType } from '../type'
import { LocalStorageCache } from '@/utils'


const useUserStore = defineStore({
  id: 'user',
  persist: true,
  state: ():UserStoreType => ({
    isLogin: false,
    userInfo: {
      UserId: '',
      UserName: '',
      Phone: '',
      UserMail: '',
      TokenCreateTime: ''
    }
  }),
  actions: {
    changeState({ key, value }: { key: keyof UserStoreType, value: any }) {
      if (['userInfo'].includes(key)) {
        this[key] = Object.assign({}, this[key], value)
        return
      }
      this[key] = value
    },
    clearState() {
      LocalStorageCache.clear()
      this.isLogin = false
      this.userInfo = {
        UserId: '',
        UserName: '',
        Phone: '',
        UserMail: '',
        TokenCreateTime: '',
      }
    },
  },
})

export default function useUserStoreWithOut() {
  return useUserStore(store)
}

