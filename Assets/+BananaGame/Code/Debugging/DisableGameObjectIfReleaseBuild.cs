using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class DisableGameObjectIfReleaseBuild : MonoBehaviour
    {
        void Start()
        {
            if ( !Debug.isDebugBuild || !Application.isEditor )
            {
                gameObject.SetActive(false);
            }
        }
    }
}
