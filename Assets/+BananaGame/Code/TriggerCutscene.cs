using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace BananaSoup.Cutscenes
{
    public class TriggerCutscene : MonoBehaviour
    {
        private PlayableDirector director;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
            if ( director == null )
            {
                Debug.LogError(name + " is missing a reference to the PlayableDirector!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.GetComponent<PlayerBase>() != null )
            {
                director.Play();
            }
        }
    }
}
