using UnityEngine;

namespace BananaSoup.Traps
{
    public class MovablePressureplate : MonoBehaviour
    {
        public Vector3 Position
        {
            get { return transform.position; }
            set { Position = value; }
        }
    }
}
