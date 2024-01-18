const copyText = (value: string) => {
  if (navigator.clipboard && window.isSecureContext) {
    // navigator clipboard 向剪贴板写文本
    navigator.clipboard.writeText(value).then(() => {
      proxy.$toast({
        type: 'success',
        title: `${value} 已复制至剪贴板!`,
        duration: 1500,
      })
    })
  }
  else {
    // 创建text area
    const textArea = document.createElement('textarea')
    textArea.value = value
    // 使text area不在viewport，同时设置不可见
    document.body.appendChild(textArea)
    textArea.focus()
    textArea.select()
    proxy.$toast({
      type: 'success',
      title: `${value} 已复制至剪贴板!`,
      duration: 1500,
    })
    return new Promise((resolve, reject) => {
      // 执行复制命令并移除文本框
      document.execCommand('copy') && resolve('')
      textArea.remove()
    })
  }
}