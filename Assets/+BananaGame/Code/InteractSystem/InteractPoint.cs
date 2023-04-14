using UnityEngine;

namespace BananaSoup.InteractSystem
{
    /// <summary>
    /// Point where the player character will interact an Interactable.
    /// For example, a point where the player will stay when pushing a box or pulling a lever.
    /// </summary>
    public class InteractPoint : MonoBehaviour
    {
        public Vector3 Position
        {
            get { return transform.position; }
        }
    }
}
