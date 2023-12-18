using UnityEngine;

using Gpm.Common.Multilanguage;

namespace Gpm.Ui
{
    public static class EditorConstants
    {
        public const string SERVICE_NAME = "Ui";

        public const string LANGUAGE_FILE_PATH = "GPM/Ui/Editor/XML/Language.xml";

        public const string PATH_ASSET = "Assets/";

        public const string PATH_GPM = "GPM/";
        private const string LANGUAGE_CODE_KEY = "Gpm.ui.language";

        public static string LastLanguageName
        {
            get
            {
                return PlayerPrefs.GetString(LANGUAGE_CODE_KEY);
            }
            set
            {
                PlayerPrefs.SetString(LANGUAGE_CODE_KEY, value);
            }
        }
    }
}
