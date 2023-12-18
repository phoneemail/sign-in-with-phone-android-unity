namespace Gpm.LogViewer.Internal
{
    using UnityEngine;
    using UnityEngine.UI;

    public class WaitUi : MonoBehaviour
    {
        public GameObject       bg          = null;
        public GameObject       icon        = null;
        public Text             message     = null;

        private static WaitUi   instance    = null;

        public static WaitUi Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<WaitUi>();
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

            ShowUi(false);
        }

        public void ShowUi(bool show)
        {
            if (show == true)
            {
                SetMessage(string.Empty);
            }

            bg.SetActive(show);
            icon.SetActive(show);
            message.gameObject.SetActive(show);
        }

        public void SetMessage(string message)
        {
            this.message.text = message;
        }
    }
}
