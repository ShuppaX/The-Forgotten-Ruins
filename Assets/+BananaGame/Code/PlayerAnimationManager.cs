using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        private PlayerStateManager playerStateManager = null;

        private void OnEnable()
        {
            TrySubscribing();
        }

        private void OnDisable()
        {
            if ( playerStateManager != null )
            {
                PlayerStateManager.Instance.stateChanged -= SetAnimation;
            }
        }

        private void Start()
        {
            Setup();
            TrySubscribing();
        }

        private void Setup()
        {
            playerStateManager = PlayerStateManager.Instance;
            if ( playerStateManager == null )
            {
                Debug.LogError(this + " is not getting reference to the PlayerStateManager!");
            }
        }

        private void TrySubscribing()
        {
            if ( playerStateManager != null )
            {
                PlayerStateManager.Instance.stateChanged += SetAnimation;
            }
        }

        private void SetAnimation()
        {
            Debug.Log("State changed. Set new animation");
        }
    }
}
