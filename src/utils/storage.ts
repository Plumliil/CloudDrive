interface CacheHelper {
  setItem(key: string, value: any): void;
  getItem<T>(key: string): T | null;
  removeItem(key: string): void;
}

class LocalStorageCacheHelper implements CacheHelper {

  static getInstance(): LocalStorageCacheHelper {
    if (!LocalStorageCacheHelper.instance) {
      LocalStorageCacheHelper.instance = new LocalStorageCacheHelper();
    }
    return LocalStorageCacheHelper.instance;
  }

  private static instance: LocalStorageCacheHelper | null = null;

  setItem(key: string, value: any) {
    localStorage.setItem(key, typeof value === 'string' ? value : JSON.stringify(value));
  }

  getItem<T>(key: string): T | null {
    const item = localStorage.getItem(key);
    if (item === null) return null;
    return JSON.parse(item) as T;
  }

  removeItem(key: string) {
    localStorage.removeItem(key);
  }
  clear() {
    localStorage.clear();
  }
}

export default new LocalStorageCacheHelper();

