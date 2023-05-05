using UnityEngine;
using NaughtyAttributes;

namespace BananaSoup.Utilities
{
    public class SetObjectsParent : MonoBehaviour
    {
        [SerializeField, Tooltip("If true, this GameObject will not have a parent in hierarchy.")]
        private bool isParentRoot = false;

        [SerializeField, HideIf("isParentRoot")]
        private Transform parent;

        // Start is called before the first frame update
        private void Start()
        {
            if ( isParentRoot )
            {
                gameObject.transform.parent = null;
            }
            else
            {
                gameObject.transform.parent = parent;
            }
        }
    }
}
