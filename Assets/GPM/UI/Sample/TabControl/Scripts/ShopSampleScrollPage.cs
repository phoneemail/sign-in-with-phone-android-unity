using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gpm.Ui.Sample
{
    public class ShopSampleScrollPage : MonoBehaviour
    {
        public InfiniteScroll scroll;

        public void OnNotify(Tab tab)
        {
            if (tab.IsSelected() == true)
            {
                ItemGroupData data = (ItemGroupData)tab.GetData();

                scroll.ClearData();

                for (int index = 0; index < data.itemList.Count; index++)
                {
                    AddData(data.itemList[index]);
                }
                scroll.MoveToFirstData();

                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void AddData(ItemData itemData)
        {
            ShopSampleScrollItemData scrollData = new ShopSampleScrollItemData();
            scrollData.name = itemData.name;
            scrollData.description = itemData.description;

            scrollData.buttonEnabled = true;
            scrollData.buttonText = itemData.buttonText;
            scrollData.buttonEvent = itemData.buttonEvent;

            scroll.InsertData(scrollData);
        }

        public void RemoveData(int index)
        {
            scroll.RemoveData(index);
        }
    }
}