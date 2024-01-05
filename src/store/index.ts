// stores/counter.js
import useUserStore from './module/user' //  用户模块
import useFileListStore from './module/fileList' //  文件列表模块
import useSideMenuStore from './module/sideMenu' //  左侧菜单模块
import useCommonStore from './module/common' //  公共模块
import useUploadFileStore from './module/uploadFile' //  拖拽上传文件模块
// import { allColumnList } from '@/libs/map.js'


export {
  useFileListStore,
  useSideMenuStore,
  useCommonStore,
  useUploadFileStore
}

import type { App } from 'vue'
import { createPinia } from 'pinia'

const store = createPinia()

export default function setupStore(app: App<Element>) {
  app.use(store)
}

export { store, useUserStore }

