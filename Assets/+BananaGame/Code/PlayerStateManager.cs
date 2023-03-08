using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BananaSoup
{
    public class PlayerStateManager : MonoBehaviour
    {
        public static PlayerStateManager Instance { get; private set; }

        [SerializeField]
        private TMP_Text playerStateText;

        [HideInInspector]
        public State playerState = 0;

        public enum State
        {
            Idle        = 0, // Default state
            Moving      = 1,
            Dashing     = 2,
            Attacking   = 3,
            Interacting = 4,
            InAir       = 5
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
        }

        // Show the current playerState on the UI
        private void Update()
        {
            playerStateText.SetText(playerState.ToString());
        }
    }
}
