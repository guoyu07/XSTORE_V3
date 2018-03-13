using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XStore.Common
{
    public class CacheHelper
    {
        /**//// <summary>  
            /// 获取数据缓存  
            /// </summary>  
            /// <param name="CacheKey">键</param>  
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /**//// <summary>  
            /// 设置数据缓存  
            /// </summary>  
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /**//// <summary>  
            /// 设置数据缓存  
            /// </summary>  
        public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /**//// <summary>  
            /// 设置数据缓存  
            /// </summary>  
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /**//// <summary>  
            /// 移除指定数据缓存  
            /// </summary>  
        public static void RemoveAllCache(string CacheKey)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(CacheKey);
        }

        /**//// <summary>  
            /// 移除全部缓存  
            /// </summary>  
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }

    public class CacheHelper<T> where T :class
    {
        /**//// <summary>  
            /// 获取数据缓存  
            /// </summary>  
            /// <param name="CacheKey">键</param>  
        public static T GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey] as T;
        }

        public static T GetOrSetCache(string CacheKey, Func<T> GetFun)
        {
            var objCache = GetCache(CacheKey);
            if (objCache != null) return objCache;
            else return GetFun();
        }

        /**//// <summary>  
            /// 设置数据缓存  
            /// </summary>  
        public static void SetCache(string CacheKey, T objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /**//// <summary>  
            /// 设置数据缓存  
            /// </summary>  
        public static void SetCache(string CacheKey, T objObject, TimeSpan Timeout)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /**//// <summary>  
            /// 设置数据缓存  
            /// </summary>  
        public static void SetCache(string CacheKey, T objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /**//// <summary>  
            /// 移除指定数据缓存  
            /// </summary>  
        public static void RemoveAllCache(string CacheKey)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(CacheKey);
        }

        /**//// <summary>  
            /// 移除全部缓存  
            /// </summary>  
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}