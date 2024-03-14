import { LocationQueryValue } from 'vue-router';

export type JWT_Type = {
  exp: number,
  jwt: string
}

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
  setSelectedRow:(record:any)=>void
}


export type FileDataType = {
  Id?: number;
  LeafName?: string;
  Extension?: string;
  Path?: string;
  ParentFolderId?: number | null;
  Idpath?: string;
  IsFolder?: number;
  FileType?: number;
  CreatedDate?: string;
  CreatedBy?: string;
  UpdatedDate?: string;
  UpdatedBy?: string;
  DeletedDate?: string | null;
  DeletedBy?: string | null;
  VersionId?: string;
  ItemHash?: string;
  FileSize?: number;
  PhysicalDirectory?: string;
  IsDel?: number;
  ItemFileMapUrl?: string | null;
}


type ColumnType = keyof FileDataType

export type UserStoreType = {
  isLogin: boolean
  userInfo: {
    UserId: string,
    UserName: string,
    Phone: string,
    UserMail: string,
    TokenCreateTime: string
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