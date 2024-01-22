/**
 * 节点文本内容复制
 * @param value 复制目标
 * @returns 复制指
 */
export function copyDomText(value: string) {
  if (navigator.clipboard && window.isSecureContext) {
    // navigator clipboard 向剪贴板写文本
    navigator.clipboard.writeText(value)
  }
  else {
    // 创建text area
    const textArea = document.createElement('textarea')
    textArea.value = value
    // 使text area不在viewport，同时设置不可见
    document.body.appendChild(textArea)
    textArea.focus()
    textArea.select()
    return new Promise((resolve, reject) => {
      // 执行复制命令并移除文本框
      document.execCommand('copy') && resolve('')
      textArea.remove()
    })
  }
}