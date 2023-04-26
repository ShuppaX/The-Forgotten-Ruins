using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    public class ToggleTorchesOnDisable : MonoBehaviour
    {
        [SerializeField, Tooltip("If true, this GameObject will lit the torches OnDisable that are in the array. " +
                              "I false, this GameObject will extinguish the torches OnDisable that are in the array.")]
        private bool enableTorches = true;

        [SerializeField] private TorchAction[] torches;

        private void Start()
        {
            if ( torches.Length <= 0 )
            {
                Debug.LogError(gameObject + "'s Torches[] is 0 and can't be!");
            }

            for ( int i = 0; i < torches.Length; i++ )
            {
                if ( torches[i] == null )
                {
                    Debug.LogError(gameObject + "'s Torches[" + i + "] is null and it can't be!");
                }
            }
        }

        private void OnDisable()
        {
            if ( enableTorches )
            {
                foreach ( var torch in torches )
                {
                    if ( torch != null )
                    {
                        torch.LitTorch();
                    }
                }
            }
            else
            {
                foreach ( var torch in torches )
                {
                    if ( torch != null )
                    {
                        torch.Extinguish();
                    }
                }
            }
        }
    }
}
