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
import { FileTypeEnum } from '@/common/enum'


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

/**
 * 根据索引获取文件类型枚举对应的值。
 * @param index 索引值，必须是0到枚举成员个数的一半之间的整数。
 * @returns 返回对应索引的文件类型字符串，如果索引无效则返回undefined。
 */
export function getFileTypeByIndex(index: number): string | undefined {
  if (index >= 0 && index < Object.keys(FileTypeEnum).length / 2) {
    return FileTypeEnum[index];
  } else {
    return undefined;
  }
}

/**
 * 获取文件的扩展名。
 * @param filename 文件名，类型为字符串。
 * @returns 返回文件名中的扩展名部分。如果没有扩展名，则返回空字符串。
 */
export function getFileExtension(filename: string) {
  return filename.slice((filename.lastIndexOf(".") - 1 >>> 0) + 2);
}

/**
 * 获取文件对应的图标。
 * @param fileName 文件名，用于获取文件扩展名以确定图标。
 * @returns 返回对应文件扩展名的图标，如果没有找到则返回默认图标。
 */
export function getFileIcon(fileName: string) {
  const ext: any = getFileExtension(fileName).toLowerCase();
  return logoMap[ext] || logoMap.unknow;
}


/**
 * 将字节大小转换为更易读的文件大小格式。
 * @param bytes 要转换的字节大小。
 * @returns 转换后的文件大小字符串，带单位（B, KB, MB, GB, TB）。
 */
export function formatFileSize(bytes: number): string {
  // 定义单位数组
  const units = ['B', 'KB', 'MB', 'GB', 'TB'];
  let unitIndex = 0;
  
  // 循环将字节大小除以1024，直到小于1024或达到单位数组的最大长度
  while (bytes >= 1024 && unitIndex < units.length - 1) {
      bytes /= 1024;
      unitIndex++;
  }
  
  // 返回格式化后的文件大小和单位
  return bytes.toFixed(2) + ' ' + units[unitIndex];
}

/**
 * 格式化日期字符串为YYYY-MM-DD HH:MM:SS的格式。
 * @param dateString 一个表示日期的字符串，该字符串应能被Date构造函数解析。
 * @returns 返回一个格式化后的日期字符串，格式为YYYY-MM-DD HH:MM:SS。
 */
export function formatDate(dateString:string) {
  // 使用传入的日期字符串创建一个Date对象
  const date = new Date(dateString);
  const year = date.getFullYear(); // 获取年份
  const month = String(date.getMonth() + 1).padStart(2, '0'); // 获取月份，确保其为两位数
  const day = String(date.getDate()).padStart(2, '0'); // 获取日，确保其为两位数
  const hours = String(date.getHours()).padStart(2, '0'); // 获取小时，确保其为两位数
  const minutes = String(date.getMinutes()).padStart(2, '0'); // 获取分钟，确保其为两位数
  const seconds = String(date.getSeconds()).padStart(2, '0'); // 获取秒数，确保其为两位数

  // 将上述日期和时间组件连接成一个格式化的字符串并返回
  return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
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
