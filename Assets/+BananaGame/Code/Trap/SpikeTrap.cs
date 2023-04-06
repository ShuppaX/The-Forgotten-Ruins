using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.Traps
{
    public class SpikeTrap : Trap
    {
        [SerializeField] private Transform trapDamagerObject;
        [SerializeField] private AnimationCurve activationCurve;
        private float trapTimer = 0.0f;
        int curveLastIndex = 0;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            // Get last point of the animation curve.
            curveLastIndex = (activationCurve.length - 1);
        }

        public override void ActivateTrap()
        {
            StartCoroutine(MoveSpikesRoutine());
        }

        private IEnumerator MoveSpikesRoutine()
        {
            while ( trapTimer < activationCurve[curveLastIndex].time )
            {
                trapTimer += Time.deltaTime;

                Vector3 newPosition = Vector3.zero;
                newPosition.Set(trapDamagerObject.transform.position.x,
                                activationCurve.Evaluate(trapTimer),
                                trapDamagerObject.transform.position.z);

                trapDamagerObject.transform.position = newPosition;
                yield return null;
            }
        }
    }
}
