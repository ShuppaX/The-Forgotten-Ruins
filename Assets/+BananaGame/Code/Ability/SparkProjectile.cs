using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.Ability
{
    public class SparkProjectile : ParticleProjectile
    {
        private void Awake()
        {
            projectileType = Type.Spark;
        }
    }
}
