import { defineStore } from 'pinia'
import { store } from '@/store'
// import { getToken } from '@/utils/http/cookie'

async function checkUserLoginInfo() {
  return {
    success: true,
    data: {}
  }
}

const useUserStore = defineStore({
  id: 'user',
  state: () => ({
    isLogin: false,
    userInfo: {
      name: ''
    }
  }),
  actions: {
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


