﻿namespace Gpm.LogViewer.Internal
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TabView : ViewBase
    {
        public enum TabIndex
        {
            CONSOLE = 0,
            FUNCTION,
            SYSTEM
        }

        [Serializable]
        public class Tab
        {
            public Image tabBg = null;
        }

        
        public  Tab[]       tab             = null;

        private TabIndex    currentIndex    = TabIndex.CONSOLE;
        private Color       bgSelectColor   = new Color(0.2745f, 0.2745f, 0.2745f);
        private Color       bgNormalColor   = new Color(0.16f, 0.16f, 0.16f);

        public override void InitializeView()
        {
            tab[(int)TabIndex.CONSOLE].tabBg.color = bgSelectColor;
            tab[(int)TabIndex.FUNCTION].tabBg.color = bgNormalColor;
            tab[(int)TabIndex.SYSTEM].tabBg.color = bgNormalColor;
        }

        public void SelectTab(TabIndex tabIndex)
        {
            if (tabIndex == currentIndex)
            {
                return;
            }

            tab[(int)currentIndex].tabBg.color = bgNormalColor;

            tab[(int)tabIndex].tabBg.color = bgSelectColor;

            currentIndex = tabIndex;
        }
    }

}