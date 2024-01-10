/**
 * 文件大小处理
 * @param s
 * @returns 文件大小和单位
 */
function extractNumberAndUnit(s: string) {
  const pattern = /(\d+(?:\.\d+)?)\s*(M|KB)/
  const match = s.match(pattern)

  if (match) {
    const number = parseFloat(match[1])
    const unit = match[2]
    return { number, unit }
  }
  else {
    return null
  }
}
/**
 * 通过文件名获取是否可预览标识
 * @param filename 文件名 Previewflag 是否可预览
 * @returns 文件名及文件类型
 */
export function getPreviewflag(filename: string, record?: any) {
  // JavaScript数组表示支持的文件后缀
  const supportPreview = [
    // 文档格式
    'doc', 'docx', 'odt', 'txt', 'rtf', 'epub',
    // 电子表格格式
    'xls', 'xlsx', 'ods', 'csv',
    // 演示文稿格式
    'ppt', 'pptx', 'odp', 'pps', 'ppsx',
    // PDF文件
    'pdf',
    // 图形文件
    'jpg', 'jpeg', 'png', 'gif', 'bmp', 'svg',
    // 视频文件
    'mov', 'mp4',
    // 压缩文件
    'zip', 'rar',
  ]
  // 使用正则表达式获取文件名和文件后缀
  const match = filename?.toLowerCase()?.match(/^(.+)\.(\w+)$/)
  const size = extractNumberAndUnit(record?.Size || '')
  if (match) {
    const extension = match[2]?.toLowerCase()
    // 压缩包大小大于50M不可预览
    if (['zip', 'rar'].includes(extension) && size && size.number >= 50 && size.unit === 'M')
      return false
    return supportPreview.includes(extension)
  }
  else {
    return false
  }
}
/**
 * 通过文件名获取是否可预览标识
 * @param filename 文件名 Previewflag 是否可预览
 * @returns 文件名及文件类型
 */
export function getFileNameExt(filename: string) {
  // 使用正则表达式获取文件名和文件后缀
  const match = filename?.toLowerCase()?.match(/^(.+)\.(\w+)$/)
  if (match) {
    const extension = match[2]?.toLowerCase()
    return extension
  }
  else {
    return ''
  }
}
