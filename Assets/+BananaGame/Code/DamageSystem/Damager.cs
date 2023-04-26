using UnityEngine;

namespace BananaSoup.DamageSystem
{
    public class Damager : MonoBehaviour
    {
        [SerializeField, Tooltip("The amount of damage the object should do.")]
        private int damage = 0;
        [SerializeField, Tooltip("The layers the object should be able to damage.")]
        private LayerMask canDamageWhat;

        /// <summary>
        /// Method which checks if the object collided OnTriggerEnter is something that
        /// the object can damage, if it is and the object has a component which implements
        /// IHealth interface then decrease it's health with the damage amount.
        /// If the object doesn't have a component which implements IHealth throw an error.
        /// </summary>
        /// <param name="collision"></param>
        public virtual void OnTriggerEnter(Collider collision)
        {
            if ( ((1 << collision.gameObject.layer) & canDamageWhat) == 0 )
            {
                return;
            }

            if ( collision.gameObject.TryGetComponent(out IHealth health) )
            {
                health.DecreaseHealth(damage);
            }
            else
            {
                Debug.LogError(collision.gameObject.name + " doesn't implement the " +
                    "interface IHealth and is not damageable by " + gameObject.name + "!");
            }
        }
    }
}
