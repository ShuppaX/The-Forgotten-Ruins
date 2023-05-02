using BananaSoup.DamageSystem;
using UnityEngine;

namespace BananaSoup
{
    public class RagdollOnDeath : MonoBehaviour
    {
        [SerializeField, Tooltip("The players ragdoll model's parent object.")]
        private GameObject ragdollModel;

        [SerializeField, Tooltip("The players main model's parent object.")]
        private GameObject mainModel;

        private GameObject fennecSword;

        private void Start()
        {
            fennecSword = GetComponentInChildren<PlayerSword>().gameObject;
        }

        /// <summary>
        /// Method called when player dies in PlayerHealth.cs
        /// Method deactivates players mainModel and sword if they are active and then
        /// activates the ragdollModel of the player.
        /// </summary>
        public void Ragdoll()
        {
            if ( fennecSword.activeSelf )
            {
                fennecSword.SetActive(false);
            }

            if ( mainModel.activeSelf )
            {
                mainModel.SetActive(false);
            }

            if ( !ragdollModel.activeSelf )
            {
                ragdollModel.SetActive(true);
            }
        }
    }
}
