using UnityEngine;

namespace BananaSoup.Utilities
{
    public class MovablePressurePlate : MonoBehaviour
    {
        public Vector3 Position
        {
            get { return transform.position; }
            set { Position = value; }
        }
    }
}
