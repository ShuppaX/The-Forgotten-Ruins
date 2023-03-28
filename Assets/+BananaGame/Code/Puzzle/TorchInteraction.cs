using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class TorchInteraction : PuzzleObjectBase, ISandable
    {
        [SerializeField] private ParticleSystem fireParticles;

        private void Start()
        {
            if ( fireParticles == null )
            {
                Debug.LogError(name + " is missing Fire ParticleSystem");
            }

            if ( GetPuzzleManager == null )
            {
                Debug.LogError(name + " is missing TorchPuzzleParent");
            }
        }

        public void OnSandAttack()
        {
            fireParticles.Stop(false);
            GetPuzzleManager.SetRemainingPuzzleObjectCount = -1;
        }
    }
}
