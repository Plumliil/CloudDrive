export default function (target: any) {
  const fileList = ref<File[]>([])
  target.addEventListener('mouseenter', (e: Event) => {
    e.preventDefault();
  })
  target.addEventListener('dragover', (e: Event) => {
    e.preventDefault();
    e.stopPropagation();
  })
  target.addEventListener('drop', (e: DragEvent) => {
    fileList.value = []
    if (!e.dataTransfer) return
    let items = e.dataTransfer.items;
    for (let i = 0; i <= items.length - 1; i++) {
      let item = items[i];
      if (item.kind === "file") {
        // FileSystemFileEntry 或 FileSystemDirectoryEntry 对象
        let entry = item.webkitGetAsEntry();
        // 递归地获取entry下包含的所有File
        getFileFromEntryRecursively(entry);
      }
    }
    console.log('drop');
    e.stopPropagation();
    e.preventDefault(); // 阻止默认行为，避免浏览器打开拖拽文件的默认行为
  })
  const getFileFromEntryRecursively = async (entry: any): Promise<void> => {
    if (entry.isFile) {
      try {
        const file = await new Promise<File>((resolve, reject) => {
          entry.file(resolve, reject);
        });
        addFileToList({ file, path: entry.fullPath });
      } catch (error) {
        console.error(error);
      }
    } else {
      try {
        const reader = entry.createReader();
        const entries = await new Promise<any[]>((resolve, reject) => {
          reader.readEntries(resolve, reject);
        });
        for (const childEntry of entries) {
          await getFileFromEntryRecursively(childEntry);
        }
      } catch (error) {
        console.error(error);
      }
    }
  }
  const addFileToList = ({ file, path }: { file: File; path: string }): void => {
    // 处理添加文件到列表的逻辑，可以根据需要修改
    fileList.value.push(file)
  }
  const removeFile = () => {
    console.log('removeFile --------------------');

  }
  return { fileList, removeFile, id: target.id }
}