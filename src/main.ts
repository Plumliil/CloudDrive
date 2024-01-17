import { createGlobalConfig } from '@idux/components/config'
import { createApp } from 'vue'
import router from '@/router'
// import store from '@/store'
import App from './App.vue'
import Idux from "./idux";
import './tailwind.css'
import { Button, Upload, Modal } from 'ant-design-vue';
import setupStore from '@/store'
/**
 * 1. 你可以用任意请求库来替换掉 fetch.
 * 2. 你可以将 @idux 的默认图标文件拷贝到 `public/idux-icons` 目录下，当然也可以是任意其他目录. 记得替换掉请求 url 的路径即可。
 * 3. 你也可以使用 `ant-design-icons` 的图标文件。
 * 4. 你还可以将图标文件部署到任意 cdn 中。
 * 5. 其实 `IxIcon` 组件并不关心你的文件在哪，你只需要返回一个 svg 格式的字符串即可。
 */
const loadIconDynamically = (iconName: string) => {
  return fetch(`/idux-icons/${iconName}.svg`).then(res => res.text())
}

const globalConfig = createGlobalConfig({
  icon: { loadIconDynamically },
})

const app = createApp(App);

setupStore(app)
app.use(Button);
app.use(Upload);
app.use(Modal);
// app.directive('dragUpload', {
//   mounted(el, binding, vnode) {
//     let fileList: File[] = []
//     const getFileFromEntryRecursively = async (entry: any): Promise<void> => {
//       if (entry.isFile) {
//         try {
//           const file = await new Promise<File>((resolve, reject) => {
//             entry.file(resolve, reject);
//           });
//           addFileToList({ file, path: entry.fullPath });
//         } catch (error) {
//           console.error(error);
//         }
//       } else {
//         try {
//           const reader = entry.createReader();
//           const entries = await new Promise<any[]>((resolve, reject) => {
//             reader.readEntries(resolve, reject);
//           });
//           for (const childEntry of entries) {
//             await getFileFromEntryRecursively(childEntry);
//           }
//         } catch (error) {
//           console.error(error);
//         }
//       }
//     }
//     const addFileToList = ({ file, path }: { file: File; path: string }): void => {
//       // 处理添加文件到列表的逻辑，可以根据需要修改
//       fileList.push(file)
//     }
//     el.addEventListener('click', (e: Event) => {
//       console.log(el, ' click');
//       e.preventDefault();
//     })
//     el.addEventListener('mousedown', (e: Event) => {
//       e.preventDefault();
//     })
//     el.addEventListener('dragover', (e: Event) => {
//       e.preventDefault();
//     })
//     el.addEventListener('drop', (e: DragEvent) => {
//       fileList = []
//       if (!e.dataTransfer) return
//       let items = e.dataTransfer.items;
//       for (let i = 0; i <= items.length - 1; i++) {
//         let item = items[i];
//         if (item.kind === "file") {
//           // FileSystemFileEntry 或 FileSystemDirectoryEntry 对象
//           let entry = item.webkitGetAsEntry();
//           // 递归地获取entry下包含的所有File
//           getFileFromEntryRecursively(entry);
//         }
//       }
//       console.log('fileList',fileList);
//       e.preventDefault(); // 阻止默认行为，避免浏览器打开拖拽文件的默认行为
//     })
//     console.log('fileList',fileList);
    
//     // 至少需要回调函数以及监听事件类型
//     // app.config.globalProperties.$emit('custom-event', fileList);
//   },
//   // 元素卸载前也记得清理定时器并且移除监听事件
//   beforeUnmount(el, binding) {

//   }
// })

// app.use(store);
app.use(Idux);
app.use(globalConfig);
app.use(router);

app.mount('#app');
