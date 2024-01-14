<script setup lang='ts'>
import { useUserStoreWithOut } from '@/store';
import { useMessage } from '@idux/components/message'
import { Validators, useFormGroup } from '@idux/cdk/forms'
import requestHandler from '@/request';
import router from '@/router';

interface LoginFormType {
  tel: string,
  password: string
}
const userStore = useUserStoreWithOut()

// const loginFormRules = {
//   tel: [
//     { required: true, message: '请输入手机号', trigger: 'blur' },
//   ],
//   password: [
//     { required: true, message: '请输入密码', trigger: 'blur' },
//     { min: 5, max: 20, message: '密码长度必须在5-20之间', trigger: 'blur' },
//   ],
//   isShowDragVerify: false,
//   isPassing: false,
//   loginBtnDisabled: true,
//   loginBtnLoading: false,
// }

const message = useMessage()

// const { required, minLength, maxLength } = Validators

const formGroup = useFormGroup({
  // username: ['', required],
  // password: ['', [required, minLength(6), maxLength(18)]],
  remember: [true],
})

const login = async () => {
  console.log('formGroup', formGroup.valid.value, userStore);
  // const res: any = await requestHandler("api/user/login", "post", formGroup.valid.value);
  const res = {
    success: true
  }
  if (res.success) {
    message.success("登录成功")
    userStore.changeState({
      key: 'isLogin',
      value: true
    })
    userStore.changeState({
      key: 'userInfo',
      value: {
        name: 'Admin_Li'
      }
    })
    router.replace('/')
  } else {
    formGroup.markAsDirty()
  }
}

const passwordVisible = ref(false)

</script>

<template>
  <div class="login-container">
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
        <IxButton mode="primary" class="bg-none text-blue-500 hover:bg-blue-500 hover:text-white duration-500" block
          type="submit" @click="login">Login</IxButton>
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
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: calc(100vh - 48px);
}
</style>