import { createGlobalConfig } from '@idux/components/config'
import { createApp } from 'vue'
import router from '@/router'
// import store from '@/store'
import App from './App.vue'
import Idux from "./idux";
import './tailwind.css'
import { Button ,Upload} from 'ant-design-vue';
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

// app.use(store);
app.use(Idux);
app.use(globalConfig);
app.use(router);

app.mount('#app');
