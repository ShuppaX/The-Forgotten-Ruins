using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BananaSoup.Traps
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private LayerMask triggersWith;
        [SerializeField] private bool isRepeatable = true;
        [SerializeField, ShowIf("isRepeatable"), Tooltip("A time when trap can re-activate again.")]
        private float cooldown = 1.5f;
        private bool isTrapActivated;

        public bool IsRepeatable => isRepeatable;
        public float GetCooldown => cooldown;
        public bool IsTrapActivated
        {
            get => isTrapActivated;
            set => isTrapActivated = value;
        }

        private void FixedUpdate()
        {
            if ( isTrapActivated )
            {
                ActivateTrap();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( isTrapActivated )
            {
                return;
            }

            if ( ((1 << other.gameObject.layer) & triggersWith) != 0 )
            {
                isTrapActivated = true;
            }
        }

        public virtual void ActivateTrap()
        {
        }
    }
}
