using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BananaSoup
{

    [CreateAssetMenu(fileName = "AudioData", menuName = "Data/AudioContainer")]
    public class AudioContainer : ScriptableObject
    {
        [Serializable]
        public class SoundItem
        {
            public SoundEffect type;
            public AudioClip clip;
        }

        [SerializeField] private SoundItem[] soundEffects;

        /// <summary>
        /// Returns the audio clip which matches the effect type
        /// </summary>
        /// <param name="effectType"></param>
        /// <returns>Correct clip if it is stored in the soundEffects array. Null otherwise.</returns>
        public AudioClip GetClipByType(SoundEffect effectType)
        {
            foreach (SoundItem soundEffect in soundEffects)
            {
                if (soundEffect.type == effectType)
                {
                    return soundEffect.clip;
                }
            }

            return null;


        }
    }
}
