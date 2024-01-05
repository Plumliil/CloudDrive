<script setup lang='ts'>
import { useUserStore } from '@/store';
import { useMessage } from '@idux/components/message'
import CanvasNest from 'canvas-nest.js'
interface LoginFormType {
  tel: string,
  password: string
}
const route = useRoute()
const userStore = useUserStore()
const loginForm = ref<LoginFormType>()
// 配置
const config = {
  color: '136,21,77', // 线条颜色
  pointColor: '136,21,77', // 节点颜色
  opacity: 0.6, // 线条透明度
  count: 99, // 线条数量
  zIndex: -1 // 画面层级
}

const loginFormRules = {
  tel: [
    { required: true, message: '请输入手机号', trigger: 'blur' },
  ],
  password: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    { min: 5, max: 20, message: '密码长度必须在5-20之间', trigger: 'blur' },
  ],
  isShowDragVerify: false,
  isPassing: false,
  loginBtnDisabled: true,
  loginBtnLoading: false,
}
const url = computed(() => {
  return 111111
})

const message = useMessage()
console.log('route', route);
onMounted(() => {
  console.log('userStore', userStore.isLogin);
  if (userStore.isLogin) {
    message.info('登录成功 !')
  } else {
    // message.warning('请先登录 !')
  }

  nextTick(() => {
    let element = document.getElementById('loginBackground')
    new CanvasNest(element, config)
  })
})


import { Validators, useFormGroup } from '@idux/cdk/forms'

const { required, minLength, maxLength } = Validators

const formGroup = useFormGroup({
  username: ['', required],
  password: ['', [required, minLength(6), maxLength(18)]],
  remember: [true],
})

const login = () => {
  message.success('登录成功 !')
  if (formGroup.valid.value) {
    console.log('login', formGroup.getValue())
  } else {
    formGroup.markAsDirty()
  }
}

const passwordVisible = ref(false)

</script>

<template>
  <div id="loginBackground" class="h-full w-full flex items-center">
    <IxForm class="rounded-md py-16 px-4 w-96 mx-auto" :control="formGroup">
      <IxFormItem message="Please input your username!">
        <IxInput control="username" prefix="user"></IxInput>
      </IxFormItem>
      <IxFormItem message="Please input your password, its length is 6-18!">
        <IxInput control="password" prefix="lock" :type="passwordVisible ? 'text' : 'password'">
          <template #suffix>
            <IxIcon :name="passwordVisible ? 'eye-invisible' : 'eye'" @click="passwordVisible = !passwordVisible">
            </IxIcon>
          </template>
        </IxInput>
      </IxFormItem>
      <IxFormItem messageTooltip>
        <IxCheckbox control="remember">Remember me</IxCheckbox>
      </IxFormItem>
      <IxFormItem style="margin: 8px 0" messageTooltip>
        <IxButton mode="primary" class="bg-none text-blue-500 hover:bg-blue-500 hover:text-white duration-500" block type="submit" @click="login">Login</IxButton>
      </IxFormItem>
      <IxRow>
        <IxCol span="12">
          <a>Forgot password</a>
        </IxCol>
        <IxCol span="12" class="text-right">
          <a>Register now!</a>
        </IxCol>
      </IxRow>
    </IxForm>
  </div>
</template>

<style scoped>
/* .demo-form {
  padding: 20px 10px;
  max-width: 300px;
  margin: 0 auto;
  background-color: red;
  margin-top: 100px;
} */

.text-right {
  text-align: right;
}
</style>