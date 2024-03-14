<script setup lang='ts'>
import { useUserStoreWithOut } from '@/store';
import { useMessage } from '@idux/components/message'
import { Validators, useFormGroup } from '@idux/cdk/forms'
import requestHandler, { post } from '@/request';
import { userapi } from '@/api'
import router from '@/router';
import { jwtDecode } from "jwt-decode";
import type { LoginRqType } from '@/api/type'
import { LocalStorageCache } from '@/utils'
import { JWT_Type } from '@/type';
const userStore = useUserStoreWithOut()
const message = useMessage()
const { required, minLength, maxLength } = Validators

const phoneValidator = (value: string) => {
  if (!value || /^(13[0-9]|14[0-9]|15[0-9]|16[0-9]|17[0-9]|18[0-9]|19[0-9])\d{8}$/.test(value)) {
    return undefined
  }
  return { Phone: { message: 'Mobile phone number is not valid!' } }
}

const formGroup = useFormGroup({
  Phone: ['', [required, phoneValidator]],
  PassWord: ['', [required, minLength(6), maxLength(18)]],
})

const login = async () => {
  const { IsSuccess, Data, Message } = await post<string, LoginRqType>(userapi.login, formGroup.getValue());
  if (!IsSuccess && !Data) return message.error(Message)
  message.success("登录成功")
  const decode: JWT_Type = jwtDecode(Data as string);
  LocalStorageCache.setItem('Authorization', Data)
  userStore.changeState({
    key: 'isLogin',
    value: true
  })
  userStore.changeState({
    key: 'userInfo',
    value: import.meta.env.VITE_IS_NEST ? decode : JSON.parse(decode.jwt)
  })
  router.replace('/')
}

const passwordVisible = ref(false)

</script>

<template>
  <div class="flex flex-col justify-center items-center" style="height: calc(100vh - 400px);">
    <h1 class="pb-0" style="font-size: 40px;font-weight: 800;">LOGIN</h1>
    <IxForm class="rounded-md py-16 px-4 w-96 mx-auto" :control="formGroup">
      <IxFormItem message="Please input your Phone!">
        <IxInput control="Phone" prefix="phone"></IxInput>
      </IxFormItem>
      <IxFormItem message="Please input your password, its length is 6-18!">
        <IxInput control="PassWord" prefix="lock" :type="passwordVisible ? 'text' : 'password'">
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