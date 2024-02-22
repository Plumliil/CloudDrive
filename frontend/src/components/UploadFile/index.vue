<template>
  <div class="flex justify-center items-center py-32">
    <!-- customRequest 是自定义上传的方法 -->
    <a-upload :fileList="fileList" :customRequest="upload">
      <!-- 显示的内容 -->
      <a-button type="primary" class="bg-blue-500 text-white hover:bg-white hover:text-blue-500">大文件上传</a-button>
    </a-upload>
  </div>
</template>

<script setup lang="ts">
import { useMessage } from '@idux/components/message'
import type { UploadProps } from 'ant-design-vue';
import { calcFileHash, createFileChunk, uploadChunks } from '@/utils/upload'
import type { UploadRequestOption } from 'ant-design-vue/es/vc-upload/interface';
import requestHandler from '@/request';
const fileList = ref<UploadProps["fileList"]>([]) // 声明fileList数据

const message = useMessage()
type CheckResponse = {
  success: string,
}
type CheckRequest = {
  hash: string,
}

const upload = async (params: UploadRequestOption) => {
  const formData = new FormData()
  formData.append('file', params.file)
  const hash = await calcFileHash(params.file as File) // 类型断言
  const chunks = createFileChunk(params.file as File, hash) //创建分片
  uploadChunks(chunks, hash, params);
  // const { data } = await requestHandler("/api/upload", "post", formData)
  // console.log(data);

}

// const message = useMessage()
// type CheckResponse = {
//   success: string,
// }
// type CheckRequest = {
//   hash: string,
// }

// ref 声明泛型——泛型类是从ant-design-vue中取出来的
// const upload = async (params: UploadRequestOption) => {
//获取上传的文件——进行检查——检查过后——分片——上传——合并
// 当选择一个文件就会触发 upload 方法
// const hash = await calcFileHash(params.file as File) // 类型断言
// const { data } = await requestHandler<CheckResponse, CheckRequest>("/checkFile", "post", { hash })
// if (data.success) {
//   // 如果这个值为true 意味着文件已经上传完成
//   message.success("秒传成功")
//   // 告诉 upload 组件 文件已经上传完了
//   if (params.onSuccess) {
//     params.onSuccess(data)
//   }
//   return
// }
// const chunks = createFileChunk(params.file as File, hash) //创建分片
// uploadChunks(chunks, hash, params);
// console.log('chunks', chunks);
// message.success("秒传成功")
// 有了 hash 值就可以去后端检查 这个文件有没有传过
// }


</script>

<style lang="" scoped></style>