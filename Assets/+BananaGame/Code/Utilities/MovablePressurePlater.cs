using UnityEngine;

namespace BananaSoup.Utilities
{
    public class MovablePressurePlater : MonoBehaviour
    {
        public Vector3 Position
        {
            get { return transform.position; }
            set { Position = value; }
        }
    }
}
