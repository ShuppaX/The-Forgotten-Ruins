using BananaSoup.InteractSystem;
using BananaSoup.Managers;
using UnityEngine;

namespace BananaSoup
{
    public class DropInteractableOnTrigger : MonoBehaviour
    {
        private PlayerStateManager psm = null;

        private const PlayerStateManager.PlayerState interactingIdle = PlayerStateManager.PlayerState.InteractingIdle;
        private const PlayerStateManager.PlayerState interactingMove = PlayerStateManager.PlayerState.InteractingMove;

        private void Start()
        {
            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError($"{this.GetType()} couldn't find an instance of PlayerStateManager!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.gameObject.GetComponent<PlayerStateManager>() != null )
            {
                if ( psm.CurrentPlayerState == interactingIdle
                    || psm.CurrentPlayerState == interactingMove )
                {
                    other.gameObject.GetComponent<AbilityInteract>().DropInteractable();
                }
            }
        }
    }
}
