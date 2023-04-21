using UnityEngine;
using UnityEngine.UI;

namespace BananaSoup.UI
{
    public class UIDashManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The parent gameObject (Dash_Display) with the" +
            "Image component.")]
        private GameObject dashDisplay;

        private Image dashImage;

        private PlayerBase playerBase;

        void Start()
        {
            dashImage = dashDisplay.GetComponent<Image>();
            if ( dashImage == null )
            {
                Debug.LogError($"No component of {typeof(Component).Name} couldn't" +
                    $"be found on the dashDisplay for " + gameObject.name + "!");
            }

            playerBase = PlayerBase.Instance;
            if ( playerBase == null )
            {
                Debug.LogError(gameObject.name + " couldn't find an instance of" +
                    "PlayerBase!");
            }

            if ( !playerBase.IsDashLooted )
            {
                dashDisplay.SetActive(false);
            }
        }
    }
}
