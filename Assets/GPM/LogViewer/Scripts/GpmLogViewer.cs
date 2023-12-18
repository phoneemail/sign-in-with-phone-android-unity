namespace Gpm.LogViewer
{
    using Gpm.LogViewer.Internal;
    using System;
    using UnityEngine;

    public class GpmLogViewer : MonoBehaviour
    {
        public const string VERSION         = "2.0.3";

        [Header("Gesture")]
        [Space]
        public bool         gestureEnable   = true;

        [Space(5)]

        [Header("Mail")]
        [Space]
        public MailData     mailSetting     = null;
        
        private Viewer      viewer          = null;

        private static GpmLogViewer instance = null;

        public static GpmLogViewer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GpmLogViewer>();

                    if (instance == null)
                    {
                        instance = new GameObject("GpmLogViewer").AddComponent<GpmLogViewer>();
                    }
                }
                
                return instance;
            }
        }


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
            DontDestroyOnLoad(gameObject);            
        }

        private void OnDestroy()
        {
            Clear();
        }

        public void AddCheatKeyCallback(Action<string> callback)
        {
            Function.Instance.AddCheatKeyCallback(callback);
        }

        public void AddCommand(object instance, string methodName)
        {
            Function.Instance.AddCommand(instance, methodName);
        }
        
        public void Show()
        {
            viewer.Show();
        }

        public string MakeLogWithCategory(string message, string category)
        {
            return Log.Instance.MakeLogMessageWithCategory(message, category);
        }

        private void Initialize()
        {
            Function.Instance.Initialize();

            SetMailData();

            if (viewer == null)
            {
                viewer = transform.GetChild(0).GetComponent<Viewer>();
            }

            viewer.Initialize();
            SetGestureEnable();
        }

        private void Clear()
        {
            Function.Instance.Clear();
        }

        private void SetGestureEnable()
        {
            viewer.SetGestureEnable(gestureEnable);
        }

        private void SetMailData()
        {
            Function.Instance.SetMailData(mailSetting);
        }
    }
}
