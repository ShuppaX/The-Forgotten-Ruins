using UnityEngine;

namespace BananaSoup.InteractSystem
{
    public class LiftableRockLiftPoint : MonoBehaviour
    {
        public Vector3 LiftPointPosition
        {
            get { return transform.position; }
        }
    }
}
