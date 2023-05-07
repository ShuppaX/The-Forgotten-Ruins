using BananaSoup.InteractSystem;
using UnityEngine;

namespace BananaSoup
{
    public class ToggleFiresOnTrigger : MonoBehaviour
    {
        [SerializeField, Tooltip("If true, lit the array of the fires when the Player enters on the trigger zone. " +
                              "If false, extinguishes the array of the fires when the Player enters on the trigger zone.")]
        private bool enableOnTrigger = true;
        [SerializeField] private FireToggler[] fires;
        private bool isTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if ( other.GetComponent<PlayerBase>() != null && !isTriggered )
            {
                isTriggered = true;

                foreach ( FireToggler fire in fires )
                {
                    if ( enableOnTrigger )
                    {
                        fire.LitTorch();
                    }
                    else
                    {
                        fire.Extinguish();
                    }
                }
            }
        }
    }
}
