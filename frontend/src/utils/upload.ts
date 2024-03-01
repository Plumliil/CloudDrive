import { itemapi } from '@/api'
import { ResponseDataType } from '@/api/type'
import requestHandler from '@/request'
import { UploadRequestOption } from 'ant-design-vue/es/vc-upload/interface'
import sparkMD5 from 'spark-md5'
interface Chunks {
  name: string,
  chunk: Blob //File 分片之后就是 blob
}

interface MergeRequest {
  FileHash: string,
  FileName: string,
  ParentFloderId: number | null,
  FileSize: number
}

const ChunkSize = 1024 * 1024 * 10 // 分片的大小
//创建分片
const createFileChunk = (file: File, hash: string, size = ChunkSize) => {
  // 最终要分文件 File
  // 定义一个起始值
  let curr = 0
  const chunks = []
  let index = 0 // 要记录是第几片
  while (curr < file.size) {
    // 只要没有到头就一直循环
    chunks.push({
      name: `${hash}-${index}.${file.name.substring(file.name.lastIndexOf(".") + 1)}`,//名字
      chunk: file.slice(curr, curr + size) // 分片文件
    })
    console.log(`${hash}-${index}.${file.name.substring(file.name.lastIndexOf(".") + 1)}`);

    curr += size
    index++
  }
  return chunks
}
// 创建文件Hash——判断唯一性
const calcFileHash = async (file: File): Promise<string> => {
  return new Promise(resolve => {
    const spark = new sparkMD5.ArrayBuffer()
    const reader = new FileReader()
    const size = file.size
    const offset = 2 * 1024 * 1024
    // 第一个2M ，最后一个区块数据全要
    const chunks = [file.slice(0, offset)]
    let cur = offset
    while (cur < size) {
      if (cur + offset >= size) {
        chunks.push(file.slice(cur, cur + offset))
      } else {
        // 中间的区块
        const mid = cur + offset / 2
        const end = cur + offset
        chunks.push(file.slice(cur, cur + 2))
        chunks.push(file.slice(mid, mid + 2))
        chunks.push(file.slice(end - 2, end))
      }
      cur += offset
    }
    // 中间的，取前中后各2各字节
    reader.readAsArrayBuffer(new Blob(chunks))
    reader.onloadend = e => {
      spark.append(e?.target?.result as ArrayBuffer)
      resolve(spark.end())
    }
  })
}
// 上传分片
const uploadChunks = async (
  chunks: Array<Chunks>,
  hash: string,
  params: UploadRequestOption
) => {
  // 循环操作——循环发请求，发请求之前需要生成 FormData 类型数据
  const requests = chunks.map((item) => {
    // 生成 formData
    const form = new FormData();
    form.append("FormFiles", item.chunk, item.name); // 只能需要加入 name 为什么? 因为后端用了 eggjs
    // aggjs 直接内置了 multi-part 插件，直接会解析 formData 文件
    // 如果不穿name 后端回报 Invalid file name
    return {
      form
    };
  });
  // 通过 map 循环生成了若干个，对象  对象里面有form
  // 循环发请求——每个请求上传一个分片内容
  // 要考虑浏览器并发问题——绝大部分浏览器的并发请求数量是 6 个
  const max = 1; //这个并发 是要控制的
  let index = 0;
  let taskPool: Array<Promise<any>> = []; // 任务执行池
  while (index < requests.length) {
    const task = requestHandler(itemapi.uploadFile, "post", requests[index].form); // 这个一个认任务
    task.then(() => {
      // then 执行之后，意味着 请求已经完成， 将池子里面的任务移除掉
      taskPool.splice(taskPool.findIndex((i) => i === task));
    });
    taskPool.push(task); // 每次将任务放入池子
    if (max === taskPool.length) {
      // 此时意味着已经有6个任务了
      await Promise.race(taskPool); // 至少等待有一个任务完成才继续下一不循环
    }
    index++;
  }
  // 这个时候 要判断是不是已经上传完所以切片
  await Promise.all(taskPool); // 等待所以任务执行完毕
  // 发送合并请求了
  const file = params.file as File;
  const ext = file.name.substring(file.name.lastIndexOf(".") + 1); // 文件后缀，后面的扩展名
  const mergeFileRes: ResponseDataType<any> = await requestHandler<ResponseDataType<any>, MergeRequest>(itemapi.mergeChunkFile, "post", { FileHash:hash,FileSize:file.size,FileName:file.name,ParentFloderId:null});
  if (params.onSuccess) {
    params.onSuccess(mergeFileRes);
  }
};


export { calcFileHash, createFileChunk, uploadChunks }