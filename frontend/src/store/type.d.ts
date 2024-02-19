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
export type DownLoadStoreType = { progressList: any[], progressError: string }