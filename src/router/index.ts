import { createRouter, createWebHistory } from 'vue-router'
import Home from '@/views/Home.vue'
// import { useUserStoreWithOut } from '@/store/index'

// const userStore = useUserStoreWithOut()
const routes = [
  // {
  //   path: '/',
  //   name: 'Upload',
  //   component: import( '@/views/Upload.vue'),
  // },
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/login',
    name: 'Login',
    component: import( '@/views/Login.vue'),
  },
  {
    path: '/file',
    name: 'File',
    component: import('@/views/File.vue'),
    // children: [
    //   {
    //     path: 'all', 
    //     component: import( '@/views/File.vue'),
    //   },
    //   {
    //     path: 'image', 
    //     component: import( '@/views/File.vue'),
    //   },
    //   {
    //     path: 'docs', 
    //     component: import( '@/views/File.vue'),
    //   },
    //   {
    //     path: 'video', 
    //     component: import( '@/views/File.vue'),
    //   },
    //   {
    //     path: 'audio', 
    //     component: import( '@/views/File.vue'),
    //   },
    //   {
    //     path: 'other', 
    //     component: import( '@/views/File.vue'),
    //   },
    // ]
  },
  // {
  //   path: '/upload',
  //   name: 'Upload',
  //   component: import( '@/views/Upload.vue'),
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