using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class TorchAction : PuzzleObjectBase, ISandable
    {
        [SerializeField] private ParticleSystem fireParticles;

        private void Start()
        {
            if ( fireParticles == null )
            {
                Debug.LogError(name + " is missing Fire ParticleSystem");
            }
        }

        public void OnSandAbility()
        {
            fireParticles.Stop(false);

            if ( GetPuzzleManager != null )
            {
                GetPuzzleManager.SetRemainingPuzzleObjectCount = -1;
            }
        }

        public void OnSparkAbility()
        {
            Debug.Log("The player rekindled fire: " + name);

            fireParticles.Stop(true);

            if ( GetPuzzleManager != null )
            {
                GetPuzzleManager.SetRemainingPuzzleObjectCount = +1;
            }
        }
    }
}
