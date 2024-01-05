<script setup lang="ts">
import { type MenuData } from '@idux/components/menu'
import AppLogo from '@/assets/images/logo.svg'
import { type LayoutSiderProps } from '@idux/components/layout'

const logo = {
  image: AppLogo,
  title: 'CloudDrive',
  link: '/',
}
const NoLeyoutRoutes = ['/login','/']
const showLayoutFlag = computed(() => {
  return !NoLeyoutRoutes.includes(route.path)
})
const sider = reactive<LayoutSiderProps>({
  pointer: true,
  'onUpdate:collapsed': (collapsed, changeType) => {
    console.log(collapsed, changeType)
    sider.pointer = changeType === 'pointer'
  },
})
const route = useRoute()

const dataSource: MenuData[] = [
  {
    type: 'sub',
    key: 'sub1',
    icon: 'setting',
    label: 'Sub Menu 1',
    children: [
      { type: 'item', key: 'item4', label: 'Item 4', icon: 'setting' },
      { type: 'item', key: 'item5', label: 'Item 5', icon: 'setting' },
      {
        type: 'sub',
        key: 'sub2',
        icon: 'setting',
        label: 'Menu Sub 2',
        children: [
          { type: 'item', key: 'item6', label: 'Item 6' },
          { type: 'item', key: 'item7', label: 'Item 7' },
        ],
      },
      {
        type: 'sub',
        key: 'sub3',
        icon: 'setting',
        label: 'Menu Sub 3',
        children: [
          { type: 'item', key: 'item8', label: 'Item 8' },
          { type: 'item', key: 'item9', label: 'Item 9' },
        ],
      },
    ],
  },
  {
    type: 'sub',
    key: 'sub4',
    icon: 'github',
    label: 'Menu Sub 4',
    children: [
      { type: 'item', key: 'item10', label: 'Item 10' },
      { type: 'item', key: 'item11', label: 'Item 11' },
    ],
  },
  { type: 'item', key: 'item2', icon: 'mail', label: 'Item 2' },
]
console.log('route', route);
</script>

<template>
  <div class="h-screen w-screen">
    <IxMessageProvider>
      <IxProLayout :logo="logo" :menus="showLayoutFlag ? dataSource : []" :type="showLayoutFlag ? 'mixin' : 'header'"
        theme="light" :sider="sider">
        <template #itemLabel="item">
          <router-link to="#pro-layout-demo-type">{{ item.label }}</router-link>
        </template>
        <template #headerExtra>
          <IxButtonGroup align="center" block justify="space-between" :gap="8" mode="text">
            <IxButton icon="search">搜索</IxButton>
            <IxButton icon="alert">消息</IxButton>
            <IxButton icon="setting">设置</IxButton>
            <IxButton icon="setting"><router-link to="/login">登录</router-link></IxButton>
          </IxButtonGroup>
        </template>
        <router-view></router-view>
      </IxProLayout>
    </IxMessageProvider>
  </div>
</template>