import { LocationQueryValue } from 'vue-router';

export type FileType = 'all' | 'image' | 'docs' | 'video' | 'audio' | 'other' | 'recycle' | 'share'
export type FileDisPlayType = 'list' | 'table' | 'timeLine'
export type BaseRouteType = '/' | '/file' | '/doc'
export type ShowType = 'show' | 'hidden'
export type FileDisplayPropsType = {
  type: LocationQueryValue | LocationQueryValue[] | FileType
  dataSource: FileDataType[]
  setSelectData?: (sKeys: any, sRows: any) => void
  shareHandle?: () => void
  moveHandle?: () => void
  deleteHandle?: () => void
  renameHandle?: () => void
  downHandle?: () => void
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
  siderState: ShowType

}

type CommonStoreType = {
  curRoute: BaseRouteType | VKey,
}