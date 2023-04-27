using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class ToggleTorchesOnTrigger : MonoBehaviour
    {
        [SerializeField] private bool enableTorch = false;
        [SerializeField] private TorchAction[] torches;
        private bool isTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if ( other.GetComponent<PlayerBase>() != null && !isTriggered )
            {
                isTriggered = true;

                foreach ( TorchAction torch in torches )
                {
                    if ( enableTorch )
                    {
                        torch.LitTorch();
                    }
                    else
                    {
                        torch.Extinguish();
                    }
                }
            }
        }
    }
}
