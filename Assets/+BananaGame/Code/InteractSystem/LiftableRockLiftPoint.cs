using UnityEngine;

namespace BananaSoup
{
    public class LiftableRockLiftPoint : MonoBehaviour
    {
        public Vector3 LiftPointPosition
        {
            get { return transform.position; }
        }
    }
}
