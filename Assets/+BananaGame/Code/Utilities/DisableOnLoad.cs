using UnityEngine;

namespace BananaSoup.Utilities
{
    public class DisableOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
