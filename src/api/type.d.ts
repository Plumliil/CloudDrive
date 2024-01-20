export type ResponseDataType<T> = {
  Message: string,
  IsSuccess: boolean,
  Code: number,
  TotalCount: number,
  Data: T | null
}

// itemapi
export type CreateFolderRqType = {
  FolderName: string,
  ParentFolderId: number
}
export type CreateFolderRpType = {
  FolderName: string,
  ParentFolderId: number
}

export type GetFileRqType = {
  FileType: string,
  LeafName: string,
  PageIndex: number,
  PageSize: number
}
export type GetFileRpType = {
  FileType: string,
  LeafName: string,
  PageIndex: number,
  PageSize: number
}
// userapi
export type LoginRqType = {
  Phone: string,
  PassWord: string
}
export type LoginRpType = {
  Phone: string,
  PassWord: string
}
export type RegistrationRqType = {
  UserName: string,
  Phone: string,
  PassWord: string,
  Email: string,
  Sex?: number
}
export type RegistrationRpType = {
  UserName: string,
  Phone: string,
  Password: string,
  Email: string,
  Sex: number
}