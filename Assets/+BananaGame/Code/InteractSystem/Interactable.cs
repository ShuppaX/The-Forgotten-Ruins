using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private InteractPoint[] interactionPoints;
        private bool isInteracting;

        public bool IsInteracting
        {
            get { return isInteracting; }
            set { isInteracting = value; }
        }

        internal virtual void Interact()
        {
            IsInteracting = true;
        }

        //public void InteractCompleted()
        //{
        //    IsInteracting = false;
        //}

        internal virtual void InteractCompleted()
        {
            IsInteracting = false;
        }

        public void OnValidate()
        {
            interactionPoints = GetComponentsInChildren<InteractPoint>();
        }

        /// <summary>
        /// Checks distance between the player and all InteractionPoints of the interacted Interactable.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The closest InteractPoint.</returns>
        public InteractPoint GetClosestInteractPointToPlayer(Vector3 position)
        {
            float closestDistance = float.PositiveInfinity;
            InteractPoint closest = null;

            foreach ( InteractPoint interactPoint in interactionPoints )
            {
                Vector3 toInteractPoint = interactPoint.Position - position;
                float distanceToInteractPoint = toInteractPoint.sqrMagnitude;

                if ( distanceToInteractPoint < closestDistance )
                {
                    closest = interactPoint;
                    closestDistance = distanceToInteractPoint;
                }
            }

            return closest;
        }
    }
}
