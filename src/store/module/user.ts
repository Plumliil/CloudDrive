import { defineStore } from 'pinia'
import { store } from '@/store'
import { UserStoreType } from '@/type'
// import { getToken } from '@/utils/http/cookie'

async function checkUserLoginInfo() {
  return {
    success: true,
    data: {}
  }
}
const useUserStore = defineStore({
  id: 'user',
  persist: true,
  state: () => ({
    isLogin: false,
    userInfo: {
      name: ''
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
    changeLoginState(state: boolean) {
      this.isLogin = state
    },
    changeUserInfo(data: any) {
      this.userInfo = Object.assign({}, this.userInfo, data)
    },
    getUserInfo() {
      console.log('getUserInfo');
      // return checkUserLoginInfo().then((res) => {
      //   if (res.success) {
      //     this.isLogin = true
      //     this.userInfo = res.data
      //   } else {
      //     this.isLogin = false
      //   }
      // })
    }
  },
})

export default function useUserStoreWithOut() {
  return useUserStore(store)
}


