import { LocationQueryValue } from 'vue-router';

export type FileType = 'all' | 'image' | 'docs' | 'video' | 'audio' | 'other' | 'recycle' | 'share'
export type FileDisPlayType = 'list' | 'table' | 'timeLine'
export type BaseRouteType = '/' | '/file' | '/doc'

export type FileDisplayPropsType = {
  type: LocationQueryValue | LocationQueryValue[] | FileType
  dataSource: FileDataType[]
}


export type FileDataType = {
  key: number
  name: string
  type: string
  size: string
  changeDate: string[]
  deleteDate: string[]
}


type ColumnType = keyof FileDataType

export type UserStoreType = {
  isLogin: boolean
  userInfo: {
    name: string
  }
}

export type FileStoreType = {
  displayType: FileDisPlayType,
  routeType: FileType
  columnsType: ColumnType[]
}

type CommonStoreType = {
  curRoute: BaseRouteType
}