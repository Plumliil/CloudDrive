import file from '@/assets/icon/file.svg'
import image from '@/assets/icon/image.svg'
import music from '@/assets/icon/music.svg'
import audio from '@/assets/icon/audio.svg'
import video from '@/assets/icon/videor.svg'
import word from '@/assets/icon/WORDR.svg'
import unknow from '@/assets/icon/unknow.svg'
import doc from '@/assets/icon/DOCR.svg'
import excel from '@/assets/icon/ExcelR.svg'
import eml from '@/assets/icon/EMLR.svg'
import gif from '@/assets/icon/GIFR.svg'
import mov from '@/assets/icon/MOVR.svg'
import ppt from '@/assets/icon/PPTR.svg'
import rar from '@/assets/icon/RARR.svg'
import pdf from '@/assets/icon/PDF2.svg'
import png from '@/assets/icon/PNGR.svg'
import zip from '@/assets/icon/ZIPR.svg'
import broke from '@/assets/icon/broke.svg'


import JSZip from 'jszip';


type LogoMapType = {
  [key: string]: string
};


const logoMap: LogoMapType = {
  unknow: unknow,
  svg: image,
  file: file,
  image: image,
  music: music,
  audio: audio,
  video: video,
  doc: word,
  word: word,
  excel: excel,
  eml: eml,
  gif: gif,
  mov: mov,
  ppt: ppt,
  rar: rar,
  pdf: pdf,
  png: png,
  zip: zip,
  broke: broke,
  folder: file
}


export function getFileExtension(filename: string) {
  return filename.slice((filename.lastIndexOf(".") - 1 >>> 0) + 2);
}

export function getFileIcon(fileName: string) {
  const ext: any = getFileExtension(fileName).toLowerCase();
  return logoMap[ext] || logoMap.unknow;
}

// const arrayBufferToBase64 = (buffer) => {
//   let binary = ''
//   const bytes = new Uint8Array(buffer)
//   const len = bytes.byteLength
//   for (let i = 0; i < len; i++)
//     binary += String.fromCharCode(bytes[i])

//   return window.btoa(binary)
// }
// const getFilestremById = (uri) => {
//   return request(uri, {
//     method: 'get',
//     responseType: 'blob',
//   })
// }


// 引入 jszip 库

// 递归读取目录并压缩成 ZIP
// async function compressDirectory(directoryEntry) {
//   // 创建一个 JSZip 实例
//   const zip = new JSZip();

//   // 递归读取目录下的所有文件
//   await readDirectoryRecursive(directoryEntry, zip, '');

//   // 生成 ZIP Blob
//   const zipBlob = await zip.generateAsync({ type: 'blob' });
//   return zipBlob;
// }

// // 递归读取目录
// async function readDirectoryRecursive(directoryEntry, zip, currentPath) {
//   const reader = directoryEntry.createReader();
//   const entries = await new Promise((resolve, reject) => {
//     reader.readEntries(resolve, reject);
//   });

//   for (const entry of entries) {
//     const entryPath = `${currentPath}${entry.name}`;

//     if (entry.isFile) {
//       // 如果是文件，读取文件内容并添加到 ZIP 中
//       const fileContent = await readFile(entry);
//       zip.file(entryPath, fileContent);
//     } else if (entry.isDirectory) {
//       // 如果是目录，递归读取目录
//       const subZip = zip.folder(entryPath);
//       await readDirectoryRecursive(entry, subZip, `${entryPath}/`);
//     }
//   }
// }

// // 读取文件内容
// function readFile(fileEntry) {
//   return new Promise((resolve, reject) => {
//     fileEntry.file((file) => {
//       const reader = new FileReader();
//       reader.onload = (event) => {
//         resolve(event.target.result);
//       };
//       reader.onerror = reject;
//       reader.readAsArrayBuffer(file);
//     }, reject);
//   });
// }

// 示例使用方法
// const directoryEntry = /* your DirectoryEntry object */
// const zipBlob = await compressDirectory(directoryEntry)

// 现在，你可以将 zipBlob 上传到服务器，或者进行其他操作



// {
//   "data": {},
//   "status": 200,
//   "statusText": "OK",
//   "headers": {
//       "access-control-allow-origin": "*",
//       "connection": "close",
//       "content-disposition": "attachment; filename=img2.jpg; filename*=UTF-8''img2.jpg",
//       "content-length": "135895",
//       "content-type": "application/octet-stream",
//       "date": "Mon, 26 Feb 2024 08:33:55 GMT",
//       "server": "nginx/1.24.0"
//   },
//   "config": {
//       "transitional": {
//           "silentJSONParsing": true,
//           "forcedJSONParsing": true,
//           "clarifyTimeoutError": false
//       },
//       "adapter": [
//           "xhr",
//           "http"
//       ],
//       "transformRequest": [
//           null
//       ],
//       "transformResponse": [
//           null
//       ],
//       "timeout": 600000,
//       "xsrfCookieName": "XSRF-TOKEN",
//       "xsrfHeaderName": "X-XSRF-TOKEN",
//       "maxContentLength": -1,
//       "maxBodyLength": -1,
//       "env": {},
//       "headers": {
//           "Accept": "application/json, text/plain, */*",
//           "AuthToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqd3QiOiJ7XCJVc2VySWRcIjo0MzQzLFwiTG9naW5OYW1lXCI6XCJQbHVtbGlpbC5saUBlbGl0ZXNsYW5kLmNvbVwiLFwiVXNlck5hbWVcIjpcIuadjuWuh-ixqlwiLFwiVXNlck1haWxcIjpcIlBsdW1saWlsLmxpQGVsaXRlc2xhbmQuY29tXCIsXCJFbXBsb3llZUlkXCI6MjM4OCxcIlRva2VuQ3JlYXRlVGltZVwiOlwiMjAyNC0wMi0yNlQxMDozNToyMy4xMzAxODk0KzA4OjAwXCIsXCJVc2VyVHlwZVwiOjJ9IiwiZXhwIjoxNzExNTA2OTIzfQ.f6b-oNwlFTYpDeaZr1cRWeG3ZDil3KdljuLKmPjz2fQ",
//           "Source": "1"
//       },
//       "baseURL": "",
//       "method": "get",
//       "responseType": "blob",
//       "url": "/itemapi/Item/DownOnlyOfficeEditItem?libraryId=fe9bf1f9-22b4-4f53-b2e0-fa6f4384d16c&itemId=1b369ae5-0e2d-4b59-a771-bf11ca896719&OperateType=0"
//   },
//   "request": {}
// }


// /**
//  * 下载文件
//  * @param res 流
//  */
// const getloadFile = (res: { data: any; headers: any }, key: string) => {
//   targetUriTransFlag.value = false
//   const { data, headers } = res
//   if (data.type === 'application/json') {
//     // 将blob文件流转换成json
//     const reader = new FileReader()
//     reader.onload = function (event) {
//       // eslint-disable-next-line @typescript-eslint/ban-ts-comment
//       // @ts-expect-error
//       const message = JSON.parse(reader.result).Message
//       Toast({
//         type: 'error',
//         title: message,
//         duration: 3000,
//       })
//     }
//     reader.readAsText(data)
//     targetUriTransFlag.value = true
//     return false
//   }
//   const blob = new Blob([data], { type: headers['content-type'] })
//   const url = window.URL.createObjectURL(blob)
//   targetUri.value = url
//   sessionStorage.setItem(key, url)
// }
// /**
//  * 图片音频获取流后转blob地址
//  * @param uri 流下载地址
//  */
// const getPreviewUrl = async (uri: string, key: string) => {
//   viewerLoading.value = true
//   services.getFilestremById(uri).then((res: any) => {
//     getloadFile(res, key)
//   }).finally(() => {
//     viewerLoading.value = false
//   })
// }