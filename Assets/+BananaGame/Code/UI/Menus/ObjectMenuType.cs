using UnityEngine;

namespace BananaSoup.UI.Menus
{
    public class ObjectMenuType : MonoBehaviour
    {
        [SerializeField]
        private MenuManager.MenuType menuType;

        public MenuManager.MenuType MenuType
        {
            get => menuType;
        }
    }
}
