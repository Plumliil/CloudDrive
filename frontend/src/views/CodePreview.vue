<script setup lang='ts'>
import { Codemirror } from "vue-codemirror";
import { javascript } from "@codemirror/lang-javascript";
import { oneDark } from "@codemirror/theme-one-dark";
const code = ref(`console.log('Hello, world!')`);
const extensions = [oneDark];
const options = reactive({
  style: { height: "400px" },
  mode: "text/x-c++src",
  spellcheck: true,
  autofocus: true,
  indentWithTab: true,
  tabSize: 2,
  extensions: [javascript(), oneDark], //传递给CodeMirror EditorState。创建({扩展})
});

// // 方法
// // 失去焦点时,使用已编辑的代码
// function useEditedCode() {
//   console.log("@@@blur@@@code:", code.value);
//   console.log("@@@blur@@@cpp:", cpp);
// }

// // 改变主题
// function changeTheme(e) {
//   console.log("options.extensions:", options.extensions);
//   if (e.target.innerHTML === "黑夜") {
//     options.extensions = [];
//     dateTime = e.target.innerHTML = "白天";
//   } else {
//     options.extensions = [oneDark];
//     dateTime = e.target.innerHTML = "黑夜";
//   }
// }
// // 改变模式
// function changeMode(e) {
//   console.log("selectValue:", selectValue);
//   if (selectValue === "cpp") {
//     if (dateTime === "黑夜") options.extensions = [python(), oneDark];
//     else options.extensions = [python()];
//     selectValue = "python";
//     e.target.innerHTML = "python";
//     options.mode = "text/x-python";
//   } else {
//     if (dateTime === "黑夜") options.extensions = [cpp(), oneDark];
//     else options.extensions = [cpp()];
//     selectValue = "cpp";
//     e.target.innerHTML = "C++";
//     options.mode = "text/x-c++src";
//   }
// }
</script>

<template>
  <Codemirror v-model="code" :options="options" placeholder="Code gose here..." :style="{ height: '95vh' }" :autofocus="true"
    :indent-with-tab="true" :tabSize="2" :extensions="extensions" />
  <!-- @ready="log('ready', $event)"
    @change="log('change', $event)"
    @focus="log('focus', $event)"
    @blur="log('blur', $event)" -->
</template>

<style scoped></style>
