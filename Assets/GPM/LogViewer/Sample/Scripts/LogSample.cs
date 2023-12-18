namespace Gpm.LogViewer.Sample
{
    using Gpm.LogViewer.Internal;
    using System.Collections;
    using UnityEngine;

    public class LogSample : MonoBehaviour
    {
        private static LogSample instance = null;

        public static LogSample Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<LogSample>();

                    if (instance == null)
                    {
                        instance = new GameObject("LogSample").AddComponent<LogSample>();
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
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Function.Instance.AddCommand(this, "TestLog", new object[] { 2 });
            Function.Instance.AddCommand(this, "TestCommand", new object[] { "by command text" });
            Function.Instance.AddCommand(this, "TestCategory", new object[] { "category1", "category2" });
            Function.Instance.AddCommand(this, "SendExceptionLog");
            Function.Instance.AddCommand(this, "SendAssertLog");
            Function.Instance.AddCommand(this, "SendErrorLog");
            Function.Instance.AddCommand(this, "SendWarningLog");
            Function.Instance.AddCheatKeyCallback((cheatKey) =>
            {
                Debug.Log("Call cheat key callback with : " + cheatKey);
            });

            StartCoroutine(SendLog());
        }

        private IEnumerator SendLog()
        {
            int count = 0;
            while (true)
            {
                yield return new WaitForSeconds(5.0f);

                Debug.Log("***** Sample Send Log : " + count++.ToString());
            }
        }

        public void SendThreadLog()
        {
            for(int index=0; index < 10; index++)
            {
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ThreadProc));
                t.Start(index);
            }
        }

        public static void ThreadProc(object index)
        {
            throw new System.Exception("thread : " + index);
        }

        private void TestLog(int index)
        {
            Debug.Log("*TestLog() : " + index.ToString());
            Debug.Log("category1 log");
            
            Debug.Log(GpmLogViewer.Instance.MakeLogWithCategory("Test message with category", "TestCategory"));
            Debug.Log("$(category)TempCategory$(category");
            Debug.Log("$(category)TempCategory$(category)");
            Debug.Log("$(category)TempCategory$(");
            Debug.Log("$(category)TempCategory$(category) Test");
            
            Debug.Log(SystemInformation.Instance.ToString());
            
        }

        private void TestCommand(string text)
        {
            Debug.Log("TestCommand : " + text);
        }

        private void TestCategory(string category1, string category2)
        {
            Debug.Log(GpmLogViewer.Instance.MakeLogWithCategory("Log with category(" + category1 + ")", category1));
            Debug.Log(GpmLogViewer.Instance.MakeLogWithCategory("Log with category(" + category2 + ")", category2));

        }

        public void SendExceptionLog()
        {
            Debug.LogException(new System.Exception("Exception log"));
        }

        private void SendAssertLog()
        {
            Debug.LogAssertion("Assert log");
        }

        private void SendErrorLog()
        {
            Debug.LogError("Error log");
        }

        private void SendWarningLog()
        {
            Debug.LogWarning("Warning log");
        }
    }
}
