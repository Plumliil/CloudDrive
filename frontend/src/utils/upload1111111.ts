import axios from "axios";

// 复制
const chunkSize = 5 * 1024 * 1024; // 每个分块的大小
let uploadedChunks: any = []; // 已上传的分块
let totalChunks = 0; // 总分块数

// 文件分片
function chunk(file: File) {
  let chunks = [];
  let start = 0;
  let end = 0;
  while (start < file.size) {
    end = Math.min(start + chunkSize, file.size);
    chunks.push(file.slice(start, end));
    start = end;
  }
  totalChunks = chunks.length;
  return chunks;
}

// 上传分块
function uploadChunk(file: File, chunk: Blob, index: number) {
  let formData = new FormData();
  formData.append('file', chunk);
  formData.append('index', index.toString());
  formData.append('total', totalChunks.toString());
  formData.append('name', file.name);
  formData.append('type', file.type);
  return axios.post('/upload', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
}

// 上传文件
function upload(file: File) {
  let chunks = chunk(file);
  let progress = 0;
  let loaded = 0;
  axios.defaults.onUploadProgress = function (progressEvent) {
    loaded = progressEvent.loaded;
    progress = Math.round((loaded / file.size) * 100);
    // 更新 UI 显示上传进度
  };
  return Promise.all(chunks.map(async (chunk, index) => {
    if (!uploadedChunks.includes(index)) {
      try {
        await uploadChunk(file, chunk, index);
        uploadedChunks.push(index);
      } catch (e) {
        console.error(e);
        // 记录已上传的分块
        localStorage.setItem(file.name, JSON.stringify(uploadedChunks));
        throw e;
      }
    }
  })).then(() => {
    axios.defaults.onUploadProgress = undefined;
    if (uploadedChunks.length === totalChunks) {
      merge(file);
    }
  });
}

function resumeUpload(file: File) {
  uploadedChunks = JSON.parse(localStorage.getItem(file.name) || '{}') || [];
  upload(file);
}

// 合并文件
async function merge(file: File) {
  let formData = new FormData();
  formData.append('name', file.name);
  formData.append('type', file.type);
  return axios.post('/merge', formData);
}
