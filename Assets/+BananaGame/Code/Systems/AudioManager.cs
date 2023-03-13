using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public static class AudioManager
    {
        private const string AudioContainerName = "AudioData";
        private static AudioContainer container;

        public static Audiocontainer Container
        {
            get
            {
                if (container == null)
                {
                    container = Resources.Load<Audiocontainer>(AudioContainerName);
                }
            }
        }
        
        
    }

    
}
