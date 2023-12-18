namespace Gpm.Ui.Sample
{
    using UnityEngine;

    public class DragSort : MonoBehaviour
    {
        public void SortChange()
        {
            transform.SetAsLastSibling();
        }
    }
}