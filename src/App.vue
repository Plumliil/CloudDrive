<script setup lang="ts">
import { MenuClickOptions, type MenuData } from '@idux/components/menu'
import AppLogo from '@/assets/images/logo.svg'
import { useUserStoreWithOut, useCommonStoreWithOut } from '@/store';
import router from './router';
// import { useMessage } from '@idux/components/message'
const logo = {
  image: AppLogo,
  title: 'CloudDrive',
  link: '/',
}
// const message = useMessage()
const route = useRoute()
const userStore = useUserStoreWithOut()
const commonStore = useCommonStoreWithOut()
const NoLeyoutRoutes = ['/login']
const showLayoutFlag = computed(() => {
  return !NoLeyoutRoutes.includes(route.path) || !userStore.isLogin
})
const dataSource: MenuData[] = [
  { type: 'item', key: '/', icon: 'home', label: '首页' },
  { type: 'item', key: '/file', icon: 'cloud-server', label: '网盘' },
  { type: 'item', key: '/doc', icon: 'file-text', label: '文档' },
]
const menuHandluer = (options: MenuClickOptions) => {
  console.log('options', options);
  commonStore.changeState({
    key: 'curRoute',
    value: options.key
  })
}
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
      <IxProLayout :onMenuClick="menuHandluer" :activeKey="commonStore.curRoute" :logo="logo"
        :menus="userStore.isLogin ? dataSource : []" :type="showLayoutFlag ? 'both' : 'header'" theme="light">
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
        <router-view></router-view>
      </IxProLayout>
    </IxMessageProvider>
  </div>
</template>