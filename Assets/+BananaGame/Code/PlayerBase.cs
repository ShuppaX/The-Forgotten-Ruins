using BananaSoup.Managers;
using System.Collections;
using UnityEngine;

namespace BananaSoup
{
    public class PlayerBase : MonoBehaviour
    {
        public static PlayerBase Instance { get; private set; }
        private PlayerInput playerInput;
        private bool areAbilitiesEnabled = true;
        private bool isInteractingEnabled = true;
        private bool isMovable = true;
        private bool isTurnable = true;
        private bool canDash = true;
        private bool isSwordLooted = false;
        private bool isThrowableLooted = false;
        private bool isDashLooted = false;

        private bool isDead = false;

        private float lateStartDelayTime = 0.001f;

        private Coroutine lateStartRoutine = null;

        private PlayerStateManager psm = null;
        private const PlayerStateManager.PlayerState dead = PlayerStateManager.PlayerState.Dead;

        public bool AreAbilitiesEnabled
        {
            get { return areAbilitiesEnabled; }
            set { areAbilitiesEnabled = value; }
        }

        public bool IsInteractingEnabled
        {
            get { return isInteractingEnabled; }
            set { isInteractingEnabled = value; }
        }

        public bool IsMovable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }

        public bool IsTurnable
        {
            get { return isTurnable; }
            set { isTurnable = value; }
        }

        public bool CanDash
        {
            get { return canDash; }
            set { canDash = value; }
        }

        public bool IsSwordLooted
        {
            get { return isSwordLooted; }
            set { isSwordLooted = value; }
        }

        public bool IsThrowableLooted
        {
            get { return isThrowableLooted; }
            set { isThrowableLooted = value; }
        }

        public bool IsDashLooted
        {
            get { return isDashLooted; }
            set { isDashLooted = value; }
        }

        private void Awake()
        {
            if ( Instance == null )
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (lateStartRoutine == null )
            {
                lateStartRoutine = StartCoroutine(nameof(LateStart));
            }
        }

        /// <summary>
        /// Stores the players input to playerInput.
        /// </summary>
        private void Setup()
        {
            psm = PlayerStateManager.Instance;
            if ( psm == null )
            {
                Debug.LogError($"PlayerBase in {name} couldn't find an instance of PlayerStateManager!");
            }

            isDead = false;
        }

        public void ToggleAllActions(bool value)
        {
            if ( isDead )
            {
                return;
            }

            Debug.Log($"Toggled players actions to: {value}");
            AreAbilitiesEnabled = value;
            CanDash = value;
            IsInteractingEnabled = value;
            IsMovable = value;
            IsTurnable = value;

            if ( psm.CurrentPlayerState == dead )
            {
                isDead = true;
            }
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForSeconds(lateStartDelayTime);
            Setup();
        }
    }
}
