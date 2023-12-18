using System.Collections.Generic;

namespace Gpm.CacheStorage.Internal
{
    using Common.Log;

    // This class is for use in internal services. Please don't use it.
    public static class CacheStorageInternal
    {
        public const string NAME = "CacheStorageIntenal";

        public static void Initialize()
        {
            CacheStorageImplement.InitializeCore();
        }

        public static bool IsSetting()
        {
            return CacheStorageImplement.IsSetting();
        }

        public static CachePackage GetPackage()
        {
            return CacheStorageImplement.cachePackage;
        }

        public static List<CacheInfo> GetCacheList()
        {
            if (CacheStorageImplement.Initialized == false)
            {
                GpmLogger.Error("Not initialized", NAME, typeof(CacheStorageInternal));
                return null;
            }

            return CacheStorageImplement.cachePackage.cacheStorage;
        }

        public static CacheInfo GetCacheInfo(string url)
        {
            if (CacheStorageImplement.Initialized == false)
            {
                GpmLogger.Error("Not initialized", NAME, typeof(CacheStorageInternal));
                return null;
            }

            return CacheStorageImplement.cachePackage.GetCacheInfo(url);
        }

        public static bool RemoveCacheData(CacheInfo info, bool immediately = false)
        {
            if (CacheStorageImplement.Initialized == false)
            {
                GpmLogger.Error("Not initialized", NAME, typeof(CacheStorageInternal));
                return false;
            }

            return CacheStorageImplement.cachePackage.RemoveCacheData(info, immediately);
        }

        public static ulong GetLastIndex()
        {
            if (CacheStorageImplement.Initialized == false)
            {
                GpmLogger.Error("Not initialized", NAME, typeof(CacheStorageInternal));
                return 0;
            }

            return CacheStorageImplement.cachePackage.GetLastIndex();
        }

        public static bool IsDirty()
        {
            if (CacheStorageImplement.Initialized == false)
            {
                GpmLogger.Error("Not initialized", NAME, typeof(CacheStorageInternal));
                return false;
            }

            return CacheStorageImplement.cachePackage.IsDirty();
        }


        public static void SetDirty(bool value = true)
        {
            if (GpmCacheStorage.Initialized == false)
            {
                GpmLogger.Error("Not initialized", NAME, typeof(CacheStorageInternal));
                return;
            }

            CacheStorageImplement.cachePackage.SetDirty(value);
        }

        public static string GetCachePath()
        {
            return CachePackage.GetCachePath();
        }

        public static void SavePackage()
        {
            CacheStorageImplement.SavePackage();
        }
    }
}