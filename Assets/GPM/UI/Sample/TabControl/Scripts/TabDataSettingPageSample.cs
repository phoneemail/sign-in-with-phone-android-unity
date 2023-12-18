namespace Gpm.Ui.Sample
{
    using UnityEngine;
    using UnityEngine.UI;

    public class TabDataSettingPageSample : MonoBehaviour
    {
        public Text text;
        public void DataSetting(Tab tab)
        {
            if (tab.IsSelected() == true)
            {
                TabDataSettingSampleData sampleData = (TabDataSettingSampleData)tab.GetData();

                text.text = sampleData.text;
            }
        }
    }
}