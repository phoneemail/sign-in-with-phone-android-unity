using UnityEngine;
using UnityEditor;

using Gpm.Common.Multilanguage;
using Gpm.Common.Log;

namespace Gpm.Ui
{
    static public class UiEditorLanguage
    {
        private const string EMPTY_LANGUAGES_VALUE = "-";
        private const int LANGUAGE_NOT_FOUND = -1;

        private static string[] languages;
        private static int selectedLanguageIndex;

        static UiEditorLanguage()
        {
            Load();
        }

        public static void Load()
        {
            if (IsLoadLanguage() == false)
            {
                LanguageLoad();
                InitializeLanguage();
            }
        }

        public static bool IsLoadLanguage()
        {
            return GpmMultilanguage.IsLoadService(EditorConstants.SERVICE_NAME);
        }

        public static void LanguageLoad()
        {
            if (IsLoadLanguage() == false)
            {
                GpmMultilanguage.Load(
                EditorConstants.SERVICE_NAME,
                EditorConstants.LANGUAGE_FILE_PATH,
                (result, resultMsg) =>
                {
                    if (result != MultilanguageResultCode.SUCCESS && result != MultilanguageResultCode.ALREADY_LOADED)
                    {
                        GpmLogger.Error(string.Format("Language load failed. (type= {0})", result), EditorConstants.SERVICE_NAME, typeof(UiEditorLanguage));
                        return;
                    }
                });
            }
        }

        public static void InitializeLanguage()
        {
            if (languages != null)
            {
                return;
            }

            if (IsLoadLanguage() == true)
            {
                languages = GpmMultilanguage.GetSupportLanguages(EditorConstants.SERVICE_NAME, true);
                if (languages != null)
                {
                    string lastLanguageName = EditorConstants.LastLanguageName;
                    if (string.IsNullOrEmpty(lastLanguageName) == false)
                    {
                        GpmMultilanguage.SelectLanguageByNativeName(
                            EditorConstants.SERVICE_NAME,
                            lastLanguageName,
                            (result, resultMessage) =>
                            {
                                if (result != MultilanguageResultCode.SUCCESS)
                                {
                                    GpmLogger.Warn(
                                        string.Format("{0} (Code= {1})", Strings.GetString(Strings.KEY_CHANGE_LANGUAGE_ERROR_MESSAGE), result),
                                        EditorConstants.SERVICE_NAME,
                                        typeof(UiEditorLanguage));
                                }
                            });
                    }

                    selectedLanguageIndex = GetSelectLanguageIndex(GpmMultilanguage.GetSelectLanguage(EditorConstants.SERVICE_NAME, true));
                }
                else
                {
                    languages = new[] { EMPTY_LANGUAGES_VALUE };
                    selectedLanguageIndex = 0;
                }
            }
        }

        internal static string GetSelectLanguageCode()
        {
            if (selectedLanguageIndex < 0 &&
                selectedLanguageIndex >= languages.Length)
            {
                return string.Empty;
            }

            return languages[selectedLanguageIndex];
        }

        private static int GetSelectLanguageIndex(string languageCode)
        {
            for (int i = 0; i < languages.Length; i++)
            {
                if (languages[i].Equals(languageCode) == true)
                {
                    return i;
                }
            }

            return LANGUAGE_NOT_FOUND;
        }
    }
}
