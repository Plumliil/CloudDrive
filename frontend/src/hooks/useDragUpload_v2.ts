export default async function (e: any): Promise<File[]> {
  if (!e.dataTransfer) return [];

  const fileList: File[] = [];

  const getFileFromEntryRecursively = async (entry: any): Promise<void> => {
    console.log('entry', entry);

    if (true) {
      // if (entry.isFile) {
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
  };

  let items = e.dataTransfer.items;



  // 使用 Promise.all 来等待所有异步操作完成
  await Promise.all(
    Array.from(items).map(async (item: any) => {
      if (item.kind === "file") {
        let entry = item.webkitGetAsEntry();
        await getFileFromEntryRecursively(entry);
      }
    })
  );

  return Promise.resolve(fileList);

  function addFileToList({ file, path }: { file: File; path: string }): void {
    // 处理添加文件到列表的逻辑，可以根据需要修改
    fileList.push(file);
  }
}
