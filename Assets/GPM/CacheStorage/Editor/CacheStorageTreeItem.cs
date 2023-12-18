using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System;
using System.IO;

namespace Gpm.CacheStorage
{
    using Util;

    internal class CacheStorageTreeItem : TreeViewItem, CacheStorageTreeView.IBaseTreeItem
    {
        public const string EXFIRES = "Exfires";
        public const string REQUEST = "Request";
        public const string REMOVE = "Remove";
        public const string NONE_DATA = "-";

        internal CacheInfo cacheInfo;
        public CacheStorageTreeItem(CacheInfo cacheInfo) : base((int)cacheInfo.GetSpaceID())
        {
            this.cacheInfo = cacheInfo;
        }
        
        public virtual void RowGUI()
        {
        }

        public void CellGUI(Rect cellRect, int column)
        {
            CellGUI(cellRect, (CacheStorageTreeView.ColumnId)column);
        }


        public virtual void CellGUI(Rect cellRect, CacheStorageTreeView.ColumnId column)
        { 
            switch (column)
            {
                case CacheStorageTreeView.ColumnId.NAME:
                    {
                        GUI.Label(cellRect, Path.GetFileName(cacheInfo.url));
                    }
                    break;
                case CacheStorageTreeView.ColumnId.URL:
                    {
                        GUI.Label(cellRect, cacheInfo.url);
                    }
                    break;
                case CacheStorageTreeView.ColumnId.SIZE:
                    {
                        GUI.Label(cellRect, Utility.GetSizeText(cacheInfo.contentLength));
                    }
                    break;
                case CacheStorageTreeView.ColumnId.EXFIRES:
                    {
                        int lifeTime = cacheInfo.GetFreshnessLifeTime();
                        if (lifeTime > 0)
                        {
                            int remainTime = lifeTime - cacheInfo.GetCurrentAge();
                            if (remainTime > 0)
                            {
                                EditorGUI.LabelField(cellRect, string.Format("{0}", Utility.GetTimeText(remainTime * TimeSpan.TicksPerSecond)));
                            }
                            else
                            {
                                GUI.color = Color.yellow;
                                EditorGUI.LabelField(cellRect, EXFIRES);
                                GUI.color = Color.white;
                            }
                        }
                        else
                        {
                            EditorGUI.LabelField(cellRect, NONE_DATA);
                        }
                        
                    }
                    break;
                case CacheStorageTreeView.ColumnId.REMAIN:
                    {
                        if(cacheInfo.IsAlways() == true)
                        {
                            EditorGUI.LabelField(cellRect, REQUEST);
                        }
                        else
                        {
                            double remainRequestTime = cacheInfo.GetRemainRequestTime();
                            if (remainRequestTime > 0)
                            {
                                EditorGUI.LabelField(cellRect, string.Format("{0}", Utility.GetTimeText((long)(remainRequestTime * TimeSpan.TicksPerSecond))));
                            }
                            else
                            {
                                GUI.color = Color.yellow;
                                EditorGUI.LabelField(cellRect, REQUEST);
                                GUI.color = Color.white;
                            }
                        }
                    }
                    break;
                case CacheStorageTreeView.ColumnId.REMOVE:
                    {
                        double unusedTime = GpmCacheStorage.GetUnusedPeriodTime();
                        double removeCycle = GpmCacheStorage.GetRemoveCycle();

                        double passTime = cacheInfo.GetPastTimeFromLastAccess();
                        if (unusedTime > 0 && removeCycle > 0)
                        {
                            double sec = unusedTime - passTime;

                            if (sec > 0)
                            {
                                EditorGUI.LabelField(cellRect, string.Format("{0}", Utility.GetTimeText((long)(sec * TimeSpan.TicksPerSecond))));
                            }
                            else
                            {
                                GUI.color = Color.yellow;
                                EditorGUI.LabelField(cellRect, REMOVE);
                                GUI.color = Color.white;
                            }
                            
                        }
                        else
                        {
                            GUI.enabled = false;
                            GUI.Label(cellRect, NONE_DATA);
                            GUI.enabled = true;
                        }
                    }
                    break;
            }

            
        }
    }

}