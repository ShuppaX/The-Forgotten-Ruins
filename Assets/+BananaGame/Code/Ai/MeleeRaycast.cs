using UnityEngine;

namespace BananaSoup
{
    public class MeleeRaycast : EnemyBase
    {
        //Definitions
        public EnemyMeleeDamage meleeScript; // reference to enemy's melee damage script

        public override void Attack()
        {
            //Stop enemy movement
            navMeshAgent.SetDestination(transform.position);

            if ( CanAttack )
            {
                CanAttack = false;
                anim.SetTrigger(attack);

                //TODO Attack code here
                Debug.Log("Enemy Swings");
                meleeScript.MeleeAttack();

                Invoke(nameof(ResetAttack), _timeBetweenAttacks);
            }
        }
    }
}