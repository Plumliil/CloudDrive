<script setup lang="ts">
import { type MenuData } from '@idux/components/menu'
import AppLogo from '@/assets/images/logo.svg'
import { type LayoutSiderProps } from '@idux/components/layout'
import { useUserStore } from './store';
import router from './router';
// import { useMessage } from '@idux/components/message'
const logo = {
  image: AppLogo,
  title: 'CloudDrive',
  link: '/',
}
// const message = useMessage()
const userStore = useUserStore()
const NoLeyoutRoutes = ['/login']
const showLayoutFlag = computed(() => {
  return !NoLeyoutRoutes.includes(route.path) || !userStore.isLogin
})
const sider = reactive<LayoutSiderProps>({
  pointer: false,
})
const route = useRoute()

const dataSource: MenuData[] = [
  { type: 'item', key: '/', icon: 'home', label: '首页' },
  {
    type: 'sub', key: '/file', icon: 'cloud-server', label: '网盘', children: [
      {
        type: 'sub', key: 'main', icon: 'folder-open', label: '我的文件',
        children: [
          { key: '/file?type=all', icon: 'folder', label: '全部' },
          { key: '/file?type=image', icon: 'file-image', label: '图片' },
          { key: '/file?type=docs', icon: 'audit', label: '文档' },
          { key: '/file?type=video', icon: 'play-circle', label: '视频' },
          { key: '/file?type=audio', icon: 'customer-service', label: '音乐' },
          { key: '/file?type=other', icon: 'expand-all', label: '其他' },
        ],
      },
      { key: 'recycle ', icon: 'delete', label: '回收站' },
      { key: 'share', icon: 'send', label: '我的分享' },
    ],
  },
  { type: 'item', key: 'doc', icon: 'file-text', label: '文档' },
]
/**
 * 退出登录
 * @description 清除 cookie 存放的 token  并跳转到登录页面
 */
const logout = () => {
  userStore.changeLoginState(false)
  router.push('/login')
  // message.success('退出登录成功！')
  // this.$common.removeCookies(this.$config.tokenKeyName)
  // this.$store.dispatch('getUserInfo').then(() => {
  //   this.$router.push({ name: 'Home' })
  // })
}
</script>

<template>
  <div class="h-screen w-screen">
    <IxMessageProvider>
      <IxProLayout :logo="logo" :menus="userStore.isLogin ? dataSource : []" :type="showLayoutFlag ? 'both' : 'header'"
        theme="light" :sider="sider">
        <template #itemLabel="item">
          <router-link :to="item.key">{{ item.label }}</router-link>
        </template>
        <template #headerExtra>
          <IxButtonGroup v-if="!userStore.isLogin" align="center" block justify="space-between" :gap="8" mode="text">
            <IxButton><router-link to="register">注册</router-link></IxButton>
            <IxButton><router-link to="login">登录</router-link></IxButton>
          </IxButtonGroup>
          <IxButtonGroup v-else align="center" block justify="space-between" :gap="8" mode="text">
            <IxButton>{{ userStore.userInfo.name }}</IxButton>
            <IxButton @click="logout">退出</IxButton>
          </IxButtonGroup>
        </template>
        <template #siderFooter>
          <div class="flex w-full flex-col p-1 mb-3">
            <IxProgress :percent="75" />
            <div class="flex items-center justify-between">
              <span>储存</span>
              <span>50mb / 100mb</span>
            </div>
          </div>
        </template>
        <router-view></router-view>
      </IxProLayout>
    </IxMessageProvider>
  </div>
</template>