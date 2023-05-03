using BananaSoup.DamageSystem;
using System;
using UnityEngine;

namespace BananaSoup
{
    public class RagdollOnDeath : MonoBehaviour
    {
        [SerializeField, Tooltip("The players model's parent object. (Fennec_T)")]
        private GameObject ragdollRoot;

        [Space]

        [SerializeField, Tooltip("Check this to activate ragdoll on start.")]
        private bool ragdollOnStart = false;

        private Animator playerAnimator;

        // References to ragdoll rbs, charactersjoints and colliders.
        [NonSerialized]
        public Rigidbody[] ragdollRBs;
        private CharacterJoint[] ragdollJoints;
        private Collider[] ragdollColliders;

        private GameObject fennecSword;

        private void Awake()
        {
            ragdollRBs = ragdollRoot.GetComponentsInChildren<Rigidbody>();
            ragdollJoints = ragdollRoot.GetComponentsInChildren<CharacterJoint>();
            ragdollColliders = ragdollRoot.GetComponentsInChildren<Collider>();

            fennecSword = GetComponentInChildren<PlayerSword>().gameObject;
            playerAnimator = GetComponent<Animator>();
            if ( playerAnimator == null )
            {
                Debug.LogError($"RagdollOnDeath couldn't find a Animator component on {name}!");
            }

            if ( ragdollOnStart )
            {
                EnableRagdoll();
            }
            else
            {
                EnableAnimator();
            }
        }

        /// <summary>
        /// Method called on death to enable ragdoll for the player character.
        /// </summary>
        public void EnableRagdoll()
        {
            playerAnimator.enabled = false;
            fennecSword.transform.parent = null;
            Rigidbody swordRB = fennecSword.AddComponent<Rigidbody>();
            CapsuleCollider swordCapsuleCollider = fennecSword.GetComponent<CapsuleCollider>();
            BoxCollider swordBoxCollider = fennecSword.GetComponent<BoxCollider>();
            swordBoxCollider.enabled = true;
            swordCapsuleCollider.isTrigger = false;
            swordRB.useGravity = true;
            fennecSword.layer = 14;
            ToggleRagdollComponents(true);
        }

        /// <summary>
        /// Method that can be called to enable animator and disable ragdoll for the
        /// player character.
        /// </summary>
        public void EnableAnimator()
        {
            ToggleRagdollComponents(false);
            playerAnimator.enabled = true;
        }

        /// <summary>
        /// Method used to toggle all ragdoll components (CharacterJoints, Colliders
        /// and Rigidbodies) to given bool parameter.
        /// </summary>
        /// <param name="value">True to set them active, false to deactivate.</param>
        private void ToggleRagdollComponents(bool value)
        {
            foreach ( CharacterJoint joint in ragdollJoints )
            {
                joint.enableCollision = value;
            }

            foreach ( Collider collider in ragdollColliders )
            {
                collider.enabled = value;
            }

            foreach ( Rigidbody rb in ragdollRBs )
            {
                rb.detectCollisions = value;
                rb.useGravity = value;
            }
        }
    }
}
