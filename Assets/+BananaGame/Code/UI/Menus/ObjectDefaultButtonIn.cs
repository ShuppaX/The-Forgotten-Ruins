using UnityEngine;

namespace BananaSoup.UI.Menus
{
    public class ObjectDefaultButtonIn : MonoBehaviour
    {
        [SerializeField]
        private MenuButtonHandler.DefaultButton defaultButton;

        public MenuButtonHandler.DefaultButton DefaultButton
        {
            get => defaultButton;
        }
    }
}
