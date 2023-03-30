using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public class DestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}
