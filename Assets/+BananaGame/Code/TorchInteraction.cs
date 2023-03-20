using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class TorchInteraction : MonoBehaviour, ISandable
    {
        [SerializeField] private ParticleSystem fireParticles;

        private void Start()
        {
            if ( fireParticles == null )
            {
                Debug.LogError(name + " is missing Fire ParticleSystem");
            }
        }

        public void OnSandAttack()
        {
            var main = fireParticles.main;
            main.loop = false;
        }
    }
}
