<script setup lang='ts'>
import { useUserStoreWithOut } from '@/store';
import { useMessage } from '@idux/components/message'
import { Validators, useFormGroup } from '@idux/cdk/forms'
import requestHandler from '@/request';
import userapi from '@/api/userapi'
import router from '@/router';
import type { LoginRqType, ResponseDataType } from '@/api/type'
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

const { required, minLength, maxLength } = Validators

const formGroup = useFormGroup({
  Phone: ['', required],
  PassWord: ['', [required, minLength(6), maxLength(18)]],
})

const login = async () => {
  console.log('formGroup', formGroup.valid.value, userStore);
  const { IsSuccess, Data } = await requestHandler<ResponseDataType<any>, LoginRqType>(userapi.loginApi, "post", {
    Phone: "string",
    PassWord: "string"
  });
  console.log('res', Data);
  if (IsSuccess) {
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
  <div class="flex flex-col justify-center items-center" style="height: calc(100vh - 400px);">
    <h1 class="pb-10" style="font-size: 40px;font-weight: 800;">LOGIN</h1>
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
      <IxFormItem style="margin: 8px 0" messageTooltip>
        <IxButton mode="primary" class="bg-none text-blue-500 hover:bg-blue-500 hover:text-white duration-500" block
          type="submit" @click="login">Login</IxButton>
      </IxFormItem>
    </IxForm>
  </div>
</template>