import module from './module'
export default {
  login: !import.meta.env.IS_NEST ? '/cloudapi/user/login' : module.user + '/Login',
  registe: !import.meta.env.IS_NEST ? '/cloudapi/user/register' : module.user + '/RegistrationAccount'
}