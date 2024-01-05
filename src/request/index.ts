
import { getToken } from '@/plugins/cookie'
import axios, { AxiosResponse, AxiosRequestConfig } from 'axios'
import { useMessage } from '@idux/components/message'
const message = useMessage()
const serviceAxios = axios.create({
  baseURL: 'http://jsonplaceholder.typicode.com', //这里我简写了  全局配置即可
  timeout: 5000,
})
//请求拦截器
serviceAxios.interceptors.request.use(
  config => {
    // 每次请求携带token
    const token = getToken()
    config.headers.Authorization = token
    return config
  },
  error => message.error(error)
)
serviceAxios.interceptors.response.use(
  (response: AxiosResponse) => {
    // response.headers['Access-Control-Allow-Origin'] = '*'
    console.log(response);
    if (response.status == 200) {
      return response.data
    } else {
      return requestHandler
    }

  }, ((error: any) => {
    if (error && error.response) {
      switch (error.response.status) {
        case 400:
          error.message = '错误请求'
          break;
        case 401:
          error.message = '未授权，请重新登录'
          break;
        case 403:
          error.message = '拒绝访问'
          break;
        case 404:
          error.message = '请求错误,未找到该资源'
          break;
        case 405:
          error.message = '请求方法未允许'
          break;
        case 408:
          error.message = '请求超时'
          break;
        case 500:
          error.message = '服务器端出错'
          break;
        case 501:
          error.message = '网络未实现'
          break;
        case 502:
          error.message = '网络错误'
          break;
        case 503:
          error.message = '服务不可用'
          break;
        case 504:
          error.message = '网络超时'
          break;
        case 505:
          error.message = 'http版本不支持该请求'
          break;
        default:
          error.message = `连接错误${error.response.status}`
      }
    } else {
      error.message = "连接到服务器失败"
    }
    return Promise.reject(error.message)
  })
)
// interface axiosParams<T> extends AxiosResponse {
//     data: T
// }
interface customDataParams<T> { //根据后端返回的估固定🧷格式
  data: T,
  message: string,
  status: boolean
}

// type returnResponseType<T> = axiosParams<customDataParams<T>>
type methodType = 'get' | 'post' | 'put' | 'delete'


type paramsData<T> = {
  params?: T,
  data?: T
}
const RequestAxiosInstance = <T, P>(url: string, method: methodType, params: paramsData<P> | {}, config?: AxiosRequestConfig) => {
  const data = serviceAxios<any, customDataParams<T>>({
    url,
    method,
    ...params,
    ...config,
  })
  return data
}

enum methodTypeMode {
  get = 'get',
  post = 'post',
  put = 'put',
  delete = 'delete',
}

const requestHandler = <T, P>(url: string, method: methodType, paramsData?: P, config?: AxiosRequestConfig) => {
  if (method === methodTypeMode.get || method === methodTypeMode.delete) {
    return RequestAxiosInstance<T, P>(url, method, { params: { ...paramsData } }, config)
  } else {
    return RequestAxiosInstance<T, P>(url, method, { data: paramsData }, config)
  }
}

/** 用法
  type returnData={
    name:string,
    age:18
}
type responseParams={
    username:string,
    password:string
}

const resultData=requestHandler<returnData,responseParams>("/user/login", "post", { username: "admin",password:"123456" })

**/


export default requestHandler

