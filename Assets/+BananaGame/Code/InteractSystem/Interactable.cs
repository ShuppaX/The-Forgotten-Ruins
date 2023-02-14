using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private InteractPoint[] interactionPoints;

        //public Interactable()
        //{
        //    Debug.Log("Interactable - Constructor called");
        //}

        protected virtual void Start()
        {
            Debug.Log("Interactable - Start called");
        }

        protected internal abstract void Interact();

        public void OnValidate()
        {
            interactionPoints = GetComponentsInChildren<InteractPoint>();
        }

        public InteractPoint GetClosestInteractPointToPlayer(Vector3 position)
        {
            float closestDistance = float.PositiveInfinity;
            InteractPoint closest = null;

            foreach ( InteractPoint interactPoint in interactionPoints )
            {
                Vector3 toInteractPoint = interactPoint.Position - position;
                float distanceToInteractPoint = toInteractPoint.sqrMagnitude;

                if(distanceToInteractPoint < closestDistance )
                {
                    closest = interactPoint;
                    closestDistance = distanceToInteractPoint;
                }
            }

            return closest;
        }
    }
}
