// stores/counter.js
import useUserStoreWithOut from './module/user' //  用户模块
import useFileStoreWithOut from './module/file' //  文件模块
import useSideMenuStore from './module/sideMenu' //  左侧菜单模块
import useCommonStoreWithOut from './module/common' //  公共模块
import useUploadFileStore from './module/uploadFile' //  拖拽上传文件模块
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'
export {
  useUserStoreWithOut,
  useFileStoreWithOut,
  useSideMenuStore,
  useCommonStoreWithOut,
  useUploadFileStore
}

import type { App } from 'vue'
import { createPinia } from 'pinia'

const store = createPinia()
store.use(piniaPluginPersistedstate);

export default function setupStore(app: App<Element>) {
  app.use(store)
}
export { store }

