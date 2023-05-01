using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace BananaSoup.UI.Menus
{
    public class SwapActiveInputScheme : MonoBehaviour
    {
        private bool keyboardOnly = false;

        private InputUser user;

        [SerializeField, Tooltip("The InputActionAsset used in Player's PlayerInput component.")]
        private InputActionAsset inputActionAsset;

        // InputControlScheme variables used to store the specified schemes.
        private InputControlScheme keyboardAndMouseScheme;
        private InputControlScheme keyboardScheme;

        // Constant strings used to get specified InputControlScheme(s).
        private const string keyboardAndMouseSchemeName = "KeyboardAndMouse";
        private const string keyboardSchemeName = "Keyboard";

        private void Start()
        {
            user = InputUser.all[0];

            foreach ( InputControlScheme scheme in inputActionAsset.controlSchemes )
            {
                if ( scheme.name == keyboardAndMouseSchemeName )
                {
                    keyboardAndMouseScheme = scheme;
                }
                else if ( scheme.name == keyboardSchemeName )
                {
                    keyboardScheme = scheme;
                }
            }
        }

        public void ToggleControlScheme()
        {
            if ( !keyboardOnly )
            {
                SwitchToKeyboardOnly();
            }
            else if ( keyboardOnly )
            {
                SwitchToKeyboardAndMouse();
            }
        }

        private void SwitchToKeyboardOnly()
        {
            Debug.Log("Switching to keyboard only!");

            user.ActivateControlScheme(keyboardScheme);
            keyboardOnly = true;
        }

        private void SwitchToKeyboardAndMouse()
        {
            Debug.Log("Switching to keyboard and mouse!");

            user.ActivateControlScheme(keyboardAndMouseScheme);
            keyboardOnly = false;
        }
    }
}
