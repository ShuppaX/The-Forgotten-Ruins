using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.Utilities
{
    public class DestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}
