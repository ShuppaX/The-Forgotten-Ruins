using UnityEngine;

namespace BananaSoup
{
    public class LiftableRockDropPoint : MonoBehaviour
    {
        public Vector3 DropPointPosition
        {
            get { return transform.position; }
        }
    }
}
