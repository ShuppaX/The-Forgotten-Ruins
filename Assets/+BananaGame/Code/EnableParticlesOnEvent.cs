using BananaSoup.Managers;
using UnityEngine;

namespace BananaSoup.Effects
{
    public class EnableParticlesOnEvent : MonoBehaviour
    {
        [SerializeField] private PlayerStateManager.PlayerState activationState;
        private ParticleSystem particleEffect;
        private PlayerStateManager psm;

        private void OnDisable()
        {
            psm.stateChanged -= ToggleParticleSystem;
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

            psm.stateChanged += ToggleParticleSystem;
        }

        private void ToggleParticleSystem()
        {
            if ( psm.CurrentPlayerState == activationState )
            {
                particleEffect.Play();
            }
            else
            {
                particleEffect.Stop();
            }
        }
    }
}
