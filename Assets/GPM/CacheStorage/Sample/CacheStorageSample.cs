namespace Gpm.CacheStorage.Sample
{
    using Internal;
    using Common.Log;

    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using UnityEngine.Networking;

    public class CacheStorageSample : MonoBehaviour
    {
        public const string NAME = "CacheStorageSample";

        public GameObject initializeObject;
        public GameObject cacheStorageObject;

        public InputField cacheMaxCountInputField;
        public InputField cacheMaxSizeInputField;
        public InputField cacheReRequestTimeInputField;
        public Dropdown   cacheReqeustType;
        public InputField cacheUnusedPeriodTimeInputField;
        public InputField cacheRemoveCycleInputField;

        public InputField url;

        public RawImage testUnityWebRequestImage;
        public Text testUnityWebRequestTime;
        public Text testUnityWebRequestResponseSize;

        public RawImage testCacheStorageImage;
        public Text testCacheStorageTime;
        public Text testCacheStorageResponseSize;

        public RawImage testCacheStorageLocalImage;
        public Text testCacheStorageLocalTime;
        public Text testCacheStorageLocalResponseSize;

        public Dropdown removeCacheIndex;

        public Text cacheSizeText;
        public Text cacheCountText;
        public Text cachReRequestTimeText;
        public Text cacheReqeustTypeText;
        public Text cacheUnusedPeriodTimeText;
        public Text cacheRemoveCycleText;

        public void Start()
        {
            int maxCount = 10000;
            int maxSize = 5 * 1024 * 1024;
            double reRequestTime = 0;
            CacheRequestType defaultRequestType = CacheRequestType.FIRSTPLAY;
            double unusedPeriodTime = 0;
            double removeCycle = 1;

            cacheMaxCountInputField.text = maxCount.ToString();
            cacheMaxSizeInputField.text = maxSize.ToString();

            cacheReRequestTimeInputField.text = reRequestTime.ToString();

            List<string> cacheReqeustTypeList = new List<string>(Enum.GetNames(typeof(CacheRequestType)));
            cacheReqeustType.AddOptions(cacheReqeustTypeList);
            cacheReqeustType.value = (int)defaultRequestType;

            cacheUnusedPeriodTimeInputField.text = unusedPeriodTime.ToString();
            cacheRemoveCycleInputField.text = removeCycle.ToString();

            initializeObject.SetActive(true);
            cacheStorageObject.SetActive(false);
        }

        public void Initialize()
        {
            int maxCount = int.Parse(cacheMaxCountInputField.text);
            int maxSize = int.Parse(cacheMaxSizeInputField.text);
            double reRequestTime = double.Parse(cacheReRequestTimeInputField.text);
            CacheRequestType defaultRequestType = (CacheRequestType)Enum.Parse(typeof(CacheRequestType), cacheReqeustType.options[cacheReqeustType.value].text);
            double unusedPeriodTime = double.Parse(cacheUnusedPeriodTimeInputField.text);
            double removeCycle = double.Parse(cacheRemoveCycleInputField.text);

            GpmCacheStorage.Initialize(maxCount, maxSize, reRequestTime, defaultRequestType, unusedPeriodTime, removeCycle);

            OnInitialized();
        }

        private void OnInitialized()
        {
            cacheCountText.text = GetCacheCountText();
            cacheSizeText.text = GetCacheSizeText();
            cachReRequestTimeText.text = cacheReRequestTimeInputField.text;
            cacheReqeustTypeText.text = cacheReqeustType.options[cacheReqeustType.value].text;
            cacheUnusedPeriodTimeText.text = cacheUnusedPeriodTimeInputField.text;
            cacheRemoveCycleText.text = cacheRemoveCycleInputField.text;

            SettingScroll();

            GpmCacheStorage.AddChangeCacheEvnet( () =>
            {
                cacheCountText.text = GetCacheCountText();
                cacheSizeText.text = GetCacheSizeText();

                SettingScroll();
            });

            initializeObject.SetActive(false);
            cacheStorageObject.SetActive(true);

        }

        private string GetCacheCountText()
        {
            int maxCount = GpmCacheStorage.GetMaxCount();
            if (maxCount > 0)
            {
                return string.Format("{0}/{1}", GpmCacheStorage.GetCacheCount(), maxCount);
            }

            return GpmCacheStorage.GetCacheCount().ToString();
        }

        private string GetCacheSizeText()
        {
            long cacheSize = GpmCacheStorage.GetCacheSize() + GpmCacheStorage.GetRemoveCacheSize();
            long maxSize = GpmCacheStorage.GetMaxSize();
            if (maxSize > 0)
            {
                if (maxSize > 1024)
                {
                    return string.Format("{0}/{1} ({2}/{3})", Util.Utility.GetSizeText(cacheSize), Util.Utility.GetSizeText(maxSize), cacheSize, maxSize);
                }
                else
                {
                    return string.Format("{0}/{1}", cacheSize, maxSize);
                }
            }

            return Util.Utility.GetSizeText(cacheSize);
        }

        private void SettingScroll()
        {
            removeCacheIndex.ClearOptions();

            var add = new List<string>();
            for (int i = 0; i < GpmCacheStorage.GetCacheCount(); i++)
            {
                add.Add(i.ToString());
            }
            removeCacheIndex.AddOptions(add);

            removeCacheIndex.RefreshShownValue();
        }

        public void ReuqestUnityWebRequest()
        {
            StartCoroutine(TestUnityWebRequest(url.text));
        }

        private IEnumerator TestUnityWebRequest(string url)
        {
            testUnityWebRequestImage.texture = null;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            using (UnityWebRequest request = new UnityWebRequest())
            {
                request.method = UnityWebRequest.kHttpVerbGET;
                request.downloadHandler = new DownloadHandlerBuffer();
                request.url = url;
                request.useHttpContinue = false;

                request.SendWebRequest();

                yield return request;

                while (request.isDone == false)
                {
                    yield return null;
                }

                sw.Stop();
#if UNITY_2020_2_OR_NEWER
                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
#else
                if (request.isNetworkError || request.isHttpError)
#endif
                {
                    GpmLogger.Error(request.error, NAME, typeof(CacheStorageSample), "TestUnityWebRequest");
                }
                else
                {
                    Texture2D texture = new Texture2D(1, 1);
                    texture.LoadImage(request.downloadHandler.data, true);

                    testUnityWebRequestImage.texture = texture;

                    testUnityWebRequestTime.text = sw.ElapsedMilliseconds.ToString();
                    testUnityWebRequestResponseSize.text = Util.Utility.GetSizeText((long)request.downloadedBytes);
                }
            }
        }

        public void ReuqestCacheInfo()
        {
            testCacheStorageImage.texture = null;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            CacheRequestOperation request = GpmCacheStorage.Request(url.text, (result) =>
            {
                sw.Stop();

                if (result.IsSuccess() == true)
                {
                    Texture2D texture = new Texture2D(1, 1);
                    texture.LoadImage(result.Data);
                    texture.Apply();

                    testCacheStorageImage.texture = texture;


                    testCacheStorageTime.text = sw.ElapsedMilliseconds.ToString();
                    testCacheStorageResponseSize.text = Util.Utility.GetSizeText(result.Info.contentLength);
                }
            });
        }

        public void ReuqestCacheInfo_Local()
        {
            testCacheStorageLocalImage.texture = null;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            CacheRequestOperation request = GpmCacheStorage.RequestLocalCache(url.text, (result_local) =>
            {
                if (result_local.IsSuccess() == true)
                {
                    sw.Stop();
                    Texture2D texture = new Texture2D(1, 1);
                    texture.LoadImage(result_local.Data);
                    texture.Apply();

                    testCacheStorageLocalImage.texture = texture;

                    testCacheStorageLocalTime.text = sw.ElapsedMilliseconds.ToString();
                    testCacheStorageResponseSize.text = Util.Utility.GetSizeText(result_local.Info.contentLength);
                }
                else
                {
                    request = GpmCacheStorage.RequestHttpCache(url.text, (result) =>
                    {
                        sw.Stop();

                        if (result.IsSuccess() == true)
                        {
                            Texture2D texture = new Texture2D(1, 1);
                            texture.LoadImage(result.Data);
                            texture.Apply();

                            testCacheStorageLocalImage.texture = texture;

                            testCacheStorageLocalTime.text = sw.ElapsedMilliseconds.ToString();
                            testCacheStorageResponseSize.text = Util.Utility.GetSizeText(result.Info.contentLength);
                        }
                    });
                }
            });
        }

        public void RemoveData()
        {
            int removeIIndex = removeCacheIndex.value;

            List<CacheInfo> cacheInfoList = CacheStorageInternal.GetCacheList();
            if(removeIIndex < cacheInfoList.Count)
            {
                CacheStorageInternal.RemoveCacheData(cacheInfoList[removeIIndex]);

                removeCacheIndex.RefreshShownValue();
            }
        }

        public void ClearCache()
        {
            GpmCacheStorage.ClearCache();

            removeCacheIndex.ClearOptions();
            removeCacheIndex.RefreshShownValue();
        }

        public void OpenCacheFolder()
        {
            Application.OpenURL(CacheStorageInternal.GetCachePath());
        }
    }
}