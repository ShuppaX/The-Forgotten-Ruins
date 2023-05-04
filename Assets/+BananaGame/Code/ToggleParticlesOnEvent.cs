using BananaSoup.Managers;
using UnityEngine;

namespace BananaSoup.Effects
{
    public class ToggleParticlesOnEvent : MonoBehaviour
    {
        [SerializeField] private PlayerStateManager.PlayerState activationState;
        private PlayerStateManager.PlayerState deadState = PlayerStateManager.PlayerState.Dead;
        private ParticleSystem particleEffect;
        private PlayerStateManager psm;

        private void OnDisable()
        {
            PlayerStateManager.StateChanged -= ToggleParticleSystem;
        }

        private void Start()
        {
            particleEffect = GetComponent<ParticleSystem>();
            if ( particleEffect == null )
            {
                Debug.LogError(name + "'s is missing a reference to a Particle System!");
            }

            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError(name + "'s is missing a reference to the PlayerStateManager");
            }

            PlayerStateManager.StateChanged += ToggleParticleSystem;
        }

        private void ToggleParticleSystem()
        {
            if ( psm.CurrentPlayerState == activationState )
            {
                particleEffect.Play();
            }
            else if ( psm.CurrentPlayerState == deadState )
            {
                particleEffect.Stop();
            }
            else
            {
                particleEffect.Stop();
            }
        }
    }
}
