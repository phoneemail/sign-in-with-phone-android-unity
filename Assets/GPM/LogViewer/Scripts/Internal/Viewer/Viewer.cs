namespace Gpm.LogViewer.Internal
{
    using UnityEngine;
    using UnityEngine.UI;

    public class Viewer : MonoBehaviour
    {
        public GameObject           pannel                  = null;
        public TabView              tabView                 = null;
        public ConsoleView          consoleView             = null;
        public FunctionView         functionView            = null;
        public SystemView           systemView              = null;
                
        private CanvasGroup         canvasGroup             = null;
        private CanvasScaler        canvasScaler            = null;
        private ScreenOrientation   orientation             = ScreenOrientation.LandscapeLeft;
        private Vector2             portraitResolution      = new Vector2(800.0f, 1280.0f);
        private Vector2             landscapeResolution     = new Vector2(1200.0f, 800.0f);
        private int                 screenWidth             = 0;
        private int                 screenHeight            = 0;

        private bool                gestureEnable           = false;
        private bool                isTouchBegin            = false;
        private float               touchTime               = 0f;

        private void Awake()
        {
            canvasGroup     = GetComponent<CanvasGroup>();
            canvasScaler    = GetComponent<CanvasScaler>();

            screenWidth     = Screen.width;
            screenHeight    = Screen.height;
        }

        public void Initialize()
        {
            tabView.InitializeView();
            consoleView.InitializeView();
            functionView.InitializeView();
            systemView.InitializeView();

            SelectConsole();

            Show(false);

            Log.Instance.AddLogNotificationCallback(OnLogNotification);
        }

        public void Close()
        {
            tabView.CloseView();
            consoleView.CloseView();
            functionView.CloseView();
            systemView.CloseView();

            Show(false);
        }

        public void SelectConsole()
        {
            tabView.SelectTab(TabView.TabIndex.CONSOLE);
            consoleView.gameObject.SetActive(true);
            functionView.gameObject.SetActive(false);
            systemView.gameObject.SetActive(false);
        }

        public void SelectFunction()
        {
            tabView.SelectTab(TabView.TabIndex.FUNCTION);
            consoleView.gameObject.SetActive(false);
            functionView.gameObject.SetActive(true);
            systemView.gameObject.SetActive(false);
        }

        public void SelectSystem()
        {
            tabView.SelectTab(TabView.TabIndex.SYSTEM);
            consoleView.gameObject.SetActive(false);
            functionView.gameObject.SetActive(false);
            systemView.gameObject.SetActive(true);

            systemView.Refresh();
        }

        public void SetGestureEnable(bool enable)
        {
            gestureEnable = enable;
        }

        private void Update()
        {
            if (pannel.activeSelf == false && gestureEnable == true)
            {                
                CheckGesture();
                CheckKey();
            }
            else
            {
                UpdateOrientation();
                UpdateResolution();
            }
        }

        private void UpdateResolution()
        {
            int currentWidth    = Screen.width;
            int currentHeight   = Screen.height;

            if (screenWidth != currentWidth || screenHeight != currentHeight)
            {
                screenWidth     = currentWidth;
                screenHeight    = currentHeight;

                consoleView.UpdateResolution();
            }
        }

        private void UpdateOrientation()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            ScreenOrientation newOrientation = Screen.orientation;
#else
            ScreenOrientation newOrientation = Screen.width >= Screen.height ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait;
#endif

            if (orientation != newOrientation)
            {
                orientation = newOrientation;

                if (orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown)
                {
                    canvasScaler.referenceResolution = portraitResolution;
                }
                else
                {
                    canvasScaler.referenceResolution = landscapeResolution;
                }

                consoleView.SetOrientation(orientation);
                systemView.SetOrientation(orientation);
            }            
        }

        private void CheckGesture()
        {
            if(Input.touchCount == ViewerConst.GESTURE_TOUCH_COUNT)
            {
                if(isTouchBegin == false)
                {
                    isTouchBegin = true;
                    touchTime = Time.unscaledTime;
                }
                else
                {
                    if(Time.unscaledTime - touchTime >= ViewerConst.GESTURE_TOUCH_TIME_INTERVAL)
                    {
                        isTouchBegin = false;
                        touchTime = 0;
                        Show(true);
                    }
                }
            }
            else
            {
                isTouchBegin = false;
                touchTime = 0;
            }
        }

        private void CheckKey()
        {
            if(Input.GetKeyDown(KeyCode.BackQuote) == true)
            {
                Show(true);
            }
        }

        public void Show()
        {
            pannel.SetActive(true);
        }

        private void Show(bool show)
        {
            pannel.SetActive(show);
        }

        public void OnAlphaValueChanged(float value)
        {
            canvasGroup.alpha = value;
        }

        private void OnLogNotification(Log.LogData data)
        {
            if (data.logType == LogType.Exception || data.logType == LogType.Assert)
            {
                Show(true);
                SelectConsole();
                consoleView.ListMoveToBottom();
            }
        }
    }
}
