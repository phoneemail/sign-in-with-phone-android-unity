using UnityEditor;
using UnityEngine;

using System.IO;

using UnityEditor.IMGUI.Controls;

namespace Gpm.CacheStorage
{
    using Util;
    using Internal;

    public class CacheStorageWindow : EditorWindow
    {
        public const float CACHE_INFO_UI_SIZE = 380;

        public const string MENU = "GPM/CacheStorage/Viewer";
        public const string TITLE = "CacheStorage";

        public const string MANAGEMENT = "Management";
        public const string SIZE = "Size";
        public const string COUNT = "Count";

        public const string REQUEST_INFO = "Request Info";
        public const string DEFAULT_REQUEST_TYPE = "Default RequestType";
        public const string REREQUEST = "ReRequest";

        public const string AUTO_REMOVE = "Auto Remove";
        public const string UNUSED_PERIODTIME = "UnusedPeriodTime";
        public const string REMOVECYCLE = "RemoveCycle";

        public const string STATE_DISABLE = " (Disable)";
        
        public const string COPY = "copy";

        public const string SHOW_LOCAL_TIME = "ShowLocalTime";

        public bool showLocalTime = false;

        [MenuItem(MENU)]
        private static void CreateWizard()
        {
            CacheStorageWindow window = EditorWindow.GetWindow<CacheStorageWindow>(false, TITLE, true);
            window.Show();
        }

        private TreeViewState treeState;
        private CacheStorageTreeView entryTree;
        private SearchField searchField;
        private MultiColumnHeaderState mchs;

        public void Init()
        {
            if (entryTree == null)
            {
                CacheStorageInternal.Initialize();

                if (treeState == null)
                {
                    treeState = new TreeViewState();
                }

                var headerState = CacheStorageTreeView.CreateDefaultMultiColumnHeaderState();
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(mchs, headerState) == true)
                {
                    MultiColumnHeaderState.OverwriteSerializedFields(mchs, headerState);
                }
                mchs = headerState;


                searchField = new SearchField();
                entryTree = new CacheStorageTreeView(treeState, mchs);

                GpmCacheStorage.AddChangeCacheEvnet( () =>
                {
                    entryTree.SetDirty();

                    Repaint();
                });

                entryTree.Reload();
            }
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
        }

        private void Update()
        {
            if (entryTree != null)
            {
                entryTree.Update();
            }
        }

        private void Refresh()
        {
            entryTree.SetDirty();
        }
        
        private void OnGUI()
        {
            Init();

            GUIStyle titleStyle = EditorStyles.boldLabel;
            titleStyle.fontSize = 18;

            using (new EditorGUILayout.VerticalScope())
            {
                if (CacheStorageInternal.IsSetting() == true)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.VerticalScope())
                        {
                            GUILayout.Label(MANAGEMENT, titleStyle);

                            long cacheSize = GpmCacheStorage.GetCacheSize() + GpmCacheStorage.GetRemoveCacheSize();
                            long maxSize = GpmCacheStorage.GetMaxSize();
                            if (maxSize > 0)
                            {
                                if (maxSize > 1024)
                                {
                                    EditorGUILayout.LabelField(SIZE, string.Format("{0}/{1} ({2}/{3})", Utility.GetSizeText(cacheSize), Utility.GetSizeText(maxSize), cacheSize, maxSize));
                                }
                                else
                                {
                                    EditorGUILayout.LabelField(SIZE, string.Format("{0}/{1}", cacheSize, maxSize));
                                }
                            }
                            else
                            {
                                EditorGUILayout.LabelField(SIZE, string.Format("{0}", Utility.GetSizeText(cacheSize)));
                            }

                            int maxCount = GpmCacheStorage.GetMaxCount();
                            if (maxCount > 0)
                            {
                                EditorGUILayout.LabelField(COUNT, string.Format("{0}/{1}", GpmCacheStorage.GetCacheCount(), maxCount));
                            }
                            else
                            {
                                EditorGUILayout.LabelField(COUNT, string.Format("{0}", GpmCacheStorage.GetCacheCount()));
                            }
                        }

                        using (new EditorGUILayout.VerticalScope())
                        {
                            GUILayout.Label(REQUEST_INFO, titleStyle);

                            EditorGUILayout.LabelField(DEFAULT_REQUEST_TYPE, GpmCacheStorage.GetCacheRequestType().ToString());

                            if (GpmCacheStorage.GetReRequestTime() <= 0)
                            {
                                GUI.enabled = false;
                                EditorGUILayout.LabelField(REREQUEST, GpmCacheStorage.GetReRequestTime() + STATE_DISABLE);
                                GUI.enabled = true;
                            }
                            else
                            {
                                EditorGUILayout.LabelField(REREQUEST, GpmCacheStorage.GetReRequestTime().ToString());
                            }
                        
                        }

                        using (new EditorGUILayout.VerticalScope())
                        {

                            if ( GpmCacheStorage.GetUnusedPeriodTime() <= 0 ||
                                 GpmCacheStorage.GetRemoveCycle() <= 0)
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    GUI.enabled = false;
                                    GUILayout.Label(AUTO_REMOVE + STATE_DISABLE, titleStyle);
                                }
                                EditorGUILayout.LabelField(UNUSED_PERIODTIME, GpmCacheStorage.GetUnusedPeriodTime().ToString());
                                EditorGUILayout.LabelField(REMOVECYCLE, GpmCacheStorage.GetRemoveCycle().ToString());
                                GUI.enabled = true;
                            }
                            else
                            {
                                GUILayout.Label(AUTO_REMOVE, titleStyle);

                                EditorGUILayout.LabelField(UNUSED_PERIODTIME, GpmCacheStorage.GetUnusedPeriodTime().ToString());
                                EditorGUILayout.LabelField(REMOVECYCLE, GpmCacheStorage.GetRemoveCycle().ToString());
                            }
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (entryTree != null)
                        {
                            Rect controllRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));
                            entryTree.OnGUI(controllRect);
                        }
                    }

                    CacheInfo info = entryTree.GetSelectedCache();
                    if (info != null)
                    {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(CACHE_INFO_UI_SIZE), GUILayout.ExpandHeight(true)))
                        {
                            ShowInfo(info);
                        }
                    }
                }
            }
        }
        
        private void ShowInfo(CacheInfo info)
        {
            GUIStyle titleStyle = EditorStyles.boldLabel;
            titleStyle.fontSize = 14;

            GUILayout.Label(Path.GetFileName(info.url), titleStyle);

            GUILayout.Space(10);

            GUILayout.Label(HttpElement.URL);

            GUIStyle textStyle = EditorStyles.label;
            textStyle.wordWrap = true;
            GUILayout.Label(info.url, textStyle);
            if (GUILayout.Button(COPY) == true)
            {
                EditorGUIUtility.systemCopyBuffer = info.url;
            }

            showLocalTime = EditorGUILayout.Toggle(SHOW_LOCAL_TIME, showLocalTime);

            GUILayout.Space(10);

            EditorGUILayout.LabelField(HttpElement.ETAG, info.eTag);

            EditorGUILayout.LabelField(HttpElement.LAST_MODIFIED, TimeString(info.lastModified));

            EditorGUILayout.LabelField(HttpElement.EXPIRES, TimeString(info.expires));

            EditorGUILayout.LabelField(HttpElement.REQUEST_TIME, TimeString(info.requestTime));

            EditorGUILayout.LabelField(HttpElement.RESPONSE_TIME, TimeString(info.responseTime));

            EditorGUILayout.LabelField(HttpElement.LAST_ACCESS, TimeString(info.lastAccess));

            EditorGUILayout.LabelField(HttpElement.DATE, TimeString(info.date));

            EditorGUILayout.LabelField(HttpElement.AGE, info.age);

            if(info.cacheControl != null)
            {
                EditorGUILayout.LabelField(HttpElement.MAX_AGE, info.cacheControl.maxAge);

                EditorGUILayout.LabelField(HttpElement.NO_CACHE, info.cacheControl.noCache.ToString());

                EditorGUILayout.LabelField(HttpElement.NO_STORE, info.cacheControl.noStore.ToString());

                EditorGUILayout.LabelField(HttpElement.MUST_REVALIDATE, info.cacheControl.mustRevalidate.ToString());
            }

            EditorGUILayout.LabelField(HttpElement.CONENT_LENGTH, info.contentLength.ToString());
        }

        public string TimeString(long utcTicks)
        {
            if (utcTicks == 0)
            {
                return string.Empty;
            }

            System.DateTime dateTime = new System.DateTime(utcTicks, System.DateTimeKind.Utc);
            if (showLocalTime == true)
            {
                return dateTime.ToLocalTime().ToString();
            }

            return dateTime.ToString("r");
        }
    }
}
