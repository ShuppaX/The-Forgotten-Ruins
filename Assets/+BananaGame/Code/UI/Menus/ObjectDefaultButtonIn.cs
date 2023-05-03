using BananaSoup.UI.Menus;
using UnityEngine;

namespace BananaSoup.UI.Menus
{
    public class ObjectDefaultButtonIn : MonoBehaviour
    {
        [SerializeField]
        private MenuManager.DefaultButton defaultButton;

        public MenuManager.DefaultButton DefaultButton
        {
            get => defaultButton;
        }
    }
}
