import { createRouter, createWebHistory } from 'vue-router'
import Home from '@/views/Home.vue'
import { useUserStore } from '@/store/index'

const userStore = useUserStore()
const routes = [
  {
    path: '/',
    name: 'Upload',
    component: import(/* webpackChunkName: "login" */ '@/views/Upload.vue'),
  },
  // {
  //   path: '/',
  //   name: 'Home',
  //   component: Home,
  // },
  {
    path: '/login',
    name: 'Login',
    component: import(/* webpackChunkName: "login" */ '@/views/Login.vue'),
  },
  // {
  //   path: '/upload',
  //   name: 'Upload',
  //   component: import(/* webpackChunkName: "login" */ '@/views/Upload.vue'),
  // },
  // {
  //   path: '/about',
  //   name: 'About',
  //   component: () => import(/* webpackChunkName: "about" */ '../views/About.vue'),
  // },
  // 其他路由...
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})


router.beforeEach((to, from, next) => {
  // 当前路由需要登录才可进入
  console.log('to', to);

  if (to.matched.some((m) => m.meta.requireAuth)) {

    // 调用获取用户登录状态和信息的接口
    // userStore.getUserInfo().then(() => {
    // console.log(111);

    // if (!userSotre.getters.isLogin) {
    //   if (
    //     process.env.NODE_ENV !== 'development' &&
    //     location.origin === 'https://pan.qiwenshare.com'
    //   ) {
    //     common.goAccount(`/login/account`)
    //   } else {
    //     // 没有登录时，跳转到登录页面
    //     next({
    //       path: '/login',
    //       query: { Rurl: to.fullPath } //  将to参数中的url传递给login页面进行操作
    //     })
    //   }
    // } else {
    //   next() // 正常跳转
    // }
    // })
  } else {
    // 正常跳转
    next()
    // 调用获取用户登录状态和信息的接口，以便显示顶部导航栏的用户登录信息
    // store.dispatch('getUserInfo')
  }
})

export default router