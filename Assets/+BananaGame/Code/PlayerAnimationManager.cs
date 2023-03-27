using UnityEngine;

namespace BananaSoup
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            animator = GetComponent<Animator>();
            if ( animator == null )
            {
                Debug.LogError(this + " is missing reference to the Animator component and it's required!");
            }
        }

        public void SetAnimation(string animationName)
        {
            animator.SetTrigger(animationName);
        }
    }
}
