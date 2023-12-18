
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Gpm.CacheStorage
{
    using Internal;
    public class CacheStorageTreeView : TreeView
    {
        public static class ColumnName
        {
            public const string NAME = "Name";
            public const string URL = "Url";

            public const string SIZE = "Size";
            public const string EXFIRES = "Exfires";
            public const string REMAIN = "Remain";
            public const string REMOVE = "Remove";
        }

        public enum ColumnId
        {
            NAME,
            URL,
            SIZE,
            EXFIRES,
            REMAIN,
            REMOVE,
        }

        public const string EMPTY = "Empty";

        private bool bDirty = false;

        private TreeViewItem root;

        public interface IBaseTreeItem
        {
            void CellGUI(Rect cellRect, int column);
        }

        public CacheStorageTreeView(TreeViewState state, MultiColumnHeaderState multiColumnHeader) : base(state, new MultiColumnHeader(multiColumnHeader))
        {
            showBorder = true;
            columnIndexForTreeFoldouts = 0;
        }

        protected override TreeViewItem BuildRoot()
        {
            root = new TreeViewItem(-1, -1);

            if(GpmCacheStorage.GetCacheCount() > 0)
            {
                foreach (CacheInfo cacheInfo in CacheStorageInternal.GetCacheList())
                {
                    CacheStorageTreeItem item = new CacheStorageTreeItem(cacheInfo);

                    root.AddChild(item);
                }
            }
            else
            {
                root.AddChild(new TreeViewItem(0, 0, EMPTY));
            }
            

            return root;
        }

        public void SetDirty()
        {
            bDirty = true;
        }

        internal void Update()
        {
            if (bDirty == true)
            {
                Reload();
                bDirty = false;
            }
        }

        public CacheInfo GetSelectedCache()
        {
            foreach(int id in GetSelection())
            {
                if(FindItem(id, rootItem) is CacheStorageTreeItem item)
                {
                    return item.cacheInfo;
                }
            }

            return null;
        }
        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }
        protected override void RowGUI(RowGUIArgs args)
        {
            if (args.item is IBaseTreeItem treeItem)
            {
                for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
                {
                    CellGUI(args.GetCellRect(i), treeItem, args.GetColumn(i), ref args);
                }
            }
            else
            {
                GUI.Label(args.GetCellRect(0), args.item.displayName);
            }
        }
        
        private void CellGUI(Rect cellRect, IBaseTreeItem treeItem, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            if (column == 0)
            {
                float indent = GetContentIndent(args.item) + extraSpaceBeforeIconAndLabel;
                cellRect.xMin += indent;
            }

            treeItem.CellGUI(cellRect, column);
        }

        public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
        {
            return new MultiColumnHeaderState(GetColumns());
        }

        private static MultiColumnHeaderState.Column GetColumn(ColumnId id)
        {
            switch (id)
            {
                case ColumnId.NAME:
                    {
                        return new MultiColumnHeaderState.Column()
                        {
                            headerContent = new GUIContent(ColumnName.NAME),
                            minWidth = 40,
                            width = 150,
                            maxWidth = 600,
                            headerTextAlignment = TextAlignment.Left,
                            canSort = true,
                            autoResize = false,
                        };
                    }
                case ColumnId.URL:
                    {
                        return new MultiColumnHeaderState.Column()
                        {
                            headerContent = new GUIContent(ColumnName.URL),
                            minWidth = 100,
                            width = 400,
                            maxWidth = 1200,
                            headerTextAlignment = TextAlignment.Left,
                            canSort = true,
                            autoResize = true,
                        };
                    }
                case ColumnId.SIZE:
                    {
                        return new MultiColumnHeaderState.Column()
                        {
                            headerContent = new GUIContent(ColumnName.SIZE),
                            minWidth = 40,
                            width = 80,
                            maxWidth = 160,
                            canSort = true,
                            autoResize = false,
                        };
                    }
                case ColumnId.EXFIRES:
                    {
                        return new MultiColumnHeaderState.Column()
                        {
                            headerContent = new GUIContent(ColumnName.EXFIRES),
                            minWidth = 60,
                            width = 60,
                            maxWidth = 70,
                            headerTextAlignment = TextAlignment.Left,
                            canSort = false,
                            autoResize = false,
                        };
                    }
                case ColumnId.REMAIN:
                    {
                        return new MultiColumnHeaderState.Column()
                        {
                            headerContent = new GUIContent(ColumnName.REMAIN),
                            minWidth = 60,
                            width = 60,
                            maxWidth = 70,
                            headerTextAlignment = TextAlignment.Left,
                            canSort = false,
                            autoResize = false,
                        };
                    }
                case ColumnId.REMOVE:
                    {
                        return new MultiColumnHeaderState.Column()
                        {
                            headerContent = new GUIContent(ColumnName.REMOVE),
                            minWidth = 60,
                            width = 60,
                            maxWidth = 70,
                            headerTextAlignment = TextAlignment.Left,
                            canSort = false,
                            autoResize = false,
                        };
                    }
                default:
                    {
                        return new MultiColumnHeaderState.Column();
                    }
            }

        }
        private static MultiColumnHeaderState.Column[] GetColumns()
        {
            var columnArr = System.Enum.GetValues(typeof(ColumnId));
            MultiColumnHeaderState.Column[] retVal = new MultiColumnHeaderState.Column[columnArr.Length];

            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = GetColumn((ColumnId)columnArr.GetValue(i));
            }

            return retVal;
        }
    }
}