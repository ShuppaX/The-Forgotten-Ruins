using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class PlayerSpawner : MonoBehaviour
    {
        private void Start()
        {
            if ( PlayerBase.Instance != null )
            {
                PlayerBase.Instance.gameObject.transform.position = transform.position;
            }
            else
            {
                Debug.LogWarning("Player prefab not found. " + gameObject + " couldn't relocate Player prefab.");
            }
        }
    }
}
