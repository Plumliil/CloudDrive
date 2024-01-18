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




// 引入 jszip 库

// 递归读取目录并压缩成 ZIP
async function compressDirectory(directoryEntry) {
  // 创建一个 JSZip 实例
  const zip = new JSZip();

  // 递归读取目录下的所有文件
  await readDirectoryRecursive(directoryEntry, zip, '');

  // 生成 ZIP Blob
  const zipBlob = await zip.generateAsync({ type: 'blob' });
  return zipBlob;
}

// 递归读取目录
async function readDirectoryRecursive(directoryEntry, zip, currentPath) {
  const reader = directoryEntry.createReader();
  const entries = await new Promise((resolve, reject) => {
    reader.readEntries(resolve, reject);
  });

  for (const entry of entries) {
    const entryPath = `${currentPath}${entry.name}`;

    if (entry.isFile) {
      // 如果是文件，读取文件内容并添加到 ZIP 中
      const fileContent = await readFile(entry);
      zip.file(entryPath, fileContent);
    } else if (entry.isDirectory) {
      // 如果是目录，递归读取目录
      const subZip = zip.folder(entryPath);
      await readDirectoryRecursive(entry, subZip, `${entryPath}/`);
    }
  }
}

// 读取文件内容
function readFile(fileEntry) {
  return new Promise((resolve, reject) => {
    fileEntry.file((file) => {
      const reader = new FileReader();
      reader.onload = (event) => {
        resolve(event.target.result);
      };
      reader.onerror = reject;
      reader.readAsArrayBuffer(file);
    }, reject);
  });
}

// 示例使用方法
// const directoryEntry = /* your DirectoryEntry object */
// const zipBlob = await compressDirectory(directoryEntry)

// 现在，你可以将 zipBlob 上传到服务器，或者进行其他操作
