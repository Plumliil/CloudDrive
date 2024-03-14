
import axios, { AxiosResponse, AxiosRequestConfig, AxiosError } from 'axios'
import router from '@/router'
import { message } from 'ant-design-vue'
import { useUserStoreWithOut } from '@/store'

const userStore = useUserStoreWithOut()

const serviceAxios = axios.create({
  baseURL: '/',
  timeout: 5000,
  headers: {
    Source: 1
  }
})


/**
 * @desc: 异常拦截处理器
 * @param { Object } error 错误信息
 */
const errorHandler = (error: AxiosError): AxiosError | Promise<AxiosError> => {
  userStore.clearState()
  router.push({ path: '/login', query: { redirect: router.currentRoute.value.fullPath } })
  return Promise.reject(error)
}

//请求拦截器
serviceAxios.interceptors.request.use(
  config => {
    // 每次请求携带token
    config!.headers!.AuthToken = localStorage.getItem('Authorization') || ''
    return config
  },
  errorHandler
)
serviceAxios.interceptors.response.use(
  (response: AxiosResponse) => {
    if (![400,401,403,405,408,500,501,502,503,504,505].includes(response.status)) {
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

interface customDataParams<T> { //根据后端返回的估固定🧷格式
  Message: string,
  IsSuccess: boolean,
  Code: number,
  TotalCount: number,
  Data: T | null
}

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
    return RequestAxiosInstance<T, P>(url, method, { params:  { ...paramsData } }, config)
  } else {
    return RequestAxiosInstance<T, P>(url, method, { data: paramsData }, config)
  }
}
export const get = <T, P>(url: string,  paramsData?: P, config?: AxiosRequestConfig) => {
    return RequestAxiosInstance<T, P>(url, 'get', { data: paramsData }, config)
}
export const post = <T, P>(url: string,  paramsData?: P, config?: AxiosRequestConfig) => {
    return RequestAxiosInstance<T, P>(url, 'post', { data: paramsData }, config)
}
export const del = <T, P>(url: string,  paramsData?: P, config?: AxiosRequestConfig) => {
    return RequestAxiosInstance<T, P>(url, 'delete', { data: { ...paramsData } }, config)
}
export const put = <T, P>(url: string,  paramsData?: P, config?: AxiosRequestConfig) => {
    return RequestAxiosInstance<T, P>(url, 'put', { data: { ...paramsData } }, config)
}



export default requestHandler
