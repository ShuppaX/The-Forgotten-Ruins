using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class TorchAction : PuzzleObjectBase, IThrowReactable
    {
        [SerializeField] private ParticleSystem fireParticles;
        private bool isBurning = true;

        private void Start()
        {
            if ( fireParticles == null )
            {
                Debug.LogError(name + " is missing Fire ParticleSystem");
            }
        }

        public void OnThrowAbility(ParticleProjectile.Type projectileType)
        {
            switch ( projectileType )
            {
                case ParticleProjectile.Type.Sand:
                    if ( !isBurning )
                    {
                        return;
                    }

                    fireParticles.Stop();
                    isBurning = false;

                    if ( GetPuzzleManager != null )
                    {
                        GetPuzzleManager.SetRemainingPuzzleObjectCount = -1;
                    }

                    break;

                case ParticleProjectile.Type.Spark:
                    if ( isBurning )
                    {
                        return;
                    }

                    fireParticles.Play();
                    isBurning = true;

                    if ( GetPuzzleManager != null )
                    {
                        GetPuzzleManager.SetRemainingPuzzleObjectCount = +1;
                    }

                    break;
            }
        }
    }
}
