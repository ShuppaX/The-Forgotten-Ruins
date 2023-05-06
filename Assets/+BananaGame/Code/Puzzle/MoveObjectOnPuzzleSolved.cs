using UnityEngine;

namespace BananaSoup.PuzzleSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveObjectOnPuzzleSolved : PuzzleSolutionGameObject
    {
        [SerializeField] private Vector3 endPoint;

        [SerializeField, Tooltip("Normally it removes extra collider at End Point. This is for the cases " +
            "for example, bridge is starting from normally, walkable location but end location is out of reach")]
        private bool removeExtraColliderAtStartPoint;

        [SerializeField] private float lerpModifier = 3.0f;
        [SerializeField] private float movementBlockerExtraHeight = 2.0f;
        private float distanceCompare = 0.001f;
        private bool hasMoved;
        private Vector3 startingPosition;
        private BoxCollider movementBlocker;

        private void OnValidate()
        {
            // HACK: Setting this object to be kinematic and turning gravity false so it won't be affected by any forces.
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            startingPosition = transform.position;

            movementBlocker = gameObject.AddComponent<BoxCollider>();
            movementBlocker.size = new Vector3(movementBlocker.size.x, movementBlocker.size.y + movementBlockerExtraHeight, movementBlocker.size.z);
        }

        public void SetCurrentLocation()
        {
            endPoint = transform.position;
        }

        // TODO: If have time change this to for example, Coroutine to happen only once
        private void FixedUpdate()
        {
            // Puzzle solved, move object.
            if ( IsSolved && !hasMoved )
            {
                if ( removeExtraColliderAtStartPoint )
                {
                    movementBlocker.enabled = true;
                }

                transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime * lerpModifier);
                float distance = (transform.position - endPoint).sqrMagnitude;
                if ( distance < distanceCompare )
                {
                    transform.position = endPoint;
                    hasMoved = true;
                    if ( !removeExtraColliderAtStartPoint )
                    {
                        movementBlocker.enabled = false;
                    }
                }
            }
            // Puzzle is unsolved again, re-move it to the start position.
            // NOTE: Not the best way to make this, but it's working
            else if ( !IsSolved )
            {
                if ( !removeExtraColliderAtStartPoint )
                {
                    movementBlocker.enabled = true;
                }
                hasMoved = false;
                if ( removeExtraColliderAtStartPoint )
                {
                    movementBlocker.enabled = false;
                }
                transform.position = Vector3.Lerp(transform.position, startingPosition, Time.deltaTime * lerpModifier);
                float distance = (transform.position - startingPosition).sqrMagnitude;
                if ( distance < distanceCompare )
                {
                    transform.position = startingPosition;
                }
            }
        }
    }
}
