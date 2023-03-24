using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class TorchInteraction : PuzzleObjectBase, ISandable
    {
        [SerializeField] private ParticleSystem fireParticles;
        [SerializeField] private PuzzleObject torchPuzzleParent;

        private void Start()
        {
            if ( fireParticles == null )
            {
                Debug.LogError(name + " is missing Fire ParticleSystem");
            }

            if ( torchPuzzleParent == null )
            {
                Debug.LogError(name + " is missing TorchPuzzleParent");
            }
        }

        public void OnSandAttack()
        {
            fireParticles.Stop(false);
            torchPuzzleParent.SetRemainingPuzzleObjectCount = -1;
        }
    }
}
