using UnityEngine;

namespace BananaSoup
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] private int damage = 0;
        [SerializeField] private LayerMask canDamageWhat;

        public virtual void OnTriggerEnter(Collider collision)
        {
            if ( ((1 << collision.gameObject.layer) & canDamageWhat) == 0 )
            {
                return;
            }

            if ( collision.gameObject.GetComponent<IHealth>() != null )
            {
                collision.gameObject.GetComponent<IHealth>().DecreaseHealth(damage);
            }
            else
            {
                Debug.LogError(collision.gameObject.name + " doesn't implement the " +
                    "interface IHealth and is not damageable by " + gameObject.name + "!");
            }
        }
    }
}
