<script setup lang="ts">
import { MenuClickOptions, type MenuData } from '@idux/components/menu'
import AppLogo from '@/assets/images/logo.svg'
import { useUserStoreWithOut, useCommonStoreWithOut } from '@/store';
import router from './router';
const logo = {
  image: AppLogo,
  title: 'CloudDrive',
  link: '/',
}
const route = useRoute()
const userStore = useUserStoreWithOut()
const commonStore = useCommonStoreWithOut()
const NoLeyoutRoutes = ['/login']
const showLayoutFlag = computed(() => {
  return !NoLeyoutRoutes.includes(route.path) || !userStore.isLogin
})
const menuData: MenuData[] = [
  { type: 'item', key: '/', icon: 'home', label: '首页' },
  { type: 'item', key: '/file?type=all', icon: 'cloud-server', label: '网盘' },
  { type: 'item', key: '/doc', icon: 'file-text', label: '文档' },
]
const menuHandluer = (options: MenuClickOptions) => {
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
  commonStore.changeState({
    key: 'curRoute',
    value: '/'
  })
  userStore.clearState()
  // requestHandler<string, LoginRqType>(userapi.login, "post", formGroup.getValue());
  router.push('/login')
}
onMounted(() => {
  commonStore.changeState({
    key: 'curRoute',
    value: history.state.current
  })
})
</script>

<template>
  <div class="h-screen w-screen">
    <router-view v-if="commonStore.curRoute.includes('/preview')" class="m-0 p-0"></router-view>
    <IxMessageProvider v-else>
      <IxModalProvider>
        <IxProLayout class="m-0 p-0" :onMenuClick="menuHandluer" :activeKey="commonStore.curRoute" :logo="logo"
          :menus="userStore.isLogin ? menuData : []" :type="showLayoutFlag ? 'both' : 'header'" theme="light">
          <template #itemLabel="item">
            <router-link :to="item.key">{{ item.label }}</router-link>
          </template>
          <template #headerExtra>
            <IxButtonGroup v-if="!userStore.isLogin" align="center" block justify="space-between" :gap="8" mode="text">
              <IxButton><router-link to="registe">注册</router-link></IxButton>
              <IxButton><router-link to="login">登录</router-link></IxButton>
            </IxButtonGroup>
            <IxButtonGroup v-else align="center" block justify="space-between" :gap="8" mode="text">
              <IxButton>{{ userStore.userInfo.UserName }}</IxButton>
              <IxButton @click="logout">退出</IxButton>
            </IxButtonGroup>
          </template>
          <router-view class="m-0 p-0"></router-view>
        </IxProLayout>
      </IxModalProvider>
    </IxMessageProvider>
  </div>
</template>
<style scoped>
:deep(.ix-layout-content) {
  padding: 0;
}

:deep(.ix-pro-layout-content) {
  padding: 0;
}
</style>