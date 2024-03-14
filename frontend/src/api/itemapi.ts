import module from './module'
export default {
  createFolder: !import.meta.env.VITE_IS_NEST ? '/cloudapi/file/createFolder' : module.item + '/CreateFolder',
  getFiles: !import.meta.env.VITE_IS_NEST ? '/cloudapi/file/getFiles' : module.item + '/GetFiles',
  uploadFile: !import.meta.env.VITE_IS_NEST ? '/cloudapi/file/blockUpload' : module.item + '/BlockUpload',
  checkFileHash: !import.meta.env.VITE_IS_NEST ? '/cloudapi/file/checkFileHash' : module.item + '/CheckFileHash',
  mergeChunkFile: !import.meta.env.VITE_IS_NEST ? '/cloudapi/file/mergeChunkFile' : module.item + '/MergeChunkFile',
} 