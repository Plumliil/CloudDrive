import { defineStore } from 'pinia'

async function checkUserLoginInfo() {
  return {
    success: true,
    data: {}
  }
}

export default defineStore('user', {
  state: () => ({
    isLogin: false,
    userInfo: {}
  }),
  actions: {
    getUserInfo() {
      return checkUserLoginInfo().then((res) => {
        if (res.success) {
          // 改变登录状态
          this.isLogin = true
          // 保存用户信息
          this.userInfo = res.data
        } else {
          this.isLogin = false
        }
      })
    }
  },
})