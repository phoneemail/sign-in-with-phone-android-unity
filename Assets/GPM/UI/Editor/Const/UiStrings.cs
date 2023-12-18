using Gpm.Common.Multilanguage;
using UnityEditor;

namespace Gpm.Ui
{
    public static class Strings
    {
        public const string KEY_NO_ISSUE = "KEY_NO_ISSUE";

        public const string KEY_CHANGE_LANGUAGE_ERROR_MESSAGE = "KEY_CHANGE_LANGUAGE_ERROR_MESSAGE";

        public const string KEY_TAB_ERROR_MESSAGE = "KEY_TAB_ERROR_MESSAGE";

        public const string KEY_CONTROL_ADDTAB_ONLYONE_MESSAGE = "KEY_CONTROL_ADDTAB_ONLYONE_MESSAGE";
        public const string KEY_CONTROL_ADDTAB_CHECK_MESSAGE = "KEY_CONTROL_ADDTAB_CHECK_MESSAGE";

        public const string KEY_TAB_RELEASE_CONTROL_CHECK_MESSAGE = "KEY_TAB_RELEASE_CONTROL_CHECK_MESSAGE";
        public const string KEY_TAB_CHANGE_CONTROL_CHECK_MESSAGE = "KEY_TAB_CHANGE_CONTROL_CHECK_MESSAGE";
        

        public const string KEY_OK = "KEY_OK";
        public const string KEY_CANCEL = "KEY_CANCEL";

        public const string KEY_YES = "KEY_YES";
        public const string KEY_NO = "KEY_NO";

        public static bool MessageBox(string title, string message)
        {
            UiEditorLanguage.Load();

            return EditorUtility.DisplayDialog(GetString(title), GetString(message), GetString(KEY_OK));
        }

        public static bool MessageCheckBox(string title, string message)
        {
            UiEditorLanguage.Load();

            return EditorUtility.DisplayDialog(GetString(title), GetString(message), GetString(KEY_YES), GetString(KEY_NO));
        }

        public static string GetString(string key)
        {
            return GpmMultilanguage.GetString(EditorConstants.SERVICE_NAME, key);
        }
    }
}