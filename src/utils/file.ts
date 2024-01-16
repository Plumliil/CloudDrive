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
  broke: broke
}


export function getFileExtension(filename: string) {
  return filename.slice((filename.lastIndexOf(".") - 1 >>> 0) + 2);
}

export function getFileIcon(fileName: string) {
  const ext: any = getFileExtension(fileName).toLowerCase();
  return logoMap[ext] || logoMap.unknow;
}