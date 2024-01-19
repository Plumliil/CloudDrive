import module from './module'
export default {
  loginApi: module.proxy + module.user + '/User/Login'
}