namespace Gpm.Ui.Sample
{
    using UnityEngine;
    using UnityEngine.UI;

    public class TabDataSettingButtonSample : MonoBehaviour
    {
        public Text title;
        public void DataSetting(ITabData data)
        {
            TabDataSettingSampleData sampleData = (TabDataSettingSampleData)data;

            title.text = sampleData.name;
        }
    }
}