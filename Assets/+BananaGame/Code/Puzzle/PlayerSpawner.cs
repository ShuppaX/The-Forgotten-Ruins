using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class PlayerSpawner : MonoBehaviour
    {
        private bool isStartingPoint;

        public bool IsStartingPoint
        {
            set
            {
                isStartingPoint = value;
            }
        }

        private void Start()
        {
            if ( isStartingPoint )
            {
                TeleportPlayer();
            }
        }

        public void TeleportPlayer()
        {
            if ( PlayerBase.Instance != null )
            {
                PlayerBase.Instance.transform.position = transform.position;
                PlayerBase.Instance.transform.rotation = transform.rotation;
            }
            else
            {
                Debug.LogWarning("The player prefab not found. " + gameObject + " couldn't relocate the player prefab.");
            }
        }
    }
}
