using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.Ability
{
    public class SandProjectile : ParticleProjectile
    {
        private void Awake()
        {
            projectileType = Type.Sand;
        }
    }
}
