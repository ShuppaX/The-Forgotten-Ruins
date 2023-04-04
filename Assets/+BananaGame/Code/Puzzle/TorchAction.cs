using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class TorchAction : PuzzleObjectBase, IThrowReactable
    {
        [SerializeField] private ParticleSystem fireParticles;
        [SerializeField] private bool isTorchAlreadyBurning = true;
        private bool isBurning = true;

        public bool IsTorchAlreadyBurning => isTorchAlreadyBurning;

        private void Start()
        {
            if ( fireParticles == null )
            {
                Debug.LogError(name + " is missing Fire ParticleSystem");
            }

            if ( isTorchAlreadyBurning )
            {
                LitTorch();
            }
            else
            {
                Extinguish();
            }
        }

        public int GetCompletitionValueAtStart()
        {
            if ( IsTorchAlreadyBurning )
            {
                if ( !IsSolutionReversed )
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if ( !IsSolutionReversed )
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        public void Extinguish()
        {
            fireParticles.Stop();
            isBurning = false;
        }

        public void LitTorch()
        {
            fireParticles.Play();
            isBurning = true;
        }

        public void OnThrowAbility(ParticleProjectile.Type projectileType)
        {
            switch ( projectileType )
            {
                case ParticleProjectile.Type.Sand:
                {
                    if ( !isBurning )
                    {
                        return;
                    }

                    Extinguish();

                    if ( !IsSolutionReversed )
                    {
                        UpdateRemainingPuzzle(-1);
                    }
                    else
                    {
                        UpdateRemainingPuzzle(1);
                    }

                    break;
                }
                case ParticleProjectile.Type.Spark:
                {
                    if ( isBurning )
                    {
                        return;
                    }

                    LitTorch();

                    if ( !IsSolutionReversed )
                    {
                        UpdateRemainingPuzzle(1);
                    }
                    else
                    {
                        UpdateRemainingPuzzle(-1);
                    }

                    break;
                }
            }
        }

        private void UpdateRemainingPuzzle(int value)
        {
            if ( GetPuzzleManager != null )
            {
                GetPuzzleManager.SetRemainingPuzzleObjectCount = value;
            }
        }
    }
}
