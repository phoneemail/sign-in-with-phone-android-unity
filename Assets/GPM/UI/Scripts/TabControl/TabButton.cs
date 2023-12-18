using UnityEngine.EventSystems;

namespace Gpm.Ui
{
    public class TabButton : Tab, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }

    }
}
