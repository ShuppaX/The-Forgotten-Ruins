using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BananaSoup
{
    public class DropdownAutoscroll : MonoBehaviour, ISelectHandler
    {
        private ScrollRect scrollRect;
        private float scrollPosition = 1;

        // Start is called before the first frame update
        private void Start()
        {
            scrollRect = GetComponentInParent<ScrollRect>(true);

            int childCount = scrollRect.content.transform.childCount - 1;
            int childIndex = transform.GetSiblingIndex();

            childIndex = childIndex < ((float)childCount / 4f) ? childIndex - 1 : childIndex;

            scrollPosition = 1 - ((float)childIndex / childCount);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if ( scrollRect )
            {
                scrollRect.verticalScrollbar.value = scrollPosition;
            }
        }
    }
}
