import axios from "axios"

export const downFileProgress  =(url:string,parameter:any,callback:any,totalSize: any,uniSign: any) =>{
  return axios({
    url: url,
    params: parameter,
    method:'get' ,
    responseType: 'blob',
    onDownloadProgress (progress) {
      callback(progress, totalSize, uniSign)
    }
  })
}
