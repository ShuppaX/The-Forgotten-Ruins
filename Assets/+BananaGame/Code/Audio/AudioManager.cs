using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    
    /// <summary>
    /// To use playClip from other scripts:
    /// AudioManager.PlayClip(openAudio, Folder.SoundEffect.EnumName);
    /// AudioManager.PlayClip(openAudio, Audio.SoundEffect.ChestOpen);
    /// </summary>
    public static class AudioManager
    {
        private const string AudioContainerName = "AudioData";
        private static AudioContainer container;

        public static AudioContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = Resources.Load<AudioContainer>(AudioContainerName);
                }

                return container;
            }
        }

        public static bool PlayClip(AudioSource source, SoundEffect effectType)
        {
            AudioClip clip = Container.GetClipByType(effectType);
            if (clip != null && source != null)
            {
                source.PlayOneShot(clip);
                return true;
            }

            return false;
        }

        public static float ToLinear(float db)
        {
            return Mathf.Clamp01(Mathf.Pow(10.0f, db / 20.0f));
        }

        public static float ToDB(float linear)
        {
            return linear <= 0 ? -80f : Mathf.Log10(linear * 20.0f);
        }
        
        
    }

    
}
